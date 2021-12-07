using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Dto.Models;
namespace Example.Dto.Events
{
    public record CaruciorPaidEvent
    {
        public List<ProductDto> Products { get; init; }
    }
}
