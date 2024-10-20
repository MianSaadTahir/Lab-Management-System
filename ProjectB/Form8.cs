using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectB
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
            GetData();
            LoadAssessmentTitlesIntoComboBox();
            LoadDetailsIntoComboBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
            this.Hide();
        }
        private void button4_Click(object sender, EventArgs e) //ADD CODE
        {
            // Get the selected Assessment Id based on the selected Assessment Title
            int assessmentId = GetAssessmentIdByTitle(comboBox1.Text);

            // Get other values from ComboBoxes and TextBoxes
            string name = textBox1.Text;
            int rubricId = GetRubricIdByName(comboBox2.Text);
            int totalMarks = Convert.ToInt32(textBox2.Text);
            DateTime dateAdded = DateTime.Now;
            DateTime dateUpdated = DateTime.Now;

            string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "INSERT INTO AssessmentComponent (Name, RubricId, TotalMarks, DateCreated, DateUpdated, AssessmentId) " +
                           "VALUES (@Name, @RubricId, @TotalMarks, @DateCreated, @DateUpdated, @AssessmentId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@RubricId", rubricId);
                command.Parameters.AddWithValue("@TotalMarks", totalMarks);
                command.Parameters.AddWithValue("@DateCreated", dateAdded);
                command.Parameters.AddWithValue("@DateUpdated", dateUpdated);
                command.Parameters.AddWithValue("@AssessmentId", assessmentId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Assessment Component added successfully!");
                        RemoveAllText(); // Custom method to clear TextBoxes
                        GetData(); // Custom method to refresh data in DataGridView or elsewhere
                    }
                    else
                    {
                        MessageBox.Show("Failed to add Assessment Component.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        private int GetAssessmentIdByTitle(string title)
        {
            int assessmentId = 0;
            string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT Id FROM Assessment WHERE Title = @Title";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Title", title);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        assessmentId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

            return assessmentId;
        }

        private int GetRubricIdByName(string details)
        {
            int rubricId = 0;
            string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT Id FROM Rubric WHERE Details = @Details";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Details", details);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        rubricId = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

            return rubricId;
        }
        private void RemoveAllText()
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox2.Text = "";
        }
        private void button5_Click(object sender, EventArgs e) //UPDATE CODE
        {
            if (AreFieldsComplete())
            {
                if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    int assessmentComponenetID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    int assessmentId = GetAssessmentIdByTitle(comboBox1.Text);
                    string name = textBox1.Text;
                    int rubricId = GetRubricIdByName(comboBox2.Text);
                    int totalMarks = Convert.ToInt32(textBox2.Text);
                    DateTime dateUpdated = DateTime.Now;

                    string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                    string query = "UPDATE AssessmentComponent SET Name = @Name, RubricId = @RubricId, TotalMarks = @TotalMarks,DateCreated = @DateCreated, DateUpdated = @DateUpdated, AssessmentId = @AssessmentId " +
                                   "WHERE Id = @Id";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", assessmentComponenetID);
                            command.Parameters.AddWithValue("@Name", name);
                            command.Parameters.AddWithValue("@RubricId", rubricId);
                            command.Parameters.AddWithValue("@TotalMarks", totalMarks);
                            command.Parameters.AddWithValue("@DateCreated", dateUpdated);
                            command.Parameters.AddWithValue("@DateUpdated", dateUpdated);
                            command.Parameters.AddWithValue("@AssessmentId", assessmentId);

                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Assessment Component updated successfully!");
                                RemoveAllText();
                                GetData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update Component.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a record to update.");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all CLO fields before updating.");
            }
        }
        private bool AreFieldsComplete()
        {
            if (textBox1.Text == " " || comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || textBox2.Text == " ")
            {
                return false;
            }
            return true;
        }
        private void button6_Click(object sender, EventArgs e) //DELETE CODE
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Check if the selected row contains valid data
                if (selectedRow.Cells["ID"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["ID"].Value.ToString()))
                {
                    int assessmentcomponentID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                    string cmd = "DELETE FROM AssessmentComponent WHERE Id = @Id";

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();

                        SqlCommand command = new SqlCommand(cmd, con);
                        command.Parameters.AddWithValue("@Id", assessmentcomponentID);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Assessment Componenet Record deleted successfully!");
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^(?:100|[1-9][0-9]?)$"; // Pattern for marks between 1 and 100 (inclusive)

            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Focus();
                errorProvider1.SetError(textBox2, "Marks are required.");
            }
            else if (!Regex.IsMatch(textBox2.Text, pattern))
            {
                textBox2.Focus();
                errorProvider1.SetError(textBox2, "Marks should be between 1 to 100.");
            }
            else
            {
                errorProvider1.SetError(textBox2, null);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }
        private void GetData()
        {
            string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT AssessmentComponent.Id ,AssessmentComponent.Name, AssessmentComponent.RubricId, AssessmentComponent.TotalMarks, " +
                           "AssessmentComponent.DateCreated, AssessmentComponent.DateUpdated, AssessmentComponent.AssessmentId " +
                           "FROM AssessmentComponent " +
                           "INNER JOIN Rubric ON AssessmentComponent.RubricId = Rubric.Id " +
                           "INNER JOIN Assessment ON AssessmentComponent.AssessmentId = Assessment.Id";

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

        private void LoadAssessmentTitlesIntoComboBox()
        {
            string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT Title FROM Assessment";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox1.Items.Clear();

                    while (reader.Read())
                    {
                        string assessmentTitle = reader["Title"].ToString();

                        if (!comboBox1.Items.Contains(assessmentTitle))
                        {
                            comboBox1.Items.Add(assessmentTitle);
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        private void LoadDetailsIntoComboBox()
        {
            string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT Details FROM RubricLevel";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    comboBox2.Items.Clear();

                    while (reader.Read())
                    {
                        string details = reader["Details"].ToString();

                        if (!comboBox2.Items.Contains(details))
                        {
                            comboBox2.Items.Add(details);
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        private string GetTitleByAssessmentId(int assessmentId)
        {
            string title = string.Empty;
            string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT Title FROM Assessment WHERE Id = @AssessmentId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AssessmentId", assessmentId);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        title = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

            return title;
        }

        private string GetNameByRubricId(int rubricId)
        {
            string name = string.Empty;
            string connectionString = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT Details FROM RubricLevel WHERE Id = @RubricId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RubricId", rubricId);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        name = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }

            return name;
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
                    textBox1.Text = selectedRow.Cells["Name"].Value?.ToString() ?? string.Empty;
                    comboBox1.SelectedItem = GetTitleByAssessmentId(Convert.ToInt32(selectedRow.Cells["AssessmentId"].Value));
                    textBox2.Text = selectedRow.Cells["TotalMarks"].Value?.ToString() ?? string.Empty;
                    int rubricId = Convert.ToInt32(selectedRow.Cells["RubricId"].Value);
                    string name = GetNameByRubricId(rubricId);
                    comboBox2.SelectedItem = name;


                }
            }
        }

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    xc X = new for () ;
        //    X.Show();
        //    this.Hide();
        //}

        private void Form8_Load(object sender, EventArgs e)
        {

        }
    }
}
