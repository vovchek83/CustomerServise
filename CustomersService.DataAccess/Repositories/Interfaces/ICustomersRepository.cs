using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomersService.DataModel.Models;

namespace CustomersService.DataModel.Repositories.Interfaces
{
    public interface ICustomersRepository
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(Guid id);
        Task<bool> AddCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);

    }
}