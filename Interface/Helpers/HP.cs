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
        public static bool ContainsWord(string input, string searchTerm)
        {
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase;

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] searchTermBytes = Encoding.UTF8.GetBytes(searchTerm);

            string utf8Input = Encoding.UTF8.GetString(inputBytes);
            string utf8SearchTerm = Encoding.UTF8.GetString(searchTermBytes);

            return utf8Input.IndexOf(utf8SearchTerm, comparison) >= 0;
        }
    }
}
