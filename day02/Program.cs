using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day02
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

        private static void Part01()
        {
            System.Console.WriteLine("----------------");
            System.Console.WriteLine("Day 02 - Part 01");
            System.Console.WriteLine("----------------");

            string[] boxIds = ReadStringLines(System.IO.Path.Combine(GetAppPath(), "input-part-01.txt"));

            List<string> twoLetterBoxIds = new List<string>();
            List<string> threeLetterBoxIds = new List<string>();
            for (int i = 0; i < boxIds.Length; ++i)
            {
                string boxId = boxIds[i];

                bool twoLetterOccurence = false;
                bool threeLetterOccurence = false;
                for (char l = 'a'; l <= 'z'; ++l)
                {
                    int letterCount = boxId.Count(c => c == l);
                    if (letterCount == 2)
                        twoLetterOccurence = true;
                    if (letterCount == 3)
                        threeLetterOccurence = true;
                }
                if (twoLetterOccurence)
                    twoLetterBoxIds.Add(boxId);
                if (threeLetterOccurence)
                    threeLetterBoxIds.Add(boxId);
            }
            int checksum = twoLetterBoxIds.Count * threeLetterBoxIds.Count;

            System.Console.WriteLine("Checksum is: " + checksum);
        }

        private static void Part02()
        {
            System.Console.WriteLine("----------------");
            System.Console.WriteLine("Day 02 - Part 02");
            System.Console.WriteLine("----------------");

            string[] boxIds = ReadStringLines(System.IO.Path.Combine(GetAppPath(), "input-part-02.txt"));
            bool foundBoxIds = false;
            string equalId = "";
            for (int i = 0; i < boxIds.Length && !foundBoxIds; ++i)
            {
                string boxId0 = boxIds[i];
                for (int j = i; j < boxIds.Length && !foundBoxIds; ++j)
                {
                    string boxId1 = boxIds[j];

                    // compare id positions (TODO: Refactor into function)
                    // Track number of differences in variable and end loop if it is larger than 2 since these ids are not valid
                    int numOfDifferences = 0;
                    int lastDiffId = -1; // if numOfDifferences == 1 this will hold the diff position
                    for (int l = 0; l < boxId0.Length && numOfDifferences <= 1; ++l)
                    {
                        if (boxId0[l] != boxId1[l])
                        {
                            ++numOfDifferences;
                            lastDiffId = l;
                        }
                    }

                    if (numOfDifferences == 1)
                    {
                        foundBoxIds = true;
                        equalId = boxId0.Remove(lastDiffId, 1);
                    }
                }
            }
            System.Console.WriteLine("Equal part of box ids: " + equalId);
        }
        
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            //Console.WriteLine("Hello World!");
            Part01();
            Part02();
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
