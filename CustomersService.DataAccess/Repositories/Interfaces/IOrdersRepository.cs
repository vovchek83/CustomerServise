using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomersService.DataModel.Models;

namespace CustomersService.DataModel.Repositories.Interfaces
{
    public interface IOrdersRepository
    {
        Task<List<Order>> GetOrdersAsync();

        Task<List<Order>> GetOrdersForCustomersAsync(Guid customerId);
 
        Task<List<Order>> GetCustomerOrdersPerPeriod(Guid customerId, DateTime startTime, DateTime endTime);

        Task<Order> AddOrderAsync(Order order);

        //Task<Order> UpdateOrderAsync(Order order);
        //Task DeleteOrderAsync(int orderId);

    }
}