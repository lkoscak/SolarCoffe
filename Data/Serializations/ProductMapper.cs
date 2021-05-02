using System;
using SolarCoffe.Data.Models;
using SolarCoffe.Data.ViewModels;

namespace SolarCoffe.Data.Serializations
{
    public static class ProductMapper
    {
        public static ProductModel SerializeProductModel(Product product)
        {
            return new ProductModel
            {
                Id = product.Id,
                CreatedOn = product.CreatedOn,
                UpdatedOn = product.UpdatedOn,
                Price = product.Price,
                Name = product.Name,
                Description = product.Description,
                IsTaxable = product.IsTaxable,
                isArchived = product.isArchived
            };
        }

        public static Product SerializeProductModel(ProductModel productModel)
        {
            return new Product
            {
                Id = productModel.Id,
                CreatedOn = productModel.CreatedOn,
                UpdatedOn = productModel.UpdatedOn,
                Price = productModel.Price,
                Name = productModel.Name,
                Description = productModel.Description,
                IsTaxable = productModel.IsTaxable,
                isArchived = productModel.isArchived
            };
        }
    }
}
