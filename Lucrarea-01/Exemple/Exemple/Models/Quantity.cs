using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static bool TryParseQuantity(string quantityString, out Quantity quantity)
        {
            bool isValid = false;
            quantity = null;
            if (decimal.TryParse(quantityString, out decimal numericQuantity))
            {
                if (IsValid(numericQuantity))
                {
                    isValid = true;
                    quantity = new(numericQuantity);
                }
            }

            return isValid;
        }

        private static bool IsValid(decimal numericQuantity) => numericQuantity > 0 && numericQuantity <= 1000;
    }
}
