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
    /// Interaction logic for CourseDialog.xaml
    /// </summary>
    public partial class CourseDialog : Window
    {
        public string CourseName { get; set; }
        public int CourseTeacherId { get; set; }
        public string CourseSchedule { get; set; }
        public bool CourseStatus { get; set; }
        public CourseDialog()
        {
            InitializeComponent();
            PopulateTeacherIdComboBox();
            CourseName = string.Empty;
            CourseTeacherId = 0;
            CourseSchedule = string.Empty;
            CourseStatus = false;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(CourseNameTextBox.Text))
            {
                MessageBox.Show("Please enter the course's name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(CourseTeacherIdComboBox.Text))
            {
                MessageBox.Show("Please enter the course's teacher id.");
                return;
            }
            if (string.IsNullOrWhiteSpace(CourseScheduleTextBox.Text))
            {
                MessageBox.Show("Please enter the course's schedule.");
                return;
            }

            // Set the CourseName and CourseTeacherId property with the entered name.
            CourseName = CourseNameTextBox.Text;
            CourseTeacherId = int.Parse(CourseTeacherIdComboBox.Text.Split('(', ')')[1]);
            CourseSchedule = CourseScheduleTextBox.Text;
            CourseStatus = CourseStatusCheckBox.IsChecked ?? false;

            // Close the dialog with a "true" result to indicate success.
            DialogResult = true;
        }

        private void PopulateTeacherIdComboBox()
        {
            // Populate the TeacherIdComboBox with the teacher ids.
            string query = "SELECT first_name, last_name, teacher_id FROM teachers";
            MySqlCommand cmd = new(query, MainWindow.connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CourseTeacherIdComboBox.Items.Add(reader[0] + " " + reader[1] + " (" + reader[2] + ")");
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
