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

namespace ProjectB
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
            GetData();
            addStudentNamesIntoComboBox();
            addComponentMarksIntoComboBox();
            addAssessmentNameIntoComboBox();
            addRubricLevelsIntoComboBox();
            addRubricDetailsIntoComboBox();
            addRubricLevelIdIntoComboBox();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.Show();
            this.Hide();
        }
        private void addStudentNamesIntoComboBox()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT FirstName FROM Student", con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["FirstName"].ToString();
                    if (!comboBox1.Items.Contains(name))
                    {
                        comboBox1.Items.Add(name);
                    }
                }

                reader.Close();
            }
        }
        private void addComponentMarksIntoComboBox()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT TotalMarks FROM AssessmentComponent", con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int marks = Convert.ToInt32(reader["TotalMarks"]);
                    if (!comboBox4.Items.Contains(marks))
                    {
                        comboBox4.Items.Add(marks);
                    }
                }
                reader.Close();
            }
        }
        private void addAssessmentNameIntoComboBox()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT Name FROM AssessmentComponent", con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["Name"].ToString();
                    if (!comboBox6.Items.Contains(name))
                    {
                        comboBox6.Items.Add(name);
                    }
                }
                reader.Close();
            }
        }
        private void addRubricLevelsIntoComboBox()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT MeasurementLevel FROM RubricLevel", con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int level = Convert.ToInt32(reader["MeasurementLevel"]);
                    if (!comboBox3.Items.Contains(level))
                    {
                        comboBox3.Items.Add(level);
                    }
                }
                reader.Close();
            }
        }
        private void addRubricDetailsIntoComboBox()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT Details FROM RubricLevel", con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string rubricDetails = reader["Details"].ToString();
                    if (!comboBox5.Items.Contains(rubricDetails))
                    {
                        comboBox5.Items.Add(rubricDetails);
                    }
                }
                reader.Close();
            }
        }
        private void addRubricLevelIdIntoComboBox()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("SELECT Id FROM RubricLevel", con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["Id"]);
                    if (!comboBox2.Items.Contains(Id))
                    {
                        comboBox2.Items.Add(Id);
                    }
                }
                reader.Close();
            }
        }
        private void GetData()
        {
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = @"
    SELECT
        StudentResult.ObtainedMarks,
        Student.FirstName AS Student,
        RubricLevel.Details,
        Assessment.Title AS AssessmentTitle,
        AssessmentComponent.TotalMarks AS ComponentMarks,
        RubricLevel.MeasurementLevel
    FROM
        StudentResult
    INNER JOIN Student ON StudentResult.StudentId = Student.Id
    INNER JOIN AssessmentComponent ON StudentResult.AssessmentComponentId = AssessmentComponent.Id
    INNER JOIN Assessment ON AssessmentComponent.AssessmentId = Assessment.Id
    INNER JOIN Rubric ON AssessmentComponent.RubricId = Rubric.Id
    INNER JOIN RubricLevel ON Rubric.Id = RubricLevel.RubricId;
";
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

        private void button1_Click(object sender, EventArgs e)  //EVALUATE CODE
        {
            EvaluateStudent();
        }
        private void EvaluateStudent()
        {
            // Calculate obtained marks
            int componentMarks = Convert.ToInt32(comboBox4.SelectedItem);
            int rubricLevelValue = Convert.ToInt32(comboBox3.SelectedItem);
            int obtainedMarks = (int)Math.Round((double)rubricLevelValue / 4 * componentMarks);
            // Get other selected values
            int studentId = GetStudentIdByName(comboBox1.SelectedItem.ToString());
            int assessmentComponentId = GetAssessmentComponentIdByName(comboBox6.SelectedItem.ToString());
            int rubricMeasurementId = Convert.ToInt32(comboBox2.SelectedItem);
            DateTime evaluationDate = dateTimePicker1.Value;

            // Add data to StudentResult table
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";
            string query = "INSERT INTO StudentResult (StudentId, AssessmentComponentId, RubricMeasurementId, EvaluationDate, ObtainedMarks) " +
                           "VALUES (@StudentId, @AssessmentComponentId, @RubricMeasurementId, @EvaluationDate, @ObtainedMarks)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentId", studentId);
                command.Parameters.AddWithValue("@AssessmentComponentId", assessmentComponentId);
                command.Parameters.AddWithValue("@RubricMeasurementId", rubricMeasurementId);
                command.Parameters.AddWithValue("@EvaluationDate", evaluationDate);
                command.Parameters.AddWithValue("@ObtainedMarks", obtainedMarks);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Student evaluation successful!");
                        GetData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to evaluate student.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
        private int GetStudentIdByName(string studentName)
        {
            int studentId = 0;
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id FROM Student WHERE FirstName = @StudentName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentName", studentName);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            studentId = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log or display error message)
                    }
                }
            }

            return studentId;
        }
        private int GetAssessmentComponentIdByName(string assessmentName)
        {
            int assessmentComponentId = 0;
            string connectionString = "Data Source=TALHA;Initial Catalog=ProjectB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id FROM AssessmentComponent WHERE Name = @AssessmentName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AssessmentName", assessmentName);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            assessmentComponentId = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exception (e.g., log or display error message)
                    }
                }
            }

            return assessmentComponentId;
        }
    }
}
