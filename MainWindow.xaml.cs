using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string studentsQuery = 
            "SELECT students.*, GROUP_CONCAT(courses.course_name SEPARATOR ', ') AS enrolled_courses " +
            "FROM students " +
            "LEFT JOIN enrollments ON students.student_id = enrollments.student_id " +
            "LEFT JOIN courses ON enrollments.course_id = courses.course_id " +
            "GROUP BY students.student_id";
        private const string employeesQuery = "SELECT * FROM employees";
        private const string coursesQuery = "SELECT * FROM courses";
        private const string connectionString = "Server=localhost;Database=project;PASSWORD=1234;UID=root;";
        public static MySqlConnection connection = new(connectionString);
        public MainWindow()
        {
            InitializeComponent();
            connection.Open();

            MySqlCommand studentsCmd = new(studentsQuery, connection);
            MySqlCommand employeesCmd = new(employeesQuery, connection);
            MySqlCommand coursesCmd = new(coursesQuery, connection);

            DataTable studentsDataTable = new();
            DataTable employeesDataTable = new();
            DataTable coursesDataTable = new();

            studentsDataTable.Load(studentsCmd.ExecuteReader());
            employeesDataTable.Load(employeesCmd.ExecuteReader());
            coursesDataTable.Load(coursesCmd.ExecuteReader());

            StudentsGrid.ItemsSource = studentsDataTable.DefaultView;
            EmployeesGrid.ItemsSource = employeesDataTable.DefaultView;
            CoursesGrid.ItemsSource = coursesDataTable.DefaultView;

            // Print the headers of the students table.
            foreach (DataColumn column in studentsDataTable.Columns)
            {
                Debug.Write($"{column.ColumnName} ");
            }
            Debug.WriteLine("");
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDialog studentDialog = new();
            bool? result = studentDialog.ShowDialog();

            if (result == true)
            {
                string studentFirstName = studentDialog.StudentFirstName;
                string studentLastName = studentDialog.StudentLastName;
                string studentAge = studentDialog.StudentAge;
                bool studentGraduate = studentDialog.Graduate;
                string studentEmail = studentDialog.ContactEmail;
                string studentPhone = studentDialog.ContactPhone;
                string studentAvailability = studentDialog.Availability;

                // Add the student to the database.

                string insertQuery = "INSERT INTO students (first_name, last_name, age, graduate, contact_email, contact_phone, availability) VALUES (@first_name, @last_name, @age, @graduate, @contact_email, @contact_phone, @availability)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@first_name", studentFirstName);
                insertCommand.Parameters.AddWithValue("@last_name", studentLastName);
                insertCommand.Parameters.AddWithValue("@age", studentAge);
                insertCommand.Parameters.AddWithValue("@graduate", studentGraduate);
                insertCommand.Parameters.AddWithValue("@contact_email", studentEmail);
                insertCommand.Parameters.AddWithValue("@contact_phone", studentPhone);
                insertCommand.Parameters.AddWithValue("@availability", studentAvailability);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh the students grid with the updated data.
                    RefreshStudentsGrid();
                }
            }
        }

        private void AddCourseButton_Click(object sender, RoutedEventArgs e)
        {
            CourseDialog courseDialog = new();
            bool? result = courseDialog.ShowDialog();
            
            if (result == true)
            {
                string courseName = courseDialog.CourseName;
                int courseTeacherId = courseDialog.CourseTeacherId;
                string courseSchedule = courseDialog.CourseSchedule;
                bool courseStatus = courseDialog.CourseStatus;

                // Add the course to the database.
                string insertQuery = "INSERT INTO courses (course_name, teacher_id, schedule, status) VALUES (@course_name, @teacher_id, @schedule, @status)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@course_name", courseName);
                insertCommand.Parameters.AddWithValue("@teacher_id", courseTeacherId);
                insertCommand.Parameters.AddWithValue("@schedule", courseSchedule);
                insertCommand.Parameters.AddWithValue("@status", courseStatus);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh the courses grid with the updated data.
                    RefreshCoursesGrid();
                }
            }
        }

        // Method to refresh the StudentsGrid with updated data from the database.
        private void RefreshStudentsGrid()
        {
            MySqlCommand studentsCmd = new(studentsQuery, connection);
            DataTable studentsDataTable = new();
            studentsDataTable.Load(studentsCmd.ExecuteReader());

            StudentsGrid.ItemsSource = studentsDataTable.DefaultView;
        }

        // Method to refresh the CoursesGrid with updated data from the database.
        private void RefreshCoursesGrid()
        {
            string query = "SELECT * FROM courses";

            MySqlCommand coursesCmd = new(query, connection);
            DataTable coursesDataTable = new();
            coursesDataTable.Load(coursesCmd.ExecuteReader());

            CoursesGrid.ItemsSource = coursesDataTable.DefaultView;
        }
    }
}
