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
    /// Interaction logic for ScheduleSelectorDialog.xaml
    /// </summary>
    public partial class ScheduleSelectorDialog : Window
    {
        public CheckBox[] checkboxes;
        public string Schedule { get; set; }
        public ScheduleSelectorDialog()
        {
            InitializeComponent();
            checkboxes = new CheckBox[] { checkbox0, checkbox1, checkbox2, checkbox3, checkbox4, checkbox5, checkbox6,
                checkbox7, checkbox8, checkbox9, checkbox10, checkbox11, checkbox12, checkbox13,
                checkbox14, checkbox15, checkbox16, checkbox17, checkbox18, checkbox19, checkbox20,
                checkbox21, checkbox22, checkbox23, checkbox24, checkbox25, checkbox26, checkbox27,
                checkbox28, checkbox29, checkbox30, checkbox31, checkbox32, checkbox33, checkbox34,
                checkbox35, checkbox36, checkbox37, checkbox38, checkbox39, checkbox40, checkbox41,
                checkbox42, checkbox43, checkbox44, checkbox45, checkbox46, checkbox47, checkbox48,
                checkbox49, checkbox50, checkbox51, checkbox52, checkbox53, checkbox54, checkbox55,
                checkbox56, checkbox57, checkbox58, checkbox59, checkbox60, checkbox61, checkbox62,
                checkbox63, checkbox64, checkbox65, checkbox66, checkbox67, checkbox68, checkbox69,
                checkbox70, checkbox71, checkbox72, checkbox73, checkbox74, checkbox75, checkbox76};
            Schedule = String.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Schedule = "";
            for (int i = 0; i < checkboxes.Length; i++)
            {
                if (checkboxes[i].IsChecked == true)
                {
                    Schedule += checkboxes[i].Name.Substring(8) + ",";
                }
                Schedule = Schedule.TrimEnd(',');
            }
            DialogResult = true;
        }
    }
}
