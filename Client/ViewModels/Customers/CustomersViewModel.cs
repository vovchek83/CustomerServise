using System;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.Windows.Data;
using Client.Mappers;
using CustomersService.Infrastructure.Presentation.MVVM;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Client.Navigation.Messages;
using CustomersService.DataModel.Models;
using CustomersService.DataModel.Repositories.Interfaces;


namespace Client.ViewModels.Customers
{

    /// <summary>
    /// Represents the Customers View
    /// </summary>
    [Export]
    public class CustomersViewModel : ScreenViewModel
    {

        #region Data Member

        private readonly ICustomersRepository _customerRepo;
        private readonly IOrdersRepository _orderRepository;
        private readonly IEventAggregator _eventAggregator;

        private string _searchInput;
        private ICollectionView _customersView;
        private CustomerViewModel _selectedCustomer;

        private static readonly ILog Logger = LogManager.GetLog(typeof(CustomersViewModel));

        #endregion

        #region Properties

        public string SearchInput
        {
            get { return _searchInput; }
            set
            {
                _searchInput = value;
                FilterCustomers(_searchInput);
            }
        }

        private ObservableCollection<CustomerViewModel> _customerList;

        public ObservableCollection<CustomerViewModel> CustomerList
        {
            get { return _customerList; }
            set
            {
                _customerList = value;
                NotifyOfPropertyChange();
            }
        }

        public ICollectionView CustomersView
        {
            get
            {
                return _customersView;
            }

            set
            {
                _customersView = value;

                NotifyOfPropertyChange();
            }
        }

        public CustomerViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                    NotifyOfPropertyChange(() => SelectedCustomer);
                    LoadCustomersOrders();
                    RiseCanExecute();
                }
            }
        }
        private int _selctedIndex = 0;

        public int SelectedIndex
        {
            get { return _selctedIndex; }
            set
            {
                if (value >= 0)
                {
                    _selctedIndex = value;
                }
            }
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
        #endregion

        #region Constructors

        [ImportingConstructor]
        public CustomersViewModel(IEventAggregator eventAggregator, ICustomersRepository customerRepo, IOrdersRepository ordersRepository)
        {
            _eventAggregator = eventAggregator;
            _customerRepo = customerRepo;
            _orderRepository = ordersRepository;

            FromDate = DateTime.Now.AddDays(-7);
            ToDate = DateTime.Now;

        }

        protected async override void OnActivate()
        {
            await LoadCustomers();
        }

        #endregion

        #region Private Methods

        private async Task LoadCustomers()
        {
            IsBusy = true;
            try
            {
                var list = await _customerRepo.GetCustomersAsync();
                CustomerList = await CustomersMapper.Map(list);

                CustomersView = CollectionViewSource.GetDefaultView(CustomerList);

                if (CustomerList.Count >= SelectedIndex)
                    SelectedCustomer = CustomerList[SelectedIndex];
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void FilterCustomers(string searchInput)
        {
            if (string.IsNullOrEmpty(searchInput))
            {
                CustomersView.Filter = null;
            }
            else
            {
                CustomersView.Filter = customerItem =>
                {
                    CustomerViewModel vitem = customerItem as CustomerViewModel;
                    return (vitem != null) && (vitem.FullName.ToLower().Contains(searchInput.ToLower()));
                };
            }
        }

        private void RiseCanExecute()
        {
            NotifyOfPropertyChange("CanPlaceOrder");
            NotifyOfPropertyChange("CanAddNew");
            NotifyOfPropertyChange("CanEditCustomer");
            NotifyOfPropertyChange("CanApplyOrderFilter");
        }

        #endregion

        #region Commands Methods

        private async void LoadCustomersOrders()
        {
            try
            {
                if (SelectedCustomer != null)
                {
                    var orders = await _orderRepository.GetOrdersForCustomersAsync(SelectedCustomer.Id);
                    SelectedCustomer.OrdersList = await OrdersMapper.Map(orders);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public async void LoadOrders(DateTime fromDate, DateTime toDate)
        {

            var filteredOrders = await _orderRepository.GetCustomerOrdersPerPeriod(SelectedCustomer.Id, fromDate, toDate);

            SelectedCustomer.OrdersList = await OrdersMapper.Map(filteredOrders);

        }

        public void ApplyOrderFilter()
        {
            LoadOrders(FromDate, ToDate);
        }

        public bool CanApplyOrderFilter => SelectedCustomer != null;

        public void EditCustomer()
        {
            try
            {
                _eventAggregator.PublishOnUIThread(new AddEditCustomerMessage(this.SelectedCustomer.GetCustomer(), true));
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public bool CanEditCustomer => SelectedCustomer != null;

        public void PlaceOrder()
        {
            _eventAggregator.PublishOnUIThread(new AddOrderMessage(SelectedCustomer.Id));
        }

        public bool CanPlaceOrder => SelectedCustomer != null;

        public void AddNew()
        {
            Customer customer = new Customer()
            {
                Id = Guid.NewGuid(),
            };

            try
            {
                _eventAggregator.PublishOnUIThread(new AddEditCustomerMessage(customer, false));
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public bool CanAddNew => SelectedCustomer != null;

        #endregion

    }
}
