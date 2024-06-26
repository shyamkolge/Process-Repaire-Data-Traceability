﻿using System;
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
    /// Interaction logic for DepartmentMaster.xaml
    /// </summary>
    public partial class DepartmentMaster : Window
    {
        string connectionString = "Data Source=SHYAM\\SQLEXPRESS;Initial Catalog=ModiInnovations;Integrated Security=True;Encrypt=false;";// Replace with your actual connection string


        public DepartmentMaster()
        {
            InitializeComponent();
            LoadGridData();
        }


        public void LoadGridData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM DepartmentModel", conn);
                DataTable dataTable = new DataTable();

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
                conn.Close();

                dataGrid.ItemsSource = dataTable.DefaultView;
            }

        }

        public bool isValid()
        {

            if (DepartmentId_txt.Text == String.Empty)
            {
                MessageBox.Show("Username is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (DepartmentName_txt.Text == String.Empty)
            {
                MessageBox.Show("Firstnmae is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public void clearData()
        {
            DepartmentId_txt.Clear();
            DepartmentName_txt.Clear();
            ActiveRadioButton.IsChecked = false;
            InactiveRadioButton.IsChecked = false;

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
            string departmentName = DepartmentName_txt.Text;
            string createdBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            string query = "INSERT INTO DepartmentModel (DepartmentName, CreatedBy, CreatedDateTime, Status) " +
                           "VALUES (@DepartmentName, @CreatedBy, GETDATE(), @Status)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set parameters
                        command.Parameters.AddWithValue("@DepartmentName", departmentName);
                        command.Parameters.AddWithValue("@CreatedBy", createdBy);
                        command.Parameters.AddWithValue("@Status", status);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Department registered successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
            string id = DepartmentId_txt.Text;
            string departmentName = DepartmentName_txt.Text;
            string updatedBy = "Admin"; // Assuming "Admin" for now
            string status = ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";

            string query = "UPDATE DepartmentModel SET DepartmentName=@DepartmentName," +
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
                        command.Parameters.AddWithValue("@DepartmentName", departmentName);
                        command.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                        command.Parameters.AddWithValue("@Status", status);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGridData(); // Refresh the grid data
                    clearData();
                    save_btn.Content = "Save";
                    DepartmentId_txt.IsEnabled = true;

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

            string query = "DELETE FROM DepartmentModel WHERE Id=@Id";

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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItem;
                DepartmentId_txt.Text = row["Id"].ToString();
                DepartmentName_txt.Text = row["DepartmentName"].ToString();
    
                if (row["Status"].ToString() == "Active")
                {
                    ActiveRadioButton.IsChecked = true;
                }
                else
                {
                    InactiveRadioButton.IsChecked = true;
                }

                save_btn.Content = "Update";
                DepartmentId_txt.IsEnabled = false;
            }

        }
    }
}
