using System;
using System.Collections.Generic;

namespace RestaurantBooking.Models
{
    public partial class Tabless
    {
        public Tabless()
        {
            Orders = new HashSet<Order>();
        }

        public int TableId { get; set; }
        public string? TableCode { get; set; }
        public string? TableName { get; set; }
        public string? Floors { get; set; }
        public string? TableType { get; set; }
        public string? TableStatus { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
