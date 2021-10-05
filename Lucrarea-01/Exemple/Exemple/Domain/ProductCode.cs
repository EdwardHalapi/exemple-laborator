using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public record ProductCode
    {
        private static readonly Regex ValidPattern = new("([A-Z])(a*)");

        public string Value { get; }

        private ProductCode(string value)
        {
            if (ValidPattern.IsMatch(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException("Wrong Product Code");
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
