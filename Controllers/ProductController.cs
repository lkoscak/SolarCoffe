using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffe.Services.Product;

namespace SolarCoffe.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Getting all products");
            _productService.GetAllProducts();
            return Ok("");
        }
    }
}
