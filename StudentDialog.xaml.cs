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
    /// Interaction logic for StudentDialog.xaml
    /// </summary>
    public partial class StudentDialog : Window
    {
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentAge { get; set; }
        public bool Graduate { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Availability { get; set; }
        public StudentDialog()
        {
            InitializeComponent();
            StudentFirstName = string.Empty;
            StudentLastName = string.Empty;
            StudentAge = string.Empty;
            Graduate = false;
            ContactEmail = string.Empty;
            ContactPhone = string.Empty;
            Availability = string.Empty;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(StudentFirstNameTextBox.Text))
            {
                MessageBox.Show("Please enter the student's first name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(StudentLastNameTextBox.Text))
            {
                MessageBox.Show("Please enter the student's last name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(StudentAgeTextBox.Text))
            {
                MessageBox.Show("Please enter the student's age.");
                return;
            }

            // Set the StudentName and StudentAge property with the entered name.
            StudentFirstName = StudentFirstNameTextBox.Text;
            StudentLastName = StudentLastNameTextBox.Text;
            StudentAge = StudentAgeTextBox.Text;
            Graduate = StudentGraduateCheckBox.IsChecked ?? false;
            ContactEmail = StudentEmailTextBox.Text;
            ContactPhone = StudentPhoneTextBox.Text;
            Availability = StudentAvailabilityTextBox.Text;

            // Close the dialog with a "true" result to indicate success.
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
