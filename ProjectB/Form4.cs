using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectB
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            GetData();
            addRegIntoCombo();
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
        private void addRegIntoCombo()
        {
            string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand command = new SqlCommand("SELECT RegistrationNumber FROM Student", con);
            SqlDataReader reader;
            reader = command.ExecuteReader();

            comboBox2.Items.Clear();

            while (reader.Read())
            {
                comboBox2.Items.Add(reader["RegistrationNumber"].ToString());
            }

            reader.Close();
            con.Close();
        }
        private void GetData()
        {
            string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM StudentAttendance", con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private int GetStudentIdByRegistrationNumber(string registrationNumber)
        {
            int studentId = 0;
            string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                string query = "SELECT Id FROM Student WHERE RegistrationNumber = @RegistrationNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RegistrationNumber", registrationNumber);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        studentId = Convert.ToInt32(result);
                    }
                }
            }

            return studentId;
        }
        private void button3_Click(object sender, EventArgs e) //ADD CODE
        {
            if (AreFieldsComplete())
            {
                string registrationNumber = comboBox2.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(registrationNumber))
                {
                    int studentId = GetStudentIdByRegistrationNumber(registrationNumber);

                    if (studentId > 0) // Check if a valid StudentId is retrieved
                    {
                        string statusText = comboBox1.Text;
                        int statusValue = GetStatusValue(statusText);

                        string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            con.Open();

                            // Insert into ClassAttendance table with AttendanceDate
                            SqlCommand classAttendanceCmd = new SqlCommand("INSERT INTO ClassAttendance (AttendanceDate) VALUES (@AttendanceDate);", con);
                            classAttendanceCmd.Parameters.AddWithValue("@AttendanceDate", DateTime.Now);
                            classAttendanceCmd.ExecuteNonQuery();

                            // Insert into StudentAttendance with AttendanceId, StudentId, and AttendanceStatus
                            SqlCommand studentAttendanceCmd = new SqlCommand("INSERT INTO StudentAttendance (StudentId, AttendanceStatus) VALUES ( @StudentId, @AttendanceStatus)", con);

                            studentAttendanceCmd.Parameters.AddWithValue("@StudentId", studentId);
                            studentAttendanceCmd.Parameters.AddWithValue("@AttendanceStatus", statusValue);
                            studentAttendanceCmd.ExecuteNonQuery();

                            MessageBox.Show("Record inserted successfully!");
                            RemoveAllText();
                            GetData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid registration number. StudentId not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a registration number.");
                }
            }
            else
            {
                MessageBox.Show("Please fill in all the required fields.");
            }
        }


        private int GetStatusValue(string statusText)
        {
            switch (statusText)
            {
                case "Present":
                    return 1;
                case "Absent":
                    return 2;
                case "Leave":
                    return 3;
                case "Late":
                    return 4;
                default:
                    throw new ArgumentException("Invalid status text.");
            }
        }
        private bool AreFieldsComplete()
        {
            if (comboBox1.SelectedItem == null || comboBox1.Text == null)
            {
                return false;
            }
            return true;
        }
        private void RemoveAllText()
        {
            comboBox1.SelectedIndex = -1; // Clear Status dropdown selection
            comboBox2.SelectedIndex = -1; // Clear Status dropdown selection
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
                    int statusValue = Convert.ToInt32(selectedRow.Cells["AttendanceStatus"].Value);
                    switch (statusValue)
                    {
                        case 1:
                            comboBox1.SelectedItem = "Present";
                            break;
                        case 2:
                            comboBox1.SelectedItem = "Absent";
                            break;
                        case 3:
                            comboBox1.SelectedItem = "Leave";
                            break;
                        case 4:
                            comboBox1.SelectedItem = "Late";
                            break;
                        default:
                            comboBox1.SelectedItem = null;
                            break;
                    }
                    int studentId = Convert.ToInt32(selectedRow.Cells["StudentId"].Value);

                    string registrationNumber = GetRegistrationNumberByStudentId(studentId);
                    comboBox2.SelectedItem = registrationNumber;
                }
            }
        }
        private string GetRegistrationNumberByStudentId(int studentId)
        {
            string registrationNumber = string.Empty;
            string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "SELECT RegistrationNumber FROM Student WHERE Id = @StudentId";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        registrationNumber = result.ToString();
                    }
                }
            }

            return registrationNumber;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRegistrationNumber = comboBox2.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedRegistrationNumber))
            {
                int studentId = GetStudentIdByRegistrationNumber(selectedRegistrationNumber);

                if (studentId > 0)
                {
                    MessageBox.Show($"Selected StudentId: {studentId}");
                }
                else
                {
                    MessageBox.Show("Invalid registration number. StudentId not found.");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)  //DELETE CODE
        {
            if (dataGridView1.SelectedRows.Count == 1) // Check if a single row is selected
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Retrieve the student ID from the selected row
                int studentId = Convert.ToInt32(selectedRow.Cells["StudentId"].Value);

                if (studentId > 0) // Check if a valid StudentId is retrieved
                {
                    string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();

                        // Delete attendance records for the selected student
                        SqlCommand deleteAttendanceCmd = new SqlCommand("DELETE FROM StudentAttendance WHERE StudentId = @StudentId", con);
                        deleteAttendanceCmd.Parameters.AddWithValue("@StudentId", studentId);
                        int rowsAffected = deleteAttendanceCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Attendance record deleted successfully!");
                            GetData(); // Refresh the data in the DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No attendance record found for the selected student.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid student ID. Attendance record not found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a single row in the DataGridView.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e) //UPDATE CODE
        {
            if (AreFieldsComplete())
            {
                string registrationNumber = comboBox2.SelectedItem?.ToString();
                if (dataGridView1.SelectedRows.Count == 1) // Check if a row is selected
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int studentId = GetStudentIdByRegistrationNumber(registrationNumber);
                        int attendanceID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["AttendanceID"].Value);


                        string statusText = comboBox1.Text;
                        int statusValue = GetStatusValue(statusText);

                        string constr = "Data Source=HP;Initial Catalog=ProjectB;Integrated Security=True;";
                        string cmd = "UPDATE StudentAttendance SET StudentId = @StudentId , AttendanceStatus = @AttendanceStatus WHERE AttendanceId = @AttendanceId";

                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand(cmd, con);
                            command.Parameters.AddWithValue("@AttendanceId", attendanceID);
                            command.Parameters.AddWithValue("@StudentId", studentId);
                            command.Parameters.AddWithValue("@AttendanceStatus", statusValue);


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
                MessageBox.Show("Please fill in all fields before updating.");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
