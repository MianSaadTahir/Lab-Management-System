using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjectB
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            GetRecords();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void textBox5_Validation(object sender, EventArgs e)
        {
            string pattern = @"^(?:100|[1-9][0-9]?)$"; // Pattern for marks between 1 and 100 (inclusive)

            if (string.IsNullOrEmpty(textBox5.Text))
            {
                textBox5.Focus(); 
                errorProvider2.SetError(textBox5, "Marks are required.");
            }
            else if (!Regex.IsMatch(textBox5.Text, pattern))
            {
                textBox5.Focus();
                errorProvider2.SetError(textBox5, "Marks should be between 1 to 100.");
            }
            else
            {
                errorProvider2.SetError(textBox5, null);
            }
            
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void textBox6_Validation(object sender, EventArgs e)
        {
            string pattern = @"^(?:100|[1-9][0-9]?)$"; // Pattern for marks between 1 and 100 (inclusive)

            if (string.IsNullOrEmpty(textBox6.Text))
            {
                textBox6.Focus();
                errorProvider3.SetError(textBox6, "Weightage% is required.");
            }
            else if (!Regex.IsMatch(textBox6.Text, pattern))
            {
                textBox6.Focus();
                errorProvider3.SetError(textBox6, "Weightage should be between 1 to 100.");
            }
            else
            {
                errorProvider3.SetError(textBox6, null);
            }
        }
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
           if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == ' ')
           {
              e.Handled = true;
           }
            
        }

        private void textBox4_Validation(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                textBox4.Focus();
                errorProvider1.SetError(textBox4, "Title is required");
            }
            else if (!char.IsUpper(textBox4.Text[0]))
            {
                textBox4.Focus();
                errorProvider1.SetError(textBox4, "The Title should start with a capital letter.");
            }
            else
            {
                errorProvider1.SetError(textBox4, null);
            }
        }

        private void button4_Click(object sender, EventArgs e) //ADD CODE
        {
            if (AreFieldsComplete())
            {
                DateTime currentDate = DateTime.Now;
                string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                string query = "INSERT INTO Assessment (Title, DateCreated, TotalMarks, TotalWeightage) VALUES (@Title, @DateCreated, @TotalMarks, @TotalWeightage)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", textBox4.Text);
                        command.Parameters.AddWithValue("@DateCreated", currentDate);
                        command.Parameters.AddWithValue("@TotalMarks", textBox5.Text); 
                        command.Parameters.AddWithValue("@TotalWeightage", textBox6.Text);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Assessments are added successfully!");
                            RemoveAllText();
                            GetRecords();
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
                MessageBox.Show("Please fill in all required fields");
            }
        }


        private bool AreFieldsComplete()
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                return false;
            }

            return true;
        }
        private void RemoveAllText()
        {
            textBox4.Clear(); 
            textBox5.Clear(); 
            textBox6.Clear(); 
        }
        private void GetRecords()
        {
            string constr = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Assessment", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void button5_Click(object sender, EventArgs e) //UPDATE CODE
        {
            if (AreFieldsComplete())
            {
                if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int assessmentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                        string constr = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                        string cmd = "UPDATE Assessment SET Title = @Title, DateCreated = @DateCreated, TotalMarks = @TotalMarks, TotalWeightage = @TotalWeightage WHERE Id = @Id";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand(cmd, con);
                            command.Parameters.AddWithValue("@Id", assessmentID);
                            command.Parameters.AddWithValue("@Title", textBox4.Text);
                            command.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                            command.Parameters.AddWithValue("@TotalMarks", textBox5.Text);
                            command.Parameters.AddWithValue("@TotalWeightage", textBox6.Text);
                           
                            command.ExecuteNonQuery();
                            MessageBox.Show("Data Updated Successfully!");
                        }
                        RemoveAllText();
                        GetRecords();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a row to update.");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields before updating.");
            }
        }

        private void button6_Click(object sender, EventArgs e) //DELETE CODE
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Check if the selected row contains valid data
                if (selectedRow.Cells["ID"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["ID"].Value.ToString()))
                {
                    int assessmentID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    string constr = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                    string cmd = "DELETE FROM Assessment WHERE Id = @Id";

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();

                        SqlCommand command = new SqlCommand(cmd, con);
                        command.Parameters.AddWithValue("@Id", assessmentID);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully!");
                            RemoveAllText();
                            GetRecords();
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a single row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

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
                    textBox4.Text = selectedRow.Cells["Title"].Value?.ToString() ?? string.Empty;
                    textBox5.Text = selectedRow.Cells["TotalMarks"].Value?.ToString() ?? string.Empty;
                    textBox6.Text = selectedRow.Cells["TotalWeightage"].Value?.ToString() ?? string.Empty;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.Show();
            this.Hide();
        }
    }
}
