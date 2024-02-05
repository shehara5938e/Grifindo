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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Exit Entire Application

            Application.Exit();

            //Exit Entire Application
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //login btn

            string Username = textBox1.Text;
            string Password = textBox2.Text;
            
            if (Username == "Admin" && Password == "Admin")
            {
                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                this.Hide();
            }
            else if (Username == "" && Password == "")
            {
                MessageBox.Show("Please Enter User Credentials To Proceed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Invalid User Credentials", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //login btn
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //show hide password

            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }

            //show hide password
        }
    }
}
