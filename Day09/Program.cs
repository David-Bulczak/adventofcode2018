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
        /**
         * @brief Adds marbel into game and returns points
         */
        public int AddMarble(int marbleNumber)
        {
            if ((marbleNumber % 23) == 0)
            {
                int points = marbleNumber;
                int removeId = CurrentId - 7;
                if (removeId < 0)
                    removeId = Marbles.Count - 1 - Math.Abs(removeId+1);
                points += Marbles[removeId];
                Marbles.RemoveAt(removeId);
                CurrentId = removeId % Marbles.Count;
                return points;
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

        public List<int> Marbles;
        public int CurrentId;

        public MarbleGame()
        {
            Marbles = new List<int>();
            Marbles.Add(0);
            CurrentId = 0;
        }
    }

    class Day09Tasks : DayTask<int>
    {
        public override int Part01(string[] input = null)
        {
            int numberOfPlayers = 479;
            if (IsTesting)
                numberOfPlayers = 9;
            int numberOfMarbles = 71035 * 100;
            if (IsTesting)
                numberOfMarbles = 25;
            var players = new List<Player>();
            for (int p = 0; p < numberOfPlayers; ++p)
                players.Add(new Player());

            // actual algorithm
            var game = new MarbleGame();
            int currentPlayerId = 0; // actually in task +1
            for (int s = 1; s <= numberOfMarbles; ++s)
            {
                int points = game.AddMarble(s);
                players[currentPlayerId].Score += points;
                ++currentPlayerId;
                currentPlayerId = currentPlayerId % numberOfPlayers;
            }
            players.Sort((p0, p1) => p0.Score > p1.Score ? -1 : 1);
            return players[0].Score;
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
