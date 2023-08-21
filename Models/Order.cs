using System;
using System.Collections.Generic;

namespace RestaurantBooking.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            Payments = new HashSet<Payment>();
        }

        public int OrderId { get; set; }
        public int? TableId { get; set; }
        public DateTime? OrderTime { get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public decimal? TotalAmount { get; set; }

        public virtual Table? Table { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
