using Microsoft.Data.SqlClient.Server;
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
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private string errormsg = "";
        public LoginForm()
        {
            InitializeComponent();
        }

        private bool IsVaild(string username, string password)
        {
            if (username == "" || password == "")
            {
                if (username == "")
                {
                    errormsg = "Username must not empty!";
                }
                else if (password == "")
                {
                    errormsg = "Password must not empty!";
                }
                return false;
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
                        acc = listAccounts.FirstOrDefault(e => username.Equals(e.UserName) && e.UserRole.Equals("Admin"));
                        if (acc != null)
                        {
                            this.Hide();
                            var mainWindow = new MainWindow();
                            mainWindow.Closed += (s, args) => this.Close();
                            mainWindow.Show();
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
                        }
                        else
                        {
                            MessageBox.Show(errormsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text;
            string password = txtPassword.Password;
            login(username, password);
        }

        private void registration_button_Click(object sender, RoutedEventArgs e)
        {
            var resgisterForm = new RegisterForm();
            resgisterForm.Show();
        }
    }
}
