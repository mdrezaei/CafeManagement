using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class CustomerDeletedEvent
    {
        public Guid CustomerId { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
