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
    /// Interaction logic for ItemDialog.xaml
    /// </summary>
    public partial class ItemDialog : Window
    {
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public string ItemDescription { get; set; }
        public int ItemCourseId { get; set; }
        public ItemDialog()
        {
            InitializeComponent();
            PopulateCourseIdComboBox();
            ItemName = string.Empty;
            ItemQuantity = 0;
            ItemDescription = string.Empty;
            ItemCourseId = 0;
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(ItemNameTextBox.Text))
            {
                MessageBox.Show("Please enter the item's name.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ItemQuantityTextBox.Text))
            {
                MessageBox.Show("Please enter the item's quantity.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ItemCourseIdComboBox.Text))
            {
                MessageBox.Show("Please enter the item's course id.");
                return;
            }

            // Set the ItemName and ItemQuantity property with the entered name.
            ItemName = ItemNameTextBox.Text;
            ItemQuantity = int.Parse(ItemQuantityTextBox.Text);
            ItemDescription = ItemDescriptionTextBox.Text;
            ItemCourseId = int.Parse(ItemCourseIdComboBox.Text.ToLower().Split("id: ")[1].Replace(")", ""));

            // Close the dialog box and return control to the calling window.
            DialogResult = true;
        }
        private void PopulateCourseIdComboBox()
        {
            // Populate the CourseIdComboBox with the list of courses.
            string query = "SELECT course_name, course_id FROM courses";
            MySqlCommand cmd = new(query, MainWindow.connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ItemCourseIdComboBox.Items.Add(reader[0] + " (id: " + reader[1] + ")");
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
