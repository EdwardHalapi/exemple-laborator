using LanguageExt;
using System.Text.RegularExpressions;
using static LanguageExt.Prelude;

namespace Exemple.Domain.Models
{
    public record ProductCode
    {
        public const string Pattern = "([A-Z])(a*)";
        private static readonly Regex ValidPattern = new(Pattern);

        public string Value { get; }

        public ProductCode(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException("Wrong Product Code");
            }
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);
        public override string ToString()
        {
            return Value;
        }
        public static Option<ProductCode> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ProductCode>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
    }
}
