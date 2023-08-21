using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IQ_Test
{
    public partial class Form2 : Form
    {
        public User User;
        public Form2()
        {
            InitializeComponent();
        }

        private void btnenter_Click(object sender, EventArgs e)
        {
           if(ValidateChildren())
            {
                User = new User(txtusername.Text);
                MessageBox.Show(User.Name);
                Form1 frm = new Form1();
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
                e.Cancel = true; // Cancel validation and keep focus on the control
            }
            else
            {
                errorProvider1.SetError(txtusername, ""); // Clear error message if validation succeeds
                e.Cancel = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmleaderbord frmleaderbord = new frmleaderbord();
            frmleaderbord.ShowDialog();
        }
    }
}
