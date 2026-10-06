using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class MenuItemUpdatedEvent
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
