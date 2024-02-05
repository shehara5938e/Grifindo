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
    public partial class employee : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='D:\Files SHEHA25\SE Projects\Grifindo\Database.mdf';Integrated Security=True");
        string id = "0";

        public void RefreshDGV()
        {
            string Query = "Select * From Employee";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, conn);
            DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds, "Employee");
            dataGridView1.DataSource = ds.Tables[0];
        }

        public employee()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            salary salary = new salary();
            salary.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            settings settings = new settings();
            settings.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Employee = textBox1.Text;
            string Address = textBox2.Text;
            string Phone = textBox3.Text;
            float MonthlySalary = float.Parse(textBox5.Text);
            float OvertimeRate = float.Parse(textBox6.Text);

            if (Employee != "" && Address != "" && Phone != "" && MonthlySalary != 0 && OvertimeRate != 0)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Employee(Employee, Address, Phone, MonthlySalary, OVertimeRate) VALUES('" + Employee + "', '" + Address + "', '" + Phone + "', '" + MonthlySalary + "', '" + OvertimeRate + "')", conn);

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
                textBox5.Clear();
                textBox6.Clear();
                textBox17.Clear();

                RefreshDGV();
            }
            else
            {
                MessageBox.Show("Please Fill Requied Spaces To Save.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string ID = textBox17.Text;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Employee WHERE Id = '" + ID + "'", conn);

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
                            textBox2.Text = reader["Address"].ToString();
                            textBox3.Text = reader["Phone"].ToString();
                            textBox5.Text = reader["MonthlySalary"].ToString();
                            textBox6.Text = reader["OvertimeRate"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("0 Results From This ID");
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

        private void button2_Click(object sender, EventArgs e)
        {
            string Employee = textBox1.Text;
            string Address = textBox2.Text;
            string Phone = textBox3.Text;
            float MonthlySalary = float.Parse(textBox5.Text);
            float OvertimeRate = float.Parse(textBox6.Text);

            if (Employee != "" && Address != "" && Phone != "" && MonthlySalary != 0 && OvertimeRate != 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE Employee SET Employee = '" + Employee + "', Address = '" + Address + "', Phone = '" + Phone + "', MonthlySalary = '" + MonthlySalary + "', OvertimeRate = '" + OvertimeRate + "' WHERE id = '" + id + "'", conn);

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
                textBox5.Clear();
                textBox6.Clear();
                textBox17.Clear();

                RefreshDGV();
            }
            else
            {
                MessageBox.Show("Please Fill Requied Spaces To Edit.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Employee WHERE Id = '" + id + "' ", conn);

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
            textBox5.Clear();
            textBox6.Clear();
            textBox17.Clear();

            RefreshDGV();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox17.Clear();
        }

        private void employee_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'employeeDataSet.Employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.employeeDataSet.Employee);

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
