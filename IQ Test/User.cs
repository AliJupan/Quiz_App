using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQ_Test
{
    public class User
    {
        public string Name { get; set; }
        public int Points { get; set; }

        public User(string name)
        {
            Name = name;
        }

        public void IncreaseScore(int points)
        {
            Points += points;
        }

        public void DecreaseScore(int points)
        {
            Points -= points;
        }

        public void ResetScore()
        {
            Points = 0;
        }
    }
}
