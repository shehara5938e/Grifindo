using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grifindo
{

    internal class DatabaseConnection
    {
        SqlConnection con;

        public SqlConnection conn
        {
            get { return con; }
        }

        public void OpenConnection()
        {
            string conString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";
            con = new SqlConnection(conString);
            con.Open();
        }

        public void CloseConnection()
        {
            con.Close();
        }
    }

}
