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

namespace Process_Repaire_Data_Traceability
{
    /// <summary>
    /// Interaction logic for ActionModel.xaml
    /// </summary>
    public partial class ActionModel : Window
    {
        string connectionString = "Data Source=SHYAM\\SQLEXPRESS;Initial Catalog=ModiInnovations;Integrated Security=True;Encrypt=false;";// Replace with your actual connection string

        public ActionModel()
        {
            InitializeComponent();
            LoadGridData();
            loadComboOptions();
        }

        public void LoadGridData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ActionModel", conn);
                DataTable dataTable = new DataTable();

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
                conn.Close();

                dataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        public void loadComboOptions()
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT DefectName  FROM DefectMaster", conn); // Adjust this query to match your database schema
                DataTable dataTable = new DataTable();

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
                conn.Close();


                DepartmentComboBox.Items.Clear();
                DepartmentComboBox.Items.Add(new ComboBoxItem { Content = "Select Defect", IsEnabled = false, IsSelected = true });

                foreach (DataRow row in dataTable.Rows)
                {
                    DepartmentComboBox.Items.Add(row["DefectName"].ToString());
                }

                /* DepartmentComboBox.ItemsSource = dataTable.DefaultView;
                 DepartmentComboBox.DisplayMemberPath = "DepartmentName";
                 DepartmentComboBox.SelectedValuePath = "DepartmentName";*/


            }
        }

        public void clearData()
        {
            ActionId_txt.Clear();
            Action_txt.Clear();
            DepartmentComboBox.SelectedIndex = 0;
            ActiveRadioButton.IsChecked = false;
            InactiveRadioButton.IsChecked = false;
            save_btn.Content = "Save";
            Action_txt.IsEnabled = true;
        }

        public bool isValid()
        {
            if (Action_txt.Text == String.Empty)
            {
                MessageBox.Show("Defect Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (ActionId_txt.Text == String.Empty)
            {
                MessageBox.Show("Defect Id is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            if (ActiveRadioButton.IsChecked == false && InactiveRadioButton.IsChecked == false)
            {
                MessageBox.Show("Status is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
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
            string ActionId = ActionId_txt.Text;
            string ActionName = Action_txt.Text;
            string Department = DepartmentComboBox.SelectedValue.ToString();
            string createdBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            if (Department == "Select Defect")
            {
                MessageBox.Show("Please select a valid department.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = "INSERT INTO ActionModel (ActionId, ActionName,DefectName, CreatedBy, CreatedDateTime, Status) " +
                           "VALUES (@ActionId, @ActionName, @Department, @CreatedBy, GETDATE(), @Status)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@ActionId", ActionId);

                        command.Parameters.AddWithValue("@ActionName", ActionName);
                        command.Parameters.AddWithValue("@Department", Department);
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
            string ActionId = ActionId_txt.Text;
            string ActionName = Action_txt.Text;
            string Department = DepartmentComboBox.SelectedValue.ToString();
            string createdBy = "Admin"; // Assuming "Admin" for now
            string updatedBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            if (Department == "Select Defect")
            {
                MessageBox.Show("Please select a valid department.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = "UPDATE ActionModel SET ActionId=@ActionId, ActionName=@ActionName," +
                           "DefectName=@Department, UpdatedBy=@UpdatedBy, UpdatedDateTime=GETDATE(), Status=@Status WHERE ActionId=@ActionId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@ActionId", ActionId);
                        command.Parameters.AddWithValue("@ActionName", ActionName);
                        command.Parameters.AddWithValue("@Department", Department);
                        command.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                        command.Parameters.AddWithValue("@Status", status);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGridData(); // Refresh the grid data
                    clearData();
                    save_btn.Content = "Save"; // Change button text back to "Save"
                    ActionId_txt.IsEnabled = true;
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
            string ActionId = row["ActionId"].ToString();

            string query = "DELETE FROM ActionModel WHERE ActionId=@ActionId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@ActionId", ActionId);
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                ActionId_txt.Text = row["ActionId"].ToString();
                Action_txt.Text = row["ActionName"].ToString();

                DepartmentComboBox.SelectedValue = row["DefectName"].ToString();
                if (row["Status"].ToString() == "Active")
                {
                    ActiveRadioButton.IsChecked = true;
                }
                else
                {
                    InactiveRadioButton.IsChecked = true;
                }

                save_btn.Content = "Update";
                ActionId_txt.IsEnabled = false;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



    }
}
