using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Client.Navigation.Messages;
using CustomersService.DataModel.Emums;
using CustomersService.DataModel.Models;
using CustomersService.DataModel.Repositories.Interfaces;
using CustomersService.Infrastructure.Presentation.MVVM;


namespace Client.ViewModels.Orders
{
    [Export]
    public class AddNewOrderViewModel : ScreenViewModel
    {
        #region Data Members

        private readonly IEventAggregator _eventAggregator;
        private readonly IOrdersRepository _repo;
        private Order _editingOrder;

        private static readonly ILog Logger = LogManager.GetLog(typeof(AddNewOrderViewModel));

        #endregion

        #region Properties

        public Guid CustomerId { get; set; }
        public SimpleEditableOrder Order { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        #endregion

        #region Constructors/Initials

        [ImportingConstructor]
        public AddNewOrderViewModel(IEventAggregator eventAggregator, IOrdersRepository repo)
        {
            _eventAggregator = eventAggregator;
            _repo = repo;
            WireCommands();
        }

        private void WireCommands()
        {
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Activate

        protected override void OnActivate()
        {
            _editingOrder = new Order();
            Order = new SimpleEditableOrder();
        }

        #endregion

        #region Commands Handlers

        private void OnCancel()
        {
            Done();
        }

        private async void OnSave()
        {
            try
            {
                UpdateCustomer(Order, _editingOrder);

                await _repo.AddOrderAsync(_editingOrder);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            Done();
        }

        private bool CanSave()
        {
            return !Order.HasErrors;
        }

        #endregion

        #region Private Methods

        private void Done()
        {
            _eventAggregator.PublishOnUIThread(new OrderSavedMessage());
        }

        private void UpdateCustomer(SimpleEditableOrder source, Order target)
        {
            target.CustomerId = CustomerId;
            target.Status = EOrderStatus.Preparing;
            target.OrderDate = DateTime.Now;
            target.DeliveryDate = DateTime.MaxValue;
            target.DeliveryAddress = source.DeliveryAddress;
            target.DeliveryCharge = source.DeliveryCharge;
            target.ItemsTotal = source.ItemsTotal;
            target.Phone = source.Phone;
        }

        #endregion

    }
}