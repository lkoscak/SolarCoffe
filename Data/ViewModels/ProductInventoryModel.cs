using System;
using SolarCoffe.Data.ViewModels;

namespace SolarCoffe.Data.Models
{
    public class ProductInventoryModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int QuantityOnHand { get; set; }

        public int IdealQuantity { get; set; }

        public ProductModel Product { get; set; }
    }
}
