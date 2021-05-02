using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffe.Data;
using SolarCoffe.Data.Models;
using SolarCoffe.Services.Inventory;
using SolarCoffe.Services.Product;
using SolarCoffe.Services.Responses;

namespace SolarCoffe.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        public OrderService(SolarDbContext solarDbContext, ILogger logger, IProductService productService, IInventoryService inventoryService)
        {
            _dbContext = solarDbContext;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }
        public ServiceResponse<SalesOrder> GenerateOpenOrder(SalesOrder order)
        {
            foreach (var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductById(item.Product.Id);

                var inventoryId = _inventoryService.GetByProduct(item.Product.Id).Id;
                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }
            try
            {
                _dbContext.SalesOrders.Add(order);
                _dbContext.SaveChanges();
                return new ServiceResponse<SalesOrder>
                {
                    IsSuccess = true,
                    Data = order,
                    Time = DateTime.UtcNow,
                    Message = "Order created"
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<SalesOrder>
                {
                    IsSuccess = false,
                    Data = order,
                    Time = DateTime.UtcNow,
                    Message = $"Error creating order: {e.StackTrace}"
                };
            }
        }

        public List<SalesOrder> GetAllOrders()
        {
            return _dbContext.SalesOrders
            .Include(so => so.Customer)
                .ThenInclude(customer => customer.PrimaryAddress)
            .Include(so => so.SalesOrderItems)
                .ThenInclude(item => item.Product)
            .ToList();
        }

        public ServiceResponse<SalesOrder> MarkFulfiled(int id)
        {
            try
            {
                var order = _dbContext.SalesOrders.Find(id);
                if (order != null)
                {
                    order.IsPaid = true;
                    order.UpdatedOn = DateTime.UtcNow;
                    _dbContext.Update(order);
                    _dbContext.SaveChanges();
                    return new ServiceResponse<SalesOrder>
                    {
                        IsSuccess = true,
                        Data = order,
                        Time = DateTime.UtcNow,
                        Message = "Order marked fulfiled"
                    };
                }
                else
                {
                    return new ServiceResponse<SalesOrder>
                    {
                        IsSuccess = false,
                        Data = null,
                        Time = DateTime.UtcNow,
                        Message = "Order not found"
                    };
                }
            }
            catch (Exception e)
            {
                return new ServiceResponse<SalesOrder>
                {
                    IsSuccess = false,
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = $"Error marking order as fulfiled: {e.StackTrace}"
                };
            }
        }
    }
}
