using MySql.Data.MySqlClient;
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
    /// Interaction logic for ParentDialog.xaml
    /// </summary>
    public partial class ParentDialog : Window
    {
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public int ParentStudentId { get; set; }
        public string ParentEmail { get; set; }
        public string ParentPhone { get; set; }
        public ParentDialog()
        {
            InitializeComponent();
            PopulateStudentIdComboBox();
            ParentFirstName = string.Empty;
            ParentLastName = string.Empty;
            ParentStudentId = 0;
            ParentEmail = string.Empty;
            ParentPhone = string.Empty;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(ParentFirstNameTextBox.Text))
            {
                MessageBox.Show("Please enter the parent's first name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ParentLastNameTextBox.Text))
            {
                MessageBox.Show("Please enter the parent's last name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ParentStudentIdComboBox.Text))
            {
                MessageBox.Show("Please enter the parent's student id.");
                return;
            }

            // Set the ParentFirstName and ParentLastName property with the entered name.
            ParentFirstName = ParentFirstNameTextBox.Text;
            ParentLastName = ParentLastNameTextBox.Text;
            ParentStudentId = int.Parse(ParentStudentIdComboBox.Text.ToLower().Split("id: ")[1].Replace(")", ""));
            ParentEmail = ParentEmailTextBox.Text;
            ParentPhone = ParentPhoneTextBox.Text;

            // Close the dialog box and return to the parent window.
            DialogResult = true;
        }

        private void PopulateStudentIdComboBox()
        {
            // Populate the StudentId with the student ids.
            string query = "SELECT first_name, last_name, student_id FROM students";

            // Create a connection to the database.
            MySqlCommand cmd = new(query, MainWindow.connection);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ParentStudentIdComboBox.Items.Add(reader[0] + " " + reader[1] + " (id: " + reader[2] + ")");
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
