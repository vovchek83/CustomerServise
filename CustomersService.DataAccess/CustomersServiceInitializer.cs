using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using CustomersService.DataModel.Emums;
using CustomersService.DataModel.Models;

namespace CustomersService.DataModel
{
    internal class CustomersServiceInitializer : DropCreateDatabaseIfModelChanges<CustomersServiceDbContext>

    {
        protected override void Seed(CustomersServiceDbContext context)
        {
            var users = new List<User>
            {
                new User() { Name="Admin" ,Password="Admin"}
            };

            users.ForEach(s => context.Users.Add(s));

            context.SaveChanges();

            var customers = GenerateCustomers();

            customers.ForEach(s => context.Customers.Add(s));
            context.SaveChanges();
        }


        private List<Customer> GenerateCustomers()
        {
            var customers = new List<Customer>();

            customers.Add(new Customer()
            {
                Id = Guid.Parse("7462c7c8-e24c-484a-8f93-013f1c479615"),
                FirstName = "Derek",
                LastName = "Puckett",
                Phone = "(954) 594-9355",
                Address = "P.O. Box 914, 9990 Dapibus St. Quam OH 55154",
                Email = "derek.puckett@vulputate.net",
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        Id = 1,
                        CustomerId = Guid.Parse("7462c7c8-e24c-484a-8f93-013f1c479615"),
                        Status = EOrderStatus.Inprocess,
                        ItemsTotal = 54,
                        DeliveryCharge = 31.5M,
                        DeliveryDate = DateTime.Now,
                        OrderDate = DateTime.Now.AddDays(-6)
                    },
                    new Order()
                    {
                        Id = 2,
                        CustomerId = Guid.Parse("7462c7c8-e24c-484a-8f93-013f1c479615"),
                        Status = EOrderStatus.Done,
                        ItemsTotal = 23,
                        DeliveryCharge = 211.5M,
                        DeliveryDate = DateTime.Now,
                        OrderDate = DateTime.Now.AddDays(-21)
                    }
                }
            });

            customers.Add(new Customer()
            {
                Id = Guid.Parse("8f64b183-a881-42f5-9c1d-013f1c479616"),
                FirstName = "Bernard",
                LastName = "Russell",
                Phone = "(203) 652-0465",
                Address = "324-6843 Dolor Ave QuisFL 28034",
                Email = "bernard.russell@torquentper.com",
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        Id = 3,
                        CustomerId = Guid.Parse("8f64b183-a881-42f5-9c1d-013f1c479616"),
                        Status = EOrderStatus.Inprocess,
                        ItemsTotal = 21,
                        DeliveryCharge = 11.5M,
                        DeliveryDate = DateTime.Now,
                        OrderDate = DateTime.Now.AddDays(-45)
                    },
                    new Order()
                    {
                        Id = 4,
                        CustomerId = Guid.Parse("8f64b183-a881-42f5-9c1d-013f1c479616"),
                        ItemsTotal = 23,
                        Status = EOrderStatus.Done,
                        DeliveryCharge = 243.5M,
                        DeliveryDate = DateTime.Now,
                        OrderDate = DateTime.Now.AddDays(-6)
                    }
                }
            });

            return customers;
        }

    }
}

