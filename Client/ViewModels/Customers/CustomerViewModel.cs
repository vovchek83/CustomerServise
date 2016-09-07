using CustomersService.Infrastructure.Presentation.MVVM;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CustomersService.DataModel.Models;

namespace Client.ViewModels.Customers
{
    public class CustomerViewModel : ScreenViewModel
    {
        private ObservableCollection<CustomerOrderViewModel> _orderList;


        #region Properties
        public Guid Id
        {
            get { return _customer.Id; }
            set { _customer.Id = value; }
        }

        public string FirstName
        {
            get { return _customer.FirstName; }
            set { _customer.FirstName = value; }
        }

        [Required]
        public string LastName
        {
            get { return _customer.LastName; }
            set { _customer.LastName = value; }
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        
        public string Phone
        {
            get { return _customer.Phone; }
            set { _customer.Phone = value; }
        }

        public string Email
        {
            get { return _customer.Email; }
            set { _customer.Email = value; }
        }
        public string Address
        {
            get { return _customer.Address; }
            set { _customer.Address = value; }
        }

        public ObservableCollection<CustomerOrderViewModel> OrdersList
        {
            get { return _orderList; }
            set
            {
                _orderList = value;
                NotifyOfPropertyChange(() => OrdersList);
            }
        }

        #endregion

        private readonly Customer _customer;

        public CustomerViewModel(Customer customer)
        {
            _customer = customer;
        }

        public Customer GetCustomer()
        {
            return _customer;
        }
    }
}