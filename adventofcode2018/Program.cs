using System;
using System.Collections.Generic;

namespace adventofcode2018
{
    class Day01
    {
        static void Main(string[] args)
        {
            string appPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.AppContext.BaseDirectory, "..//..//.."));
            System.Console.WriteLine(appPath);
            
            // Process command line input
            if (args.Length > 0)
            {
                string inputPart01FilePath = System.IO.Path.Combine(appPath, args[0]);
                System.Console.WriteLine(inputPart01FilePath);
                string[] frequencyStrings = System.IO.File.ReadAllLines(inputPart01FilePath);
                int[] frequencies = Array.ConvertAll(frequencyStrings, int.Parse);
                // sum up frequencies (non LINQ version)
                int result = 0;
                Array.ForEach(frequencies, delegate (int i) { result += i; });
                System.Console.WriteLine("Day 01 - Part 01");
                System.Console.WriteLine("Bzzzt... The final frequency resulting from received input is " + result);

            }
            else
            {
                System.Console.WriteLine("Bzzzzt... ERROR: No input received.");
            }
        }
    }
}
