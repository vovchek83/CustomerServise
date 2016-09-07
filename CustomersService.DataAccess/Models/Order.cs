using System;
using CustomersService.DataModel.Emums;

namespace CustomersService.DataModel.Models
{
    public class Order 
    {
      
        public long Id { get; set; }
        public Guid CustomerId { get; set; }
        public EOrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal ItemsTotal { get; set; }
        public string Phone { get; set; }
        public string DeliveryAddress { get; set; }

    }
}