using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffe.Data;
using SolarCoffe.Data.Models;
using SolarCoffe.Services.Responses;

namespace SolarCoffe.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDbContext _dbContext;
        private readonly ILogger _logger;
        public InventoryService(SolarDbContext solarDbContext, ILogger logger)
        {
            _dbContext = solarDbContext;
            _logger = logger;
        }
        public ProductInventory GetByProduct(int productId)
        {
            return _dbContext.ProductInventories
            .Include(pi => pi.Product)
            .FirstOrDefault(pi => pi.Product.Id == productId);
        }

        public List<ProductInventory> GetCurrentInventory()
        {
            return _dbContext.ProductInventories
            .Include(pi => pi.Product)
            .Where(pi => !pi.Product.isArchived)
            .ToList();
        }

        public List<ProductInventorySnapshot> GetSnapshotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);
            return _dbContext.ProductInventorySnapshots
            .Include(snap => snap.Product)
            .Where(snap => snap.SnapshotTime > earliest && !snap.Product.isArchived)
            .ToList();
        }

        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int productId, int adjustment)
        {
            try
            {
                var inventory = _dbContext.ProductInventories
                .Include(inv => inv.Product)
                .First(inv => inv.Product.Id == productId);

                inventory.QuantityOnHand += adjustment;

                try
                {
                    CreateSnapshot(inventory);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error creating inventory snapshot: {e.StackTrace}");
                }

                _dbContext.SaveChanges();

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Data = inventory,
                    Message = $"Product {productId} inventory adjusted",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Data = null,
                    Message = $"Error adjusting inventory: {e.StackTrace}",
                    Time = DateTime.UtcNow
                };
            }
        }
        private void CreateSnapshot(ProductInventory inventory)
        {
            var snapshot = new ProductInventorySnapshot
            {
                SnapshotTime = DateTime.UtcNow,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand
            };

            _dbContext.Add(snapshot);
        }
    }
}
