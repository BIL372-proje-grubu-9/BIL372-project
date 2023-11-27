using MySql.Data.MySqlClient;
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
    /// Interaction logic for StudentDialog.xaml
    /// </summary>
    public partial class StudentDialog : Window
    {
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public int StudentAge { get; set; }
        public bool StudentGraduate { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Availability { get; set; }
        public string Enrollments { get; set; }
        public StudentDialog()
        {
            InitializeComponent();
            populateStudentEnrollmentsListBox();
            StudentFirstName = string.Empty;
            StudentLastName = string.Empty;
            StudentAge = 0;
            StudentGraduate = false;
            ContactEmail = string.Empty;
            ContactPhone = string.Empty;
            Availability = string.Empty;
            Enrollments = string.Empty;
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

            // Set the StudentName and StudentAge property with the entered name.
            StudentFirstName = StudentFirstNameTextBox.Text;
            StudentLastName = StudentLastNameTextBox.Text;
            StudentAge = int.Parse(StudentAgeTextBox.Text);
            StudentGraduate = StudentGraduateCheckBox.IsChecked ?? false;
            ContactEmail = StudentEmailTextBox.Text;
            ContactPhone = StudentPhoneTextBox.Text;
            Availability = StudentAvailabilityTextBox.Text;
            foreach (var item in StudentEnrollmentsListBox.SelectedItems)
            {
                Enrollments += item.ToString().Split("ID: ")[1].Split(" - ")[0] + ",";
            }
            Enrollments = Enrollments.TrimEnd(',');

            // Close the dialog with a "true" result to indicate success.
            DialogResult = true;
        }

        private void populateStudentEnrollmentsListBox()
        {
            // Populate the list box with the courses.
            string query = "SELECT course_id, course_name FROM courses";
            MySqlCommand cmd = new(query, MainWindow.connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                StudentEnrollmentsListBox.Items.Add($"ID: {reader[0]} - {reader[1]}");
            }
            reader.Close();
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
