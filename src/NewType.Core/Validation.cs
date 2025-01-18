using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Oneiro;

internal record struct ValidationRule<T>(
    Predicate<T> Validate,
    string ErrorMessage
) where T : struct; 

public record struct ValidationError(
    string RuleName,
    string ErrorMessage,
    object Value
);

internal sealed class ValidationRules<T> : ConcurrentDictionary<string, ValidationRule<T>>
    where T : struct {
    internal void Add(string name, Predicate<T> predicate, string errorMessage) =>
        GetOrAdd(key: name, _ => new ValidationRule<T>(predicate, errorMessage));
    
    internal (bool result, IReadOnlyList<ValidationError> errors) Validate(T value) {
        var errors = this.Where(r => !r.Value.Validate(value))
            .Select(r => new ValidationError(
                r.Key,
                $"[Violation of rule {{{r.Key}}}]: {r.Value.ErrorMessage}",
                value)
            ).ToList();
        
        return (!errors.Any(), errors);
    }
}

internal static class Errors {
    internal static void Throw(IReadOnlyList<ValidationError> errors, [CallerMemberName] string caller = "") {
        if (!errors.Any()) return;
        var message = string.Join("\n", errors.Select(e => $"{e.RuleName}: {e.ErrorMessage}, Value: {e.Value}"));
        throw new NewTypeValidationException(message, errors) {
            Source = caller,
        };
    }
}

public sealed class NewTypeValidationException : Exception {
    public NewTypeValidationException(string message, IReadOnlyList<ValidationError> errors) : base(message) {
        Data["Errors"] = errors;
    }
}

