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

        public long Score { get; set; }
    }

    // This is a brute force solution with a lot modulo operations etc.
    // Part 2 taskes quite long due to it's complexity.
    // Idea: Use double linked circular list. This speeds up traversing!!!
    class MarbleGame
    {
        /**
         * @brief Adds marbel longo game and returns polongs
         */
        public long AddMarble(long marbleNumber)
        {
            if ((marbleNumber % 23) == 0)
            {
                long polongs = marbleNumber;
                int removeId = CurrentId - 7;
                if (removeId < 0)
                    removeId = Marbles.Count - 1 - Math.Abs(removeId+1);
                polongs += Marbles[removeId];
                Marbles.RemoveAt(removeId);
                CurrentId = removeId % Marbles.Count;
                return polongs;
            }
            else
            {
                int oneStepId = (CurrentId + 1) % Marbles.Count;
                int twoStepId = (CurrentId + 2) % Marbles.Count;

                Marbles.Insert(oneStepId + 1, marbleNumber);
                CurrentId = oneStepId + 1;
                return 0;
            }
            return 0;
        }

        public List<long> Marbles;
        public int CurrentId;

        public MarbleGame()
        {
            Marbles = new List<long>();
            Marbles.Add(0);
            CurrentId = 0;
        }
    }

    class Day09Tasks : DayTask<long>
    {
        public override long Part01(string[] input = null)
        {
            int numberOfPlayers = 479;
            if (IsTesting)
                numberOfPlayers = 9;
            long numberOfMarbles = 71035 * 100;
            if (IsTesting)
                numberOfMarbles = 25;
            var players = new List<Player>();
            for (long p = 0; p < numberOfPlayers; ++p)
                players.Add(new Player());

            // actual algorithm
            var game = new MarbleGame();
            int currentPlayerId = 0; // actually in task +1
            for (long s = 1; s <= numberOfMarbles; ++s)
            {
                long polongs = game.AddMarble(s);
                players[currentPlayerId].Score += polongs;
                ++currentPlayerId;
                currentPlayerId = currentPlayerId % numberOfPlayers;
            }
            players.Sort((p0, p1) => p0.Score > p1.Score ? -1 : 1);
            return players[0].Score;
        }

        public override long Part02(string[] input = null)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will prlong ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            var tasks = new Day09Tasks();
            tasks.Exec();
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
