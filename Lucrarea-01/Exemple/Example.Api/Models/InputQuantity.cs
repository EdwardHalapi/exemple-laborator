using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Exemple.Domain.Models;

namespace Example.Api.Models
{
    public class InputProduct
    {
        [Required]
        [RegularExpression(ProductCode.Pattern)]
        public string ProductCodeId { get; set; }

        [Required]
        [Range(1, 1000)]
        public decimal Quantity { get; set; }

        [Required]
        public string  Address { get; set; }
    }
}