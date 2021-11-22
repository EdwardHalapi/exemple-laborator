using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using LanguageExt;

namespace Exemple.Domain.Models
{
    public record Adresa
    {
        private static readonly Regex ValidPattern = new("/d{1,}(s{1}w{1,})(s{1}?w{1,})+g");

        public string Value { get; }

        private Adresa(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidateAddressException("");
            }
        }

        public Adresa(object p)
        {
        }

        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static Option<Adresa> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<Adresa>(new(stringValue));
            }
            else
            {
                return None;
            };
        }
    }
}
