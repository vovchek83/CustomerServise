using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Client.Navigation.Messages;
using CustomersService.DataModel.Models;
using CustomersService.DataModel.Repositories.Interfaces;
using CustomersService.Infrastructure.Presentation.MVVM;

namespace Client.ViewModels.Customers
{
    [Export]
    public class AddEditCustomerViewModel : ScreenViewModel
    {
        #region Data Members

        private readonly IEventAggregator _eventAggregator;
        private readonly ICustomersRepository _repo;
        private Customer _editingCustomer = null;
        private SimpleEditableCustomer _customer;
        private bool _editMode;

        private static readonly ILog Logger = LogManager.GetLog(typeof(AddEditCustomerViewModel));

        #endregion

        #region Properties

        public SimpleEditableCustomer Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                NotifyOfPropertyChange();
            }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                NotifyOfPropertyChange();
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        #endregion

        #region Constractos/Initialization

        [ImportingConstructor]
        public AddEditCustomerViewModel(IEventAggregator eventAggregator, ICustomersRepository repo)
        {
            _eventAggregator = eventAggregator;
            _repo = repo;
            _editingCustomer = new Customer();
            WireCommands();
        }

        private void WireCommands()
        {
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
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
                UpdateCustomer(Customer, _editingCustomer);
                if (EditMode)
                    await _repo.UpdateCustomerAsync(_editingCustomer);
                else
                    await _repo.AddCustomerAsync(_editingCustomer);

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            Done();
        }

        private bool CanSave()
        {
            return !Customer.HasErrors && (!string.IsNullOrEmpty(Customer.FirstName) && !string.IsNullOrEmpty(Customer.LastName));
        }

        #endregion

        #region Events Handlers

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Public Methods

        public void SetCustomer(Customer cust)
        {
            try
            {
                _editingCustomer = cust;
                if (Customer != null) Customer.ErrorsChanged -= RaiseCanExecuteChanged;
                Customer = new SimpleEditableCustomer();
                Customer.ErrorsChanged += RaiseCanExecuteChanged;
                CopyCustomer(cust, Customer);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        #endregion 

        #region Private Methods

        private void Done()
        {
            _eventAggregator.PublishOnUIThread(new CustomerSaveMessage());
        }

        private void CopyCustomer(Customer source, SimpleEditableCustomer target)
        {
            target.Id = source.Id;
            if (EditMode)
            {
                target.FirstName = source.FirstName;
                target.LastName = source.LastName;
                target.Phone = source.Phone;
                target.Email = source.Email;
                target.Address = source.Address;
            }
        }

        private void UpdateCustomer(SimpleEditableCustomer source, Customer target)
        {
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Phone = source.Phone;
            target.Email = source.Email;
            target.Address = source.Address;
        }

        private void ClearFields()
        {
            _editingCustomer.FirstName = string.Empty;
            _editingCustomer.LastName = string.Empty;
            _editingCustomer.Phone = string.Empty;
            _editingCustomer.Email = string.Empty;
            _editingCustomer.Address = string.Empty;
        }

        #endregion

    }
}