using RestaurantBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RestaurantBooking
{
    /// <summary>
    /// Interaction logic for UserRegister.xaml
    /// </summary>
    public partial class UserRegister : Window
    {
        public UserRegister()
        {
            InitializeComponent();
        }

        private void userRegister()
        {
            try
            {
                using (var db = new RestaurantBookingContext())
                {
                    var userNew = new User
                    {
                        Username = TxtUsername.Text,
                        Password = TxtPassword.Password.ToString(),
                        FullName = TxtUsername.Text,
                        UserRole = "1",
                    };
                    db.Users.Add(userNew);
                    if (db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add New user(s) success.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            userRegister();
        }
    }
}
