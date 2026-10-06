using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class OrderPlacedEvent
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public int TableId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderPlacedItem> Items { get; set; } = new List<OrderPlacedItem>();
    }
    public class OrderPlacedItem
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
