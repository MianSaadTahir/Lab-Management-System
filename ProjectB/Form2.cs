using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ProjectB
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            GetRecords();
        }
        private void button1_Click_2(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
            this.Hide();
        }
        private int GetStatusValue(string statusText)
        {
            switch (statusText)
            {
                case "Active":
                    return 5;
                case "NotActive":
                    return 6;
                default:
                    throw new ArgumentException("Invalid status text.");
            }
        }
        private bool AreFieldsComplete()
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                comboBox1.SelectedItem == null)
            {
                return false;
            }

            return true;
        }

        private void RemoveAllText()
        {
            textBox5.Clear(); // Clear FirstName
            textBox3.Clear(); // Clear LastName
            textBox4.Clear(); // Clear Contact
            textBox2.Clear(); // Clear Email
            textBox6.Clear(); // Clear RegistrationNumber
            comboBox1.SelectedIndex = -1; // Clear Status dropdown selection
        }
        private void GetRecords()
        {
            string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Student", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                textBox5.Focus();
                errorProvider1.SetError(textBox5, "Name is required. The name should start with a capital letter and spaces are not allowed.");
            }
            else if (!char.IsUpper(textBox5.Text[0]))
            {
                textBox5.Focus();
                errorProvider1.SetError(textBox5, "The name should start with a capital letter.");
            }
            else if (textBox5.Text.Contains(" "))
            {
                textBox5.Focus();
                errorProvider1.SetError(textBox5, "Spaces are not allowed.");
            }
            else
            {
                errorProvider1.SetError(textBox5, null);
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                textBox3.Focus();
                errorProvider2.SetError(textBox3, "Name is required. The name should start with a capital letter and spaces are not allowed.");
            }
            else if (!char.IsUpper(textBox3.Text[0]))
            {
                textBox3.Focus();
                errorProvider2.SetError(textBox3, "The name should start with a capital letter.");
            }
            else if (textBox3.Text.Contains(" "))
            {
                textBox3.Focus();
                errorProvider2.SetError(textBox3, "Spaces are not allowed.");
            }
            else
            {
                errorProvider2.SetError(textBox3, null);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^03\d{2}-\d{7}$";// Regular expression pattern for the format "XXXX-XXXXXXX"

            if (!Regex.IsMatch(textBox4.Text, pattern))
            {
                textBox4.Focus();
                errorProvider3.SetError(textBox4, "Invalid contact number. Please enter a valid number in the format 03XX-XXXXXXX");
            }
            else
            {
                errorProvider3.SetError(textBox4, null);
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^[a-zA-Z0-9]+@gmail\.com$"; // Regular expression pattern for the format "@gmail.com"

            if (!Regex.IsMatch(textBox2.Text, pattern))
            {
                textBox2.Focus();
                errorProvider4.SetError(textBox2, "Invalid email address. Please enter a valid Gmail address without spaces and special characters in the format '@gmail.com'");
            }
            else
            {
                errorProvider4.SetError(textBox2, null);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"^\d{4}-CS-\d{1}$";  // Regular expression pattern for the format "2022-CS-789"
            string pattern1 = @"^\d{4}-CS-\d{2}$";
            string pattern2 = @"^\d{4}-CS-\d{3}$";

            if (!Regex.IsMatch(textBox6.Text, pattern) && !Regex.IsMatch(textBox6.Text, pattern1) && !Regex.IsMatch(textBox6.Text, pattern2))
            {
                textBox6.Focus();
                errorProvider5.SetError(textBox6, "Invalid registration number. Please enter a valid number in the format 2022-CS-789.");
            }
            else
            {
                errorProvider5.SetError(textBox6, null);
            }
        }

        private void button2_Click_1(object sender, EventArgs e) //ADD CODE
        {
            if (AreFieldsComplete())
            {


                string statusText = comboBox1.Text;
                int statusValue = GetStatusValue(statusText);

                string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Student VALUES(@FirstName, @LastName, @Contact, @Email, @RegistrationNumber, @Status) ", con);

                cmd.Parameters.AddWithValue("@FirstName", textBox5.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox3.Text);
                cmd.Parameters.AddWithValue("@Contact", textBox4.Text);
                cmd.Parameters.AddWithValue("@Email", textBox2.Text);
                cmd.Parameters.AddWithValue("@RegistrationNumber", textBox6.Text);
                cmd.Parameters.AddWithValue("@Status", statusValue);

                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record inserted successfully!");
                RemoveAllText();
                GetRecords();
            }
            else
            {
                MessageBox.Show("Please fill in all the required fields.");
            }
        }

        private void button4_Click_1(object sender, EventArgs e) //UPDATE CODE
        {
            if (AreFieldsComplete())
            {
                if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int studentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                        string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                        string cmd = "UPDATE STUDENT SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Email = @Email, RegistrationNumber = @RegistrationNumber, Status = @Status WHERE Id = @Id";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            con.Open();

                            int valueToInsert = comboBox1.SelectedItem.ToString() == "Active" ? 5 : 6;

                            SqlCommand command = new SqlCommand(cmd, con);
                            command.Parameters.AddWithValue("@Id", studentID);
                            command.Parameters.AddWithValue("@FirstName", textBox5.Text);
                            command.Parameters.AddWithValue("@LastName", textBox3.Text);
                            command.Parameters.AddWithValue("@Contact", textBox4.Text);
                            command.Parameters.AddWithValue("@Email", textBox2.Text);
                            command.Parameters.AddWithValue("@RegistrationNumber", textBox6.Text);
                            command.Parameters.AddWithValue("@Status", valueToInsert);
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

        private void button5_Click_1(object sender, EventArgs e)  //DELETE CODE
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Check if the selected row contains valid data
                if (selectedRow.Cells["ID"].Value != null && !string.IsNullOrEmpty(selectedRow.Cells["ID"].Value.ToString()))
                {
                    int studentID = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                    string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                    string cmd = "DELETE FROM STUDENT WHERE Id = @Id";

                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();

                        SqlCommand command = new SqlCommand(cmd, con);
                        command.Parameters.AddWithValue("@Id", studentID);
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



        private void textBox5_KeyPress_1(object sender, KeyPressEventArgs e)
        {

            if (!char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress_1(object sender, KeyPressEventArgs e)
        {

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                textBox6.Focus();
                errorProvider6.SetError(comboBox1, "Please select an item.");
            }
            else
            {
                errorProvider6.SetError(comboBox1, null);
            }

        }

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
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
                    textBox5.Text = selectedRow.Cells["FirstName"].Value?.ToString() ?? string.Empty;
                    textBox3.Text = selectedRow.Cells["LastName"].Value?.ToString() ?? string.Empty;
                    textBox4.Text = selectedRow.Cells["Contact"].Value?.ToString() ?? string.Empty;
                    textBox2.Text = selectedRow.Cells["Email"].Value?.ToString() ?? string.Empty;
                    textBox6.Text = selectedRow.Cells["RegistrationNumber"].Value?.ToString() ?? string.Empty;

                    // Set the ComboBox selection based on the "Status" value
                    int statusValue = Convert.ToInt32(selectedRow.Cells["Status"].Value);
                    comboBox1.SelectedItem = statusValue == 5 ? "Active" : "InActive";
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
            this.Hide();
        }
    }
}

