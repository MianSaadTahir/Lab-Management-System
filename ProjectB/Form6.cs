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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ProjectB
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            GetData();
            addRubricIdsIntoComboBox();
            comboBox2.Items.Add("Unsatisfactory");
            comboBox2.Items.Add("Fair");
            comboBox2.Items.Add("Good");
            comboBox2.Items.Add("Exceptional");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
            this.Hide();
        }
        private int GetStatusValue(string statusText)
        {
            switch (statusText)
            {
                case "Unsatisfactory":
                    return 1;
                case "Fair":
                    return 2;
                case "Good":
                    return 3;
                case "Exceptional":
                    return 4;
                default:
                    throw new ArgumentException("Invalid status text.");
            }
        }
        private void button4_Click(object sender, EventArgs e)   //ADD CODE
        {
            string details = textBox4.Text;
            int rubricId = 0;

            if (comboBox1.SelectedItem != null && int.TryParse(comboBox1.SelectedItem.ToString(), out rubricId))
            {
                AddRubric(details, rubricId);
            }
            else
            {
                MessageBox.Show("Please select a valid RubricId from the ComboBox.");
            }
        }

        private void AddRubric(string details, int rubricId)
        {
            if (AreFieldsComplete())
            {
                string statusText = comboBox2.Text;
                int statusValue = GetStatusValue(statusText);
                string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                string query = "INSERT INTO RubricLevel (RubricId, Details, MeasurementLevel) VALUES (@RubricId, @Details, @MeasurementLevel)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RubricId", rubricId); 
                    command.Parameters.AddWithValue("@Details", details);
                    command.Parameters.AddWithValue("@MeasurementLevel", statusValue);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Rubric Level added successfully!");
                            RemoveAllText();
                            GetData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add Rubric Level");
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

        private bool AreFieldsComplete()
        {
            if (comboBox1.SelectedItem == null || textBox4.Text == " " || comboBox2.Text == null)
            {
                return false;
            }
            return true;
        }
        private void button5_Click(object sender, EventArgs e)   //DELETE CODE
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Check if the selected row contains valid data
                if (selectedRow.Cells["ID"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["ID"].Value.ToString()))
                {
                    int rubricLevelID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    string constr = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                    string cmd = "DELETE FROM RubricLevel WHERE Id = @Id";

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();

                        SqlCommand command = new SqlCommand(cmd, con);
                        command.Parameters.AddWithValue("@Id", rubricLevelID);
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

        private void button6_Click(object sender, EventArgs e)    //UPDATE CODE
        {
            string details = textBox4.Text;
            int rubricId = 0;

            if (comboBox1.SelectedItem != null && int.TryParse(comboBox1.SelectedItem.ToString(), out rubricId))
            {
                UpdateRubric(details, rubricId);
            }
            else
            {
                MessageBox.Show("Please select a valid RubricId from the ComboBox.");
            }
        }
        private void UpdateRubric(string details, int rubricId)
        {
            if (AreFieldsComplete())
            {
                if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int RubricId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                        string statusText = comboBox2.Text;
                        int statusValue = GetStatusValue(statusText);
                        string constr = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
                        string cmd = "UPDATE RubricLevel SET RubricId = @RubricId, Details = @Details, MeasurementLevel = @MeasurementLevel WHERE Id = @Id";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            con.Open();


                            SqlCommand command = new SqlCommand(cmd, con);
                            command.Parameters.AddWithValue("@Id", RubricId);
                            command.Parameters.AddWithValue("@RubricId", rubricId);
                            command.Parameters.AddWithValue("@Details", details);
                            command.Parameters.AddWithValue("@MeasurementLevel", statusValue);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedRubricId = Convert.ToInt32(comboBox1.SelectedItem);

            int rubricIdInRubricTable = GetIdByRubricId(selectedRubricId);

            if (rubricIdInRubricTable > 0)
            {
                MessageBox.Show($"Selected RubricId in Rubric Table: {rubricIdInRubricTable}");
            }
        }
        private int GetIdByRubricId(int rubricId)
        {
            int id = 0;
            string constr = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                string query = "SELECT Id FROM Rubric WHERE Id = @RubricId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RubricId", rubricId);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        id = Convert.ToInt32(result);
                    }
                }
            }

            return id;
        }
        private void addRubricIdsIntoComboBox()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT Id FROM Rubric", con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int rubricId = Convert.ToInt32(reader["Id"]);

                    if (!comboBox1.Items.Contains(rubricId))
                    {
                        comboBox1.Items.Add(rubricId);
                    }
                }

                reader.Close();
            }
        }
        private void GetData()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT RubricLevel.Id, Rubric.Id AS RubricId, RubricLevel.Details, RubricLevel.MeasurementLevel " +
               "FROM RubricLevel " +
               "INNER JOIN Rubric ON RubricLevel.RubricId = Rubric.Id";


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

        private void RemoveAllText()
        {
            comboBox1.SelectedIndex = -1;
            textBox4.Text = "";
            comboBox2.SelectedIndex = -1;
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
                    int rubricId = Convert.ToInt32(selectedRow.Cells["RubricId"].Value);

                    // Check if the RubricId is in the comboBox1 items
                    if (comboBox1.Items.Contains(rubricId))
                    {
                        comboBox1.SelectedItem = rubricId;
                    }
                    else
                    {
                        // If RubricId is not in comboBox1 items, add it and then select it
                        comboBox1.Items.Add(rubricId);
                        comboBox1.SelectedItem = rubricId;
                    }

                    // Rest of your code for setting other controls based on selected row data
                    string details = selectedRow.Cells["Details"].Value.ToString();
                    int measurementLevel = Convert.ToInt32(selectedRow.Cells["MeasurementLevel"].Value);
                    textBox4.Text = details;

                    switch (measurementLevel)
                    {
                        case 1:
                            comboBox2.SelectedItem = "Unsatisfactory";
                            break;
                        case 2:
                            comboBox2.SelectedItem = "Fair";
                            break;
                        case 3:
                            comboBox2.SelectedItem = "Good";
                            break;
                        case 4:
                            comboBox2.SelectedItem = "Exceptional";
                            break;
                        default:
                            comboBox2.SelectedItem = null;
                            break;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.Show();
            this.Hide();
        }
    }
}

