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

namespace Process_Repaire_Data_Traceability
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
         

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UserMaster userMaster = new UserMaster();
            userMaster.Show();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DepartmentMaster departmentMaster = new DepartmentMaster();
            departmentMaster.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ActionModel actionModel = new ActionModel();
            actionModel.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DefectMaster defectMaster = new DefectMaster(); 
            defectMaster.Show();
        }
    }
}
