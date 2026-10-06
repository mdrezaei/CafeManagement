using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class TableCreatedEvent
    {
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
