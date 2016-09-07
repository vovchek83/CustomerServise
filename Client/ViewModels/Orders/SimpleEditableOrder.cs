using System;
using System.ComponentModel.DataAnnotations;
using CustomersService.DataModel;
using CustomersService.DataModel.Emums;
using CustomersService.Infrastructure.Presentation.MVVM;

namespace Client.ViewModels.Orders
{
    public class SimpleEditableOrder : ValidatableBindableBase
    {
        private EOrderStatus _status;
        private decimal _itemsTotal;
        private string _phone;
        private decimal _deliveryCharge;
        private string _deliveryAddress;

        public long Id { get; set; }
        public Guid CustomerId { get; set; }

        [Required]
        public decimal ItemsTotal
        {
            get { return _itemsTotal; }
            set { SetProperty(ref _itemsTotal, value); }
        }

        public EOrderStatus Status
        {
            get { return _status; }
            set { SetProperty(ref _status , value); }
        }

        public DateTime OrderDate { get; set; }

        public DateTime DeliveryDate { get; set; }

        
        public decimal DeliveryCharge
        {
            get { return _deliveryCharge; }
            set { SetProperty(ref _deliveryCharge, value); }
        }

        [Phone]
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        public string DeliveryAddress
        {
            get { return _deliveryAddress; }
            set
            {
                _deliveryAddress = value;
                NotifyOfPropertyChange();
            }
        }
    }
}