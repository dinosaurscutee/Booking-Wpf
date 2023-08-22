using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantBooking.Models
{
    public partial class MenuItem
    {
        public MenuItem()
        {

            OrderItems = new HashSet<OrderItem>();
        }


        public int MenuItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public int? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        
    }
}
