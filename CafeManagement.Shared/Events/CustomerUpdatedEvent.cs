using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class CustomerUpdatedEvent
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
