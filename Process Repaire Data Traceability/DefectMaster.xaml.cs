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
    /// Interaction logic for DefectMaster.xaml
    /// </summary>
    public partial class DefectMaster : Window
    {
        string connectionString = "Data Source=SHYAM\\SQLEXPRESS;Initial Catalog=ModiInnovations;Integrated Security=True;Encrypt=false;";// Replace with your actual connection string

        public DefectMaster()
        {
            InitializeComponent();
            loadComboOptions();
            LoadGridData();
        }

        public void LoadGridData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM DefectMaster", conn);
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
                SqlCommand cmd = new SqlCommand("SELECT DepartmentName FROM DepartmentModel", conn); // Adjust this query to match your database schema
                DataTable dataTable = new DataTable();

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
                conn.Close();


                DepartmentComboBox.Items.Clear();
                DepartmentComboBox.Items.Add(new ComboBoxItem { Content = "Select Department", IsEnabled = false, IsSelected = true });

                foreach (DataRow row in dataTable.Rows)
                {
                    DepartmentComboBox.Items.Add(row["DepartmentName"].ToString());
                }

               /* DepartmentComboBox.ItemsSource = dataTable.DefaultView;
                DepartmentComboBox.DisplayMemberPath = "DepartmentName";
                DepartmentComboBox.SelectedValuePath = "DepartmentName";*/

           
            }
        }

        public void clearData()
        {
            DefectId_txt.Clear();
            Defect_txt.Clear();
            Remart_txt.Clear();
            DepartmentComboBox.SelectedIndex = 0;
            ActiveRadioButton.IsChecked = false;
            InactiveRadioButton.IsChecked = false;
            save_btn.Content = "Save";
            DefectId_txt.IsEnabled = true;
        }

        public bool isValid()
        {
            if (Defect_txt.Text == String.Empty)
            {
                MessageBox.Show("Defect Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (DefectId_txt.Text == String.Empty)
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
            string DefectId = DefectId_txt.Text;
            string DefectName = Defect_txt.Text;
            string Remark = Remart_txt.Text;
            string Department = DepartmentComboBox.SelectedValue.ToString();
            string createdBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            if (Department == "Select Department")
            {
                MessageBox.Show("Please select a valid department.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = "INSERT INTO DefectMaster (DefectId, DefectName, Remark ,Department, CreatedBy, CreatedDateTime, Status) " +
                           "VALUES (@DefectId, @DefectName, @Remark, @Department, @CreatedBy, GETDATE(), @Status)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@DefectId", DefectId);
                        command.Parameters.AddWithValue("@DefectName", DefectName);
                        command.Parameters.AddWithValue("@Remark", Remark);
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
            string DefectId = DefectId_txt.Text;
            string DefectName = Defect_txt.Text;
            string Remark = Remart_txt.Text;
            string Department = DepartmentComboBox.SelectedValue.ToString();
            string createdBy = "Admin"; // Assuming "Admin" for now
            string updatedBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            if (Department == "Select Department")
            {
                MessageBox.Show("Please select a valid department.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = "UPDATE DefectMaster SET DefectId=@DefectId, DefectName=@DefectName, Remark=@Remark," +
                           "Department=@Department, UpdatedBy=@UpdatedBy, UpdatedDateTime=GETDATE(), Status=@Status WHERE DefectId=@DefectId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@DefectId", DefectId);
                        command.Parameters.AddWithValue("@DefectName", DefectName);
                        command.Parameters.AddWithValue("@Remark", Remark);
                        command.Parameters.AddWithValue("@Department", Department);
                        command.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                        command.Parameters.AddWithValue("@Status", status);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGridData(); // Refresh the grid data
                    clearData();
                    save_btn.Content = "Save"; // Change button text back to "Save"
                    DefectId_txt.IsEnabled = true;
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
            string DefectId = row["DefectId"].ToString();

            string query = "DELETE FROM DefectMaster WHERE DefectId=@DefectId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@DefectId", DefectId);
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
                DefectId_txt.Text = row["DefectId"].ToString();
                Defect_txt.Text = row["DefectName"].ToString();
                Remart_txt.Text = row["Remark"].ToString();
                DepartmentComboBox.SelectedValue = row["Department"].ToString();
                if (row["Status"].ToString() == "Active")
                {
                    ActiveRadioButton.IsChecked = true;
                }
                else
                {
                    InactiveRadioButton.IsChecked = true;
                }

                save_btn.Content = "Update";
                DefectId_txt.IsEnabled = false;
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
