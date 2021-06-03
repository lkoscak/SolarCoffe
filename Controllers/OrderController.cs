using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffe.Data.Serializations;
using SolarCoffe.Data.ViewModels;
using SolarCoffe.Services.Customer;
using SolarCoffe.Services.Order;

namespace SolarCoffe.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService, ICustomerService customerService)
        {
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost("/api/invoice")]
        public ActionResult GenerateNewOrder([FromBody] InvoiceModel invoice)
        {
            _logger.LogInformation("Generating invoice");
            var order = OrderMapper.SerializeInvoiceToOrder(invoice);
            order.Customer = _customerService.GetById(invoice.CustomerId);
            _orderService.GenerateOpenOrder(order);
            return Ok();
        }
        [HttpGet("/api/order")]
        public ActionResult GetOrders()
        {
            var orders = _orderService.GetAllOrders();
            var orderModels = OrderMapper.SerializeOrdersToViewModels(orders):
            return Ok(orderModels);
        }
        [HttpPatch("/api/order/complete/{id}")]
        public ActionResult CompleteOrder(int id)
        {
            _logger.LogInformation($"Marking order {id} as completed!");
            _orderService.MarkFulfiled(id);
            return Ok();
        }
    }
}
