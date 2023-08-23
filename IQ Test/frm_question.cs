using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using IQTestApp;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace IQ_Test
{
    public partial class frm_question : Form
    {
        public Question question = new Question();
        public int question_no = 0;
        public int total_question = 15;
        public string username;
        private const string ConnectionString = "Data Source=DESKTOP-0SV48E5;Initial Catalog=IQ_Test;Integrated Security=True";
        private int correctAnswerIndex = -1;
        public User User;

        public frm_question()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            User = new User(username);
            label4.Text ="Player: " + User.Name.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NextQuestion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm_leaderbord frmleaderbord = new frm_leaderbord();
            frmleaderbord.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your score has been added to leaderboard", "LeaderBoard", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InsertUserIntoLeaderboard(User.Name, User.Points);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            RestartQuiz();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("User "+User.Name+" has "+User.Points.ToString(),"User Points",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void AnswerButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int clickedButtonIndex = GetButtonIndex(clickedButton);

            if (clickedButtonIndex == correctAnswerIndex)
            {
                clickedButton.BackColor = Color.Green;
                User.IncreaseScore(2);
            }
            else
            {
                clickedButton.BackColor = Color.Red;
                User.DecreaseScore(1);
                TurnCorrectAnswerGreen();
                TurnIncorrectButtonsRed(correctAnswerIndex);
            }

            DisableAnswerButtons();
        }

        private void DisableAnswerButtons()
        {
            btnanswer1.Enabled = false;
            btnanswer4.Enabled = false;
            btnanswer2.Enabled = false;
            btnanswer3.Enabled = false;
        }

        private void TurnCorrectAnswerGreen()
        {
            Button correctButton = GetButtonByIndex(correctAnswerIndex);
            correctButton.BackColor = Color.Green;
            correctButton.Enabled = false;
        }

        private void TurnIncorrectButtonsRed(int correctIndex)
        {
            Button[] answerButtons = { btnanswer1, btnanswer3, btnanswer4, btnanswer2 };
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (i != correctIndex)
                {
                    answerButtons[i].BackColor = Color.Red;
                    answerButtons[i].Enabled = false;
                }
            }
        }

        private int GetButtonIndex(Button button)
        {
            Button[] answerButtons = { btnanswer1, btnanswer3, btnanswer4, btnanswer2 };
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (answerButtons[i] == button)
                {
                    return i;
                }
            }
            return -1;
        }

        private Button GetButtonByIndex(int index)
        {
            Button[] answerButtons = { btnanswer1, btnanswer3, btnanswer4, btnanswer2 };
            if (index >= 0 && index < answerButtons.Length)
            {
                return answerButtons[index];
            }
            return null;
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

        private void RestartQuiz()
        {
            if(question_no == total_question)
            {
                User.ResetScore();
                question_no = 0;
                NextQuestion();
            }
        }

        private async void NextQuestion()
        {
            if (question_no == 0)
            {
                btnquestion.Text = "Get Question";
                Button();
                btnquestion.Enabled = true;
            }
            if (question_no != 0)
            {
                btnquestion.Text = "Next Question";
                Button();
            }
            if (question_no != total_question)
            {
                string apiUrl = "https://opentdb.com/api.php?amount=1&category=18&difficulty=medium&type=multiple";
                await question.FetchQuestionFromApi(apiUrl);

                label1.Text = "Question: " + question.QuestionText;
                DisplayAnswerOptions(question.CorrectAnswer, question.IncorrectAnswers);
                question_no++;
                label3.Text = "Question Number: " + question_no.ToString();

                correctAnswerIndex = btnanswer1.Text == question.CorrectAnswer ? 0 :
                                     btnanswer3.Text == question.CorrectAnswer ? 1 :
                                     btnanswer4.Text == question.CorrectAnswer ? 2 : 3;
            }
            if (question_no == total_question)
            {
                btnquestion.Enabled = false;
            }
        }
        private void Button()
        {
            btnanswer1.BackColor = Color.DarkGray;
            btnanswer4.BackColor = Color.DarkGray;
            btnanswer2.BackColor = Color.DarkGray;
            btnanswer3.BackColor = Color.DarkGray;
            btnanswer1.Enabled = true;
            btnanswer4.Enabled = true;
            btnanswer2.Enabled = true;
            btnanswer3.Enabled = true;
        }

        private void DisplayAnswerOptions(string correctAnswer, List<string> incorrectAnswers)
        {
            List<Button> answerButtons = new List<Button> { btnanswer1, btnanswer3, btnanswer4, btnanswer2 };

            List<string> allAnswers = new List<string>();
            allAnswers.Add(correctAnswer);
            allAnswers.AddRange(incorrectAnswers);
            allAnswers = allAnswers.OrderBy(a => Guid.NewGuid()).ToList();

            for (int i = 0; i < answerButtons.Count; i++)
            {
                answerButtons[i].Click -= AnswerButton_Click;
                answerButtons[i].Text = allAnswers[i];
                answerButtons[i].Click += AnswerButton_Click;
            }
        }
    }
}
