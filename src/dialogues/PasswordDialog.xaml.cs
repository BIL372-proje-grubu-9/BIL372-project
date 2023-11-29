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

namespace WpfApp1.src.dialogues
{
    /// <summary>
    /// Interaction logic for PasswordDialog.xaml
    /// </summary>
    public partial class PasswordDialog : Window
    {
        public string Password { get; set; }
        public PasswordDialog()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input (you can add more validation if needed).
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                MessageBox.Show("Please enter the password.");
                return;
            }

            // Set the Password property with the entered name.
            Password = passwordBox.Password;

            // Close the dialog box.
            DialogResult = true;
        }
    }
}
