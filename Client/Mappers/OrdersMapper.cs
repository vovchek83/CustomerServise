using Client.ViewModels.Customers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CustomersService.DataModel.Models;

namespace Client.Mappers
{
    public static class OrdersMapper
    {
       
        public static async Task<ObservableCollection<CustomerOrderViewModel>> Map(IEnumerable<Order> source)
        {
            return await Task.Factory.StartNew(() =>
            {
                return new ObservableCollection<CustomerOrderViewModel>(source.Select(order => new CustomerOrderViewModel(order))); 
            });
        }
    }
}
