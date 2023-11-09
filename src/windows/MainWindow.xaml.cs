using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "Server=localhost;Database=project;PASSWORD=1234;UID=root;";

            MySqlConnection connection = new(connectionString);
            connection.Open();

            MySqlCommand studentsCmd = new("SELECT * FROM students", connection);
            MySqlCommand employeesCmd = new("SELECT * FROM employees", connection);

            DataTable studentsDataTable = new();
            DataTable employeesDataTable = new();

            studentsDataTable.Load(studentsCmd.ExecuteReader());
            employeesDataTable.Load(employeesCmd.ExecuteReader());

            StudentsGrid.ItemsSource = studentsDataTable.DefaultView;
            EmployeesGrid.ItemsSource = employeesDataTable.DefaultView;

            // Print the headers of the students table.
            foreach (DataColumn column in studentsDataTable.Columns)
            {
                Console.Write($"{column.ColumnName} ");
            }
            Console.WriteLine();

            connection.Close();
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDialog studentDialog = new();
            bool? result = studentDialog.ShowDialog();

            if (result == true)
            {
                string studentName = studentDialog.StudentName;
                string studentAge = studentDialog.StudentAge;

                // Add the student to the database.
                using MySqlConnection connection = new("Server=localhost;Database=project;PASSWORD=1234;UID=root;");
                connection.Open();

                string insertQuery = "INSERT INTO students (first_name, age) VALUES (@first_name, @age)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@first_name", studentName);
                insertCommand.Parameters.AddWithValue("@age", studentAge);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh the students grid with the updated data.
                    RefreshStudentsGrid();
                }

                connection.Close();
            }
        }

        // Method to refresh the StudentsGrid with updated data from the database.
        private void RefreshStudentsGrid()
        {
            using MySqlConnection connection = new("Server=localhost;Database=project;PASSWORD=1234;UID=root;");
            connection.Open();

            MySqlCommand studentsCmd = new("SELECT * FROM students", connection);
            DataTable studentsDataTable = new();
            studentsDataTable.Load(studentsCmd.ExecuteReader());

            StudentsGrid.ItemsSource = studentsDataTable.DefaultView;

            connection.Close();
        }
    }
}
