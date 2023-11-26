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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ExpenseDialog.xaml
    /// </summary>
    public partial class ExpenseDialog : Window
    {
        public string ExpenseType { get; set; }
        public int ExpenseAmount { get; set; }
        public string ExpenseDate { get; set; }
        public string ExpenseDescription { get; set; }
        public ExpenseDialog()
        {
            InitializeComponent();
            ExpenseType = string.Empty;
            ExpenseAmount = 0;
            ExpenseDate = string.Empty;
            ExpenseDescription = string.Empty;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(ExpenseTypeTextBox.Text))
            {
                MessageBox.Show("Please enter the expense's type.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ExpenseAmountTextBox.Text))
            {
                MessageBox.Show("Please enter the expense's amount.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ExpenseDatePicker.Text))
            {
                MessageBox.Show("Please enter the expense's date.");
                return;
            }

            // Set the ExpenseType and ExpenseAmount property with the entered name.
            ExpenseType = ExpenseTypeTextBox.Text;
            ExpenseAmount = int.Parse(ExpenseAmountTextBox.Text);
            ExpenseDate = DateTime.Parse(ExpenseDatePicker.Text).ToString("yyyy-MM-dd");
            ExpenseDescription = ExpenseDescriptionTextBox.Text;

            // Close the dialog window and return to the main window.
            DialogResult = true;
        }

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Use this event handler to allow only numeric input.
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true; // Cancel the input if it's not a valid integer.
            }
        }
    }
}
