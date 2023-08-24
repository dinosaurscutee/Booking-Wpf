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
    /// Interaction logic for RegisterForm.xaml
    /// </summary>
    public partial class RegisterForm : Window
    {
        private string errormsg = "";

        public RegisterForm()
        {
            InitializeComponent();
        }

        private bool IsVaild(string username, string password,string repassword,string fullname)
        {
            if (username == "" || password == "" || repassword == "" || fullname == "" || !repassword.Equals(password) || !password.Equals(repassword))
            {
                if (username == "")
                {
                    errormsg = "Username must not empty!";
                }
                else if (password == "")
                {
                    errormsg = "Password must not empty!";
                } else if (repassword == "")
                {
                    errormsg = "Re-enter Password must not empty!";
                } else if (fullname == "")
                {
                    errormsg = "Fullname must not empty!";
                } else if (!repassword.Equals(password))
                {
                    errormsg = "The re-enter password isn't match with password!";
                }
                else if (!password.Equals(repassword))
                {
                    errormsg = "The password isn't match with re-enter password!";
                }



                return false;
            }
            return true;
        }

        private void registerNewAccount(string username, string password, string repassword, string fullname)
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
                    var newuser = listAccounts.FirstOrDefault(e => username.Equals(e.UserName) || fullname.Equals(e.FullName));
                    if (newuser != null)
                    {
                        newuser = listAccounts.FirstOrDefault(e => username.Equals(e.UserName));
                        if (newuser != null)
                        {
                            MessageBox.Show("Username is exist");
                        }
                        newuser = listAccounts.FirstOrDefault(e => fullname.Equals(e.FullName));
                        if (newuser != null)
                        {
                            MessageBox.Show("Fullname is exist");
                        }
                    } else
                    {
                        if (IsVaild(username, password, repassword,fullname) == true)
                        {
                            try
                            {
                                    var userNew = new User
                                    {
                                        Username = TxtUsername.Text,
                                        Password = txtRePassword.Password.ToString(),
                                        FullName = TxtUsername.Text,
                                        UserRole = "Admin",
                                    };
                                    db.Users.Add(userNew);
                                    if (db.SaveChanges() > 0)
                                    {
                                        MessageBox.Show("Add New user success.");
                                    this.Close();
                                    }
                                }
                            
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show(errormsg);
                        }
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
            private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text;
            string password = txtPassword.Password.ToString();
            string repassword = txtRePassword.Password.ToString();
            string fullname = TxtFullname.Text;
            registerNewAccount(username, password, repassword, fullname);
        }
    }
}
