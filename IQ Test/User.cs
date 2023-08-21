using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQ_Test
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public int Points { get; set; }

        public User(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}",Name,Points);
        }
    }
}
