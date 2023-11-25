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
    /// Interaction logic for AdministrativeEmployeeDialog.xaml
    /// </summary>
    public partial class AdministrativeEmployeeDialog : Window
    {
        public string AdministrativeEmployeeFirstName { get; set; }
        public string AdministrativeEmployeeLastName { get; set; }
        public string AdministrativeEmployeeEmail { get; set; }
        public string AdministrativeEmployeePhone { get; set; }
        public string AdministrativeEmployeeDepartment { get; set; }
        public string AdministrativeEmployeeHireDate { get; set; }
        public int AdministrativeEmployeeSalary { get; set; }
        public bool AdministrativeEmployeeIsFullTime { get; set; }
        public string AdministrativeEmployeeAvailability { get; set; }
        public AdministrativeEmployeeDialog()
        {
            InitializeComponent();
            AdministrativeEmployeeFirstName = string.Empty;
            AdministrativeEmployeeLastName = string.Empty;
            AdministrativeEmployeeEmail = string.Empty;
            AdministrativeEmployeePhone = string.Empty;
            AdministrativeEmployeeDepartment = string.Empty;
            AdministrativeEmployeeHireDate = string.Empty;
            AdministrativeEmployeeSalary = 0;
            AdministrativeEmployeeIsFullTime = false;
            AdministrativeEmployeeAvailability = string.Empty;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(AdministrativeEmployeeFirstNameTextBox.Text))
            {
                MessageBox.Show("Please enter the administrative employee's first name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(AdministrativeEmployeeLastNameTextBox.Text))
            {
                MessageBox.Show("Please enter the administrative employee's last name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(AdministrativeEmployeeHireDateDatePicker.Text))
            {
                MessageBox.Show("Please enter the administrative employee's hire date.");
                return;
            }

            // Save values.
            AdministrativeEmployeeFirstName = AdministrativeEmployeeFirstNameTextBox.Text;
            AdministrativeEmployeeLastName = AdministrativeEmployeeLastNameTextBox.Text;
            AdministrativeEmployeeEmail = AdministrativeEmployeeEmailTextBox.Text;
            AdministrativeEmployeePhone = AdministrativeEmployeePhoneTextBox.Text;
            AdministrativeEmployeeDepartment = AdministrativeEmployeeDepartmentTextBox.Text;
            AdministrativeEmployeeHireDate = AdministrativeEmployeeHireDateDatePicker.Text;
            AdministrativeEmployeeSalary = int.Parse(AdministrativeEmployeeSalaryTextBox.Text);
            AdministrativeEmployeeIsFullTime = AdministrativeEmployeeIsFullTimeCheckBox.IsChecked ?? false;
            AdministrativeEmployeeAvailability = AdministrativeEmployeeAvailabilityTextBox.Text;

            // Close dialog box with OK result.
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
