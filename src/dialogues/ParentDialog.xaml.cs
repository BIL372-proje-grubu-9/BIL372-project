using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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

		// Add more validation if needed
		private bool IsInputValid()
		{
			if (string.IsNullOrWhiteSpace(ParentFirstNameTextBox.Text))
			{
				MessageBox.Show("Please enter the parent's first name.");
				return false;
			}

			if (string.IsNullOrWhiteSpace(ParentLastNameTextBox.Text))
			{
				MessageBox.Show("Please enter the parent's last name.");
				return false;
			}

			if (string.IsNullOrWhiteSpace(ParentStudentIdComboBox.Text))
			{
				MessageBox.Show("Please enter the parent's student id.");
				return false;
			}

            return true;
		}

        private void SaveInputValues()
        {
			ParentFirstName = ParentFirstNameTextBox.Text;
			ParentLastName = ParentLastNameTextBox.Text;
			ParentStudentId = int.Parse(ParentStudentIdComboBox.Text.ToLower().Split("id: ")[1].Replace(")", ""));
			ParentEmail = ParentEmailTextBox.Text;
			ParentPhone = ParentPhoneTextBox.Text;
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

		private MySqlDataReader GetReader()
		{
			// Populate the StudentId with the student ids.
			string query = "SELECT first_name, last_name, student_id FROM students";

			// Create a connection to the database.
			MySqlCommand cmd = new(query, MainWindow.connection);
			cmd.ExecuteNonQuery();

			return cmd.ExecuteReader();
		}

		private void AddToParentStudentIdComboBox(Object x1, Object x2, Object x3)
		{
			ParentStudentIdComboBox.Items.Add(x1 + " " + x2 + " (id: " + x3 + ")");
		}

		private void PopulateStudentIdComboBox()
        {
            MySqlDataReader reader = GetReader();

            while (reader.Read())
            {
				AddToParentStudentIdComboBox(reader[0], reader[1], reader[2]);
            }

            reader.Close();
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
