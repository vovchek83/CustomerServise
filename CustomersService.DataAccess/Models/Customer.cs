using System;
using System.Collections.Generic;

namespace CustomersService.DataModel.Models
{  
    public class Customer 
    {
        public Customer()
        {
            Orders = new List<Order>();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<Order> Orders { get; set; }
    }
}