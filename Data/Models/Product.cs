using System;
using System.ComponentModel.DataAnnotations;

namespace SolarCoffe.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsTaxable { get; set; }
        public bool isArchived { get; set; } = false;


    }
}
