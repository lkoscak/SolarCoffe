using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffe.Data;
using SolarCoffe.Services.Responses;

namespace SolarCoffe.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _dbContext;
        public CustomerService(SolarDbContext solarDbContext)
        {
            _dbContext = solarDbContext;
        }
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _dbContext.Customers.Add(customer);
                _dbContext.SaveChanges();

                return new ServiceResponse<Data.Models.Customer>
                {
                    Data = customer,
                    Time = DateTime.UtcNow,
                    Message = "Saved new customer",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    Data = customer,
                    Time = DateTime.UtcNow,
                    Message = $"Error saving new customer: {e.StackTrace}",
                    IsSuccess = false
                };
            }
        }

        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            try
            {
                Data.Models.Customer customer = _dbContext.Customers.Find(id);
                if (customer != null)
                {
                    customer.UpdatedOn = DateTime.UtcNow;
                    _dbContext.Update(customer);
                    _dbContext.SaveChanges();

                    return new ServiceResponse<bool>
                    {
                        Data = true,
                        Time = DateTime.UtcNow,
                        Message = "Customer deleted",
                        IsSuccess = true
                    };
                }
                else
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Time = DateTime.UtcNow,
                        Message = "Customer not found",
                        IsSuccess = false
                    };
                }
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Time = DateTime.UtcNow,
                    Message = $"Error deleting a customer: {e.StackTrace}",
                    IsSuccess = true
                };
            }
        }

        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _dbContext.Customers.Include(customer => customer.PrimaryAddress)
            .OrderBy(customer => customer.LastName)
            .ToList();
        }

        public Data.Models.Customer GetById(int id)
        {
            return _dbContext.Customers.Find(id);
        }
    }
}
