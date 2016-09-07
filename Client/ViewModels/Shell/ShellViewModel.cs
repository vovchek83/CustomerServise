using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Client.Navigation;
using Client.Navigation.Messages;
using Client.ViewModels.Customers;
using Client.ViewModels.Orders;

namespace Client.ViewModels.Shell
{
    public interface IShellViewModel :
        IHandle<CustomerSaveMessage>,  //handle CustomerSaved event
        IHandle<AddEditCustomerMessage>,
        IHandle<AddOrderMessage>,
        IHandle<OrderSavedMessage>
    {

    }

    [Export(typeof(IShellViewModel))]
    public class ShellViewModel :
        Conductor<IScreen>.Collection.OneActive, //-->only one screen is activated inside the shell
        IShellViewModel


    {
        #region Data Members

        private readonly CustomersViewModel _customerListVM;
        private readonly AddEditCustomerViewModel _addEditCustomerVM;
        private readonly AddNewOrderViewModel _addNewOrderVM;

        private NavigationState _navigationState;

        private readonly Dictionary<NavigationState, Action<NavigationState>> _navigationStates;

        #endregion

        #region Properties

        public override string DisplayName { get; set; } = "Customer Service";


        public NavigationState NavigationState
        {
            get { return _navigationState; }
            private set
            {
                if (value == _navigationState) return;

                _navigationState = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Constractor

        [ImportingConstructor]
        public ShellViewModel(
            CustomersViewModel customerListVM,
            OrdersListViewModel ordersListVM,
            AddEditCustomerViewModel addEditCustomerVM,
            AddNewOrderViewModel addNewOrderVM,
            IEventAggregator eventAggregator)
        {

            _navigationStates = new Dictionary<NavigationState, Action<NavigationState>>
            {
                [NavigationState.Search] = x => InternalNavigateTo(x, customerListVM),
                [NavigationState.Orders] = x => InternalNavigateTo(x, ordersListVM)
            };

            _customerListVM = customerListVM;
            _addEditCustomerVM = addEditCustomerVM;
            _addNewOrderVM = addNewOrderVM;

            eventAggregator.Subscribe(this);
        }

        #endregion

        #region Nvigations

        protected override void OnActivate()
        {
            NavigateTo(NavigationState.Search);
        }

        public void NavigateTo(NavigationState state)
        {
            InternalNavigateTo(state);
        }


        private bool InternalNavigateTo(NavigationState state)
        {
            Action<NavigationState> navAction;
            if (!_navigationStates.TryGetValue(state, out navAction))
                return false;

            navAction(state);
            return true;
        }

        private void InternalNavigateTo(NavigationState state, IScreen screen)
        {
            if (NavigationState == state) return;

            NavigationState = state;
            ActivateItem(screen);

        }

        #endregion

        #region Events Handles

        /// <summary>
        /// Handles the CustomerSave message.
        /// After save we display all customers
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(CustomerSaveMessage message)
        {
            ActivateItem(_customerListVM);
        }

        /// <summary>
        /// Handles the OrderSaved message.
        /// After save we display all customers
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(OrderSavedMessage message)
        {
            ActivateItem(_customerListVM);
        }

        /// <summary>
        /// Handles the AddOrder message.
        /// Navigate to AddNewOrderView.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(AddOrderMessage message)
        {
            _addNewOrderVM.CustomerId = message.CustomerId;
            ActivateItem(_addNewOrderVM);
        }

        /// <summary>
        /// Handles the AddEditCustomer message.
        /// Navigate to AddEditCustomerView.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(AddEditCustomerMessage message)
        {
            _addEditCustomerVM.EditMode = message.IsEditMode;
            _addEditCustomerVM.SetCustomer(message.Customer);
            ActivateItem(_addEditCustomerVM);
        }

        #endregion
    }
}