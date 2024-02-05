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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Grifindo
{
    public partial class Employee : Form
    {
        //---Sql Connection
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");

        //---Static id
        static string id = "0";


        //---clearAll() Fn
        public void clearAll()
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox6.Text = string.Empty;
            textBox5.Text = string.Empty;
        }


        //---Refresh DGV fn
        public void RefreshDGV()
        {
            string Query = "Select * From Employee";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, con);
            DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds, "Employee");
            dataGridView1.DataSource = ds.Tables[0];
        }

        public Employee()
        {
            InitializeComponent();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            //Close Entire Application

            Application.Exit();

            //Close Entire Application
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Logout btn

            Login login = new Login();
            login.Show();
            this.Hide();

            //Logout btn
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Save btn

            //---textbox data
            string Name = textBox1.Text.Trim();
            string Email = textBox2.Text.Trim();
            string Phone = textBox3.Text.Trim();
            string Position = textBox4.Text.Trim();
            float MonthlySalary = 0.0f;
            float OvertimeRate = 0.0f;

            if (!float.TryParse(textBox6.Text, out MonthlySalary) || !float.TryParse(textBox5.Text, out OvertimeRate))
            {
                MessageBox.Show("Monthly Salary and Overtime Rate must be valid numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Position))
            {
                MessageBox.Show("Please fill in all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlCommand cmd = new SqlCommand("insert into Employee(Name, Email, Phone, Position, MonthlySalary, OvertimeRate) values('" + Name + "', '" + Email + "', '" + Phone + "', '" + Position + "', '" + MonthlySalary + "', '" + OvertimeRate + "')", con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                clearAll();
                MessageBox.Show("Employee Data Saved Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

            RefreshDGV();

            //Save btn
        }


        private void Employee_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'employee_DataSet.Employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.employee_DataSet.Employee);

            button8.Enabled = false;
            button9.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //search btn//search btn//search btn

            string Name = textBox8.Text;
            SqlCommand cmd = new SqlCommand("select * from Employee where Name = '" + Name + "'", con);

            if (Name != "")
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

                            textBox1.Text = reader["Name"].ToString();
                            textBox2.Text = reader["Email"].ToString();
                            textBox3.Text = reader["Phone"].ToString();
                            textBox4.Text = reader["Position"].ToString();
                            textBox6.Text = reader["MonthlySalary"].ToString();
                            textBox5.Text = reader["OvertimeRate"].ToString();

                            button8.Enabled = true;
                            button9.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Data Available From This Name: " + Name, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("First Enter The Name You Want To Search", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //search btn//search btn//search btn
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //update btn//update btn//update btn

            //---textbox data
            string Name = textBox1.Text.Trim();
            string Email = textBox2.Text.Trim();
            string Phone = textBox3.Text.Trim();
            string Position = textBox4.Text.Trim();
            float MonthlySalary = 0.0f;
            float OvertimeRate = 0.0f;

            // Check for empty or null MonthlySalary and OvertimeRate fields
            if (!float.TryParse(textBox6.Text, out MonthlySalary) || !float.TryParse(textBox5.Text, out OvertimeRate))
            {
                MessageBox.Show("Monthly Salary and Overtime Rate must be valid numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check for empty Name, Email, Phone, and Position fields
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Position))
            {
                MessageBox.Show("Please fill in all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlCommand cmd = new SqlCommand("update Employee Set Name = '" + Name + "', Email = '" + Email + "', Phone = '" + Phone + "', Position = '" + Position + "', MonthlySalary = '" + MonthlySalary + "', OvertimeRate = '" + OvertimeRate + "' where id = '" + id + "'", con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                clearAll();
                MessageBox.Show("Employee Data Updated Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

            RefreshDGV();

            //update btn//update btn//update btn
        }


        private void button9_Click(object sender, EventArgs e)
        {
            //delete btn//delete btn//delete btn

            SqlCommand cmd = new SqlCommand("Delete from Employee where Id = '" + id + "' ", con);

            if (MessageBox.Show("Are You Sure?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    clearAll();
                    MessageBox.Show("Emloyee Data Deleted Succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            //clear btn

            clearAll();

            //clear btn
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //salary tab

            Salary salary = new Salary();
            salary.Show();
            this.Hide();

            //salary tab
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Settings tab

            Settings settings = new Settings();
            settings.Show();
            this.Hide();

            //Settings tab
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Dashboard btn

            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();

            //Dashboard btn
        }
    }
}
