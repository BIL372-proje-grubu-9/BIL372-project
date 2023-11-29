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
    /// Interaction logic for Janitors.xaml
    /// </summary>
    public partial class JanitorDialog : Window
    {
        public string JanitorFirstName { get; set; }
        public string JanitorLastName { get; set; }
        public string JanitorEmail { get; set; }
        public string JanitorPhone { get; set; }
        public string JanitorHireDate { get; set; }
        public int JanitorSalary { get; set; }
        public bool JanitorIsFullTime { get; set; }
        public string JanitorAvailability { get; set; }
        public JanitorDialog()
        {
            InitializeComponent();
            JanitorFirstName = string.Empty;
            JanitorLastName = string.Empty;
            JanitorEmail = string.Empty;
            JanitorPhone = string.Empty;
            JanitorHireDate = string.Empty;
            JanitorSalary = 0;
            JanitorIsFullTime = false;
            JanitorAvailability = string.Empty;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(JanitorFirstNameTextBox.Text))
            {
                MessageBox.Show("Please enter the Janitor's first name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(JanitorLastNameTextBox.Text))
            {
                MessageBox.Show("Please enter the Janitor's last name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(JanitorHireDateDatePicker.Text))
            {
                MessageBox.Show("Please enter the Janitor's hire date.");
                return;
            }

            // Set the JanitorName and JanitorAge property with the entered name.
            JanitorFirstName = JanitorFirstNameTextBox.Text;
            JanitorLastName = JanitorLastNameTextBox.Text;
            JanitorEmail = JanitorEmailTextBox.Text;
            JanitorPhone = JanitorPhoneTextBox.Text;
            JanitorHireDate = DateTime.Parse(JanitorHireDateDatePicker.Text).ToString("yyyy-MM-dd");
            JanitorSalary = int.Parse(JanitorSalaryTextBox.Text);
            JanitorIsFullTime = JanitorIsFullTimeCheckBox.IsChecked ?? false;
            JanitorAvailability = JanitorAvailabilityTextBox.Text;

            // Close the dialog box and return true.
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
