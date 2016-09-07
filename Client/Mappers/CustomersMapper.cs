using Client.ViewModels.Customers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CustomersService.DataModel.Models;

namespace Client.Mappers
{

    public static class CustomersMapper
    {
        public static async Task<ObservableCollection<CustomerViewModel>> Map(IEnumerable<Customer> source)
        {
            return await Task.Factory.StartNew(() =>
            {
                return new ObservableCollection<CustomerViewModel>(source.Select(customer => new CustomerViewModel(customer)));
            });
        }
    }
}
