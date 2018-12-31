using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;
using System.Numerics;
using System.Text.RegularExpressions;
//using System.Windows;

namespace Day10
{
    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }

    class Rect
    {
        public Rect(float top, float left, float bottom, float right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public float Area()
        {
            return Math.Abs(Top - Bottom) * Math.Abs(Right - Left);
        }

        public float Top { get; set; }
        public float Left { get; set; }
        public float Bottom { get; set; }
        public float Right { get; set; }
    }

    class LightPoint : ICloneable
    {
        public LightPoint(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
            //return new LightPoint(Position, Velocity);
        }
    }

    class LightPointSimulator
    {
        public LightPointSimulator(List<LightPoint> lightPoints)
        {
            LightPoints = lightPoints;
            BoundingBoxMax = new Vector2(int.MinValue, int.MinValue);
            BoundingBoxMin = new Vector2(int.MaxValue, int.MaxValue);

            CurrentBoundingBoxArea = int.MaxValue;
            BoundingBox = new Rect(int.MaxValue, int.MaxValue, int.MinValue, int.MinValue);
            Step = 0;
        }
        
        public List<LightPoint> MoveLightPoints(List<LightPoint> lightPoints)
        {
            foreach (var lightPoint in lightPoints)
                lightPoint.Position += lightPoint.Velocity;
            return lightPoints;
        }

        public void DrawRow(List<LightPoint> rowLights)
        {
            int numberOfCols = (int)(BoundingBox.Right - BoundingBox.Left);
            for (int c = 0; c <= numberOfCols; ++c)
            {
                if (rowLights.Where(p => (c + BoundingBox.Left) == p.Position.X).Any())
                    System.Console.Write('#');
                else
                    System.Console.Write('.');
            }
            System.Console.Write('\n');
        }
        public void DrawLights(List<LightPoint> lightPoints)
        {
            System.Console.Clear();
            System.Console.WriteLine("---- Step " + Step + " ----");

            int numberOfRows = (int)(BoundingBox.Bottom - BoundingBox.Top);
            for (int r = 0; r <= numberOfRows; ++r)
            {
                var rowPoints = lightPoints.Where(p => (p.Position.Y - BoundingBox.Top) == r).ToList();
                DrawRow(rowPoints);
            }

            System.Console.WriteLine("--------------");
        }

        private Rect DetermineBoundingBox(List<LightPoint> lightPoints)
        {
            var tmpMinX = lightPoints.Min(p => p.Position.X);
            var tmpMinY = lightPoints.Min(p => p.Position.Y);
            var tmpMaxX = lightPoints.Max(p => p.Position.X);
            var tmpMaxY = lightPoints.Max(p => p.Position.Y);

            return new Rect(tmpMinY, tmpMinX, tmpMaxY, tmpMaxX);
        }

        public bool SimulateStep()
        {
            // 1. simulate step
            //var pointCopy = LightPoints;
            //var clonedList = new List<LightPoint>(LightPoints);
            var clonedList = LightPoints.Select(x => (LightPoint)x.Clone()).ToList();
            clonedList = MoveLightPoints(clonedList);
            var newBoundingBox = DetermineBoundingBox(clonedList);
            if (newBoundingBox.Area() > BoundingBox.Area()) // previous step has potential solution
            {
                DrawLights(LightPoints);

                var key = Console.ReadKey();
                if (key.KeyChar.Equals('y'))
                    return true;

                LightPoints = MoveLightPoints(LightPoints);
                BoundingBox = DetermineBoundingBox(LightPoints);
                ++Step;

                return false;
            }
            else // update
            {
                LightPoints = MoveLightPoints(LightPoints);
                BoundingBox = DetermineBoundingBox(LightPoints);
                ++Step;

                return false;
            }

            // 2. if bounding box larger than previous than previous may be a solution due to local are minimum

        }

        public List<LightPoint> LightPoints { get; set; }
        private Rect BoundingBox { get; set; }
        private int Step { get; set; }

        private Vector2 BoundingBoxMax { get; set; }
        private Vector2 BoundingBoxMin { get; set; }
        private int CurrentBoundingBoxArea { get; set; }
    }

    class LightPointParser : InputParser<LightPoint>
    {
        public LightPointParser(string[] input) : base(input)
        {
            //var 
        }

        public override LightPoint ParseInputEntry(string inputEntry)
        {
            //Regex pattern = new Regex(@"(?<position>position=<\-*\d{5}, {1,2}\-*\d{5}>)" +
            //                          @"(?<velocity>velocity=<\-*\d{1}, {1,2}\-*\d{1}>)");
            Regex pattern = new Regex(@"(?<position>position=< *\-*\d+, *\-*\d+>)" + @"(?<velocity> velocity=< *\-*\d+, *\-*\d+>)");
            Match matchEntries = pattern.Match(inputEntry);
            if (matchEntries.Success)
            {
                var positionString = matchEntries.Groups["position"].Value;
                var positionMatches = Regex.Match(positionString, @"(?<x> *\-*\d+)" + @"(?<excl>, )" + @"(?<y> *\-*\d+)");
                if (!positionMatches.Success)
                    throw new SystemException("Invalid position values");
                var xCompPos = int.Parse(positionMatches.Groups["x"].Value);
                var yCompPos = int.Parse(positionMatches.Groups["y"].Value);

                var velocityString = matchEntries.Groups["velocity"].Value;
                var velocityMatches = Regex.Match(velocityString, @"(?<x> *\-*\d+)" + @"(?<excl>, )" + @"(?<y> *\-*\d+)");
                if (!velocityMatches.Success)
                    throw new SystemException("Invalid velocity values");
                var xCompVel = int.Parse(velocityMatches.Groups["x"].Value);
                var yCompVel = int.Parse(velocityMatches.Groups["y"].Value);

                return new LightPoint(new Vector2(xCompPos, yCompPos), new Vector2(xCompVel, yCompVel));
            }
            return null;
        }
    }

    class Day10Tasks : DayTask<string>
    {
        public override string Part01(string[] input = null)
        {
            var parser = new LightPointParser(input);
            parser.ExecParsing();
            var lightPoints = parser.Results;

            var lightSim = new LightPointSimulator(lightPoints);
            bool done = false;
            while (!done)
            {
                done = lightSim.SimulateStep();
                //var key = Console.ReadKey();
                //if (key.Equals("y"))
                //    done = true;
            }
            // TODO: find smalles bounding box

            return "HI";
        }

        public override string Part02(string[] input = null)
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new Day10Tasks();
            tasks.Exec(true, false);
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
