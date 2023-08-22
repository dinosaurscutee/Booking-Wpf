using RestaurantBooking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace TableManage
{
    public class ViewModel
    {
        private RestaurantBookingContext dbContext;

        public ObservableCollection<Tabless> Tables { get; set; }
        public Tabless SelectedTable { get; set; }

        public ViewModel(RestaurantBookingContext dbContext)
        {
            this.dbContext = dbContext;
            TableTypes = dbContext.Tablesses.Select(table => table.TableType).Distinct().ToList();
            TableTypes.Insert(0, "Tất cả");

            // Tạo danh sách các tầng từ dữ liệu bảng
            Floors = dbContext.Tablesses.Select(table => table.Floors).Distinct().ToList();
            Floors.Insert(0, "Tất cả");
            RefreshTables();
        }

        public void RefreshTables()
        {
            IEnumerable<Tabless> filtered = dbContext.Tablesses; // Lấy tất cả bàn từ DbContext

            // Áp dụng bộ lọc cho TableType (nếu có lựa chọn)
            if (!string.IsNullOrEmpty(SelectedTableType) && SelectedTableType != "Tất cả")
            {
                filtered = filtered.Where(table => table.TableType.Equals(SelectedTableType));
                
            }

            // Áp dụng bộ lọc cho Floors (nếu có lựa chọn)
            if (!string.IsNullOrEmpty(SelectedFloor) && SelectedFloor != "Tất cả")
            {
                filtered = filtered.Where(table => table.Floors.Equals(SelectedFloor));
            }

            Tables = new ObservableCollection<Tabless>(filtered);
        }


        public void UpdateTableStatus(Tabless table, string newStatus)
        {
            table.TableStatus = newStatus;
            dbContext.SaveChanges();
        }
        private List<string> _tableTypes;
        public List<string> TableTypes
        {
            get { return _tableTypes; }
            set
            {
                _tableTypes = value;
                RefreshTables(); // Khi TableTypes thay đổi, làm mới danh sách bàn
            }
        }


        private string _selectedTableType;
        public string SelectedTableType
        {
            get { return _selectedTableType; }
            set
            {
                _selectedTableType = value;
                RefreshTables(); // Khi SelectedTableType thay đổi, làm mới danh sách bàn
            }
        }

        private List<string?> _floors;
        public List<string?> Floors
        {
            get { return _floors; }
            set
            {
                _floors = value;
                RefreshTables(); // Khi Floors thay đổi, làm mới danh sách bàn
            }
        }

        private string _selectedFloor;
        public string SelectedFloor
        {
            get { return _selectedFloor; }
            set
            {
                _selectedFloor = value;
                RefreshTables(); // Khi SelectedFloor thay đổi, làm mới danh sách bàn
            }
        }

        public class RadioButtonFloorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int floorValue = System.Convert.ToInt32(parameter);
                int selectedFloor = (int)value;

                return floorValue == selectedFloor;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    }
}