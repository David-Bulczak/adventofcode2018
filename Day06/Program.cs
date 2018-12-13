using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;
using System.Numerics;

namespace Day06
{
    class Helpers
    {
        public static int ManhattanDistance(Vector2 p0, Vector2 p1)
        {
            return (int)(Math.Abs(p0.X - p1.X)) + (int)(Math.Abs(p0.Y - p1.Y));
        }

        public static List<Vector2> SearchMinMax(List<Vector2> points)
        {
            var minCoords = new Vector2(int.MaxValue, int.MaxValue);
            var maxCoords = new Vector2(int.MinValue, int.MinValue);

            foreach (var point in points)
            {
                minCoords.X = Math.Min(minCoords.X, point.X);
                minCoords.Y = Math.Min(minCoords.Y, point.Y);
                maxCoords.X = Math.Max(maxCoords.X, point.X);
                maxCoords.Y = Math.Max(maxCoords.Y, point.Y);
            }

            return new List<Vector2>() { minCoords, maxCoords };
        }
    }

    class Cell // represents areas in task
    {
        public Cell(Vector2 center)
        {
            Center = center;
            Points = new List<Vector2>();
            Points.Add(Center);
        }

        public int Distance(Vector2 point)
        {
            return Helpers.ManhattanDistance(Center, point);
        }

        public int Size()
        {
            return Points.Count;
        }

        //private int ManhattanDistance(Vector2 p0, Vector2 p1)
        //{
        //    return (int)(Math.Abs(p0.X - p1.X)) + (int)(Math.Abs(p0.Y - p1.Y));
        //}

        public void AddPoint(Vector2 point)
        {
            if (!Points.Contains(point))
                Points.Add(point);
        }

        public List<Vector2> MinMax()
        {
            return Helpers.SearchMinMax(Points);
        }

        public Vector2 Center { get; set; }
        private List<Vector2> Points { get; set; }

        /**
         * @brief Generates and returns a list of empty cells based on given centerpoints
         */
        public static List<Cell> GetCellList(List<Vector2> centers)
        {
            var result = new List<Cell>();
            foreach (var center in centers)
            {
                result.Add(new Cell(center));
            }
            return result;
        }
    }

    class CoordinateParser : InputParser<Vector2>
    {
        public CoordinateParser(string[] input) : base(input)
        {
        }

        public override Vector2 ParseInputEntry(string inputEntry)
        {
            var auxString = inputEntry.Replace(" ", ""); // remove blanks
            var coordString = inputEntry.Split(',');
            return new Vector2(int.Parse(coordString[0]), int.Parse(coordString[1]));
        }
    }

    class Day06Tasks : DayTask
    {
        

        public override int Part01(string[] input = null)
        {
            var coordParser = new CoordinateParser(input);
            coordParser.ExecParsing();
            var cellCenters = coordParser.Results;
            var cells = Cell.GetCellList(cellCenters);

            // 1. We have to find min max coordinates for all cell centers.
            // If a cell/area exceeds this than is is infinite in the corresponding "direction".
            var minMaxCoords = Helpers.SearchMinMax(cellCenters);

            // 2. Check area distances within min/max grid.
            for (int row = (int)minMaxCoords[0].Y; row <= (int)minMaxCoords[1].Y; ++row)
            {
                for (int col = (int)minMaxCoords[0].X; col <= (int)minMaxCoords[1].X; ++col)
                {

                    // compare current grid point with cell centers and track all cells with min distance
                    // if point has exactly one such cell it should be added to it otherwise it doesn't belong to any cell.
                    var currentPoint = new Vector2(col, row);
                    int minDistanceToCells = int.MaxValue;
                    var cellsWithMinDistance = new List<Cell>();

                    foreach (var cell in cells)
                    {
                        var tmpDist = Helpers.ManhattanDistance(currentPoint, cell.Center);

                        // new minimum found
                        // => we need to reset tracking of nearest cells
                        if (tmpDist < minDistanceToCells)
                        {
                            cellsWithMinDistance.Clear();
                            minDistanceToCells = tmpDist;
                        }

                        // a new cell with min distance needs to be tracked
                        if (tmpDist == minDistanceToCells)
                        {
                            cellsWithMinDistance.Add(cell);
                        }


                    }
                    
                    // Add point to corresponding cell if it fullfils the requirenments
                    // 1. It has exactly one cell with min distance
                    if (cellsWithMinDistance.Count == 1)
                        cellsWithMinDistance[0].AddPoint(currentPoint);
                }
            }

            // 3. Filter out cells/areas that reach min/max borders.
            // These cells are infinit.
            var validCells = new List<Cell>();
            foreach(var cell in cells)
            {
                var cellMinMax = cell.MinMax();
                bool cellInvalid = (cellMinMax[0].X <= minMaxCoords[0].X) ||
                                   (cellMinMax[0].Y <= minMaxCoords[0].Y) ||
                                   (cellMinMax[1].X >= minMaxCoords[1].X) ||
                                   (cellMinMax[1].Y >= minMaxCoords[1].Y);
                if (!cellInvalid)
                    validCells.Add(cell);
            }

            // 4. Find largest area within the remaining cells.
            Cell resultCell = null;
            int maxArea = int.MinValue;
            foreach (var cell in validCells)
            {
                if (maxArea < cell.Size())
                {
                    maxArea = cell.Size();
                    resultCell = cell;
                }

            }

            return resultCell.Size();
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

            var d06 = new Day06Tasks();
            d06.Exec(true, true);
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
