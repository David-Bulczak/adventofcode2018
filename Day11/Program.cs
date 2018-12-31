using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;

namespace Day11
{
    class Day11Tasks : DayTask<string>
    {
        public int PowerLevel(int x, int y, int serialNumber)
        {
            var rackID = x + 10;
            var powerLevel = rackID * y;
            powerLevel += serialNumber;
            powerLevel *= rackID;
            powerLevel = powerLevel / 100;
            powerLevel = powerLevel % 10;
            powerLevel = powerLevel - 5;
            return powerLevel;
        }
        public override string Part01(string[] input = null)
        {
            var gridSerialNumber = int.Parse(input[0]);
            int gridSizeX = 300;
            int gridSizeY = 300;

            // brute force approach
            int maxGridPower = int.MinValue;
            int maxGridX     = 0;
            int maxGridY     = 0;
            for (int r = 2; r <= gridSizeY - 1; ++r)
            {
                for (int c = 2; c <= gridSizeX - 1; ++c)
                {
                    int tmpPowerLevel = 0;
                    tmpPowerLevel += PowerLevel(c - 1, r - 1, gridSerialNumber);
                    tmpPowerLevel += PowerLevel(c - 0, r - 1, gridSerialNumber);
                    tmpPowerLevel += PowerLevel(c + 1, r - 1, gridSerialNumber);
                    tmpPowerLevel += PowerLevel(c - 1, r - 0, gridSerialNumber);
                    tmpPowerLevel += PowerLevel(c - 0, r - 0, gridSerialNumber);
                    tmpPowerLevel += PowerLevel(c + 1, r - 0, gridSerialNumber);
                    tmpPowerLevel += PowerLevel(c - 1, r + 1, gridSerialNumber);
                    tmpPowerLevel += PowerLevel(c - 0, r + 1, gridSerialNumber);
                    tmpPowerLevel += PowerLevel(c + 1, r + 1, gridSerialNumber);

                    if (tmpPowerLevel > maxGridPower)
                    {
                        maxGridPower = tmpPowerLevel;
                        maxGridX = c - 1;
                        maxGridY = r - 1;
                    }
                    
                }
            }

            return maxGridX + "," + maxGridY;
        }

        public int SumUpGrid(int x, int y, int gridSize, int gridSerialNumber)
        {
            int totalPower = 0;
            for (int r = y; r <= y + gridSize - 1; ++r)
            {
                for (int c = x; c <= x + gridSize - 1; ++c)
                {
                    totalPower += PowerLevel(c, r, gridSerialNumber);
                    //var tmpGridPower = SumUpGrid(c, r, g);
                }
            }
            return totalPower;
        }

        public override string Part02(string[] input = null)
        {
            var gridSerialNumber = int.Parse(input[0]);

            int maxGridPower = int.MinValue;
            int maxGridX     = 0;
            int maxGridY     = 0;
            int maxGridSize  = 0;

            for (int g = 1; g <= 300; ++g)
            {
                for (int r = 1; r <= 300 - g + 1; ++r)
                {
                    for (int c = 1; c <= 300 - g + 1; ++c)
                    {
                        var tmpGridPower = SumUpGrid(c, r, g, gridSerialNumber);
                        if (tmpGridPower > maxGridPower)
                        {
                            maxGridPower = tmpGridPower;
                            maxGridX = c;
                            maxGridY = r;
                            maxGridSize = g;
                        }
                    }
                }
            }
            return maxGridX + "," + maxGridY + "," + maxGridSize;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new Day11Tasks();
            tasks.Exec();
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
