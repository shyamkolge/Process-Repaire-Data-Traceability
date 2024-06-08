using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace Process_Repaire_Data_Traceability
{
    /// <summary>
    /// Interaction logic for UserMaster.xaml
    /// </summary>
    public partial class UserMaster : Window
    {

        string connectionString = "Data Source=SHYAM\\SQLEXPRESS;Initial Catalog=ModiInnovations;Integrated Security=True;Encrypt=false;";// Replace with your actual connection string

        public UserMaster()
        {
            InitializeComponent();
            LoadGridData();
        }

        public bool isValid()
        {

            if (Username_txt.Text == String.Empty)
            {
                MessageBox.Show("Username is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Firstname_txt.Text == String.Empty)
            {
                MessageBox.Show("Firstnmae is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Lastname_txt.Text == String.Empty)
            {
                MessageBox.Show("Lastname is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (password_txt.Password == String.Empty)
            {
                MessageBox.Show("Password is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }


        public void LoadGridData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserModel", conn);
                DataTable dataTable = new DataTable();

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
                conn.Close();

                dataGrid.ItemsSource = dataTable.DefaultView;
            }

        }

        public void clearData()
        {
            UserId_txt.Clear();
            Firstname_txt.Clear();
            Username_txt.Clear();
            password_txt.Clear();
            Lastname_txt.Clear();
            ActiveRadioButton.IsChecked = false;
            InactiveRadioButton.IsChecked = false;

        }


        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                UserId_txt.Text = row["Id"].ToString();
                Firstname_txt.Text = row["FirstName"].ToString();
                Lastname_txt.Text = row["LastName"].ToString();
                Username_txt.Text = row["UserName"].ToString();
                password_txt.Password = row["Password"].ToString();
                if (row["Status"].ToString() == "Active")
                {
                    ActiveRadioButton.IsChecked = true;
                }
                else
                {
                    InactiveRadioButton.IsChecked = true;
                }

                save_btn.Content = "Update";
                UserId_txt.IsEnabled = false;
            }

        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            // Read input values
            string firstName = Firstname_txt.Text;
            string lastName = Lastname_txt.Text;
            string userName = Username_txt.Text;
            string password = password_txt.Password;
            string updatedBy = "Admin"; // Assuming "Admin" for now
            string createdBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            string query = "INSERT INTO UserModel (FirstName, LastName, UserName, Password, UpdatedBy, CreatedBy, UpdatedDateTime, Status) " +
                           "VALUES (@FirstName, @LastName, @UserName, @Password, @UpdatedBy, @CreatedBy, GETDATE(), @Status)";


            if (isValid())
            {
                if (save_btn.Content.ToString() == "Save")
                {
                    InsertUser();
                }
                else if (save_btn.Content.ToString() == "Update")
                {
                    UpdateUser();
                }
            }

        }

        private void InsertUser()
        {
            // Read input values
            string firstName = Firstname_txt.Text;
            string lastName = Lastname_txt.Text;
            string userName = Username_txt.Text;
            string password = password_txt.Password;
            string createdBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            string query = "INSERT INTO UserModel (FirstName, LastName, UserName, Password, CreatedBy, CreatedDateTime, Status) " +
                           "VALUES (@FirstName, @LastName, @UserName, @Password, @CreatedBy, GETDATE(), @Status)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@CreatedBy", createdBy);
                        command.Parameters.AddWithValue("@Status", status);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("User registered successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGridData(); // Refresh the grid data
                    clearData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UpdateUser()
        {
            // Read input values
            string id = UserId_txt.Text;
            string firstName = Firstname_txt.Text;
            string lastName = Lastname_txt.Text;
            string userName = Username_txt.Text;
            string password = password_txt.Password;
            string updatedBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            string query = "UPDATE UserModel SET FirstName=@FirstName, LastName=@LastName, UserName=@UserName, Password=@Password, " +
                           "UpdatedBy=@UpdatedBy, UpdatedDateTime=GETDATE(), Status=@Status WHERE Id=@Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                        command.Parameters.AddWithValue("@Status", status);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGridData(); // Refresh the grid data
                    clearData();
                    save_btn.Content = "Save";
                    UserId_txt.IsEnabled = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a user to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRowView row = (DataRowView)dataGrid.SelectedItem;
            string id = row["Id"].ToString();

            string query = "DELETE FROM UserModel WHERE Id=@Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGridData(); // Refresh the grid data
                    clearData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
