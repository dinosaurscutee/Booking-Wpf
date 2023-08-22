using System;
using System.Collections.Generic;

namespace RestaurantBooking.Models
{
    public partial class User
    {

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string UserRole { get; set; }
    }
}
