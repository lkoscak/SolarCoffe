using System;
using System.Collections.Generic;
using SolarCoffe.Data.Models;
using SolarCoffe.Services.Responses;

namespace SolarCoffe.Services.Inventory
{
    public interface IInventoryService
    {
        List<ProductInventory> GetCurrentInventory();
        ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
        ProductInventory GetByProduct(int productId);
        List<ProductInventorySnapshot> GetSnapshotHistory();
    }
}
