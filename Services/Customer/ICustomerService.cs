using System;
using System.Collections.Generic;
using SolarCoffe.Data.Models;
using SolarCoffe.Services.Responses;

namespace SolarCoffe.Services.Customer
{
    public interface ICustomerService
    {
        List<SolarCoffe.Data.Models.Customer> GetAllCustomers();
        ServiceResponse<SolarCoffe.Data.Models.Customer> CreateCustomer(SolarCoffe.Data.Models.Customer customer);
        ServiceResponse<bool> DeleteCustomer(int id);
        SolarCoffe.Data.Models.Customer GetById(int id);
    }
}
