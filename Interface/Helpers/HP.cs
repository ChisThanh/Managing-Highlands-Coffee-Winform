using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Helpers
{
    public class HP
    {
        static public string GetLast2Words(string input)
        {
            string[] words = input.Split(' ');
            string result = "";

            if (words.Length >= 2)
            {
                result = words[words.Length - 2] + " " + words[words.Length - 1];
            }
            return result;
        }
    }
}
