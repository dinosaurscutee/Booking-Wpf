using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using RestaurantBooking.Models;
using TableManage;

namespace RestaurantBooking
{
    public partial class Home : Window
    {
        private readonly RestaurantBookingContext _context;
        private List<Models.MenuItem> menuItemsList;

        private ViewModel viewModel;

        public Home()
        {
            InitializeComponent();
            _context = new RestaurantBookingContext();
            viewModel = new ViewModel(_context);
            DataContext = viewModel;
            LoadMenuItems();
        }


            //load menu tu db
        private void LoadMenuItems()
        {

            menuItemsList = _context.MenuItems.ToList();

           
            foreach (var menuItem in menuItemsList)
            {
                menuItem.Quantity = 0;
            }

            foodListView.ItemsSource = menuItemsList;
        }

        //add food vào bảng order
        private void SelectFood_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTable != null && viewModel.SelectedTable.TableStatus == "Pending")
            {
                List<Models.MenuItem> selectedItems = menuItemsList.Where(item => item.IsSelected).ToList();

                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một món ăn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool hasZeroQuantity = selectedItems.Any(item => item.Quantity == 0);
                if (hasZeroQuantity)
                {
                    MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0 cho các món ăn đã chọn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Tạo bản ghi Order mới
                        Order newOrder = new Order
                        {
                            OrderTime = DateTime.Now,
                            Status = "Pending",
                            PaymentStatus = "Unpaid",
                            TotalAmount = selectedItems.Sum(item => item.ItemPrice * item.Quantity),
                            TableId = viewModel.SelectedTable.TableId // Thêm TableId vào đơn hàng
                        };
                        _context.Orders.Add(newOrder);
                        _context.SaveChanges();

                        // Lấy OrderId mới
                        int newOrderId = newOrder.OrderId;

                        // Thêm các món ăn đã chọn vào bảng OrderItem
                        foreach (var selectedItem in selectedItems)
                        {
                            decimal itemPrice = selectedItem.ItemPrice ?? 0;
                            int quantity = selectedItem.Quantity ?? 1;

                            if (quantity > 0)
                            {
                                OrderItem newOrderItem = new OrderItem
                                {
                                    OrderId = newOrderId,
                                    MenuItemId = selectedItem.MenuItemId,
                                    ItemPrice = itemPrice,
                                    Quantity = quantity
                                };
                                _context.OrderItems.Add(newOrderItem);
                            }
                        }

                        // Cập nhật trạng thái bàn thành "Occupied" sau khi đã đặt món
                        viewModel.UpdateTableStatus(viewModel.SelectedTable, "Occupied");
                        _context.SaveChanges();

                        transaction.Commit();

                        MessageBox.Show("Đã thêm các món vào đơn hàng và cập nhật trạng thái bàn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Bàn cần ở trạng thái 'Pending' để có thể đặt món ăn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void SetTableButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTable != null)
            {
                if (viewModel.SelectedTable.TableStatus == "Available")
                {
                    string inputCode = PromptForInput("Nhập mã bàn:");

                    if (inputCode == viewModel.SelectedTable.TableCode)
                    {
                        viewModel.UpdateTableStatus(viewModel.SelectedTable, "Pending");
                        MessageBox.Show("Bàn đã được đặt thành công.");
                        RefreshTableDataContext();
                    }
                    else
                    {
                        MessageBox.Show("Mã bàn không đúng.");
                    }
                }
                else
                {
                    MessageBox.Show("Bàn đã có người đặt.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bàn trước.");
            }
        }


        private void CancelTableButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTable != null)
            {
                if (viewModel.SelectedTable.TableStatus == "Occupied")
                {
                    viewModel.UpdateTableStatus(viewModel.SelectedTable, "Available");
                    MessageBox.Show("Hủy đặt bàn thành công.");
                    RefreshTableDataContext();
                }
                else
                {
                    MessageBox.Show("Bàn không thể hủy đặt.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bàn trước.");
            }
        }
        private string PromptForInput(string prompt)
        {
            InputDialog inputDialog = new InputDialog(prompt);
            if (inputDialog.ShowDialog() == true)
            {
                return inputDialog.InputValue;
            }
            return null;
        }
        private void RefreshTableDataContext()
        {
            ListBoxTables.DataContext = null;
            ListBoxTables.DataContext = viewModel;
        }

        private void TableTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.RefreshTables();
            RefreshTableDataContext();

        }

        private void FloorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.RefreshTables();
            RefreshTableDataContext();
        }

    }
}
