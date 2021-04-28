using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffe.Data;
using SolarCoffe.Data.Models;
using SolarCoffe.Services.Responses;

namespace SolarCoffe.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDbContext _dbContext;
        public ProductService(SolarDbContext solarDbContext)
        {
            _dbContext = solarDbContext;
        }
        public ServiceResponse<bool> ArchiveProduct(int id)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _dbContext.Products.Add(product);
                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10
                };
                _dbContext.ProductInventories.Add(newInventory);
                _dbContext.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Saved new product",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = $"Error saving new product: {e.StackTrace}",
                    IsSuccess = false
                };
            }
        }

        public List<Data.Models.Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        public Data.Models.Product GetProductById(int id)
        {
            return _dbContext.Products.Find(id);
        }

        ServiceResponse<Data.Models.Product> IProductService.ArchiveProduct(int id)
        {
            try
            {
                Data.Models.Product product = _dbContext.Products.Find(id);
                if (product != null)
                {
                    product.isArchived = true;
                    product.UpdatedOn = DateTime.UtcNow;
                    _dbContext.Update(product);
                    _dbContext.SaveChanges();

                    return new ServiceResponse<Data.Models.Product>
                    {
                        Data = product,
                        Time = DateTime.UtcNow,
                        Message = "Product archived",
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ServiceResponse<Data.Models.Product>
                    {
                        Data = null,
                        Time = DateTime.UtcNow,
                        Message = "Product not found",
                        IsSuccess = false
                    };
                }
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = $"Error archiving a product: {e.StackTrace}",
                    IsSuccess = true
                };
            }
        }
    }
}
