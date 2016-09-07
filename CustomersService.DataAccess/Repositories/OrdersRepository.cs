using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CustomersService.DataModel.Models;
using CustomersService.DataModel.Repositories.Interfaces;

namespace CustomersService.DataModel.Repositories
{
    [Export(typeof(IOrdersRepository))]
    public class OrdersRepository : IOrdersRepository
    {

        public async Task<List<Order>> GetOrdersAsync()
        {
            using (var context = new CustomersServiceDbContext())
            {
                return await context.Orders.ToListAsync();
            }
        }

        public async Task<List<Order>> GetOrdersForCustomersAsync(Guid customerId)
        {
            using (var context = new CustomersServiceDbContext())
            {
                return await context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
            }
        }

        public async Task<List<Order>> GetCustomerOrdersPerPeriod(Guid customerId, DateTime startTime, DateTime endTime)
        {
            using (var context = new CustomersServiceDbContext())
            {
                return await
                    context.Orders.Where(
                        o => o.CustomerId == customerId && (o.OrderDate >= startTime && o.OrderDate <= endTime))
                        .ToListAsync();
            }
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            using (var context = new CustomersServiceDbContext())
            {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }

            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            using (var context = new CustomersServiceDbContext())
            {
                if (!context.Orders.Local.Any(o => o.Id == order.Id))
                {
                    context.Orders.Attach(order);
                }
                context.Entry(order).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            return order;
        }
    }
}