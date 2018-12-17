using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;

namespace Day09
{
    class Player
    {
        public Player()
        {
            Score = 0;
        }

        public int Score { get; set; }
    }

    class MarbleGame
    {
        
        public int AddMarble(int marbleNumber)
        {
            return 0;
        }

        public List<int> Marbles;

        public MarbleGame()
        {
            Marbles = new List<int>();
            Marbles.Add(0);
        }
    }

    class Day09Tasks : DayTask<int>
    {
        public override int Part01(string[] input = null)
        {
            throw new NotImplementedException();
        }

        public override int Part02(string[] input = null)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            var tasks = new Day09Tasks();
            tasks.Exec();
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
