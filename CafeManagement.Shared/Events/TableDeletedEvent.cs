using System;
using System.Collections.Generic;
using System.Text;

namespace CafeManagement.Shared.Events
{
    public class TableDeletedEvent
    {
        public int TableId { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}
