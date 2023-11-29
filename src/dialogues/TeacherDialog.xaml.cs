using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for TeacherDialog.xaml
    /// </summary>
    public partial class TeacherDialog : Window
    {
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherPhone { get; set; }
        public string TeacherSpecialty { get; set; }
        public string TeacherHireDate { get; set; }
        public int TeacherSalary { get; set; }
        public bool TeacherIsFullTime { get; set; }
        public string TeacherAvailability { get; set; }
        public TeacherDialog()
        {
            InitializeComponent();
            TeacherFirstName = string.Empty;
            TeacherLastName = string.Empty;
            TeacherEmail = string.Empty;
            TeacherPhone = string.Empty;
            TeacherSpecialty = string.Empty;
            TeacherHireDate = string.Empty;
            TeacherSalary = 0;
            TeacherIsFullTime = false;
            TeacherAvailability = string.Empty;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(TeacherFirstNameTextBox.Text))
            {
                MessageBox.Show("Please enter the teacher's first name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(TeacherLastNameTextBox.Text))
            {
                MessageBox.Show("Please enter the teacher's last name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(TeacherHireDateDatePicker.Text))
            {
                MessageBox.Show("Please enter the teacher's hire date.");
                return;
            }

            // Set the TeacherName and TeacherAge property with the entered name.
            TeacherFirstName = TeacherFirstNameTextBox.Text;
            TeacherLastName = TeacherLastNameTextBox.Text;
            TeacherEmail = TeacherEmailTextBox.Text;
            TeacherPhone = TeacherPhoneTextBox.Text;
            TeacherSpecialty = TeacherSpecialtyTextBox.Text;
            TeacherHireDate = DateTime.Parse(TeacherHireDateDatePicker.Text).ToString("yyyy-MM-dd");
            TeacherSalary = int.Parse(TeacherSalaryTextBox.Text);
            TeacherIsFullTime = TeacherIsFullTimeCheckBox.IsChecked ?? false;
            TeacherAvailability = TeacherAvailabilityTextBox.Text;

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
