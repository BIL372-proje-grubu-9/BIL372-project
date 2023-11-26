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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string employeesQuery = "SELECT * FROM employees";
        private const string teachersQuery = "SELECT * FROM teachers";
        private const string administraiveQuery = "SELECT * FROM administrative_employees";
        private const string janitorsQuery = "SELECT * FROM janitors";
        private const string studentsQuery = 
            "SELECT students.*, GROUP_CONCAT(courses.course_name SEPARATOR ', ') AS enrolled_courses " +
            "FROM students " +
            "LEFT JOIN enrollments ON students.student_id = enrollments.student_id " +
            "LEFT JOIN courses ON enrollments.course_id = courses.course_id " +
            "GROUP BY students.student_id";
        private const string parentsQuery = "SELECT * FROM parents";
        private const string coursesQuery = "SELECT * FROM courses";
        private const string itemsQuery = "SELECT items.*, GROUP_CONCAT(courses.course_id SEPARATOR ', ') AS course_id " +
            "FROM items " +
            "LEFT JOIN course_item ON items.item_id = course_item.item_id " +
            "LEFT JOIN courses ON course_item.course_id = courses.course_id " +
            "GROUP BY items.item_id";
        private const string incomesQuery = "SELECT * FROM incomes";
        private const string expensesQuery = "SELECT * FROM expenses";
        private const string connectionString = "Server=localhost;Database=project;PASSWORD=1234;UID=root;";
        public static readonly MySqlConnection connection = new(connectionString);
        public MainWindow()
        {
            InitializeComponent();
            connection.Open();

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
                    // Refresh the employees and teachers grid with the updated data.
                    RefreshEmployeesGrid();
                    RefreshTeachersGrid();
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
                    // Refresh the employees and janitors grid with the updated data.
                    RefreshEmployeesGrid();
                    RefreshJanitorsGrid();
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
                    // Refresh the employees and administrative employees grid with the updated data.
                    RefreshEmployeesGrid();
                    RefreshAdministrativeEmployeesGrid();
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
                    // Refresh the parents grid with the updated data.
                    RefreshParentsGrid();
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
                    // Refresh the items grid with the updated data.
                    RefreshItemsGrid();
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
                    // Refresh the incomes grid with the updated data.
                    RefreshIncomesGrid();
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
                    // Refresh the expenses grid with the updated data.
                    RefreshExpensesGrid();
                }
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
            TeacherAvailabilityFilter.Text = "";

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
            if (TeacherAvailabilityFilter.Text != "")
            {
                query += $"availability LIKE '%{TeacherAvailabilityFilter.Text}%' AND ";
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
    }
}
