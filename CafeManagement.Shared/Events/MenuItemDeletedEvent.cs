using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class MenuItemDeletedEvent
    {
        public Guid MenuItemId { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
