using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Exemple.Domain.Models;

namespace Example.Api.Models
{
    public class InputOrderLine
    {
        [Required]
        public int OrderLineId { get; set; }

        [Required]
        [Range(1, 1000)]
        public decimal Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}