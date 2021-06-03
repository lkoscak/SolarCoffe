using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffe.Data.Models;
using SolarCoffe.Data.Serializations;
using SolarCoffe.Data.ViewModels;
using SolarCoffe.Services.Inventory;

namespace SolarCoffe.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger _logger;
        public InventoryController(ILogger<InventoryController> logger, IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }
        [HttpGet("/api/inventory")]
        public ActionResult GetCurrentInventory()
        {
            _logger.LogInformation("Getting current inventory...");
            var inventory = _inventoryService.GetCurrentInventory()
            .Select(pi => new ProductInventoryModel
            {
                Id = pi.Id,
                Product = ProductMapper.SerializeProductModel(pi.Product),
                IdealQuantity = pi.IdealQuantity,
                QuantityOnHand = pi.QuantityOnHand
            })
            .OrderBy(inv => inv.Product.Name)
            .ToList();
            return Ok(inventory);
        }
        [HttpPatch("/api/inventory")]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            _logger.LogInformation($"Updating inventory for {shipment.ProductId}...");
            var inventory = _inventoryService.UpdateUnitsAvailable(shipment.ProductId, shipment.Adjustment);
            return Ok();
        }
    }
}
