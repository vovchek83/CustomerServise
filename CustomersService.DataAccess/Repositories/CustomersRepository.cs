using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Threading.Tasks;
using CustomersService.DataModel.Models;
using CustomersService.DataModel.Repositories.Interfaces;


namespace CustomersService.DataModel.Repositories
{
    [Export(typeof(ICustomersRepository))]
    public class CustomersRepository : ICustomersRepository
    {
        public async Task<List<Customer>> GetCustomersAsync()
        {
            using (var context = new CustomersServiceDbContext())
            {
                return await context.Customers.ToListAsync();
            }
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            using (var context = new CustomersServiceDbContext())
            {
                return await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            }
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            bool retValue = true;

            using (var context = new CustomersServiceDbContext())
            {
                context.Customers.Add(customer);
                retValue = await context.SaveChangesAsync() >= 1;
            }

            return retValue;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            using (var context = new CustomersServiceDbContext())
            {
                var cust = await context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);

                if (cust != null)
                {
                    cust.FirstName = customer.FirstName;
                    cust.LastName = customer.LastName;
                    cust.Address = customer.Address;
                    cust.Phone = customer.Phone;
                    cust.Email = customer.Email;
                    context.Entry(cust).State = EntityState.Modified;
                }
                else
                {
                    context.Customers.Attach(customer);
                }

                await context.SaveChangesAsync();
            }

            return customer;
        }
    }
}