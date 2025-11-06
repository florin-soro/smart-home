using Thermostat.Domain.Common.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.Domain.Shared
{
    public record ValidationRule<T>(
    Func<T, bool> Check,
    string ErrorMessage
);

    public static class Guard
    {

        public static T ValidEnum<T>(T enumValue, string paramName) where T : struct, Enum =>
        Enum.IsDefined(typeof(T), enumValue)
            ? enumValue
            : throw new DomainRuleValidationException( $"Invalid enum value {enumValue}.Param name: {paramName}");


        public static T NotNull<T>(T value, string paramName)
        {
            if (value is null)
                throw new DomainRuleValidationException($"{paramName} cannot be null.");
            return value;
        }
        public static bool IsNotNull<T>(T value, string paramName)
        {
            if (value is null)
                return false;
            return true;
        }
        public static ValidationRule<T> IsNotNull<T>() =>
       new ValidationRule<T>(
           value => value is not null,
           "Value must not be null."
       );

        public static bool IsGuidEmpty(Guid id)
        {
            return id == Guid.Empty;
        }

        public static ValidationRule<Guid> IsGuidNotEmpty() => new ValidationRule<Guid>(guid=>Guid.Empty!=guid, "Guid must not be empty.");

        public static Guid GuidNotEmpty(Guid value, string paramName)
        {
            if (IsGuidEmpty(value))
                    throw new DomainRuleValidationException($"{paramName} cannot be empty.");
            return value;
        }
        public static bool IsStringNotNullOrEmpty(string value, string paramName)
        {
            return !IsStringNullOrEmpty(value, paramName);
        }

        public static ValidationRule<string> IsStringNotNullOrEmpty() => new ValidationRule<string>(string.IsNullOrWhiteSpace, "Value must not be null.");

        public static bool IsStringNullOrEmpty(string value, string paramName)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        public static string StringNotNullOrEmpty(string value, string paramName)
        {
            if (IsStringNullOrEmpty(value, paramName))
                throw new DomainRuleValidationException($"{paramName} cannot be empty or whitespace.");

            return value;
        }

        public static T Validate<T>(T value, string paramName, params Func<T,string, bool>[] checks)
        {
            foreach (var check in checks)
            {
                if (!check(value, paramName))
                {
                    throw new DomainRuleValidationException($"{paramName} failed validation check.");
                }
            }
            return value;
        }

        public static T Validate<T>(T value, string paramName, params Func<T, string, (bool Success, string? Message)>[] checks)
        {
            foreach (var check in checks)
            {
                var (success, message) = check(value, paramName);
                if (!success)
                {
                    var errorMessage = message ?? $"{paramName} failed validation check.";
                    throw new DomainRuleValidationException(errorMessage);
                }
            }
            return value;
        }

        public static T Validate<T>(T value, string paramName, params ValidationRule<T>[] rules)
        {
            foreach (var rule in rules)
            {
                if (!rule.Check(value))
                {
                    throw new DomainRuleValidationException($"Validation of {paramName}: {rule.ErrorMessage}");
                }
            }
            return value;
        }
        public static string CheckMultiple(string value, string paramName, params Func<string, string, bool>[] checks)
        {
            foreach (var check in checks)
            {
                if (!check(value, paramName))
                {
                    throw new DomainRuleValidationException($"{paramName} failed validation check.");
                }
            }
            return value;
        }

        public static bool IsMet(double value1, double value2, Func<double,double, bool> predicate, string message)
        {
            if (!predicate(value1, value2))
                return false;
            return true;
        }

        public static T Check<T>(T value, Func<T, bool> predicate, string message)
        {
            if (!predicate(value))
                throw new DomainRuleValidationException(message);
            return value;
        }
    }
}
