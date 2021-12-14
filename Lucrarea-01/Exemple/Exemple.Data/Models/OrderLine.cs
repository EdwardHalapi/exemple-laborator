using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Data.Models
{
    public class OrderLine
    {
        public int OrderLineId { get; set; }
        public int Qunatity { get; set; }
        public int Price { get; set; }
    }
}
