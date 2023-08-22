using RestaurantBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantBooking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string errormsg = "";
        public Login()
        {
            InitializeComponent();
        }


        private bool IsVaild(string username, string password)
        {
            if (username == "" || password == "")
            {
                if(username == "")
                {
                    errormsg = "Username must not empty!";
                } else if (password == "")
                {
                    errormsg = "Password must not empty!";
                } return false;
            }
            return true;
        }

        private void login(string username, string password)
        {
            try
            {
                using (var db = new RestaurantBookingContext())
                {
                    var listAccounts = db.Users.Select(a => new
                    {
                        UserId = a.UserId,
                        UserName = a.Username,
                        Password = a.Password,
                        FullName = a.FullName,
                        UserRole = a.UserRole
                    }).ToList();
                        var acc = listAccounts.FirstOrDefault(e => username.Equals(e.UserName) && password.Equals(e.Password));
                        if (acc != null)
                        {
                            acc = listAccounts.FirstOrDefault(e => username.Equals(e.UserName) && e.UserRole.Equals("1"));
                            if (acc != null)
                            {
                                MessageBox.Show("Login successfully");
                            }
                            else
                            {
                                MessageBox.Show("Deny permission to access");
                            }
                        }
                        else
                        {

                        if (IsVaild(username, password) == true)
                        {
                            MessageBox.Show("Login failed, worng login username or password!");
                        } else
                        {
                            MessageBox.Show(errormsg);
                        }
                    }            
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }            
        }
        private void btnlogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            login(username, password);
        }

        private void btnregister_Click(object sender, RoutedEventArgs e)
        {
            UserRegister userRegister = new UserRegister();
            userRegister.Show();
        }
    }
}
