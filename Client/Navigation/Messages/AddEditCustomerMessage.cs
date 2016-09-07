using CustomersService.DataModel.Models;

namespace Client.Navigation.Messages
{
    public class AddEditCustomerMessage
    {

        public Customer Customer { get; private set; }
        public bool IsEditMode { get; private set; }

        public AddEditCustomerMessage(Customer customer, bool isEditMode)
        {
            Customer = customer;
            IsEditMode = isEditMode;
        }
    }
}