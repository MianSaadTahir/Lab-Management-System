using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ProjectB
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            GetData();
            LoadCloIdsIntoComboBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)   //ADD CODE
        {
            string details = textBox1.Text; 
            int cloId = 0;

            if (comboBox1.SelectedItem != null && int.TryParse(comboBox1.SelectedItem.ToString(), out cloId))
            {
                AddRubric(details, cloId);
            }
            else
            {
                MessageBox.Show("Please select a valid CloId from the ComboBox.");
            }
        }
        private void AddRubric(string details, int cloId)
        {
            if (AreFieldsComplete())
            {
                string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                string query = "INSERT INTO Rubric (Details, CloId) VALUES (@Details, @CloId)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Details", details);
                    command.Parameters.AddWithValue("@CloId", cloId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Rubric added successfully!");
                            RemoveAllText();
                            GetData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add Rubric.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all the required fields.");
            }
        }
        private void button4_Click(object sender, EventArgs e)  //UPDATE CODE
        {
            string details = textBox1.Text;
            int cloId = 0;

            if (comboBox1.SelectedItem != null && int.TryParse(comboBox1.SelectedItem.ToString(), out cloId))
            {
                UpdateRubric(details, cloId);
            }
            else
            {
                MessageBox.Show("Please select a valid CloId from the ComboBox.");
            }
        }
        private void UpdateRubric(string details, int cloId)
        {
            if (AreFieldsComplete())
            {
                if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int RubricId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

                        string constr = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                        string cmd = "UPDATE Rubric SET Details = @Details, CloId = @CloId WHERE Id = @Id";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            con.Open();


                            SqlCommand command = new SqlCommand(cmd, con);
                            command.Parameters.AddWithValue("@Id", RubricId);
                            command.Parameters.AddWithValue("@Details", details);
                            command.Parameters.AddWithValue("@CloId", cloId);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Data Updated Successfully!");
                        }
                        RemoveAllText();
                        GetData();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to update.");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all the required fields.");
            }
        }
        private bool AreFieldsComplete()
        {
            if(textBox1.Text == " " || comboBox1.SelectedItem == null)
            {
                return false;
            }
            return true;
        }
        private void button5_Click(object sender, EventArgs e)  //DELETE CODE
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Check if the selected row contains valid data
                if (selectedRow.Cells["ID"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["ID"].Value.ToString()))
                {
                    int rubricID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    string constr = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                    string cmd = "DELETE FROM Rubric WHERE Id = @Id";

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();

                        SqlCommand command = new SqlCommand(cmd, con);
                        command.Parameters.AddWithValue("@Id", rubricID);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully!");
                            RemoveAllText();
                            GetData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete record.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The selected row does not contain valid data.");
                }
            }
            else if (dataGridView1.SelectedRows.Count == 0) // Handle empty selection
            {
                MessageBox.Show("Please select a row to delete.");
            }
            else // Handle multiple rows selected
            {
                MessageBox.Show("Please select only one row to delete.");
            }

        }
        private void RemoveAllText()
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
        }
    
        private void GetData()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT Rubric.Id, Rubric.Details, Clo.Id AS CloId " +
                    "FROM Rubric " +
                    "INNER JOIN Clo ON Rubric.CloId = Clo.Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void LoadCloIdsIntoComboBox()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT Id FROM Clo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox1.Items.Clear(); // Clear existing items

                    while (reader.Read())
                    {
                        int cloId = reader.GetInt32(0); 
                        comboBox1.Items.Add(cloId);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a single row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Check if the selected row is empty (all cells are null or empty)
                bool isEmptyRow = true;
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        isEmptyRow = false;
                        break;
                    }
                }

                if (!isEmptyRow)
                {
                    // Populate textboxes with data from selected row
                    textBox1.Text = selectedRow.Cells["Details"].Value?.ToString() ?? string.Empty;

                    // Set the ComboBox selection based on the "CloId" value
                    int cloId = Convert.ToInt32(selectedRow.Cells["CloId"].Value);
                    comboBox1.SelectedItem = cloId; 
                }
            }
        }
    }
}
