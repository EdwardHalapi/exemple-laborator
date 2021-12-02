using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record CalculatedCustomerPrice(ProductCode ProductCode, Quantity Quantity,Quantity price)
    {
        public int ProductId { get; set; }
        public bool IsUpdated { get; set; }
    }
}
