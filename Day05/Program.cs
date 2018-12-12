using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;

namespace Day05
{
    class Day05Tasks : DayTask
    {
        public string ReactPolymer(string polymer)
        {
            List<char> charList = new List<char>();
            charList.AddRange(polymer);
            for (int c = 0; c < charList.Count - 1;)
            {
                // compare successive elements
                var current = charList[c];
                var next = charList[c + 1];

                bool diffPolarization = false;

                if (65 <= current && current <= 90) // uppercase
                    diffPolarization = diffPolarization || (current == (next - 32));
                else // lowercase
                    diffPolarization = diffPolarization || (current == (next + 32));

                if (diffPolarization)
                {
                    charList.RemoveAt(c);
                    charList.RemoveAt(c);

                    // Since due to removing of chars the two surrounding may have different polarization
                    // we need to "step back"
                    if (c > 0)
                        c--;
                }
                else
                    ++c;
            }
            return string.Join("",charList.ToArray());
        }
        public override int Part01(string[] input = null)
        {
            //var resultString = string.Join(charList.ToString);
            //System.Console.WriteLine("Remining poly: " + charList.ToString());
            return ReactPolymer(input[0]).Length;
        }

        public override int Part02(string[] input = null)
        {
            int minLength = int.MaxValue;
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                string aux = input[0].Replace(c.ToString(), "");
                aux = aux.Replace(((char)(c + 32)).ToString(), "");
                aux = ReactPolymer(aux);

                minLength = Math.Min(minLength, aux.Length);

                //var replString = input.
            }
            return minLength;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Day05Tasks dayInst = new Day05Tasks();
            dayInst.Exec(true, true);
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
