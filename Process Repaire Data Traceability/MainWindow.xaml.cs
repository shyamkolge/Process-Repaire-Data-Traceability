using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Process_Repaire_Data_Traceability
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=SHYAM\\SQLEXPRESS;Initial Catalog=ModiInnovations;Integrated Security=True;";
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            string userName = tbUserName.Text;
            string password = tbPassword.Text;

            string query = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName AND Password = @Password";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@Password", password);

            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
            {
                // Valid credentials, allow user to proceed
                HomePage homePage = new HomePage();
                homePage.WindowState = WindowState.Maximized;
                homePage.Show();

                // Close current login window
                this.Close();
            }
            else
            {
                // Invalid credentials, display error message
    
                MessageBox.Show("Invalid Username or Password", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            connection.Close();


        }

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}