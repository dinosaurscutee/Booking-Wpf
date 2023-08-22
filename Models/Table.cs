using System;
using System.Collections.Generic;

namespace RestaurantBooking.Models
{
    public partial class Table
    {
        public Table()
        {
            Orders = new HashSet<Order>();
        }

        public int TableId { get; set; }
        public string TableName { get; set; }
        public string TableType { get; set; }
        public string TableStatus { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
