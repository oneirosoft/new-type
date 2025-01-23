using System.Collections.Concurrent;

namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <typeparamref name="T"/>.
/// The new type is validated using rules that are added to the new type.
/// <code>
/// public sealed class WrappedInt : ValidatedNewType&lt;int, WrappedInt> {
///     private WrappedInt(int value) : base(value) {
///         RuleFor("Positive", v => v > 0, "Value must be positive.");
///     }
/// }
/// </code>
/// </summary>
/// <remarks>
/// When creating a validated new type it is recommended to keep the
/// constructor <c>private</c> or <c>protected</c>.
/// </remarks>
/// <typeparam name="T">
/// The type of the value being wrapped.
/// </typeparam>
/// <typeparam name="TNewType">
/// The type of the new type.
/// </typeparam>
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
    
    /// <summary>
    /// Can be used to determine if the new type has errors.
    /// </summary>
    /// <returns>
    /// True if the new type has errors. Otherwise, false.
    /// </returns>
    public bool HasErrors() => _errors.Any();
    
    /// <summary>
    /// Returns the errors for the new type.
    /// If there are no errors, an empty list is returned.
    /// </summary>
    /// <returns>
    /// The errors for the new type or an empty list if no errors are present.
    /// </returns>
    public IReadOnlyList<ValidationError> GetErrors() => _errors;

    /// <summary>
    /// Throws a <see cref="NewTypeValidationException"/> if the new type has errors.
    /// </summary>
    /// <returns>
    /// The new type if no errors are present.
    /// </returns>
    /// <exception cref="NewTypeValidationException">
    /// Thrown if the new type has errors.
    /// </exception>
    public TNewType ThrowIfErrored() {
        Errors.Throw(_errors);
        return From(_value);
    }
    
    /// <summary>
    /// Provides a way to handle errors and returns an alternative new type
    /// if the new type has errors.
    ///
    /// Otherwise, returns the original new type (w/o) errors.
    /// <code>
    /// // Provided a rule where the value must be positive.
    /// var wrapped = WrappedInt.From(-1)
    ///     .IfErrored(e => WrappedInt.From(1));
    /// // wrapped is now WrappedInt(1)
    /// </code>
    /// </summary>
    /// <param name="func">
    /// The function to execute if the new type has errors.
    /// </param>
    /// <returns>
    /// The original new type if no errors are present. Otherwise, the new type
    /// produced by the function.
    /// </returns>
    public TNewType IfErrored(Func<IReadOnlyList<ValidationError>, TNewType> func) =>
        HasErrors() ? func(GetErrors()) : From(_value);
    
    private void SetErrors(IReadOnlyList<ValidationError> errors) => _errors = errors;
    
    /// <summary>
    /// Creates a new rule for the new type's underlying value.
    /// See <see cref="ValidatedNewType{T,TNewType}"/> comments for an example.
    /// </summary>
    /// <param name="name">
    /// The name of the rule to help identify what rule was violated.
    /// </param>
    /// <param name="validator">
    /// The predicate that determines if the rule is violated.
    /// </param>
    /// <param name="errorMessage">
    /// A user-friendly to help why the rule was violated.
    /// </param>
    protected void RuleFor(string name, Predicate<T> validator, string errorMessage) =>
        Rules.Value.GetOrAdd(typeof(TNewType), _ => [])
            .Add(name, validator, errorMessage);
    
    /// <inheritdoc cref="NewType{T,TNewType}.From"/>
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