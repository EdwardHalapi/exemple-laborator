using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
namespace Exemple.Domain.Models
{
    public record Quantity
    {
        public decimal Value { get; }

        public Quantity(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidQuantityException($"{value:0.##} is an invalid Quantity value.");
            }
        }

        public Quantity Round()
        {
            var roundedValue = Math.Round(Value);
            return new Quantity(roundedValue);
        }

        public override string ToString()
        {
            return $"{Value:0.##}";
        }
        public static Option<Quantity> TryParseQuantity(string quantityString)
        {
            if (decimal.TryParse(quantityString, out decimal numericQuantity) && IsValid(numericQuantity))
            {
                return Some<Quantity>(new(numericQuantity));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(decimal numericQuantity) => numericQuantity > 0 && numericQuantity <= 1000;
    }
}
