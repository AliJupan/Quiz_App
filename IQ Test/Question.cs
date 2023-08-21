using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IQTestApp
{
    public class Question
    { 
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }

        public async Task FetchQuestionFromApi(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();


                        JObject jsonResponse = JObject.Parse(responseBody);

                        QuestionText = jsonResponse["results"][0]["question"].ToString();
                        CorrectAnswer = jsonResponse["results"][0]["correct_answer"].ToString();
                    }
                    else
                    {
                        QuestionText = "Request failed with status code: " + response.StatusCode;
                        CorrectAnswer = "Request failed with status code: " + response.StatusCode;
                    }
                }
                catch (Exception ex)
                {
                    QuestionText = "An error occurred: " + ex.Message;
                    CorrectAnswer = "An error occurred: " + ex.Message;
                }
            }
        }
    }
}
