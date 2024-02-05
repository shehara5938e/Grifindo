using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grifindo
{
    public partial class Dashboard : Form
    {
        //---Sql Connection
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");

        public Dashboard()
        {
            InitializeComponent();
        }

        //---Greating Time Date Fn
        private void UpdateTime()
        {
            //---Date Time Greeting
            label5.Text = DateTime.Now.ToString("T");
            label4.Text = DateTime.Now.ToString("D");

            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            string greetingMessage;
            if (currentHour >= 00 && currentHour < 12)
            {
                greetingMessage = "Good Morning!";
                SunMorningPic.Visible = true;
            }
            else if (currentHour >= 12 && currentHour < 18)
            {
                greetingMessage = "Good Afternoon!";
                SunAfternoonPic.Visible = true;
            }
            else
            {
                greetingMessage = "Good Evening!";
                MoonPic.Visible = true;
            }

            label3.Text = greetingMessage;

            label19.Text = DateTime.Now.ToString("yyyy");
            label20.Text = DateTime.Now.ToString("MM");
            label21.Text = DateTime.Now.ToString("dd");

            label22.Text = DateTime.Now.ToString("HH");
            label23.Text = DateTime.Now.ToString("mm");
            label24.Text = DateTime.Now.ToString("ss");
            label25.Text = DateTime.Now.ToString("fff");
        }

        //---Greeting Time Date Updater
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            timer1.Start();
            UpdateTime();

            MoonPic.Visible = false;
            SunMorningPic.Visible = false;
            SunAfternoonPic.Visible = false;

            //---Data Display
            con.Open();

            SqlCommand cmd1 = null;
            cmd1 = new SqlCommand("Select Count(*) From Employee", con);

            int EmployeeCount = Convert.ToInt32(cmd1.ExecuteScalar());
            label6.Text = EmployeeCount.ToString();

            SqlCommand cmd2 = null;
            cmd2 = new SqlCommand("Select Count(*) From Salary", con);

            int SalaryCount = Convert.ToInt32(cmd2.ExecuteScalar());
            label9.Text = SalaryCount.ToString();

            SqlCommand cmd3 = null;
            cmd3 = new SqlCommand("Select Count (*) From Setting", con);

            int Settingscount = Convert.ToInt32(cmd3.ExecuteScalar());
            label11.Text = Settingscount.ToString();


            con.Close();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            //close entier application

            Application.Exit();

            //close entier application
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

        private void button1_Click(object sender, EventArgs e)
        {
            //Employee tab

            Employee employee = new Employee();
            employee.Show();
            this.Hide();

            //Employee tab
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Logout btn

            Login login = new Login();
            login.Show();
            this.Hide();

            //Logout btn
        }
    }
}
