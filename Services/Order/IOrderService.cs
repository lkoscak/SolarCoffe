using System;
using System.Collections.Generic;
using SolarCoffe.Data.Models;
using SolarCoffe.Services.Responses;

namespace SolarCoffe.Services.Order
{
    public interface IOrderService
    {
        List<SalesOrder> GetAllOrders();
        ServiceResponse<SalesOrder> GenerateOpenOrder(SalesOrder order);
        ServiceResponse<SalesOrder> MarkFulfiled(int id);
    }
}
