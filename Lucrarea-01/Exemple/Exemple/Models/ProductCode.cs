using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

namespace Exemple.Domain.Models
{
    public record ProductCode
    {
        private static readonly Regex ValidPattern = new("([A-Z])(a*)");

        public string Value { get; }

        private ProductCode(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                if(value.Length <= 5)
                {
                    Value = value;
                } 
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
