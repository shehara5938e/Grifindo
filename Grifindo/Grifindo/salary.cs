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

namespace Grifindo
{
    public partial class salary : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='D:\Files SHEHA25\SE Projects\Grifindo\Database.mdf';Integrated Security=True");
        string id = "0";

        public void RefreshDGV()
        {
            string Query = "SELECT * FROM Salary";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, conn);
            DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds, "Salary");
            dataGridView1.DataSource = ds.Tables[0];
        }

        public salary()
        {
            InitializeComponent();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string Employee = textBox1.Text;
            string ScID = textBox6.Text;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Settings WHERE SalaryCycleID = @ScID", conn);
            SqlCommand cmd1 = new SqlCommand("SELECT * FROM Employee WHERE Employee = @Employee", conn);

            cmd.Parameters.AddWithValue("@ScID", ScID);
            cmd1.Parameters.AddWithValue("@Employee", Employee);

            if (ScID != "" && Employee != "")
            {
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            textBox4.Text = DateTime.Parse(reader["SalaryCycleEnd"].ToString()).ToString("MM-dd-yyyy");
                            textBox8.Text = reader["SalaryCycleDuration"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("0 Results From That Salary Cycle Identifier");
                    }

                    conn.Close();
                    conn.Open();

                    SqlDataReader reader1 = cmd1.ExecuteReader();

                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            textBox7.Text = reader1["MonthlySalary"].ToString();
                            textBox9.Text = reader1["OvertimeRate"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("0 Results From That Employee Name");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int Holidays = int.Parse(textBox11.Text);
            int Absents = int.Parse(textBox10.Text);
            int OvertimeH = int.Parse(textBox13.Text);
            int Leaves = int.Parse(textBox12.Text);

            float Allowances = 0.0f;
            if (!string.IsNullOrWhiteSpace(textBox14.Text))
            {
                Allowances = float.Parse(textBox14.Text);
            }

            float GovTaxRate = 0.0f;
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (float.TryParse(textBox2.Text, out float GovTaxRate1))
                {
                    GovTaxRate = GovTaxRate1 / 100;
                }
            }

            int Duration = 0;
            float OvertimeRate = 0.0f;
            float MonthlySalary = 0.0f;

            if (!string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MonthlySalary = float.Parse(textBox7.Text);
            }
            else
            {
                MessageBox.Show("Missing Imported Data");
            }

            if (!string.IsNullOrWhiteSpace(textBox9.Text))
            {
                OvertimeRate = float.Parse(textBox9.Text);
            }
            else
            {
                MessageBox.Show("Missing Imported Data");
            }

            if (!string.IsNullOrWhiteSpace(textBox8.Text))
            {
                Duration = int.Parse(textBox8.Text);
            }
            else
            {
                MessageBox.Show("Missing Imported Data");
            }

            float OvertimePay = OvertimeH * OvertimeRate;
            float BasePay = MonthlySalary + Allowances + OvertimePay;
            float TotSalary = BasePay + OvertimePay + Allowances;
            float NonPay = (TotSalary / Duration) * Absents;
            float GrossPay = BasePay - (NonPay + (BasePay * GovTaxRate));

            textBox5.Text = Convert.ToString(BasePay);
            textBox16.Text = Convert.ToString(GrossPay);
            textBox3.Text = Convert.ToString(OvertimePay);
            textBox15.Text = Convert.ToString(NonPay);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Employee = textBox1.Text;
            string ScID = textBox6.Text;

            if (string.IsNullOrWhiteSpace(Employee) && string.IsNullOrWhiteSpace(ScID))
            {
                MessageBox.Show("Imported Data Missing");
                return;
            }

            int Holidays = int.Parse(textBox11.Text);
            int Absents = int.Parse(textBox10.Text);
            int OvertimeH = int.Parse(textBox13.Text);
            int Leaves = int.Parse(textBox12.Text);

            float Allowances = 0.0f;
            float GovTaxRate = 0.0f;
            float BasePay = 0.0f;
            float GrossPay = 0.0f;
            float OvertimePay = 0.0f;
            float NonPayValue = 0.0f;

            if (!string.IsNullOrWhiteSpace(textBox14.Text))
            {
                if (float.TryParse(textBox14.Text, out Allowances))
                {

                }
                else
                {
                    MessageBox.Show("Invalid Allowances format.");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (float.TryParse(textBox2.Text, out GovTaxRate))
                {

                }
                else
                {
                    MessageBox.Show("Invalid Government Tax Rate format.");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox5.Text))
            {
                if (float.TryParse(textBox5.Text, out BasePay))
                {

                }
            }

            if (!string.IsNullOrWhiteSpace(textBox16.Text))
            {
                if (float.TryParse(textBox16.Text, out GrossPay))
                {

                }
            }

            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                if (float.TryParse(textBox3.Text, out OvertimePay))
                {

                }
            }

            if (!string.IsNullOrWhiteSpace(textBox15.Text))
            {
                if (float.TryParse(textBox15.Text, out NonPayValue))
                {

                }
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO Salary(Employee, SalaryCycleID, Holidays, Absents, OvertimeH, Leaves, Allowances, TaxRate, BasePay, GrossPay, OvertimePay, NonPay) VALUES('" + Employee + "', '" + ScID + "', '" + Holidays + "', '" + Absents + "', '" + OvertimeH + "', '" + Leaves + "', '" + Allowances + "', '" + GovTaxRate + "', '" + BasePay + "', '" + GrossPay + "', '" + OvertimePay + "', '" + NonPayValue + "')", conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Data Saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                conn.Close();
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();

            RefreshDGV();
        }

        private void salary_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'salaryDataSet.Salary' table. You can move, or remove it, as needed.
            this.salaryTableAdapter.Fill(this.salaryDataSet.Salary);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Employee = textBox1.Text;
            string ScID = textBox6.Text;

            if (string.IsNullOrWhiteSpace(Employee) && string.IsNullOrWhiteSpace(ScID))
            {
                MessageBox.Show("Imported Data Missing");
                return;
            }

            int Holidays = int.Parse(textBox11.Text);
            int Absents = int.Parse(textBox10.Text);
            int OvertimeH = int.Parse(textBox13.Text);
            int Leaves = int.Parse(textBox12.Text);

            float Allowances = 0.0f;
            float GovTaxRate = 0.0f;
            float BasePay = 0.0f;
            float GrossPay = 0.0f;
            float OvertimePay = 0.0f;
            float NonPay = 0.0f;

            if (!string.IsNullOrWhiteSpace(textBox14.Text))
            {
                if (float.TryParse(textBox14.Text, out Allowances))
                {

                }
                else
                {
                    MessageBox.Show("Invalid Allowances format.");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (float.TryParse(textBox2.Text, out GovTaxRate))
                {

                }
                else
                {
                    MessageBox.Show("Invalid Government Tax Rate format.");
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(textBox5.Text))
            {
                if (float.TryParse(textBox5.Text, out BasePay))
                {

                }
            }

            if (!string.IsNullOrWhiteSpace(textBox16.Text))
            {
                if (float.TryParse(textBox16.Text, out GrossPay))
                {

                }
            }

            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                if (float.TryParse(textBox3.Text, out OvertimePay))
                {

                }
            }

            if (!string.IsNullOrWhiteSpace(textBox15.Text))
            {
                if (float.TryParse(textBox15.Text, out NonPay))
                {

                }
            }

            SqlCommand cmd = new SqlCommand("UPDATE Salary SET Employee = '" + Employee + "', SalaryCycleID = '" + ScID + "', Holidays = '" + Holidays + "', Absents = '" + Absents + "', OvertimeH = '" + OvertimeH + "', Leaves = '" + Leaves + "', Allowances = '" + Allowances + "', TaxRate = '" + GovTaxRate + "', BasePay = '" + BasePay + "', GrossPay = '" + GrossPay + "', OvertimePay = '" + OvertimePay + "', NonPay= '" + NonPay + "' where id = '" + id + "'", conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Data Edited");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                conn.Close();
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
            textBox17.Clear();

            RefreshDGV();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string ID = textBox17.Text;
            SqlCommand cmd = new SqlCommand("SELECT * FROM Salary WHERE Id = '" + ID + "'", conn);

            if (ID != "")
            {
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = reader["Id"].ToString();

                            textBox1.Text = reader["Employee"].ToString();
                            textBox6.Text = reader["SalaryCycleID"].ToString();
                            textBox14.Text = reader["Allowances"].ToString();
                            textBox2.Text = reader["TaxRate"].ToString();
                            textBox5.Text = reader["BasePay"].ToString();
                            textBox16.Text = reader["GrossPay"].ToString();
                            textBox3.Text = reader["OvertimePay"].ToString();
                            textBox15.Text = reader["NonPay"].ToString();
                            textBox11.Text = reader["Holidays"].ToString();
                            textBox10.Text = reader["Absents"].ToString();
                            textBox13.Text = reader["OvertimeH"].ToString();
                            textBox12.Text = reader["Leaves"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("0 Results From That ID");
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                    conn.Close();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Salary WHERE Id = '" + id + "' ", conn);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Data Deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                conn.Close();
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
            textBox17.Clear();

            RefreshDGV();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            employee employee = new employee();
            employee.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            settings settings = new settings();
            settings.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin admin = new Admin();
            admin.Show();
        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
        }
    }
}
