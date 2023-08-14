using System;
using System.Collections.Generic;

namespace RestaurantBooking.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? UserRole { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
