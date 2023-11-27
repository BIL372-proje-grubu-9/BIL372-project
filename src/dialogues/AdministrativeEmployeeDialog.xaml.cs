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

        // Add more validation if needed
        private bool IsInputValid()
        {
			if (string.IsNullOrWhiteSpace(AdministrativeEmployeeFirstNameTextBox.Text))
			{
				MessageBox.Show("Please enter the administrative employee's first name.");
				return false;
			}

			if (string.IsNullOrWhiteSpace(AdministrativeEmployeeLastNameTextBox.Text))
			{
				MessageBox.Show("Please enter the administrative employee's last name.");
				return false;
			}

			if (string.IsNullOrWhiteSpace(AdministrativeEmployeeHireDateDatePicker.Text))
			{
				MessageBox.Show("Please enter the administrative employee's hire date.");
				return false;
			}

            return true;
		}

        private void SaveInputValues()
        {
			AdministrativeEmployeeFirstName = AdministrativeEmployeeFirstNameTextBox.Text;
			AdministrativeEmployeeLastName = AdministrativeEmployeeLastNameTextBox.Text;
			AdministrativeEmployeeEmail = AdministrativeEmployeeEmailTextBox.Text;
			AdministrativeEmployeePhone = AdministrativeEmployeePhoneTextBox.Text;
			AdministrativeEmployeeDepartment = AdministrativeEmployeeDepartmentTextBox.Text;
			AdministrativeEmployeeHireDate = DateTime.Parse(AdministrativeEmployeeHireDateDatePicker.Text).ToString("yyyy-MM-dd");
			AdministrativeEmployeeSalary = int.Parse(AdministrativeEmployeeSalaryTextBox.Text);
			AdministrativeEmployeeIsFullTime = AdministrativeEmployeeIsFullTimeCheckBox.IsChecked ?? false;
			AdministrativeEmployeeAvailability = AdministrativeEmployeeAvailabilityTextBox.Text;
		}

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            if (IsInputValid() == false)
            {
                return;
            }

			SaveInputValues();

            // Close dialog box with OK result.
            DialogResult = true;
        }

        private bool IsParsableToInt(string text)
        {
            return !int.TryParse(text, out _);
		}

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsParsableToInt(e.Text) == false)
            {
                // Cancel the input
                e.Handled = true;
            }
        }
    }
}
