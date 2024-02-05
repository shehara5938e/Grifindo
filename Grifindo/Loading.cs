using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grifindo
{
    public partial class Loading : Form
    {
        static int point = 0;

        public Loading()
        {
            InitializeComponent();
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            point += 1;
            bunifuCircleProgressbar1.Value = point;
            if(bunifuCircleProgressbar1.Value == 100)
            {
                timer1.Stop();

                Login login = new Login();
                login.Show();
                this.Hide();
            }

            if (point == 1)
            {
                label4.Text = "Executing exe File...";
            }
            else if (point == 10)
            {
                label4.Text = "Loading Application...";
            }
            else if (point == 15)
            {
                label4.Text = "Database Connecting...";
            }
            else if (point == 20)
            {
                label4.Text = "Database Connected.";
            }
            else if (point == 35)
            {
                label4.Text = "Checking Errors & Fetching Data...";
            }
            else if (point == 45)
            {
                label4.Text = "Security Checkup Succeed.";
            }
            else if (point == 65)
            {
                label4.Text = "Collecting Realtime Data...";
            }
            else if (point == 85)
            {
                label4.Text = "No Errors & Data fetched Successfully.";
            }
            else if (point == 90)
            {
                label4.Text = "Validation Process Completed.";
            }
            else if (point == 95)
            {
                label4.Text = "Loading UI...";
            }
            else if (point == 100)
            {
                label4.Text = "Completed.";
            }
        }
    }
}
