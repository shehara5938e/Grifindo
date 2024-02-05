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

namespace Grifindo
{
    public partial class settings : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='D:\Files SHEHA25\SE Projects\Grifindo\Database.mdf';Integrated Security=True");
        string id = "0";

        public void RefreshDGV()
        {
            string Query = "Select * From Settings";
            SqlDataAdapter adapter = new SqlDataAdapter(Query, conn);
            DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds, "Settings");
            dataGridView1.DataSource = ds.Tables[0];
        }

        public settings()
        {
            InitializeComponent();
        }

        private void settings_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'settingsDataSet.Settings' table. You can move, or remove it, as needed.
            this.settingsTableAdapter.Fill(this.settingsDataSet.Settings);

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ScID = textBox1.Text;
            DateTime Start = dateTimePicker1.Value;
            DateTime End = dateTimePicker2.Value;
            int Duration = int.Parse(textBox3.Text);
            int Leaves = int.Parse(textBox5.Text);

            if (ScID != "" && Duration != 0 && Leaves != 0)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Settings(SalaryCycleID, SalaryCycleStart, SalaryCycleEnd, SalaryCycleDuration, AllowedLeaves) VALUES('" + ScID + "', '" + Start + "', '" + End + "', '" + Duration + "', '" + Leaves + "')", conn);

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
                textBox3.Clear();
                textBox5.Clear();
                textBox6.Clear();
                dateTimePicker1.DataBindings.Clear();
                dateTimePicker2.DataBindings.Clear();

                RefreshDGV();
            }
            else
            {
                MessageBox.Show("Please Fill Requied Spaces To Save.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string ID = textBox6.Text;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Settings WHERE Id = '" + ID + "'", conn);

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

                            textBox1.Text = reader["SalaryCycleID"].ToString();
                            dateTimePicker1.Value = DateTime.Parse(reader["SalaryCycleStart"].ToString());
                            dateTimePicker2.Value = DateTime.Parse(reader["SalaryCycleEnd"].ToString());
                            textBox3.Text = reader["SalaryCycleDuration"].ToString();
                            textBox5.Text = reader["AllowedLeaves"].ToString();
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
            string ScID = textBox1.Text;
            DateTime Start = dateTimePicker1.Value;
            DateTime End = dateTimePicker2.Value;
            int Duration = int.Parse(textBox3.Text);
            int Leaves = int.Parse(textBox5.Text);

            if (ScID != "" && Duration != 0 && Leaves != 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE Settings SET SalaryCycleID = '" + ScID + "', SalaryCycleStart = '" + Start + "', SalaryCycleEnd = '" + End + "', SalaryCycleDuration = '" + Duration + "', AllowedLeaves = '" + Leaves + "' WHERE id = '" + id + "'", conn);

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
                textBox3.Clear();
                textBox5.Clear();
                dateTimePicker1.DataBindings.Clear();
                dateTimePicker2.DataBindings.Clear();

                RefreshDGV();
            }
            else
            {
                MessageBox.Show("Please Fill Requied Spaces To Edit.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Settings WHERE Id = '" + id + "' ", conn);

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
            textBox3.Clear();
            textBox5.Clear();
            dateTimePicker1.DataBindings.Clear();
            dateTimePicker2.DataBindings.Clear();

            RefreshDGV();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox5.Clear();
            dateTimePicker1.DataBindings.Clear();
            dateTimePicker2.DataBindings.Clear();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin admin = new Admin(); 
            admin.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            employee employee = new employee();
            employee.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            salary salary = new salary();
            salary.Show();
        }
    }
}
