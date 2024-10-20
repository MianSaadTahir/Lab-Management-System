using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectB
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            GetData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.Show();
            this.Hide();
        }
        private void GetData()
        {
            string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Clo", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
        private void RemoveAllText()
        {
            textBox2.Clear(); // Clear CLO name

        }
        private bool AreFieldsComplete()
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                return false;
            }

            return true;
        }
        private void button4_Click(object sender, EventArgs e) //ADD CODE
        {
            string cloName = textBox2.Text.Trim();

            if (!string.IsNullOrEmpty(cloName))
            {
                DateTime currentDate = DateTime.Now;
                string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                string query = "INSERT INTO CLO (Name, DateCreated, DateUpdated) VALUES (@Name, @DateCreated, @DateUpdated)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", cloName);
                        command.Parameters.AddWithValue("@DateCreated", currentDate);
                        command.Parameters.AddWithValue("@DateUpdated", currentDate); // Set DateUpdated to DateCreated initially

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record added successfully!");
                            RemoveAllText();
                            GetData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add record.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid CLO name.");
            }
        }

        private void button5_Click(object sender, EventArgs e)  //UPDATE CODE
        {
            if (AreFieldsComplete())
            {
                if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    int cloID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                    string query = "UPDATE CLO SET Name = @Name, DateUpdated = @DateUpdated WHERE ID = @ID";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID", cloID);
                            command.Parameters.AddWithValue("@Name", textBox2.Text);
                            command.Parameters.AddWithValue("@DateUpdated", DateTime.Now);

                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("CLO updated successfully!");
                                RemoveAllText();
                                GetData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update CLO.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a CLO to update.");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all CLO fields before updating.");
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
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
                    textBox2.Text = selectedRow.Cells["Name"].Value?.ToString() ?? string.Empty;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Check if the selected row contains valid data
                if (selectedRow.Cells["ID"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["ID"].Value.ToString()))
                {
                    int cloID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                    string query = "DELETE FROM CLO WHERE ID = @ID";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@ID", cloID);
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("CLO record deleted successfully!");
                                RemoveAllText();
                                GetData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete CLO record.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The selected row does not contain valid data.");
                }
            }
            else
            {
                MessageBox.Show("Please select a CLO record to delete.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }
    }
}
