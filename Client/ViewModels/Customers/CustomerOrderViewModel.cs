using System;
using System.Collections.Generic;
using Caliburn.Micro;
using CustomersService.DataModel;
using CustomersService.DataModel.Emums;
using CustomersService.DataModel.Models;
using CustomersService.DataModel.Repositories.Interfaces;


namespace Client.ViewModels.Customers
{
    public class CustomerOrderViewModel
    {

        public long Id
        {
            get { return _order.Id; }
            set { _order.Id = value; }
        }
        public Guid CustomerId
        {
            get { return _order.CustomerId; }
            set { _order.CustomerId = value; }
        }


        public string CustomerName { get; set; }

        public EOrderStatus Status
        {
            get { return _order.Status; }
            set { _order.Status = value; }
        }

        public string StatusText => Status.ToString();

        public DateTime OrderDate
        {
            get { return _order.OrderDate; }
            set { _order.OrderDate = value; }
        }

        public DateTime DeliveryDate
        {
            get { return _order.DeliveryDate; }
            set { _order.DeliveryDate = value; }
        }
        public decimal DeliveryCharge
        {
            get { return _order.DeliveryCharge; }
            set { _order.DeliveryCharge = value; }
        }

        public decimal ItemsTotal
        {
            get { return _order.ItemsTotal; }
            set { _order.ItemsTotal = value; }
        }
        public string Phone
        {
            get { return _order.Phone; }
            set { _order.Phone = value; }
        }

        public string DeliveryAddress
        {
            get { return _order.DeliveryAddress; }
            set { _order.DeliveryAddress = value; }
        }


        private readonly Order _order;

        public CustomerOrderViewModel(Order order)
        {
            _order = order;
            LoadCustomerName();
        }

        private async void LoadCustomerName()
        {
            var customerRepo = IoC.Get<ICustomersRepository>();
            var customer = await customerRepo.GetCustomerAsync(CustomerId);
            CustomerName = customer.FullName;
        }
    }
}