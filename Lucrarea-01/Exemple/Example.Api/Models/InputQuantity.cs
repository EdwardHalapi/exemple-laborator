using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Exemple.Domain.Models;

namespace Example.Api.Models
{
    public class InputProduct
    {
        [Required]
        public int ProductCodeId { get; set; }

        [Required]
        [RegularExpression(ProductCode.Pattern)]
        public string Code { get; set; }

        [Required]
        public int  Stoc { get; set; }
    }
}