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

namespace Grifindo
{
    public partial class Salary : Form
    {
        //---Sql Connection
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");

        //---Static id
        static string id = "0";

        //---clearAll() Fn
        public void clearAll()
        {
            //---Clear textboxes
            textBox4.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox9.Text = string.Empty;
            textBox11.Text = string.Empty;
            textBox12.Text = string.Empty;
            textBox13.Text = string.Empty;

            //---Reset numericUpDown controls
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 0;
            numericUpDown5.Value = 0;

            //---Clear processed data output textboxes
            textBox5.Text = string.Empty;
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox7.Text = string.Empty;

            button8.Enabled = false;
            button9.Enabled = false;
        }

        //---Refresh DGV fn
        public void RefreshDGV()
        {
            string Query = "Select * From Salary";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, con);
            DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds, "Salary");
            dataGridView1.DataSource = ds.Tables[0];
        }

        public Salary()
        {
            InitializeComponent();
        }

        //Salary Processing Codes//Salary Processing Codes//Salary Processing Codes              

        private void button11_Click(object sender, EventArgs e)
        {
            //Import btn

            string EmployeeName = textBox13.Text;
            string identifier = textBox10.Text;

            //---Settings data Import
            SqlCommand cmd = new SqlCommand("SELECT * FROM Setting WHERE Identifier = @Identifier", con);
            cmd.Parameters.AddWithValue("@Identifier", identifier);

            if (!string.IsNullOrEmpty(identifier))
            {
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            textBox14.Text = DateTime.Parse(reader["EndDate"].ToString()).ToString("MM-dd-yyyy");
                            textBox9.Text = reader["Duration"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable To Fetch Settings Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Enter An Identifier to Fetch Settings data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //---Employee Data Import
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM Employee WHERE Name = @Name", con);
            cmd1.Parameters.AddWithValue("@Name", EmployeeName);

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                try
                {
                    con.Open();
                    SqlDataReader reader1 = cmd1.ExecuteReader();

                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            textBox11.Text = reader1["OvertimeRate"].ToString();
                            textBox12.Text = reader1["MonthlySalary"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable To Fetch Employee Data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Enter An Employee Name to Fetch Employee data", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //Import btn
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Calaculate btn

            //---User Inputs
            int Holidays = Convert.ToInt32(numericUpDown1.Value);
            int Absents = Convert.ToInt32(numericUpDown2.Value);
            int OvertimeH = Convert.ToInt32(numericUpDown3.Value);
            int Leaves = Convert.ToInt32(numericUpDown5.Value);
            float Allowances = 0.0f;
            float GovTaxRate = 0.0f;

            if (!string.IsNullOrWhiteSpace(textBox4.Text))
            {
                Allowances = float.Parse(textBox4.Text);
            }
            else
            {
                MessageBox.Show("Allowances cannot be empty. Please enter a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                        
            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                if (float.TryParse(textBox3.Text, out float parsedGovTaxRate))
                {
                    GovTaxRate = parsedGovTaxRate / 100;
                }
                else
                {
                    MessageBox.Show("Invalid Government Tax Rate format. Please enter a valid percentage value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Government Tax Rate cannot be empty. Please enter a valid value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //---Imported Data
            int Duration = 0;
            float OvertimeRate = 0.0f;
            float MonthlySalary = 0.0f;

            if (!string.IsNullOrWhiteSpace(textBox9.Text))
            {
                Duration = int.Parse(textBox9.Text);
            }
            else
            {
                MessageBox.Show("Duration cannot be empty. Please import the required data to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(textBox11.Text))
            {
                OvertimeRate = float.Parse(textBox11.Text);
            }
            else
            {
                MessageBox.Show("Overtime Rate cannot be empty. Please import the required data to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrWhiteSpace(textBox12.Text))
            {
                MonthlySalary = float.Parse(textBox12.Text);
            }
            else
            {
                MessageBox.Show("Monthly Salary cannot be empty. Please import the required data to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //---Calculations
            float OvertimePay = OvertimeH * OvertimeRate;
            float BasePay = MonthlySalary + Allowances + OvertimePay;
            float TotSalary = BasePay + OvertimePay + Allowances;
            float NonPay = (TotSalary / Duration) * Absents;
            float GrossPay = BasePay - (NonPay + (BasePay * GovTaxRate));

            //---Processed Data Output
            textBox5.Text = Convert.ToString(BasePay);
            textBox1.Text = Convert.ToString(GrossPay);
            textBox2.Text = Convert.ToString(OvertimePay);
            textBox7.Text = Convert.ToString(NonPay);

            //Calaculate btn
        }

        //Salary Processing Codes//Salary Processing Codes//Salary Processing Codes

        private void button3_Click(object sender, EventArgs e)
        {
            //Settings tab

            Settings settings = new Settings();
            settings.Show();
            this.Hide();

            //Settings tab
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Employee tab

            Employee employee = new Employee();
            employee.Show();
            this.Hide();

            //Employee tab
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

        private void Salary_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'salary_DataSet.Salary' table. You can move, or remove it, as needed.
            this.salaryTableAdapter.Fill(this.salary_DataSet.Salary);

            button8.Enabled = false;
            button9.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            string EmployeeName = textBox13.Text;
            string SalaryIdentifier = textBox10.Text;

            //---Null Data Prevention
            if (string.IsNullOrWhiteSpace(EmployeeName) || string.IsNullOrWhiteSpace(SalaryIdentifier))
            {
                MessageBox.Show("Employee Name and Salary Identifier cannot be empty. Please enter valid values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int Holidays = Convert.ToInt32(numericUpDown1.Value);
            int AbsentDays = Convert.ToInt32(numericUpDown2.Value);
            int OvertimeH = Convert.ToInt32(numericUpDown3.Value);
            int Leaves = Convert.ToInt32(numericUpDown5.Value);

            float Allowances = 0.0f;
            float GovTaxRate = 0.0f;
            float BasePay = 0.0f;
            float GrossPay = 0.0f;
            float OvertimePay = 0.0f;
            float NonPayValue = 0.0f;

            //---Null Data Prevention
            if (!string.IsNullOrWhiteSpace(textBox4.Text))
            {
                if (float.TryParse(textBox4.Text, out Allowances))
                {
                    
                }
                else
                {
                    MessageBox.Show("Invalid Allowances format. Please enter a valid numeric value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                if (float.TryParse(textBox3.Text, out GovTaxRate))
                {
                    
                }
                else
                {
                    MessageBox.Show("Invalid Government Tax Rate format. Please enter a valid percentage value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox5.Text))
            {
                if (float.TryParse(textBox5.Text, out BasePay))
                {
                    
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (float.TryParse(textBox1.Text, out GrossPay))
                {
                    
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (float.TryParse(textBox2.Text, out OvertimePay))
                {
                    
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox7.Text))
            {
                if (float.TryParse(textBox7.Text, out NonPayValue))
                {
                    
                }
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO Salary(EmployeeName, SalaryCycleIdentifier, Holidays, AbsentDays, OvertimeHours, Leaves, Allowances, [GovTaxRate%], BasePay, GrossPay, [Non-PayValue], OvertimePay) VALUES('" + EmployeeName + "', '" + SalaryIdentifier + "', '" + Holidays + "', '" + AbsentDays + "', '" + OvertimeH + "', '" + Leaves + "', '" + Allowances + "', '" + GovTaxRate + "', '" + BasePay + "', '" + GrossPay + "', '" + NonPayValue + "', '" + OvertimePay + "')", con);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                clearAll();
                MessageBox.Show(EmployeeName + "'s " + SalaryIdentifier + " Salary Data Saved Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

            RefreshDGV();

            // Save btn// Save btn// Save btn
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //search btn//search btn//search btn

            string ID = textBox8.Text;
            SqlCommand cmd = new SqlCommand("select * from Salary where Id = '" + ID + "'", con);

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
                            
                            //---Textbox
                            textBox13.Text = reader["EmployeeName"].ToString();
                            textBox10.Text = reader["SalaryCycleIdentifier"].ToString();
                            textBox4.Text = reader["Allowances"].ToString();
                            textBox3.Text = reader["GovTaxRate%"].ToString();
                            textBox5.Text = reader["BasePay"].ToString();
                            textBox1.Text = reader["GrossPay"].ToString();
                            textBox2.Text = reader["OvertimePay"].ToString();
                            textBox7.Text = reader["Non-PayValue"].ToString();

                            //---NumericUpDown
                            numericUpDown1.Value = Convert.ToInt32(reader["Holidays"].ToString());
                            numericUpDown1.Value = Convert.ToInt32(reader["AbsentDays"].ToString());
                            numericUpDown1.Value = Convert.ToInt32(reader["OvertimeHours"].ToString());
                            numericUpDown1.Value = Convert.ToInt32(reader["Leaves"].ToString());

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
                MessageBox.Show("First Enter The Salary ID You Want To Search", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //search btn//search btn//search btn
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Update btn// Update btn// Update btn

            string EmployeeName = textBox13.Text;
            string SalaryIdentifier = textBox10.Text;

            //---Null Data Prevention
            if (string.IsNullOrWhiteSpace(EmployeeName) || string.IsNullOrWhiteSpace(SalaryIdentifier))
            {
                MessageBox.Show("Employee Name and Salary Identifier cannot be empty. Please enter valid values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int Holidays = Convert.ToInt32(numericUpDown1.Value);
            int AbsentDays = Convert.ToInt32(numericUpDown2.Value);
            int OvertimeH = Convert.ToInt32(numericUpDown3.Value);
            int Leaves = Convert.ToInt32(numericUpDown5.Value);

            float Allowances = 0.0f;
            float GovTaxRate = 0.0f;
            float BasePay = 0.0f;
            float GrossPay = 0.0f;
            float OvertimePay = 0.0f;
            float NonPayValue = 0.0f;

            //----Null Data Prevention
            if (!string.IsNullOrWhiteSpace(textBox4.Text))
            {
                if (float.TryParse(textBox4.Text, out Allowances))
                {
                    
                }
                else
                {
                    MessageBox.Show("Invalid Allowances format. Please enter a valid numeric value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                if (float.TryParse(textBox3.Text, out GovTaxRate))
                {
                    
                }
                else
                {
                    MessageBox.Show("Invalid Government Tax Rate format. Please enter a valid percentage value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox5.Text))
            {
                if (float.TryParse(textBox5.Text, out BasePay))
                {
                    
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (float.TryParse(textBox1.Text, out GrossPay))
                {
                    
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (float.TryParse(textBox2.Text, out OvertimePay))
                {
                    
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox7.Text))
            {
                if (float.TryParse(textBox7.Text, out NonPayValue))
                {
                    
                }
            }

            SqlCommand cmd = new SqlCommand("update Salary Set EmployeeName = '" + EmployeeName + "', SalaryCycleIdentifier = '" + SalaryIdentifier + "', Holidays = '" + Holidays + "', Absentdays = '" + AbsentDays + "', OvertimeHours = '" + OvertimeH + "', Leaves = '" + Leaves + "', Allowances = '" + Allowances + "', GovTaxRate% = '" + GovTaxRate + "', BasePay = '" + BasePay + "', GrossPay = '" + GrossPay + "', Non-PayValue = '" + NonPayValue + "', OvertimePay = '" + OvertimePay + "' where id = '" + id + "'", con);

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

            // Update btn// Update btn// Update btn
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //delete btn//delete btn//delete btn

            SqlCommand cmd = new SqlCommand("Delete from Salary where Id = '" + id + "' ", con);

            if (MessageBox.Show("Are You Sure?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    clearAll();
                    MessageBox.Show("Salary Data Deleted Succesfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
