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

        static void Main(string[] args)
        {
            string appPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.AppContext.BaseDirectory, "..//..//.."));
            System.Console.WriteLine(appPath);
            
            // Process command line input
            if (args.Length > 0)
            {
                string inputPart01FilePath = System.IO.Path.Combine(appPath, args[0]);
                int[] frequenciesPart01 = ReadFrequencies(inputPart01FilePath);
                
                // sum up frequencies (non LINQ version)
                int result = 0;
                Array.ForEach(frequenciesPart01, delegate (int i) { result += i; });
                System.Console.WriteLine("Day 01 - Part 01");
                System.Console.WriteLine("Bzzzt... The final frequency resulting from received input is " + result);

                int[] frequenciesPart02 = ReadFrequencies(System.IO.Path.Combine(appPath, "input-part-02.txt"));
                List<int> usedFrequencies = new List<int>();
                int sumPart02 = 0;
                bool done = false;
                int firstDuplicate = 0;
                usedFrequencies.Add(0); // initial frequency has to be added since it also can be visited multiple times
                //foreach (int freq in frequenciesPart02)
                while (!done)
                {
                    for (int i = 0; i < frequenciesPart02.Length && !done; ++i)
                    {
                        sumPart02 += frequenciesPart02[i];
                        if (!usedFrequencies.Contains(sumPart02))
                            usedFrequencies.Add(sumPart02);
                        else
                        {
                            done = true;
                            firstDuplicate = sumPart02;
                        }
                    }
                }
                System.Console.WriteLine("Day 01 - Part 02");
                System.Console.WriteLine("Bzzzzt... frequency " + firstDuplicate + " has been reached twice.");
            }
            else
            {
                System.Console.WriteLine("Bzzzzt... ERROR: No input received.");
            }
        }
    }
}
