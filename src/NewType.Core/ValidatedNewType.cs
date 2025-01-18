using System.Collections.Concurrent;

namespace Oneiro;

public abstract 
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    ValidatedNewType<T, TNewType> : NewType<T, TNewType>
    where TNewType : ValidatedNewType<T, TNewType>
    where T : struct {
    
    private static readonly 
        Lazy<ConcurrentDictionary<Type, ValidationRules<T>>> Rules = 
            new (() => []);
    private IReadOnlyList<ValidationError> _errors = [];
    
    protected ValidatedNewType(T value) : base(value) {}
    
    public bool HasErrors() => _errors.Any();
    public IReadOnlyList<ValidationError> GetErrors() => _errors;

    public TNewType ThrowIfErrored() {
        Errors.Throw(_errors);
        return From(_value);
    }
    
    public TNewType IfErrored(Func<IReadOnlyList<ValidationError>, TNewType> func) =>
        HasErrors() ? func(GetErrors()) : From(_value);
    
    private void SetErrors(IReadOnlyList<ValidationError> errors) => _errors = errors;
    
    protected void RuleFor(string name, Predicate<T> validator, string errorMessage) =>
        Rules.Value.GetOrAdd(typeof(TNewType), _ => [])
            .Add(name, validator, errorMessage);
    
    public new static TNewType From(T value) {
        var lambda = ActivatorUtil.CreateInstanceFactory<T, TNewType>();
        var r = lambda(value);
        if (!Rules.Value.ContainsKey(typeof(TNewType))) return r;
        var (result, errors) = Rules.Value[typeof(TNewType)].Validate(value);
        if (result) return r;
        
        var nr = lambda(default);
        nr.SetErrors(errors);
        return nr;
    }
    
#if !NET6_0_OR_GREATER
    public override bool Equals(object? obj) =>
        obj is ValidatedNewType<T, TNewType> other && _value.Equals(other._value);
    
    public override int GetHashCode() => _value.GetHashCode();
    
    public static bool operator ==(ValidatedNewType<T, TNewType> left, ValidatedNewType<T, TNewType> right) =>
        left.Equals(right);
    
    public static bool operator !=(ValidatedNewType<T, TNewType> left, ValidatedNewType<T, TNewType> right) =>
        !left.Equals(right);    
#endif
}