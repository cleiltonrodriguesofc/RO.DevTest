using System;
using System.ComponentModel.DataAnnotations;

namespace RO.DevTest.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } // primary key

        [Required]
        [RegularExpression("^[A-Z0-9\\-]{5,20}$", ErrorMessage = "invalid code format")]
        public string Code { get; set; } = null!; // unique product code

        [Required]
        public string Name { get; set; } = null!; // product name

        public string Description { get; set; } = null!; // short description

        [Range(0.0, double.MaxValue, ErrorMessage = "price must be non-negative")]
        public decimal Price { get; set; } // product price

        [Range(0, int.MaxValue, ErrorMessage = "stock quantity must be non-negative")]
        public int StockQuantity { get; set; } // available units in stock

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // creation date
    }
}
