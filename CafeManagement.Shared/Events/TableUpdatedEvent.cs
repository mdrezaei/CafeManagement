using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class TableUpdatedEvent
    {
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
