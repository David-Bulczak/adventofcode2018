using System;
using System.Collections.Generic;

namespace adventofcode2018
{
    class Day01
    {
        private static int[] ReadFrequencies(string inputFilePath)
        {
            string[] tmpFrequencyString = System.IO.File.ReadAllLines(inputFilePath);
            return Array.ConvertAll(tmpFrequencyString, int.Parse);
        }

        private static string GetAppPath()
        {
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.AppContext.BaseDirectory, "..//..//.."));
        }

        private static void Part01()
        {
            System.Console.WriteLine("----------------");
            System.Console.WriteLine("Day 01 - Part 01");
            System.Console.WriteLine("----------------");

            int[] frequencies       = ReadFrequencies(System.IO.Path.Combine(GetAppPath(), "input-part-02.txt")); 
            
            // sum up frequencies (non LINQ version)
            int sum = 0;
            Array.ForEach(frequencies, delegate (int i) { sum += i; });
            System.Console.WriteLine("Bzzzt... The final frequency resulting from received input is " + sum);
        }

        private static void Part02()
        {
            System.Console.WriteLine("----------------");
            System.Console.WriteLine("Day 01 - Part 02");
            System.Console.WriteLine("----------------");

            int[] frequencies = ReadFrequencies(System.IO.Path.Combine(GetAppPath(), "input-part-02.txt"));
            
            List<int> usedFrequencies = new List<int>() { 0 }; /// tracks used frequencies
            int sum = 0;
            bool done = false;
            int firstDuplicate = 0;
            while (!done)
            {
                for (int i = 0; i < frequencies.Length && !done; ++i)
                {
                    sum += frequencies[i];
                    if (!usedFrequencies.Contains(sum))
                        usedFrequencies.Add(sum);
                    else
                    {
                        done = true;
                        firstDuplicate = sum;
                    }
                }
            }
            System.Console.WriteLine("Bzzzzt... frequency " + firstDuplicate + " has been reached twice.");
        }

        static void Main(string[] args)
        {
            string appPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.AppContext.BaseDirectory, "..//..//.."));

            Part01();
            Part02();
        }
    }
}
