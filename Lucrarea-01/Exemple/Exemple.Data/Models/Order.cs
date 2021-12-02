using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductIdId { get; set; }
        public string Address { get; set; }
        public decimal? Total { get; set; }
    }
}