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
    /// Interaction logic for IncomeDialog.xaml
    /// </summary>
    public partial class IncomeDialog : Window
    {
        public string IncomeType { get; set; }
        public int IncomeAmount { get; set; }
        public string IncomeDate { get; set; }
        public string IncomeDescription { get; set; }
        public IncomeDialog()
        {
            InitializeComponent();
            IncomeType = string.Empty;
            IncomeAmount = 0;
            IncomeDate = string.Empty;
            IncomeDescription = string.Empty;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(IncomeTypeTextBox.Text))
            {
                MessageBox.Show("Please enter the income's type.");
                return;
            }
            if (string.IsNullOrWhiteSpace(IncomeAmountTextBox.Text))
            {
                MessageBox.Show("Please enter the income's amount.");
                return;
            }
            if (string.IsNullOrWhiteSpace(IncomeDatePicker.Text))
            {
                MessageBox.Show("Please enter the income's date.");
                return;
            }

            // Set the IncomeType and IncomeAmount property with the entered name.
            IncomeType = IncomeTypeTextBox.Text;
            IncomeAmount = int.Parse(IncomeAmountTextBox.Text);
            IncomeDate = DateTime.Parse(IncomeDatePicker.Text).ToString("yyyy-MM-dd");
            IncomeDescription = IncomeDescriptionTextBox.Text;

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
