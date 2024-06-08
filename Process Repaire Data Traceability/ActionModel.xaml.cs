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
        }

        public bool isValid()
        {

            if (Defectname_txt.Text == String.Empty)
            {
                MessageBox.Show("DefectName is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Actionname_txt.Text == String.Empty)
            {
                MessageBox.Show("DefectName is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }



            return true;
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

        public void clearData()
        {
            Defectname_txt.Clear();
            Actionname_txt.Clear();
            ActiveRadioButton.IsChecked = false;
            InactiveRadioButton.IsChecked = false;

        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            string defectName = Defectname_txt.Text;
            string actionName = Actionname_txt.Text;
            string updatedBy = "Admin"; // Assuming "Admin" for now
            string createdBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            string query = "INSERT INTO ActionModel (DefectName, ActionName, UpdatedBy, CreatedBy, UpdatedDateTime, Status) " +
                           "VALUES (@DefectName, @ActionName, @UpdatedBy, @CreatedBy, GETDATE(), @Status)";


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
            string defectName = Defectname_txt.Text;
            string actionName = Actionname_txt.Text;
            string createdBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            string query = "INSERT INTO ActionModel (DefectName, ActionName, CreatedBy, CreatedDateTime, Status) " +
                           "VALUES (@DefectName, @ActionName, @CreatedBy, GETDATE(), @Status)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@DefectName", defectName);
                        command.Parameters.AddWithValue("@ActionName", actionName);
                        command.Parameters.AddWithValue("@CreatedBy", createdBy);
                        command.Parameters.AddWithValue("@Status", status);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Action registered successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

            string defectName = Defectname_txt.Text;
            string actionName = Actionname_txt.Text;
            string updatedBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            string query = "UPDATE ActionModel SET DefectName=@DefectName, ActionName=@ActionName," +
                   "UpdatedBy=@UpdatedBy, UpdatedDateTime=GETDATE(), Status=@Status " +
                   "WHERE DefectName=@DefectName AND ActionName=@ActionName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@DefectName", defectName);
                        command.Parameters.AddWithValue("@ActionName", actionName);
                        command.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                        command.Parameters.AddWithValue("@Status", status);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Action Master updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGridData(); // Refresh the grid data
                    clearData();
                    save_btn.Content = "Save";

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
                MessageBox.Show("Please select a Action to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataRowView row = (DataRowView)dataGrid.SelectedItem;
            string id = row["ActionId"].ToString();

            string query = "DELETE FROM ActionModel WHERE ActionId=@ActionId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@ActionId", id);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Action deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
                Defectname_txt.Text = row["DefectName"].ToString();
                Actionname_txt.Text = row["ActionName"].ToString();
                if (row["Status"].ToString() == "Active")
                {
                    ActiveRadioButton.IsChecked = true;
                }
                else
                {
                    InactiveRadioButton.IsChecked = true;
                }

                save_btn.Content = "Update";
            }
        }

    }
}
