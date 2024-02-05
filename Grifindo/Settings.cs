using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Grifindo
{
    public partial class Settings : Form
    {
        //---Sql Connection
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");

        //---Static id
        static string id = "0";

        //---clearAll() fn
        public void clearAll()
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;

            button8.Enabled = false;
            button9.Enabled = false;
        }

        //---Refresh DGV fn
        public void RefreshDGV()
        {
            string Query = "Select * From Setting";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, con);
            DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds, "Setting");
            dataGridView1.DataSource = ds.Tables[0];
        }

        public Settings()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Save btn

            string Identifier = textBox1.Text.Trim();

            DateTime StartDate = dateTimePicker1.Value;
            DateTime EndDate = dateTimePicker2.Value;

            int Duration = Convert.ToInt32(numericUpDown1.Value);
            int AllowedLeaves = Convert.ToInt32(numericUpDown2.Value);

            // Check for empty Identifier field
            if (string.IsNullOrEmpty(Identifier))
            {
                MessageBox.Show("Identifier cannot be empty. Please enter a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check for null StartDate
            if (StartDate == null)
            {
                MessageBox.Show("Start Date cannot be null. Please select a valid date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Duration != 0)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Setting(Identifier, StartDate, EndDate, Duration, AllowedLeaves) VALUES('" + Identifier + "', '" + StartDate + "', '" + EndDate + "', '" + Duration + "', '" + AllowedLeaves + "')", con);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    clearAll();
                    MessageBox.Show("Setting Saved Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }

                RefreshDGV();
            }
            else
            {
                MessageBox.Show("Please Provide Required Data To Proceed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Save btn
        }


        private void button5_Click(object sender, EventArgs e)
        {
            //search btn//search btn//search btn

            string ID = textBox8.Text;
            SqlCommand cmd = new SqlCommand("select * from Setting where Id = '" + ID + "'", con);

            if (ID != "")
            {
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = reader["Id"].ToString();

                            textBox1.Text = reader["Identifier"].ToString();

                            dateTimePicker1.Value = DateTime.Parse(reader["StartDate"].ToString());
                            dateTimePicker2.Value = DateTime.Parse(reader["EndDate"].ToString());

                            numericUpDown1.Value = int.Parse(reader["Duration"].ToString());
                            numericUpDown2.Value = int.Parse(reader["AllowedLeaves"].ToString());

                            button8.Enabled = true;
                            button9.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Data Available From This ID: " + ID, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("First Enter The Salary Cycle ID You Want To Search", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //search btn//search btn//search btn
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //update btn//update btn//update btn

            string Identifier = textBox1.Text.Trim();

            DateTime StartDate = dateTimePicker1.Value;
            DateTime EndDate = dateTimePicker2.Value;

            int Duration = Convert.ToInt32(numericUpDown1.Value);
            int AllowedLeaves = Convert.ToInt32(numericUpDown2.Value);

            if (string.IsNullOrEmpty(Identifier))
            {
                MessageBox.Show("Identifier cannot be empty. Please enter a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (StartDate == null)
            {
                MessageBox.Show("Start Date cannot be null. Please select a valid date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Duration != 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE Setting SET Identifier = '" + Identifier + "', StartDate = '" + StartDate + "', EndDate = '" + EndDate + "', Duration = '" + Duration + "', AllowedLeaves = '" + AllowedLeaves + "' WHERE id = '" + id + "'", con);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    clearAll();
                    MessageBox.Show("Setting Updated Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }

                RefreshDGV();
            }
            else
            {
                MessageBox.Show("Please provide the required data to proceed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //update btn//update btn//update btn
        }


        private void button9_Click(object sender, EventArgs e)
        {
            //delete btn//delete btn//delete btn

            SqlCommand cmd = new SqlCommand("Delete from Setting where Id = '" + id + "' ", con);

            if (MessageBox.Show("Are You Sure?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    clearAll();
                    MessageBox.Show("Setting Deleted Succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }

                RefreshDGV();
            }

            //delete btn//delete btn//delete btn
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //clear btn//clear btn//clear btn

            clearAll();

            //clear btn//clear btn//clear btn
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //salary tab

            Salary salary = new Salary();
            salary.Show();
            this.Hide();

            //salary tab
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Employee tab

            Employee employee = new Employee();
            employee.Show();
            this.Hide();

            //Employee tab
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'setting_DataSet1.Setting' table. You can move, or remove it, as needed.
            this.settingTableAdapter.Fill(this.setting_DataSet1.Setting);
            // TODO: This line of code loads data into the 'setting_DataSet.Setting' table. You can move, or remove it, as needed.
            this.settingTableAdapter.Fill(this.setting_DataSet.Setting);
            // TODO: This line of code loads data into the 'setting_DataSet.Setting' table. You can move, or remove it, as needed.
            this.settingTableAdapter.Fill(this.setting_DataSet.Setting);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Logout btn

            Login login = new Login();
            login.Show();
            this.Hide();

            //Logout btn
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            //Close Entire Application

            Application.Exit();

            //Close Entire Application
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime start = dateTimePicker1.Value;
                DateTime end = dateTimePicker2.Value;

                TimeSpan timeSpan = end - start;

                int duration = timeSpan.Days;

                if (duration >= 0)
                {
                    numericUpDown1.Value = duration;
                }
                else
                {
                    MessageBox.Show("End date cannot be before the start date or exceed 365 days.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid date format. Please enter valid dates.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Dashboard btn

            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();

            //Dashboard btn
        }
    }
}
