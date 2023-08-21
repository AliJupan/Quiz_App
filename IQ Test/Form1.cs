using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using IQTestApp;
using Newtonsoft.Json.Linq;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data.SqlClient;

namespace IQ_Test
{
    public partial class Form1 : Form
    {
        public Question question = new Question();
        public int question_no = 0;
        public string username;
        private const string ConnectionString = "Data Source=DESKTOP-0SV48E5;Initial Catalog=IQ_Test;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (question_no != 3)
            {
                string apiUrl = "https://opentdb.com/api.php?amount=1&category=18&difficulty=medium&type=multiple";
                await question.FetchQuestionFromApi(apiUrl);

                label1.Text = "Question: " + question.QuestionText;
                label2.Text = "Correct Answer: " + question.CorrectAnswer;
                question_no++;
                label3.Text = "Question Number: "+question_no.ToString();
            }
            if(question_no == 3)
            {
                button1.Enabled = false;
                InsertUserIntoLeaderboard(label4.Text.ToString(), question_no);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = username;
        }

        private void InsertUserIntoLeaderboard(string name, int points)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Leaderboard (Name, Points) VALUES (@Name, @Points)";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Points", points);
                command.ExecuteNonQuery();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmleaderbord frmleaderbord = new frmleaderbord();
            frmleaderbord.ShowDialog();
        }
    }
}
