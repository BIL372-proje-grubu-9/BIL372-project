using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfApp1.src.dialogues;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string dbName = "projectgroup9";
        private const string employeesQuery = "SELECT * FROM employees";
        private const string teachersQuery = "SELECT teachers.*, GROUP_CONCAT(courses.course_name SEPARATOR ',') AS courses_taught, GROUP_CONCAT(courses.schedule SEPARATOR ',') AS schedule " +
            "FROM teachers " +
            "LEFT JOIN courses ON teachers.teacher_id = courses.teacher_id " +
            "GROUP BY teachers.teacher_id";
        private const string administraiveQuery = "SELECT * FROM administrative_employees";
        private const string janitorsQuery = "SELECT * FROM janitors";
        private const string studentsQuery = 
            "SELECT students.*, GROUP_CONCAT(courses.course_name SEPARATOR ',') AS enrolled_courses, GROUP_CONCAT(courses.schedule SEPARATOR ',') AS schedule " +
            "FROM students " +
            "LEFT JOIN enrollments ON students.student_id = enrollments.student_id " +
            "LEFT JOIN courses ON enrollments.course_id = courses.course_id " +
            "GROUP BY students.student_id";
        private const string parentsQuery = "SELECT * FROM parents";
        private const string coursesQuery = "SELECT * FROM courses";
        private const string itemsQuery = "SELECT items.*, GROUP_CONCAT(courses.course_id SEPARATOR ',') AS course_id " +
            "FROM items " +
            "LEFT JOIN course_item ON items.item_id = course_item.item_id " +
            "LEFT JOIN courses ON course_item.course_id = courses.course_id " +
            "GROUP BY items.item_id";
        private const string incomesQuery = "SELECT * FROM incomes";
        private const string expensesQuery = "SELECT * FROM expenses";
        private const string deletedEmployeesQuery = "SELECT * FROM deletedemployees";
        private const string totalMoneyTrafficQuery = "SELECT total_income, total_expense FROM totalmoneytraffic";
        private string connectionString = "";
        public static MySqlConnection? connection;
        public MainWindow()
        {
            InitializeComponent();
            Setup();

            MySqlCommand employeesCmd = new(employeesQuery, connection);
            MySqlCommand teachersCmd = new(teachersQuery, connection);
            MySqlCommand administrativeCmd = new(administraiveQuery, connection);
            MySqlCommand janitorsCmd = new(janitorsQuery, connection);
            MySqlCommand studentsCmd = new(studentsQuery, connection);
            MySqlCommand parentsCmd = new(parentsQuery, connection);
            MySqlCommand coursesCmd = new(coursesQuery, connection);
            MySqlCommand itemsCmd = new(itemsQuery, connection);
            MySqlCommand incomesCmd = new(incomesQuery, connection);
            MySqlCommand expensesCmd = new(expensesQuery, connection);
            MySqlCommand deletedEmployeesCmd = new(deletedEmployeesQuery, connection);
            MySqlCommand totalMoneyTrafficCmd = new(totalMoneyTrafficQuery, connection);

            DataTable employeesDataTable = new();
            DataTable teachersDataTable = new();
            DataTable administrativeDataTable = new();
            DataTable janitorsDataTable = new();
            DataTable studentsDataTable = new();
            DataTable parentsDataTable = new();
            DataTable coursesDataTable = new();
            DataTable itemsDataTable = new();
            DataTable incomesDataTable = new();
            DataTable expensesDataTable = new();
            DataTable deletedEmployeesDataTable = new();
            DataTable totalMoneyTrafficDataTable = new();

            employeesDataTable.Load(employeesCmd.ExecuteReader());
            teachersDataTable.Load(teachersCmd.ExecuteReader());
            administrativeDataTable.Load(administrativeCmd.ExecuteReader());
            janitorsDataTable.Load(janitorsCmd.ExecuteReader());
            studentsDataTable.Load(studentsCmd.ExecuteReader());
            parentsDataTable.Load(parentsCmd.ExecuteReader());
            coursesDataTable.Load(coursesCmd.ExecuteReader());
            itemsDataTable.Load(itemsCmd.ExecuteReader());
            incomesDataTable.Load(incomesCmd.ExecuteReader());
            expensesDataTable.Load(expensesCmd.ExecuteReader());
            deletedEmployeesDataTable.Load(deletedEmployeesCmd.ExecuteReader());
            totalMoneyTrafficDataTable.Load(totalMoneyTrafficCmd.ExecuteReader());

            CapitalizeHeaders(employeesDataTable);
            CapitalizeHeaders(teachersDataTable);
            CapitalizeHeaders(administrativeDataTable);
            CapitalizeHeaders(janitorsDataTable);
            CapitalizeHeaders(studentsDataTable);
            CapitalizeHeaders(parentsDataTable);
            CapitalizeHeaders(coursesDataTable);
            CapitalizeHeaders(itemsDataTable);
            CapitalizeHeaders(incomesDataTable);
            CapitalizeHeaders(expensesDataTable);
            CapitalizeHeaders(deletedEmployeesDataTable);
            CapitalizeHeaders(totalMoneyTrafficDataTable);

            EmployeesGrid.ItemsSource = employeesDataTable.DefaultView;
            TeachersGrid.ItemsSource = teachersDataTable.DefaultView;
            AdministrativeEmployeesGrid.ItemsSource = administrativeDataTable.DefaultView;
            JanitorsGrid.ItemsSource = janitorsDataTable.DefaultView;
            StudentsGrid.ItemsSource = studentsDataTable.DefaultView;
            ParentsGrid.ItemsSource = parentsDataTable.DefaultView;
            CoursesGrid.ItemsSource = coursesDataTable.DefaultView;
            ItemsGrid.ItemsSource = itemsDataTable.DefaultView;
            IncomesGrid.ItemsSource = incomesDataTable.DefaultView;
            ExpensesGrid.ItemsSource = expensesDataTable.DefaultView;
            DeletedEmployeesGrid.ItemsSource = deletedEmployeesDataTable.DefaultView;
            TotalMoneyTrafficGrid.ItemsSource = totalMoneyTrafficDataTable.DefaultView;
        }

        private void Setup()
        {
            PasswordDialog passwordDialog = new();
            if (passwordDialog.ShowDialog() == true)
            {
                string password = passwordDialog.Password;
                connectionString = $"Server=localhost;PASSWORD={password};UID=root;";
                connection = new(connectionString);
                try
                {
                    connection.Open();
                    ConfigureDatabase(dbName);
                    connection.ChangeDatabase(dbName);
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    Environment.Exit(0);
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void AddTeacherButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherDialog teacherDialog = new();
            bool? result = teacherDialog.ShowDialog();

            if (result == true)
            {
                string teacherFirstName = teacherDialog.TeacherFirstName;
                string teacherLastName = teacherDialog.TeacherLastName;
                string teacherEmail = teacherDialog.TeacherEmail;
                string teacherPhone = teacherDialog.TeacherPhone;
                string teacherSpecialty = teacherDialog.TeacherSpecialty;
                string teacherHireDate = teacherDialog.TeacherHireDate;
                int teacherSalary = teacherDialog.TeacherSalary;
                bool teacherIsFullTime = teacherDialog.TeacherIsFullTime;
                string teacherAvailability = teacherDialog.TeacherAvailability;

                // Add the teacher to the employees table.
                string insertQuery = "INSERT INTO employees (first_name, last_name, email, phone, hire_date, salary, is_full_time, availability) VALUES (@first_name, @last_name, @contact_email, @contact_phone, @hire_date, @salary, @is_full_time, @availability)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@first_name", teacherFirstName);
                insertCommand.Parameters.AddWithValue("@last_name", teacherLastName);
                insertCommand.Parameters.AddWithValue("@contact_email", teacherEmail);
                insertCommand.Parameters.AddWithValue("@contact_phone", teacherPhone);
                insertCommand.Parameters.AddWithValue("@hire_date", teacherHireDate);
                insertCommand.Parameters.AddWithValue("@salary", teacherSalary);
                insertCommand.Parameters.AddWithValue("@is_full_time", teacherIsFullTime);
                insertCommand.Parameters.AddWithValue("@availability", teacherAvailability);

                // Execute the query and get the auto-generated teacher ID.

                int rowsAffected = insertCommand.ExecuteNonQuery();
                long teacherId = insertCommand.LastInsertedId;

                // Add the teacher to the database.
                insertQuery = "INSERT INTO teachers (teacher_id, first_name, last_name, email, phone, specialty) VALUES (@teacher_id, @first_name, @last_name, @contact_email, @contact_phone, @specialty)";
                insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@teacher_id", teacherId);
                insertCommand.Parameters.AddWithValue("@first_name", teacherFirstName);
                insertCommand.Parameters.AddWithValue("@last_name", teacherLastName);
                insertCommand.Parameters.AddWithValue("@contact_email", teacherEmail);
                insertCommand.Parameters.AddWithValue("@contact_phone", teacherPhone);
                insertCommand.Parameters.AddWithValue("@specialty", teacherSpecialty);

                rowsAffected += insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }
        private void AddJanitorButton_Click(object sender, RoutedEventArgs e)
        {
            JanitorDialog janitorDialog = new();
            bool? result = janitorDialog.ShowDialog();

            if (result == true)
            {
                string janitorFirstName = janitorDialog.JanitorFirstName;
                string janitorLastName = janitorDialog.JanitorLastName;
                string janitorEmail = janitorDialog.JanitorEmail;
                string janitorPhone = janitorDialog.JanitorPhone;
                string janitorHireDate = janitorDialog.JanitorHireDate;
                int janitorSalary = janitorDialog.JanitorSalary;
                bool janitorIsFullTime = janitorDialog.JanitorIsFullTime;
                string janitorAvailability = janitorDialog.JanitorAvailability;

                // Add the janitor to the employees table.
                string insertQuery = "INSERT INTO employees (first_name, last_name, email, phone, hire_date, salary, is_full_time, availability) VALUES (@first_name, @last_name, @contact_email, @contact_phone, @hire_date, @salary, @is_full_time, @availability)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@first_name", janitorFirstName);
                insertCommand.Parameters.AddWithValue("@last_name", janitorLastName);
                insertCommand.Parameters.AddWithValue("@contact_email", janitorEmail);
                insertCommand.Parameters.AddWithValue("@contact_phone", janitorPhone);
                insertCommand.Parameters.AddWithValue("@hire_date", janitorHireDate);
                insertCommand.Parameters.AddWithValue("@salary", janitorSalary);
                insertCommand.Parameters.AddWithValue("@is_full_time", janitorIsFullTime);
                insertCommand.Parameters.AddWithValue("@availability", janitorAvailability);

                // Execute the query and get the auto-generated janitor ID.

                int rowsAffected = insertCommand.ExecuteNonQuery();
                long janitorId = insertCommand.LastInsertedId;

                // Add the janitor to the database.
                insertQuery = "INSERT INTO janitors (janitor_id, first_name, last_name, email, phone) VALUES (@janitor_id, @first_name, @last_name, @contact_email, @contact_phone)";
                insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@janitor_id", janitorId);
                insertCommand.Parameters.AddWithValue("@first_name", janitorFirstName);
                insertCommand.Parameters.AddWithValue("@last_name", janitorLastName);
                insertCommand.Parameters.AddWithValue("@contact_email", janitorEmail);
                insertCommand.Parameters.AddWithValue("@contact_phone", janitorPhone);

                rowsAffected += insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        private void AddAdministrativeEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            AdministrativeEmployeeDialog administrativeEmployeeDialog = new();
            bool? result = administrativeEmployeeDialog.ShowDialog();

            if (result == true)
            {
                string administrativeEmployeeFirstName = administrativeEmployeeDialog.AdministrativeEmployeeFirstName;
                string administrativeEmployeeLastName = administrativeEmployeeDialog.AdministrativeEmployeeLastName;
                string administrativeEmployeeEmail = administrativeEmployeeDialog.AdministrativeEmployeeEmail;
                string administrativeEmployeePhone = administrativeEmployeeDialog.AdministrativeEmployeePhone;
                string administrativeEmployeeDepartment = administrativeEmployeeDialog.AdministrativeEmployeeDepartment;
                string administrativeEmployeeHireDate = administrativeEmployeeDialog.AdministrativeEmployeeHireDate;
                int administrativeEmployeeSalary = administrativeEmployeeDialog.AdministrativeEmployeeSalary;
                bool administrativeEmployeeIsFullTime = administrativeEmployeeDialog.AdministrativeEmployeeIsFullTime;
                string administrativeEmployeeAvailability = administrativeEmployeeDialog.AdministrativeEmployeeAvailability;

                // Add the administrative employee to the employees table.
                string insertQuery = "INSERT INTO employees (first_name, last_name, email, phone, hire_date, salary, is_full_time, availability) VALUES (@first_name, @last_name, @contact_email, @contact_phone, @hire_date, @salary, @is_full_time, @availability)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@first_name", administrativeEmployeeFirstName);
                insertCommand.Parameters.AddWithValue("@last_name", administrativeEmployeeLastName);
                insertCommand.Parameters.AddWithValue("@contact_email", administrativeEmployeeEmail);
                insertCommand.Parameters.AddWithValue("@contact_phone", administrativeEmployeePhone);
                insertCommand.Parameters.AddWithValue("@hire_date", administrativeEmployeeHireDate);
                insertCommand.Parameters.AddWithValue("@salary", administrativeEmployeeSalary);
                insertCommand.Parameters.AddWithValue("@is_full_time", administrativeEmployeeIsFullTime);
                insertCommand.Parameters.AddWithValue("@availability", administrativeEmployeeAvailability);

                // Execute the query and get the auto-generated administrative employee ID.
                int rowsAffected = insertCommand.ExecuteNonQuery();
                long administrativeEmployeeId = insertCommand.LastInsertedId;

                // Add the administrative employee to the database.
                insertQuery = "INSERT INTO administrative_employees (administrative_employee_id, first_name, last_name, email, phone, department) VALUES (@administrative_employee_id, @first_name, @last_name, @contact_email, @contact_phone, @department)";
                insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@administrative_employee_id", administrativeEmployeeId);
                insertCommand.Parameters.AddWithValue("@first_name", administrativeEmployeeFirstName);
                insertCommand.Parameters.AddWithValue("@last_name", administrativeEmployeeLastName);
                insertCommand.Parameters.AddWithValue("@contact_email", administrativeEmployeeEmail);
                insertCommand.Parameters.AddWithValue("@contact_phone", administrativeEmployeePhone);
                insertCommand.Parameters.AddWithValue("@department", administrativeEmployeeDepartment);

                rowsAffected += insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            StudentDialog studentDialog = new();
            bool? result = studentDialog.ShowDialog();

            if (result == true)
            {
                string studentFirstName = studentDialog.StudentFirstName;
                string studentLastName = studentDialog.StudentLastName;
                int studentAge = studentDialog.StudentAge;
                bool studentGraduate = studentDialog.StudentGraduate;
                string studentEmail = studentDialog.ContactEmail;
                string studentPhone = studentDialog.ContactPhone;
                string studentAvailability = studentDialog.Availability;
                int[] enrollments;
                string[] foobar = studentDialog.Enrollments.Split(',');
                if (studentDialog.Enrollments.Length > 0)
                {
                    enrollments = studentDialog.Enrollments.Split(',').Select(int.Parse).ToArray() ?? new int[0];
                }
                else
                {
                    enrollments = new int[0];
                }

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

                // Execute the query and get the auto-generated student ID.
                int rowsAffected = insertCommand.ExecuteNonQuery();
                long studentId = insertCommand.LastInsertedId;

                // Add the student's enrollments to the database.
                foreach (int courseId in enrollments)
                {
                    insertQuery = "INSERT INTO enrollments (student_id, course_id) VALUES (@student_id, @course_id)";
                    insertCommand = new(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@student_id", studentId);
                    insertCommand.Parameters.AddWithValue("@course_id", courseId);

                    rowsAffected += insertCommand.ExecuteNonQuery();
                }

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.,
                    RefreshAllGrids();
                }
            }
        }

        private void AddParentButton_Click(object sender, RoutedEventArgs e)
        {
            ParentDialog parentDialog = new();
            bool? result = parentDialog.ShowDialog();

            if (result == true)
            {
                string parentFirstName = parentDialog.ParentFirstName;
                string parentLastName = parentDialog.ParentLastName;
                int parentStudentId = parentDialog.ParentStudentId;
                string parentEmail = parentDialog.ParentEmail;
                string parentPhone = parentDialog.ParentPhone;

                // Add the parent to the database.
                string insertQuery = "INSERT INTO parents (first_name, last_name, student_id, contact_email, contact_phone) VALUES (@first_name, @last_name, @student_id, @email, @phone)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@first_name", parentFirstName);
                insertCommand.Parameters.AddWithValue("@last_name", parentLastName);
                insertCommand.Parameters.AddWithValue("@student_id", parentStudentId);
                insertCommand.Parameters.AddWithValue("@email", parentEmail);
                insertCommand.Parameters.AddWithValue("@phone", parentPhone);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
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
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            ItemDialog itemDialog = new();
            bool? result = itemDialog.ShowDialog();

            if (result == true)
            {
                string itemName = itemDialog.ItemName;
                int itemQuantity = itemDialog.ItemQuantity;
                string itemDescription = itemDialog.ItemDescription;
                int itemCourseId = itemDialog.ItemCourseId;

                // Add the item to the database.
                string insertQuery = "INSERT INTO items (item_name, quantity, item_description) VALUES (@item_name, @item_quantity, @item_description)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@item_name", itemName);
                insertCommand.Parameters.AddWithValue("@item_quantity", itemQuantity);
                insertCommand.Parameters.AddWithValue("@item_description", itemDescription);

                // Execute the query and get the auto-generated item ID.
                int rowsAffected = insertCommand.ExecuteNonQuery();
                long itemId = insertCommand.LastInsertedId;

                // Add the item to the course_item table.
                insertQuery = "INSERT INTO course_item (course_id, item_id) VALUES (@course_id, @item_id)";
                insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@course_id", itemCourseId);
                insertCommand.Parameters.AddWithValue("@item_id", itemId);

                rowsAffected += insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        private void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            IncomeDialog incomeDialog = new();
            bool? result = incomeDialog.ShowDialog();

            if (result == true)
            {
                string incomeType = incomeDialog.IncomeType;
                int incomeAmount = incomeDialog.IncomeAmount;
                string incomeDate = incomeDialog.IncomeDate;
                string incomeDescription = incomeDialog.IncomeDescription;

                // Add the income to the database.
                string insertQuery = "INSERT INTO incomes (income_type, income_amount, income_date, income_description) VALUES (@income_type, @income_amount, @income_date, @income_description)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@income_type", incomeType);
                insertCommand.Parameters.AddWithValue("@income_amount", incomeAmount);
                insertCommand.Parameters.AddWithValue("@income_date", incomeDate);
                insertCommand.Parameters.AddWithValue("@income_description", incomeDescription);

                int rowsAffected = insertCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            ExpenseDialog expenseDialog = new();
            bool? result = expenseDialog.ShowDialog();

            if (result == true)
            {
                string expenseType = expenseDialog.ExpenseType;
                int expenseAmount = expenseDialog.ExpenseAmount;
                string expenseDate = expenseDialog.ExpenseDate;
                string expenseDescription = expenseDialog.ExpenseDescription;

                // Add the expense to the database.
                string insertQuery = "INSERT INTO expenses (expense_type, expense_amount, expense_date, expense_description) VALUES (@expense_type, @expense_amount, @expense_date, @expense_description)";
                MySqlCommand insertCommand = new(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@expense_type", expenseType);
                insertCommand.Parameters.AddWithValue("@expense_amount", expenseAmount);
                insertCommand.Parameters.AddWithValue("@expense_date", expenseDate);
                insertCommand.Parameters.AddWithValue("@expense_description", expenseDescription);

                int rowsAffected = insertCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        // Cell edit event handler for the TeachersGrid.
        private void TeachersGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the teacher ID of the teacher that was edited.
            int teacherId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the teacher in the database.
            string updateQuery = $"UPDATE teachers SET {columnName} = @new_value WHERE teacher_id = @teacher_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@teacher_id", teacherId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            //Get the employeeGrid's columns and if columnNames contains the columnName, update the employee in the database.
            EmployeesGrid.Columns.ToList().ForEach(column =>
            {
                string employeeColumnName = column.Header.ToString().ToLower().Replace(' ', '_');
                if (employeeColumnName == columnName)
                {
                    updateQuery = $"UPDATE employees SET {columnName} = @new_value WHERE employee_id = @employee_id";
                    updateCommand = new(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@new_value", newValue);
                    updateCommand.Parameters.AddWithValue("@employee_id", teacherId);

                    rowsAffected += updateCommand.ExecuteNonQuery();
                }
            });

            rowsAffected += updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Cell edit event handler for the JanitorsGrid.
        private void JanitorsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the janitor ID of the janitor that was edited.
            int janitorId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the janitor in the database.
            string updateQuery = $"UPDATE janitors SET {columnName} = @new_value WHERE janitor_id = @janitor_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@janitor_id", janitorId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            //Get the employeeGrid's columns and if columnNames contains the columnName, update the employee in the database.
            EmployeesGrid.Columns.ToList().ForEach(column =>
            {
                string employeeColumnName = column.Header.ToString().ToLower().Replace(' ', '_');
                if (employeeColumnName == columnName)
                {
                    updateQuery = $"UPDATE employees SET {columnName} = @new_value WHERE employee_id = @employee_id";
                    updateCommand = new(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@new_value", newValue);
                    updateCommand.Parameters.AddWithValue("@employee_id", janitorId);

                    rowsAffected += updateCommand.ExecuteNonQuery();
                }
            });

            rowsAffected += updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Cell edit event handler for the AdministrativeEmployeesGrid.
        private void AdministrativeEmployeesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the administrative employee ID of the administrative employee that was edited.
            int administrativeEmployeeId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the administrative employee in the database.
            string updateQuery = $"UPDATE administrative_employees SET {columnName} = @new_value WHERE administrative_employee_id = @administrative_employee_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@administrative_employee_id", administrativeEmployeeId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            //Get the employeeGrid's columns and if columnNames contains the columnName, update the employee in the database.
            EmployeesGrid.Columns.ToList().ForEach(column =>
            {
                string employeeColumnName = column.Header.ToString().ToLower().Replace(' ', '_');
                if (employeeColumnName == columnName)
                {
                    updateQuery = $"UPDATE employees SET {columnName} = @new_value WHERE employee_id = @employee_id";
                    updateCommand = new(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@new_value", newValue);
                    updateCommand.Parameters.AddWithValue("@employee_id", administrativeEmployeeId);

                    rowsAffected += updateCommand.ExecuteNonQuery();
                }
            });

            rowsAffected += updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Cell edit event handler for the StudentsGrid.
        private void StudentsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the student ID of the student that was edited.
            int studentId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the student in the database.
            string updateQuery = $"UPDATE students SET {columnName} = @new_value WHERE student_id = @student_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@student_id", studentId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Cell edit event handler for the ParentsGrid.
        private void ParentsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the parent ID of the parent that was edited.
            int parentId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the parent in the database.
            string updateQuery = $"UPDATE parents SET {columnName} = @new_value WHERE parent_id = @parent_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@parent_id", parentId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Cell edit event handler for the CoursesGrid.
        private void CoursesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the course ID of the course that was edited.
            int courseId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the course in the database.
            string updateQuery = $"UPDATE courses SET {columnName} = @new_value WHERE course_id = @course_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@course_id", courseId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Cell edit event handler for the ItemsGrid.
        private void ItemsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the item ID of the item that was edited.
            int itemId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the item in the database.
            string updateQuery = $"UPDATE items SET {columnName} = @new_value WHERE item_id = @item_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@item_id", itemId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Cell edit event handler for the IncomesGrid.
        private void IncomesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the income ID of the income that was edited.
            int incomeId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the income in the database.
            string updateQuery = $"UPDATE incomes SET {columnName} = @new_value WHERE income_id = @income_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@income_id", incomeId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Cell edit event handler for the ExpensesGrid.
        private void ExpensesGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // Get the row and column of the cell that was edited.
            int rowIndex = e.Row.GetIndex();
            int columnIndex = e.Column.DisplayIndex;

            // Get the expense ID of the expense that was edited.
            int expenseId = (int)((DataRowView)e.Row.Item).Row.ItemArray[0];

            // Get the new value of the cell that was edited.
            string newValue = ((TextBox)e.EditingElement).Text;

            // Get the column name of the cell that was edited.
            string columnName = ((DataGridTextColumn)e.Column).Header.ToString().ToLower().Replace(' ', '_');

            // Update the expense in the database.
            string updateQuery = $"UPDATE expenses SET {columnName} = @new_value WHERE expense_id = @expense_id";
            MySqlCommand updateCommand = new(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@new_value", newValue);
            updateCommand.Parameters.AddWithValue("@expense_id", expenseId);

            int rowsAffected = updateCommand.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Refresh all the grids with the updated data.
                RefreshAllGrids();
            }
        }

        // Method to refresh the EmployeesGrid with updated data from the database.
        private void RefreshEmployeesGrid()
        {
            MySqlCommand employeesCmd = new(employeesQuery, connection);
            DataTable employeesDataTable = new();
            employeesDataTable.Load(employeesCmd.ExecuteReader());
            CapitalizeHeaders(employeesDataTable);

            EmployeesGrid.ItemsSource = employeesDataTable.DefaultView;
        }

        // Method to refresh the TeachersGrid with updated data from the database.
        private void RefreshTeachersGrid()
        {
            MySqlCommand teachersCmd = new(teachersQuery, connection);
            DataTable teachersDataTable = new();
            teachersDataTable.Load(teachersCmd.ExecuteReader());
            CapitalizeHeaders(teachersDataTable);

            TeachersGrid.ItemsSource = teachersDataTable.DefaultView;
        }

        // Method to refresh the JanitorsGrid with updated data from the database.
        private void RefreshJanitorsGrid()
        {
            MySqlCommand janitorsCmd = new(janitorsQuery, connection);
            DataTable janitorsDataTable = new();
            janitorsDataTable.Load(janitorsCmd.ExecuteReader());
            CapitalizeHeaders(janitorsDataTable);

            JanitorsGrid.ItemsSource = janitorsDataTable.DefaultView;
        }

        // Method to refresh the AdministrativeEmployeesGrid with updated data from the database.
        private void RefreshAdministrativeEmployeesGrid()
        {
            MySqlCommand administrativeCmd = new(administraiveQuery, connection);
            DataTable administrativeDataTable = new();
            administrativeDataTable.Load(administrativeCmd.ExecuteReader());
            CapitalizeHeaders(administrativeDataTable);

            AdministrativeEmployeesGrid.ItemsSource = administrativeDataTable.DefaultView;
        }

        // Method to refresh the StudentsGrid with updated data from the database.
        private void RefreshStudentsGrid()
        {
            MySqlCommand studentsCmd = new(studentsQuery, connection);
            DataTable studentsDataTable = new();
            studentsDataTable.Load(studentsCmd.ExecuteReader());
            CapitalizeHeaders(studentsDataTable);

            StudentsGrid.ItemsSource = studentsDataTable.DefaultView;
        }

        // Method to refresh the ParentsGrid with updated data from the database.
        private void RefreshParentsGrid()
        {
            MySqlCommand parentsCmd = new(parentsQuery, connection);
            DataTable parentsDataTable = new();
            parentsDataTable.Load(parentsCmd.ExecuteReader());
            CapitalizeHeaders(parentsDataTable);

            ParentsGrid.ItemsSource = parentsDataTable.DefaultView;
        }

        // Method to refresh the CoursesGrid with updated data from the database.
        private void RefreshCoursesGrid()
        {
            MySqlCommand coursesCmd = new(coursesQuery, connection);
            DataTable coursesDataTable = new();
            coursesDataTable.Load(coursesCmd.ExecuteReader());
            CapitalizeHeaders(coursesDataTable);

            CoursesGrid.ItemsSource = coursesDataTable.DefaultView;
        }

        // Method to refresh the ItemGrid with updated data from the database.
        private void RefreshItemsGrid()
        {
            MySqlCommand itemsCmd = new(itemsQuery, connection);
            DataTable itemsDataTable = new();
            itemsDataTable.Load(itemsCmd.ExecuteReader());
            CapitalizeHeaders(itemsDataTable);

            ItemsGrid.ItemsSource = itemsDataTable.DefaultView;
        }

        // Method to refresh the IncomesGrid with updated data from the database.
        private void RefreshIncomesGrid()
        {
            MySqlCommand incomesCmd = new(incomesQuery, connection);
            DataTable incomesDataTable = new();
            incomesDataTable.Load(incomesCmd.ExecuteReader());
            CapitalizeHeaders(incomesDataTable);

            IncomesGrid.ItemsSource = incomesDataTable.DefaultView;
        }

        // Method to refresh the ExpensesGrid with updated data from the database.
        private void RefreshExpensesGrid()
        {
            MySqlCommand expensesCmd = new(expensesQuery, connection);
            DataTable expensesDataTable = new();
            expensesDataTable.Load(expensesCmd.ExecuteReader());
            CapitalizeHeaders(expensesDataTable);

            ExpensesGrid.ItemsSource = expensesDataTable.DefaultView;
        }

        // Method to refresh the DeletedEmployeesGrid with updated data from the database.
        private void RefreshDeletedEmployeesGrid()
        {
            MySqlCommand deletedEmployeesCmd = new(deletedEmployeesQuery, connection);
            DataTable deletedEmployeesDataTable = new();
            deletedEmployeesDataTable.Load(deletedEmployeesCmd.ExecuteReader());
            CapitalizeHeaders(deletedEmployeesDataTable);

            DeletedEmployeesGrid.ItemsSource = deletedEmployeesDataTable.DefaultView;
        }

        // Method to refresh the TotalMoneyTrafficGrid with updated data from the database.
        private void RefreshTotalMoneyTrafficGrid()
        {
            MySqlCommand totalMoneyTrafficCmd = new(totalMoneyTrafficQuery, connection);
            DataTable totalMoneyTrafficDataTable = new();
            totalMoneyTrafficDataTable.Load(totalMoneyTrafficCmd.ExecuteReader());
            CapitalizeHeaders(totalMoneyTrafficDataTable);

            TotalMoneyTrafficGrid.ItemsSource = totalMoneyTrafficDataTable.DefaultView;
        }

        // Method to refresh all the grids with updated data from the database.
        private void RefreshAllGrids()
        {
            RefreshEmployeesGrid();
            RefreshTeachersGrid();
            RefreshJanitorsGrid();
            RefreshAdministrativeEmployeesGrid();
            RefreshStudentsGrid();
            RefreshParentsGrid();
            RefreshCoursesGrid();
            RefreshItemsGrid();
            RefreshIncomesGrid();
            RefreshExpensesGrid();
            RefreshDeletedEmployeesGrid();
            RefreshTotalMoneyTrafficGrid();
        }

        // Method to capitalize the headers of a DataTable.
        private static void CapitalizeHeaders(DataTable dataTable)
        {
            // Remove the underscores from the column headers and capitalize the first letter of each word.
            foreach (DataColumn column in dataTable.Columns)
            {
                string columnName = column.ColumnName;
                string[] words = columnName.Split('_');
                string capitalizedColumnName = "";
                foreach (string word in words)
                {
                    capitalizedColumnName += word.First().ToString().ToUpper() + word[1..] + " ";
                }
                column.ColumnName = capitalizedColumnName.Trim();
            }
        }

        // Method to clear the filters for the EmployeesGrid.
        private void EmployeeClearButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeFirstNameFilter.Text = "";
            EmployeeLastNameFilter.Text = "";
            EmployeeEmailFilter.Text = "";
            EmployeePhoneFilter.Text = "";
            EmployeeHireDateFilter.Text = "";
            EmployeeSalaryFilter.Text = "";
            EmployeeIsFullTimeFilter.IsChecked = false;
            EmployeeAvailabilityFilter.Text = "";

            RefreshEmployeesGrid();
        }

        // Method to filter the EmployeesGrid.
        private void EmployeeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM employees WHERE ";

            if (EmployeeFirstNameFilter.Text != "")
            {
                query += $"first_name LIKE '%{EmployeeFirstNameFilter.Text}%' AND ";
            }
            if (EmployeeLastNameFilter.Text != "")
            {
                query += $"last_name LIKE '%{EmployeeLastNameFilter.Text}%' AND ";
            }
            if (EmployeeEmailFilter.Text != "")
            {
                query += $"email LIKE '%{EmployeeEmailFilter.Text}%' AND ";
            }
            if (EmployeePhoneFilter.Text != "")
            {
                query += $"phone LIKE '%{EmployeePhoneFilter.Text}%' AND ";
            }
            if (EmployeeHireDateFilter.Text != "")
            {
                query += $"hire_date LIKE '%{DateTime.Parse(EmployeeHireDateFilter.Text).ToString("yyyy-MM-dd")}%' AND ";
            }
            if (EmployeeSalaryFilter.Text != "")
            {
                query += $"salary LIKE '%{EmployeeSalaryFilter.Text}%' AND ";
            }
            if (EmployeeIsFullTimeFilter.IsChecked == true)
            {
                query += $"is_full_time = 1 AND ";
            }
            if (EmployeeAvailabilityFilter.Text != "")
            {
                query += $"availability LIKE '%{EmployeeAvailabilityFilter.Text}%' AND ";
            }

            query = query.Remove(query.Length - 5);

            MySqlCommand employeesCmd = new(query, connection);
            DataTable employeesDataTable = new();
            employeesDataTable.Load(employeesCmd.ExecuteReader());
            CapitalizeHeaders(employeesDataTable);

            EmployeesGrid.ItemsSource = employeesDataTable.DefaultView;
        }

        // Method to clear the filters for the TeachersGrid.
        private void TeacherClearButton_Click(object sender, RoutedEventArgs e)
        {
            TeacherFirstNameFilter.Text = "";
            TeacherLastNameFilter.Text = "";
            TeacherEmailFilter.Text = "";
            TeacherPhoneFilter.Text = "";
            TeacherSpecialtyFilter.Text = "";
            TeacherHireDateFilter.Text = "";
            TeacherSalaryFilter.Text = "";
            TeacherIsFullTimeFilter.IsChecked = false;

            RefreshTeachersGrid();
        }

        // Method to filter the TeachersGrid.
        private void TeacherFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM teachers WHERE ";

            if (TeacherFirstNameFilter.Text != "")
            {
                query += $"first_name LIKE '%{TeacherFirstNameFilter.Text}%' AND ";
            }
            if (TeacherLastNameFilter.Text != "")
            {
                query += $"last_name LIKE '%{TeacherLastNameFilter.Text}%' AND ";
            }
            if (TeacherEmailFilter.Text != "")
            {
                query += $"email LIKE '%{TeacherEmailFilter.Text}%' AND ";
            }
            if (TeacherPhoneFilter.Text != "")
            {
                query += $"phone LIKE '%{TeacherPhoneFilter.Text}%' AND ";
            }
            if (TeacherSpecialtyFilter.Text != "")
            {
                query += $"specialty LIKE '%{TeacherSpecialtyFilter.Text}%' AND ";
            }
            if (TeacherHireDateFilter.Text != "")
            {
                query += $"hire_date LIKE '%{DateTime.Parse(TeacherHireDateFilter.Text).ToString("yyyy-MM-dd")}%' AND ";
            }
            if (TeacherSalaryFilter.Text != "")
            {
                query += $"salary LIKE '%{TeacherSalaryFilter.Text}%' AND ";
            }
            if (TeacherIsFullTimeFilter.IsChecked == true)
            {
                query += $"is_full_time = 1 AND ";
            }

            query = query.Remove(query.Length - 5);

            MySqlCommand teachersCmd = new(query, connection);
            DataTable teachersDataTable = new();
            teachersDataTable.Load(teachersCmd.ExecuteReader());
            CapitalizeHeaders(teachersDataTable);

            TeachersGrid.ItemsSource = teachersDataTable.DefaultView;
        }

        // Method to clear the filters for the JanitorsGrid.
        private void JanitorClearButton_Click(object sender, RoutedEventArgs e)
        {
            JanitorFirstNameFilter.Text = "";
            JanitorLastNameFilter.Text = "";
            JanitorEmailFilter.Text = "";
            JanitorPhoneFilter.Text = "";
            JanitorHireDateFilter.Text = "";
            JanitorSalaryFilter.Text = "";
            JanitorIsFullTimeFilter.IsChecked = false;
            JanitorAvailabilityFilter.Text = "";

            RefreshJanitorsGrid();
        }

        // Method to filter the JanitorsGrid.
        private void JanitorFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM janitors WHERE ";

            if (JanitorFirstNameFilter.Text != "")
            {
                query += $"first_name LIKE '%{JanitorFirstNameFilter.Text}%' AND ";
            }
            if (JanitorLastNameFilter.Text != "")
            {
                query += $"last_name LIKE '%{JanitorLastNameFilter.Text}%' AND ";
            }
            if (JanitorEmailFilter.Text != "")
            {
                query += $"email LIKE '%{JanitorEmailFilter.Text}%' AND ";
            }
            if (JanitorPhoneFilter.Text != "")
            {
                query += $"phone LIKE '%{JanitorPhoneFilter.Text}%' AND ";
            }
            if (JanitorHireDateFilter.Text != "")
            {
                query += $"hire_date LIKE '%{DateTime.Parse(JanitorHireDateFilter.Text).ToString("yyyy-MM-dd")}%' AND ";
            }
            if (JanitorSalaryFilter.Text != "")
            {
                query += $"salary LIKE '%{JanitorSalaryFilter.Text}%' AND ";
            }
            if (JanitorIsFullTimeFilter.IsChecked == true)
            {
                query += $"is_full_time = 1 AND ";
            }
            if (JanitorAvailabilityFilter.Text != "")
            {
                query += $"availability LIKE '%{JanitorAvailabilityFilter.Text}%' AND ";
            }

            query = query.Remove(query.Length - 5);

            MySqlCommand janitorsCmd = new(query, connection);
            DataTable janitorsDataTable = new();
            janitorsDataTable.Load(janitorsCmd.ExecuteReader());
            CapitalizeHeaders(janitorsDataTable);

            JanitorsGrid.ItemsSource = janitorsDataTable.DefaultView;
        }

        // Method to clear the filters for the AdministrativeEmployeesGrid.
        private void AdministrativeEmployeeClearButton_Click(object sender, RoutedEventArgs e)
        {
            AdministrativeEmployeeFirstNameFilter.Text = "";
            AdministrativeEmployeeLastNameFilter.Text = "";
            AdministrativeEmployeeEmailFilter.Text = "";
            AdministrativeEmployeePhoneFilter.Text = "";
            AdministrativeEmployeeDepartmentFilter.Text = "";
            AdministrativeEmployeeHireDateFilter.Text = "";
            AdministrativeEmployeeSalaryFilter.Text = "";
            AdministrativeEmployeeIsFullTimeFilter.IsChecked = false;
            AdministrativeEmployeeAvailabilityFilter.Text = "";

            RefreshAdministrativeEmployeesGrid();
        }

        // Method to filter the AdministrativeEmployeesGrid.
        private void AdministrativeEmployeeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM administrative_employees WHERE ";

            if (AdministrativeEmployeeFirstNameFilter.Text != "")
            {
                query += $"first_name LIKE '%{AdministrativeEmployeeFirstNameFilter.Text}%' AND ";
            }
            if (AdministrativeEmployeeLastNameFilter.Text != "")
            {
                query += $"last_name LIKE '%{AdministrativeEmployeeLastNameFilter.Text}%' AND ";
            }
            if (AdministrativeEmployeeEmailFilter.Text != "")
            {
                query += $"email LIKE '%{AdministrativeEmployeeEmailFilter.Text}%' AND ";
            }
            if (AdministrativeEmployeePhoneFilter.Text != "")
            {
                query += $"phone LIKE '%{AdministrativeEmployeePhoneFilter.Text}%' AND ";
            }
            if (AdministrativeEmployeeDepartmentFilter.Text != "")
            {
                query += $"department LIKE '%{AdministrativeEmployeeDepartmentFilter.Text}%' AND ";
            }
            if (AdministrativeEmployeeHireDateFilter.Text != "")
            {
                query += $"hire_date LIKE '%{DateTime.Parse(AdministrativeEmployeeHireDateFilter.Text).ToString("yyyy-MM-dd")}%' AND ";
            }
            if (AdministrativeEmployeeSalaryFilter.Text != "")
            {
                query += $"salary LIKE '%{AdministrativeEmployeeSalaryFilter.Text}%' AND ";
            }
            if (AdministrativeEmployeeIsFullTimeFilter.IsChecked == true)
            {
                query += $"is_full_time = 1 AND ";
            }
            if (AdministrativeEmployeeAvailabilityFilter.Text != "")
            {
                query += $"availability LIKE '%{AdministrativeEmployeeAvailabilityFilter.Text}%' AND ";
            }

            query = query.Remove(query.Length - 5);

            MySqlCommand administrativeCmd = new(query, connection);
            DataTable administrativeDataTable = new();
            administrativeDataTable.Load(administrativeCmd.ExecuteReader());
            CapitalizeHeaders(administrativeDataTable);

            AdministrativeEmployeesGrid.ItemsSource = administrativeDataTable.DefaultView;
        }

        // Method to clear the filters for the StudentsGrid.
        private void StudentClearButton_Click(object sender, RoutedEventArgs e)
        {
            StudentFirstNameFilter.Text = "";
            StudentLastNameFilter.Text = "";
            StudentAgeFilter.Text = "";
            StudentIsGraduateFilter.IsChecked = false;
            StudentEmailFilter.Text = "";
            StudentPhoneFilter.Text = "";
            StudentAvailabilityFilter.Text = "";

            RefreshStudentsGrid();
        }

        // Method to filter the StudentsGrid.
        private void StudentFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT students.*, GROUP_CONCAT(courses.course_name SEPARATOR ', ') AS enrolled_courses " +
                "FROM students " +
                "LEFT JOIN enrollments ON students.student_id = enrollments.student_id " +
                "LEFT JOIN courses ON enrollments.course_id = courses.course_id " +
                "WHERE ";

            bool filterApplied = false;

            if (StudentFirstNameFilter.Text != "")
            {
                query += $"first_name LIKE '%{StudentFirstNameFilter.Text}%' AND ";
                filterApplied = true ;
            }
            if (StudentLastNameFilter.Text != "")
            {
                query += $"last_name LIKE '%{StudentLastNameFilter.Text}%' AND ";
                filterApplied = true;
            }
            if (StudentAgeFilter.Text != "")
            {
                query += $"age LIKE '%{StudentAgeFilter.Text}%' AND ";
                filterApplied = true;
            }
            if (StudentIsGraduateFilter.IsChecked == true)
            {
                query += $"graduate = 1 AND ";
                filterApplied = true;
            }
            if (StudentEmailFilter.Text != "")
            {
                query += $"contact_email LIKE '%{StudentEmailFilter.Text}%' AND ";
                filterApplied = true;
            }
            if (StudentPhoneFilter.Text != "")
            {
                query += $"contact_phone LIKE '%{StudentPhoneFilter.Text}%' AND ";
                filterApplied = true;
            }
            if (StudentAvailabilityFilter.Text != "")
            {
                query += $"availability LIKE '%{StudentAvailabilityFilter.Text}%' AND ";
                filterApplied = true;
            }

            if (!filterApplied)
            {
                query = "SELECT students.*, GROUP_CONCAT(courses.course_name SEPARATOR ', ') AS enrolled_courses " +
                "FROM students " +
                "LEFT JOIN enrollments ON students.student_id = enrollments.student_id " +
                "LEFT JOIN courses ON enrollments.course_id = courses.course_id " +
                "GROUP BY students.student_id";
            }
            else
            {
                query = query.Remove(query.Length - 5);
                query += " GROUP BY students.student_id";
            }

            MySqlCommand studentsCmd = new(query, connection);
            DataTable studentsDataTable = new();
            studentsDataTable.Load(studentsCmd.ExecuteReader());
            CapitalizeHeaders(studentsDataTable);

            StudentsGrid.ItemsSource = studentsDataTable.DefaultView;
        }

        // Method to clear the filters for the ParentsGrid.
        private void ParentClearButton_Click(object sender, RoutedEventArgs e)
        {
            ParentFirstNameFilter.Text = "";
            ParentLastNameFilter.Text = "";
            ParentStudentIdFilter.Text = "";
            ParentEmailFilter.Text = "";
            ParentPhoneFilter.Text = "";

            RefreshParentsGrid();
        }

        // Method to filter the ParentsGrid.
        private void ParentFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM parents WHERE ";

            if (ParentFirstNameFilter.Text != "")
            {
                query += $"first_name LIKE '%{ParentFirstNameFilter.Text}%' AND ";
            }
            if (ParentLastNameFilter.Text != "")
            {
                query += $"last_name LIKE '%{ParentLastNameFilter.Text}%' AND ";
            }
            if (ParentStudentIdFilter.Text != "")
            {
                query += $"student_id LIKE '%{ParentStudentIdFilter.Text}%' AND ";
            }
            if (ParentEmailFilter.Text != "")
            {
                query += $"email LIKE '%{ParentEmailFilter.Text}%' AND ";
            }
            if (ParentPhoneFilter.Text != "")
            {
                query += $"phone LIKE '%{ParentPhoneFilter.Text}%' AND ";
            }

            query = query.Remove(query.Length - 5);

            MySqlCommand parentsCmd = new(query, connection);
            DataTable parentsDataTable = new();
            parentsDataTable.Load(parentsCmd.ExecuteReader());
            CapitalizeHeaders(parentsDataTable);

            ParentsGrid.ItemsSource = parentsDataTable.DefaultView;
        }

        // Method to clear the filters for the CoursesGrid.
        private void CourseClearButton_Click(object sender, RoutedEventArgs e)
        {
            CourseNameFilter.Text = "";
            CourseTeacherIdFilter.Text = "";
            CourseScheduleFilter.Text = "";
            CourseStatusFilter.IsChecked = false;

            RefreshCoursesGrid();
        }

        // Method to filter the CoursesGrid.
        private void CourseFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM courses WHERE ";

            if (CourseNameFilter.Text != "")
            {
                query += $"course_name LIKE '%{CourseNameFilter.Text}%' AND ";
            }
            if (CourseTeacherIdFilter.Text != "")
            {
                query += $"teacher_id LIKE '%{CourseTeacherIdFilter.Text}%' AND ";
            }
            if (CourseScheduleFilter.Text != "")
            {
                query += $"schedule LIKE '%{CourseScheduleFilter.Text}%' AND ";
            }
            if (CourseStatusFilter.IsChecked == true)
            {
                query += $"status = 1 AND ";
            }

            query = query.Remove(query.Length - 5);

            MySqlCommand coursesCmd = new(query, connection);
            DataTable coursesDataTable = new();
            coursesDataTable.Load(coursesCmd.ExecuteReader());
            CapitalizeHeaders(coursesDataTable);

            CoursesGrid.ItemsSource = coursesDataTable.DefaultView;
        }

        // Method to clear the filters for the ItemsGrid.
        private void ItemClearButton_Click(object sender, RoutedEventArgs e)
        {
            ItemNameFilter.Text = "";
            ItemQuantityFilter.Text = "";
            ItemDescriptionFilter.Text = "";
            ItemCourseIdFilter.Text = "";

            RefreshItemsGrid();
        }

        // Method to filter the ItemsGrid.
        private void ItemFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT items.*, GROUP_CONCAT(courses.course_id SEPARATOR ', ') AS course_id " +
                "FROM items " +
                "LEFT JOIN course_item ON items.item_id = course_item.item_id " +
                "LEFT JOIN courses ON course_item.course_id = courses.course_id " +
                "WHERE ";

            bool filterApplied = false;

            if (ItemNameFilter.Text != "")
            {
                query += $"item_name LIKE '%{ItemNameFilter.Text}%' AND ";
                filterApplied = true;
            }
            if (ItemQuantityFilter.Text != "")
            {
                query += $"quantity LIKE '%{ItemQuantityFilter.Text}%' AND ";
                filterApplied = true;
            }
            if (ItemDescriptionFilter.Text != "")
            {
                query += $"item_description LIKE '%{ItemDescriptionFilter.Text}%' AND ";
                filterApplied = true;
            }
            if (ItemCourseIdFilter.Text != "")
            {
                query += $"courses.course_id LIKE '%{ItemCourseIdFilter.Text}%' AND ";
                filterApplied = true;
            }

            if (!filterApplied)
            {
                query = "SELECT items.*, GROUP_CONCAT(courses.course_name SEPARATOR ', ') AS course " +
                "FROM items " +
                "LEFT JOIN course_item ON items.item_id = course_item.item_id " +
                "LEFT JOIN courses ON course_item.course_id = courses.course_id " +
                "GROUP BY items.item_id";
            }
            else
            {
                query = query.Remove(query.Length - 5);
                query += " GROUP BY items.item_id";
            }

            MySqlCommand itemsCmd = new(query, connection);
            DataTable itemsDataTable = new();
            itemsDataTable.Load(itemsCmd.ExecuteReader());
            CapitalizeHeaders(itemsDataTable);

            ItemsGrid.ItemsSource = itemsDataTable.DefaultView;
        }

        // Method to clear the filters for the IncomesGrid.
        private void IncomeClearButton_Click(object sender, RoutedEventArgs e)
        {
            IncomeTypeFilter.Text = "";
            IncomeAmountFilter.Text = "";
            IncomeDateFilter.Text = "";
            IncomeDescriptionFilter.Text = "";

            RefreshIncomesGrid();
        }

        // Method to filter the IncomesGrid.
        private void IncomeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM incomes WHERE ";

            if (IncomeTypeFilter.Text != "")
            {
                query += $"income_type LIKE '%{IncomeTypeFilter.Text}%' AND ";
            }
            if (IncomeAmountFilter.Text != "")
            {
                query += $"income_amount LIKE '%{IncomeAmountFilter.Text}%' AND ";
            }
            if (IncomeDateFilter.Text != "")
            {
                query += $"income_date LIKE '%{DateTime.Parse(IncomeDateFilter.Text).ToString("yyyy-MM-dd")}' AND ";
            }
            if (IncomeDescriptionFilter.Text != "")
            {
                query += $"income_description LIKE '%{IncomeDescriptionFilter.Text}%' AND ";
            }

            query = query.Remove(query.Length - 5);

            MySqlCommand incomesCmd = new(query, connection);
            DataTable incomesDataTable = new();
            incomesDataTable.Load(incomesCmd.ExecuteReader());
            CapitalizeHeaders(incomesDataTable);

            IncomesGrid.ItemsSource = incomesDataTable.DefaultView;
        }

        // Method to clear the filters for the ExpensesGrid.
        private void ExpenseClearButton_Click(object sender, RoutedEventArgs e)
        {
            ExpenseTypeFilter.Text = "";
            ExpenseAmountFilter.Text = "";
            ExpenseDateFilter.Text = "";
            ExpenseDescriptionFilter.Text = "";

            RefreshExpensesGrid();
        }

        // Method to filter the ExpensesGrid.
        private void ExpenseFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM expenses WHERE ";

            if (ExpenseTypeFilter.Text != "")
            {
                query += $"expense_type LIKE '%{ExpenseTypeFilter.Text}%' AND ";
            }
            if (ExpenseAmountFilter.Text != "")
            {
                query += $"expense_amount LIKE '%{ExpenseAmountFilter.Text}%' AND ";
            }
            if (ExpenseDateFilter.Text != "")
            {
                query += $"expense_date LIKE '%{DateTime.Parse(ExpenseDateFilter.Text).ToString("yyyy-MM-dd")}' AND ";
            }
            if (ExpenseDescriptionFilter.Text != "")
            {
                query += $"expense_description LIKE '%{ExpenseDescriptionFilter.Text}%' AND ";
            }

            query = query.Remove(query.Length - 5);

            MySqlCommand expensesCmd = new(query, connection);
            DataTable expensesDataTable = new();
            expensesDataTable.Load(expensesCmd.ExecuteReader());
            CapitalizeHeaders(expensesDataTable);

            ExpensesGrid.ItemsSource = expensesDataTable.DefaultView;
        }

        private void CourseScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            ScheduleSelectorDialog scheduleSelectorDialog = new();
            bool? result = scheduleSelectorDialog.ShowDialog();

            if (result == true)
                CourseScheduleFilter.Text = scheduleSelectorDialog.Schedule;
        }

        // Method to delete a row from the TeachersGrid.
        private void TeachersGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the teacher ID of the teacher that was deleted.
                int teacherId = (int)((DataRowView)TeachersGrid.SelectedItem).Row.ItemArray[0];

                // Delete the teacher from the database.
                string teacherDeleteQuery = $"DELETE FROM teachers WHERE teacher_id = @teacher_id";
                MySqlCommand deleteCommand = new(teacherDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@teacher_id", teacherId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                // Delete the employee from the database.
                string employeeDeleteQuery = $"DELETE FROM employees WHERE employee_id = @employee_id";
                MySqlCommand deleteEmployeeCommand = new(employeeDeleteQuery, connection);
                deleteEmployeeCommand.Parameters.AddWithValue("@employee_id", teacherId);

                rowsAffected += deleteEmployeeCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
            else if (e.Key == Key.Pause)
            {
                ScheduleViewerDialog scheduleViewerDialog = new();
                string schedule = ((DataRowView)TeachersGrid.SelectedItem).Row.ItemArray[7].ToString();
                Debug.WriteLine(schedule);
                scheduleViewerDialog.Schedule = schedule;
                scheduleViewerDialog.populateSchedule();
                scheduleViewerDialog.ShowDialog();
            }
        }

        // Method to delete a row from the JanitorsGrid.
        private void JanitorsGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the janitor ID of the janitor that was deleted.
                int janitorId = (int)((DataRowView)JanitorsGrid.SelectedItem).Row.ItemArray[0];

                // Delete the janitor from the database.
                string janitorDeleteQuery = $"DELETE FROM janitors WHERE janitor_id = @janitor_id";
                MySqlCommand deleteCommand = new(janitorDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@janitor_id", janitorId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                // Delete the employee from the database.
                string employeeDeleteQuery = $"DELETE FROM employees WHERE employee_id = @employee_id";
                MySqlCommand deleteEmployeeCommand = new(employeeDeleteQuery, connection);
                deleteEmployeeCommand.Parameters.AddWithValue("@employee_id", janitorId);

                rowsAffected += deleteEmployeeCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        // Method to delete a row from the AdministrativeEmployeesGrid.
        private void AdministrativeEmployeesGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the administrative employee ID of the administrative employee that was deleted.
                int administrativeEmployeeId = (int)((DataRowView)AdministrativeEmployeesGrid.SelectedItem).Row.ItemArray[0];

                // Delete the administrative employee from the database.
                string administrativeEmployeeDeleteQuery = $"DELETE FROM administrative_employees WHERE administrative_employee_id = @administrative_employee_id";
                MySqlCommand deleteCommand = new(administrativeEmployeeDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@administrative_employee_id", administrativeEmployeeId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                // Delete the employee from the database.
                string employeeDeleteQuery = $"DELETE FROM employees WHERE employee_id = @employee_id";
                MySqlCommand deleteEmployeeCommand = new(employeeDeleteQuery, connection);
                deleteEmployeeCommand.Parameters.AddWithValue("@employee_id", administrativeEmployeeId);

                rowsAffected += deleteEmployeeCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        // Method to delete a row from the StudentsGrid.
        private void StudentsGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the student ID of the student that was deleted.
                int studentId = (int)((DataRowView)StudentsGrid.SelectedItem).Row.ItemArray[0];

                // Delete the student from the database.
                string studentDeleteQuery = $"DELETE FROM students WHERE student_id = @student_id";
                MySqlCommand deleteCommand = new(studentDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@student_id", studentId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                // Delete the parent from the database.
                string parentDeleteQuery = $"DELETE FROM parents WHERE student_id = @student_id";
                MySqlCommand deleteParentCommand = new(parentDeleteQuery, connection);
                deleteParentCommand.Parameters.AddWithValue("@student_id", studentId);

                rowsAffected += deleteParentCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
            else if (e.Key == Key.Pause)
            {
                ScheduleViewerDialog scheduleViewerDialog = new();
                string schedule = ((DataRowView)StudentsGrid.SelectedItem).Row.ItemArray[9].ToString();
                Debug.WriteLine(schedule);
                scheduleViewerDialog.Schedule = schedule;
                scheduleViewerDialog.populateSchedule();
                scheduleViewerDialog.ShowDialog();
            }
        }

        // Method to delete a row from the ParentsGrid.
        private void ParentsGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the parent ID of the parent that was deleted.
                int parentId = (int)((DataRowView)ParentsGrid.SelectedItem).Row.ItemArray[0];

                // Delete the parent from the database.
                string parentDeleteQuery = $"DELETE FROM parents WHERE parent_id = @parent_id";
                MySqlCommand deleteCommand = new(parentDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@parent_id", parentId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        // Method to delete a row from the CoursesGrid.
        private void CoursesGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the course ID of the course that was deleted.
                int courseId = (int)((DataRowView)CoursesGrid.SelectedItem).Row.ItemArray[0];

                // Delete the course from the database.
                string courseDeleteQuery = $"DELETE FROM courses WHERE course_id = @course_id";
                MySqlCommand deleteCommand = new(courseDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@course_id", courseId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                // Delete the enrollment from the database.
                string enrollmentDeleteQuery = $"DELETE FROM enrollments WHERE course_id = @course_id";
                MySqlCommand deleteEnrollmentCommand = new(enrollmentDeleteQuery, connection);
                deleteEnrollmentCommand.Parameters.AddWithValue("@course_id", courseId);

                rowsAffected += deleteEnrollmentCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        // Method to delete a row from the ItemsGrid.
        private void ItemsGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the item ID of the item that was deleted.
                int itemId = (int)((DataRowView)ItemsGrid.SelectedItem).Row.ItemArray[0];

                // Delete the item from the database.
                string itemDeleteQuery = $"DELETE FROM items WHERE item_id = @item_id";
                MySqlCommand deleteCommand = new(itemDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@item_id", itemId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                // Delete the course_item from the database.
                string courseItemDeleteQuery = $"DELETE FROM course_item WHERE item_id = @item_id";
                MySqlCommand deleteCourseItemCommand = new(courseItemDeleteQuery, connection);
                deleteCourseItemCommand.Parameters.AddWithValue("@item_id", itemId);

                rowsAffected += deleteCourseItemCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        // Method to delete a row from the IncomesGrid.
        private void IncomesGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the income ID of the income that was deleted.
                int incomeId = (int)((DataRowView)IncomesGrid.SelectedItem).Row.ItemArray[0];

                // Delete the income from the database.
                string incomeDeleteQuery = $"DELETE FROM incomes WHERE income_id = @income_id";
                MySqlCommand deleteCommand = new(incomeDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@income_id", incomeId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        // Method to delete a row from the ExpensesGrid.
        private void ExpensesGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the expense ID of the expense that was deleted.
                int expenseId = (int)((DataRowView)ExpensesGrid.SelectedItem).Row.ItemArray[0];

                // Delete the expense from the database.
                string expenseDeleteQuery = $"DELETE FROM expenses WHERE expense_id = @expense_id";
                MySqlCommand deleteCommand = new(expenseDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@expense_id", expenseId);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        // Method to delete a row from the DeletedEmployees.
        private void DeletedEmployeesGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Get the teacher ID of the DeletedEmployees
                int deletedEmployeeID = (int)((DataRowView)DeletedEmployeesGrid.SelectedItem).Row.ItemArray[0];

                string deletedEmployeeDeleteQuery = $"DELETE FROM deletedemployees WHERE employee_id = @employee_id";
                MySqlCommand deleteCommand = new(deletedEmployeeDeleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@employee_id", deletedEmployeeID);

                int rowsAffected = deleteCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Refresh all the grids with the updated data.
                    RefreshAllGrids();
                }
            }
        }

        private static void ConfigureDatabase(string databaseName)
        {
            // Create the database and tables if it doesn't exist.
            string createDatabaseQuery = $"CREATE DATABASE IF NOT EXISTS {databaseName}";
            MySqlCommand createDatabaseCommand = new(createDatabaseQuery, connection);
            createDatabaseCommand.ExecuteNonQuery();

            // Select the database.
            string selectDatabaseQuery = $"USE {databaseName}";
            MySqlCommand selectDatabaseCommand = new(selectDatabaseQuery, connection);
            selectDatabaseCommand.ExecuteNonQuery();

            // Create the employees table if it doesn't exist.
            string createEmployeesTableQuery = "CREATE TABLE IF NOT EXISTS employees (" +
                "employee_id int auto_increment primary key," +
                "first_name varchar(50)," +
                "last_name varchar(50)," +
                "email varchar(100)," +
                "phone varchar(20)," +
                "hire_date date," +
                "salary int," +
                "is_full_time boolean," +
                "availability varchar(255)" +
                ")";
            MySqlCommand createEmployeesTableCommand = new(createEmployeesTableQuery, connection);
            createEmployeesTableCommand.ExecuteNonQuery();

            // Create the teachers table if it doesn't exist.
            string createTeachersTableQuery = "CREATE TABLE IF NOT EXISTS teachers (" +
                "teacher_id int auto_increment primary key," +
                "first_name varchar(50)," +
                "last_name varchar(50)," +
                "email varchar(100)," +
                "phone varchar(20)," +
                "specialty varchar(50)," +
                "foreign key (teacher_id) references Employees(employee_id) on delete cascade on update cascade" +
                ")";
            MySqlCommand createTeachersTableCommand = new(createTeachersTableQuery, connection);
            createTeachersTableCommand.ExecuteNonQuery();

            // Create the janitors table if it doesn't exist.
            string createJanitorsTableQuery = "CREATE TABLE IF NOT EXISTS janitors (" +
                "janitor_id int auto_increment primary key," +
                "first_name varchar(50)," +
                "last_name varchar(50)," +
                "email varchar(100)," +
                "phone varchar(20)," +
                "foreign key (janitor_id) references Employees(employee_id) on delete cascade on update cascade" +
                ")";
            MySqlCommand createJanitorsTableCommand = new(createJanitorsTableQuery, connection);
            createJanitorsTableCommand.ExecuteNonQuery();

            // Create the administrative employees table if it doesn't exist.
            string createAdministrativeEmployeesTableQuery = "CREATE TABLE IF NOT EXISTS administrative_employees (" +
                "administrative_employee_id int auto_increment primary key," +
                "first_name varchar(50)," +
                "last_name varchar(50)," +
                "email varchar(100)," +
                "phone varchar(20)," +
                "department varchar(50)," +
                "foreign key (administrative_employee_id) references Employees(employee_id) on delete cascade on update cascade" +
                ")";
            MySqlCommand createAdministrativeEmployeesTableCommand = new(createAdministrativeEmployeesTableQuery, connection);
            createAdministrativeEmployeesTableCommand.ExecuteNonQuery();

            // Create the students table if it doesn't exist.
            string createStudentsTableQuery = "CREATE TABLE IF NOT EXISTS students (" +
                "student_id int auto_increment primary key," +
                "first_name varchar(50)," +
                "last_name varchar(50)," +
                "age int," +
                "graduate boolean," +
                "contact_email varchar(100)," +
                "contact_phone varchar(20)," +
                "availability varchar(255)" +
                ")";
            MySqlCommand createStudentsTableCommand = new(createStudentsTableQuery, connection);
            createStudentsTableCommand.ExecuteNonQuery();

            // Create the parents table if it doesn't exist.
            string createParentsTableQuery = "CREATE TABLE IF NOT EXISTS parents (" +
                "parent_id int auto_increment primary key," +
                "first_name varchar(50)," +
                "last_name varchar(50)," +
                "student_id int," +
                "contact_email varchar(100)," +
                "contact_phone varchar(20)," +
                "foreign key (student_id) references Students(student_id) on delete cascade on update cascade" +
                ")";
            MySqlCommand createParentsTableCommand = new(createParentsTableQuery, connection);
            createParentsTableCommand.ExecuteNonQuery();

            // Create the courses table if it doesn't exist.
            string createCoursesTableQuery = "CREATE TABLE IF NOT EXISTS courses (" +
                "course_id int auto_increment primary key," +
                "course_name varchar(50)," +
                "teacher_id int," +
                "schedule varchar(255)," +
                "status boolean," +
                "foreign key (teacher_id) references Teachers(teacher_id) on delete cascade on update cascade" +
                ")";
            MySqlCommand createCoursesTableCommand = new(createCoursesTableQuery, connection);
            createCoursesTableCommand.ExecuteNonQuery();

            // Create the items table if it doesn't exist.
            string createItemsTableQuery = "CREATE TABLE IF NOT EXISTS items (" +
                "item_id int auto_increment primary key," +
                "item_name varchar(50)," +
                "quantity int," +
                "item_description varchar(255)" +
                ")";
            MySqlCommand createItemsTableCommand = new(createItemsTableQuery, connection);
            createItemsTableCommand.ExecuteNonQuery();

            // Create the enrollments table if it doesn't exist.
            string createEnrollmentsTableQuery = "CREATE TABLE IF NOT EXISTS enrollments (" +
                "enrollment_id int auto_increment primary key," +
                "student_id int," +
                "course_id int," +
                "foreign key (student_id) references Students(student_id) on delete cascade on update cascade," +
                "foreign key (course_id) references Courses(course_id) on delete cascade on update cascade" +
                ")";
            MySqlCommand createEnrollmentsTableCommand = new(createEnrollmentsTableQuery, connection);
            createEnrollmentsTableCommand.ExecuteNonQuery();

            // Create the course_item table if it doesn't exist.
            string createCourseItemTableQuery = "CREATE TABLE IF NOT EXISTS course_item (" +
                "course_item_id int auto_increment primary key," +
                "course_id int," +
                "item_id int," +
                "foreign key (course_id) references Courses(course_id) on delete cascade on update cascade," +
                "foreign key (item_id) references Items(item_id) on delete cascade on update cascade" +
                ")";

            MySqlCommand createCourseItemTableCommand = new(createCourseItemTableQuery, connection);
            createCourseItemTableCommand.ExecuteNonQuery();

            // Create the incomes table if it doesn't exist.
            string createIncomesTableQuery = "CREATE TABLE IF NOT EXISTS incomes (" +
                "income_id int auto_increment primary key," +
                "income_type varchar(50)," +
                "income_amount float," +
                "income_date date," +
                "income_description varchar(255)" +
                ")";
            MySqlCommand createIncomesTableCommand = new(createIncomesTableQuery, connection);
            createIncomesTableCommand.ExecuteNonQuery();

            // Create the expenses table if it doesn't exist.
            string createExpensesTableQuery = "CREATE TABLE IF NOT EXISTS expenses (" +
                "expense_id int auto_increment primary key," +
                "expense_type varchar(50)," +
                "expense_amount float," +
                "expense_date date," +
                "expense_description varchar(255)" +
                ")";
            MySqlCommand createExpensesTableCommand = new(createExpensesTableQuery, connection);
            createExpensesTableCommand.ExecuteNonQuery();

            // Create the deletedemployees table if it doesn't exist.
            string createDeletedEmployeesTableQuery = "CREATE TABLE IF NOT EXISTS deletedemployees (" +
                "employee_id int auto_increment primary key," +
                "first_name varchar(50)," +
                "last_name varchar(50)," +
                "email varchar(100)," +
                "phone varchar(20)," +
                "hire_date date," +
                "salary int," +
                "is_full_time boolean," +
                "availability varchar(255)" +
                ")";
            MySqlCommand createDeletedEmployeesTableCommand = new(createDeletedEmployeesTableQuery, connection);
            createDeletedEmployeesTableCommand.ExecuteNonQuery();

            
            string totalMoneyTrafficTableQuery = "CREATE TABLE IF NOT EXISTS totalmoneytraffic(" +
                "total_id int auto_increment primary key, " +
                "total_income float, " +
                "total_expense float" +
                ")";
            MySqlCommand totalMoneyTrafficTableCommand = new(totalMoneyTrafficTableQuery, connection);
            totalMoneyTrafficTableCommand.ExecuteNonQuery();

            string checkDataQuery = "SELECT COUNT(*) FROM totalmoneytraffic";
            MySqlCommand checkDataCommand = new MySqlCommand(checkDataQuery, connection);

            int rowCount = Convert.ToInt32(checkDataCommand.ExecuteScalar());

            if (rowCount == 0)
            {
                string insertDataQuery = "INSERT INTO totalmoneytraffic (total_income, total_expense) VALUES (0, 0)";
                MySqlCommand insertDataCommand = new MySqlCommand(insertDataQuery, connection);
                insertDataCommand.ExecuteNonQuery();
                Console.WriteLine("Initial data inserted into totalmoneytraffic table.");
            }

            string createtr_EmployeeRemovedQuery =
                "CREATE TRIGGER IF NOT EXISTS tr_EmployeeRemoved " +
                "BEFORE DELETE ON employees " +
                "FOR EACH ROW " +
                "BEGIN " +
                "   INSERT INTO deletedemployees (employee_id, first_name, last_name, email, phone, hire_date, salary, is_full_time, availability) " +
                "   VALUES (OLD.employee_id, OLD.first_name, OLD.last_name, OLD.email, OLD.phone, OLD.hire_date, OLD.salary, OLD.is_full_time, OLD.availability); " +
                "END;";
            MySqlCommand createtr_EmployeeRemovedCommand = new MySqlCommand(createtr_EmployeeRemovedQuery, connection);
            createtr_EmployeeRemovedCommand.ExecuteNonQuery();


            string createtr_EmployeeRecycleQuery =
                "CREATE TRIGGER IF NOT EXISTS tr_EmployeeRecyle " +
                "BEFORE DELETE ON deletedemployees " +
                "FOR EACH ROW " +
                "BEGIN " +
                "   INSERT INTO employees (employee_id, first_name, last_name, email, phone, hire_date, salary, is_full_time, availability) " +
                "   VALUES (OLD.employee_id, OLD.first_name, OLD.last_name, OLD.email, OLD.phone, OLD.hire_date, OLD.salary, OLD.is_full_time, OLD.availability); " +
                "END;";
            MySqlCommand createtr_EmployeeRecycleCommand = new MySqlCommand(createtr_EmployeeRecycleQuery, connection);
            createtr_EmployeeRecycleCommand.ExecuteNonQuery();
            

            string createtr_after_income_insertQuery =
                "CREATE TRIGGER IF NOT EXISTS after_income_insert " +
                "AFTER INSERT ON incomes " +
                "FOR EACH ROW " +
                "BEGIN " +
                "   UPDATE totalmoneytraffic " +
                "   SET total_income = total_income + NEW.income_amount; " +
                "END;";
            MySqlCommand createtr_after_income_insertCommand = new MySqlCommand(createtr_after_income_insertQuery, connection);
            createtr_after_income_insertCommand.ExecuteNonQuery();

            string createtr_after_income_deleteQuery =
                "CREATE TRIGGER IF NOT EXISTS after_income_delete " +
                "AFTER DELETE ON incomes " +
                "FOR EACH ROW " +
                "BEGIN " +
                "   UPDATE totalmoneytraffic " +
                "   SET total_income = total_income - OLD.income_amount; " +
                "END;";
            MySqlCommand createtr_after_income_deleteCommand = new MySqlCommand(createtr_after_income_deleteQuery, connection);
            createtr_after_income_deleteCommand.ExecuteNonQuery();

            string createtr_after_expense_insertQuery =
                "CREATE TRIGGER IF NOT EXISTS after_expense_insert " +
                "AFTER INSERT ON expenses " +
                "FOR EACH ROW " +
                "BEGIN " +
                "   UPDATE totalmoneytraffic " +
                "   SET total_expense = total_expense + NEW.expense_amount; " +
                "END;";
            MySqlCommand createtr_after_expense_inserCommand = new MySqlCommand(createtr_after_expense_insertQuery, connection);
            createtr_after_expense_inserCommand.ExecuteNonQuery();

            string createtr_after_expense_deleteQuery =
                "CREATE TRIGGER IF NOT EXISTS after_expense_delete " +
                "AFTER DELETE ON expenses " +
                "FOR EACH ROW " +
                "BEGIN " +
                "   UPDATE totalmoneytraffic " +
                "   SET total_expense = total_expense - OLD.expense_amount; " +
                "END;";
            MySqlCommand createtr_after_expense_deleteCommand = new MySqlCommand(createtr_after_expense_deleteQuery, connection);
            createtr_after_expense_deleteCommand.ExecuteNonQuery();
        }
    }
}
