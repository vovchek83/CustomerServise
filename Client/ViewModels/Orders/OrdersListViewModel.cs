using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

using CustomersService.Infrastructure.Presentation.MVVM;
using System.Threading.Tasks;
using Client.Mappers;
using System.ComponentModel;
using System.Windows.Data;
using Caliburn.Micro;
using Client.ViewModels.Customers;
using CustomersService.DataModel.Repositories.Interfaces;

namespace Client.ViewModels.Orders
{
    [Export]
    public class OrdersListViewModel : ScreenViewModel
    {

        #region Data Members

        private readonly IOrdersRepository _repo;

        private ObservableCollection<CustomerOrderViewModel> _ordersList;
        private ItemGroup _selectedItemGroup;
        private ICollectionView _ordersView;

        private static readonly ILog Logger = LogManager.GetLog(typeof(OrdersListViewModel));

        #endregion

        #region Properies

        public ObservableCollection<CustomerOrderViewModel> OrdersList
        {
            get { return _ordersList; }
            set
            {
                _ordersList = value;
                NotifyOfPropertyChange(() => OrdersList);
            }
        }

        public ICollectionView OrdersView
        {
            get { return _ordersView; }

            set
            {
                _ordersView = value;

                NotifyOfPropertyChange();
            }
        }

        public List<ItemGroup> ItemsGroup { get; set; }



        public ItemGroup SelectedItemGroup
        {
            get { return _selectedItemGroup; }
            set
            {
                _selectedItemGroup = value;
                NotifyOfPropertyChange();
                GroupOrders();
            }
        }
        #endregion

        #region Constractors/Initial

        [ImportingConstructor]
        public OrdersListViewModel(IOrdersRepository repo)
        {
            _repo = repo;

            InitItemGroup();
        }

        private void InitItemGroup()
        {
            ItemsGroup = new List<ItemGroup>()
            {
                new ItemGroup("No group", ""),
                new ItemGroup("Customer Name", "CustomerName"),
                new ItemGroup("Order Date", "OrderDate"),
                new ItemGroup("Delivery Date", "DeliveryDate"),
                new ItemGroup("Status", "StatusText")
            };
        }

        #endregion

        #region Override Methods

        protected override async void OnActivate()
        {
            await LoadOrders();
            base.OnActivate();
        }

        #endregion

        #region Private Methods

        private async Task LoadOrders()
        {
            try
            {
                IsBusy = true;

                OrdersList = await OrdersMapper.Map(await _repo.GetOrdersAsync());
                OrdersView = CollectionViewSource.GetDefaultView(OrdersList);

                SelectedItemGroup = ItemsGroup[0];
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

        private void GroupOrders()
        {
            try
            {
                OrdersView.GroupDescriptions.Clear();
                if (string.IsNullOrEmpty(SelectedItemGroup.Value) == false)
                {
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription(SelectedItemGroup.Value);

                    OrdersView.GroupDescriptions.Add(groupDescription);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        #endregion

    }

    public class ItemGroup
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ItemGroup(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

}