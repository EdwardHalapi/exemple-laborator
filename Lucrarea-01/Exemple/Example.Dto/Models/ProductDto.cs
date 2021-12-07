using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Dto.Models
{
    public record ProductDto
    {
        public string productName { get; init; }
        public string productCode { get; init; }
        public decimal quantity { get; init; }
        public decimal price { get; init; }
    }
}
