using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class OrderStatusChangedEvent
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
