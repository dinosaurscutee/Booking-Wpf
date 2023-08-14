using System;
using System.Collections.Generic;

namespace RestaurantBooking.Models
{
    public partial class MenuItem
    {
        public int MenuItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? ItemPrice { get; set; }
    }
}
