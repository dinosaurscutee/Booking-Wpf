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

namespace TableManage
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public string InputValue { get; private set; }
        public string Prompt { get; }

        public InputDialog(string prompt)
        {
            InitializeComponent();
            Prompt = prompt;
            DataContext = this;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue = InputTextBox.Text;
            DialogResult = true;
        }
    }
}
