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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace IQ_Test
{
    public partial class frm_login : Form
    {
        public User User;
        private const string ConnectionString = "Data Source=DESKTOP-0SV48E5;Initial Catalog=IQ_Test;Integrated Security=True";
        public frm_login()
        {
            InitializeComponent();
        }

        private void btnenter_Click(object sender, EventArgs e)
        {
           if(ValidateChildren())
            {
                User = new User(txtusername.Text);
                frm_question frm = new frm_question();
                frm.username = User.Name; 
                frm.ShowDialog(); 
                this.Close();
            }
        }

        private void btnenter_Validating(object sender, CancelEventArgs e)
        {
            if(txtusername.Text.Length <= 3)
            {         
                errorProvider1.SetError(txtusername, "User Name should be more than 3 characters");
                e.Cancel = true; 
            }
            else if (IsUsernameTaken(txtusername.Text.ToString())) 
            {
                errorProvider1.SetError(txtusername, "Username already taken");
                e.Cancel = true; 
            }
            else
            {
                errorProvider1.SetError(txtusername, "");
                e.Cancel = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm_leaderbord frmleaderbord = new frm_leaderbord();
            frmleaderbord.ShowDialog();
        }

        private bool IsUsernameTaken(string username)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Leaderboard WHERE Name = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
