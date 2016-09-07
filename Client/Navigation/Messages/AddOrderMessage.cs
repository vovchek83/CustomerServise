using System;

namespace Client.Navigation.Messages
{
    public class AddOrderMessage
    {
        public Guid CustomerId { get; set; }

        public AddOrderMessage( Guid id)
        {
            CustomerId = id;
        }
    }
}