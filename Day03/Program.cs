using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day03
{
    class Program
    {
        private static string[] ReadStringLines(string inputFilePath)
        {
            string[] tmpFrequencyString = System.IO.File.ReadAllLines(inputFilePath);
            return tmpFrequencyString;
            //return Array.ConvertAll(tmpFrequencyString, int.Parse);
        }

        private static string GetAppPath()
        {
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.AppContext.BaseDirectory, "..//.."));
        }

        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
