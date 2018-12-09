using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;

namespace Day03
{
    public class Rectangle
    {
        public Rectangle()
        {
            LeftEdge = 0;
            TopEdge = 0;
            Width = 0;
            Height = 0;
        }
        public Rectangle(int RLeftEdge, int RTopEdge, int RWidth, int RHeight)
        {
            LeftEdge = RLeftEdge;
            TopEdge = RTopEdge;
            Width = RWidth;
            Height = RHeight;
        }
        public int LeftEdge { get; set; }
        public int TopEdge { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int RightEdge() { return LeftEdge + Width - 1; }
        public int BottomEdge() { return TopEdge + Height - 1; }

        public bool Covered(int LeftPos, int TopPos)
        {
            var RightEdge = LeftEdge + Width - 1;
            var BottomEdge = TopEdge + Height - 1;

            return (LeftPos >= LeftEdge && LeftPos <= RightEdge && TopPos >= TopEdge && TopPos <= BottomEdge);
        }

        public bool Overlap(Rectangle testRect)
        {
            if (testRect.RightEdge() < LeftEdge || testRect.LeftEdge > RightEdge())
                return false;
            if (testRect.BottomEdge() < TopEdge || testRect.TopEdge > BottomEdge())
                return false;

            return true;
            //var TestRightEdge = 

            //if (!Covered(testRect.LeftEdge, testRect.TopEdge))
            //    return false;
            //else
            //{

            //}
        }
    }

    public class Claim
    {
        public Claim(int cId, Rectangle cRectangle)
        {
            Id = cId;
            Rectangle = cRectangle;
        }
        public int Id { get; set; }
        public Rectangle Rectangle { get; set; }
    }

    public class ClaimParser
    {
        public ClaimParser(string[] input)
        {
            Claims = new List<Claim>();
            originalInput = input;
        }

        public void ExecParsing()
        {
            Claims.Clear();
            foreach (string rawClaim in originalInput)
            {
                Claim tmpClaim = ParseRawClaim(rawClaim);
                Claims.Add(tmpClaim);
            }
        }

        private Claim ParseRawClaim(string rawClaim)
        {
            string tmpClaimString = rawClaim.Replace(" ", String.Empty);
            string[] claimDefStrings = tmpClaimString.Split('@'); // splits id and rectangle part of claim string

            // TODO add string check (length etc.)

            // get id
            string idString = claimDefStrings[0].Replace("#", String.Empty);
            int id = int.Parse(idString);

            // get rectangle defintion
            string[] rectDefStrings = claimDefStrings[1].Split(':');
            string[] rectPosStrings = rectDefStrings[0].Split(',');
            string[] rectSizeStrings = rectDefStrings[1].Split('x');
            Rectangle Rectangle = new Rectangle(int.Parse(rectPosStrings[0]), 
                                                int.Parse(rectPosStrings[1]), 
                                                int.Parse(rectSizeStrings[0]), 
                                                int.Parse(rectSizeStrings[1]));
            return new Claim(id, Rectangle);
        }

        private string[] originalInput;
        public List<Claim> Claims { get; set; }
    }

    public class Day03Tasks : DayTask
    {
        public override void Part01()
        {
            ClaimParser Parser = new ClaimParser(InputForPart01);
            Parser.ExecParsing();
            var Claims = Parser.Claims;

            //// Test data set
            //Claims = new List<Claim>
            //{
            //    new Claim(1, new Rectangle(1, 3, 4, 4)),
            //    new Claim(1, new Rectangle(3, 1, 4, 4)),
            //    new Claim(1, new Rectangle(5, 5, 2, 2))
            //};

            // simple linear approach
            int numOfValidFabricSquaremeters = 0;
            for (int i = 0; i < 1000; ++i)
                for (int j = 0; j < 1000; ++j)
                {
                    bool withinTwoClaims = false;
                    int claimCounter = 0;
                    for (int c = 0; c < Claims.Count && !withinTwoClaims; ++c)
                    {
                        var claim = Claims[c];
                        if (claim.Rectangle.Covered(i, j))
                            ++claimCounter;
                        withinTwoClaims = claimCounter > 1;
                    }
                    if (withinTwoClaims)
                        ++numOfValidFabricSquaremeters;
                }
            System.Console.WriteLine("Farbic m^2 that are covered by two or more claims: " + numOfValidFabricSquaremeters);
        }

        public override void Part02()
        {
            ClaimParser Parser = new ClaimParser(InputForPart02);
            Parser.ExecParsing();
            var Claims = Parser.Claims;

            // Test data set
            //Claims = new List<Claim>
            //{
            //    new Claim(1, new Rectangle(1, 3, 4, 4)),
            //    new Claim(2, new Rectangle(3, 1, 4, 4)),
            //    new Claim(3, new Rectangle(5, 5, 2, 2))
            //};

            bool found = false;
            int id = -1;
            for (int c0 = 0; c0 < Claims.Count && !found; ++c0)
            {
                bool overlapsFound = false;

                var claim0 = Claims[c0];
                for (int c1 = 0; c1 < Claims.Count && !overlapsFound; ++c1)
                {
                    if (c1 != c0)
                    {
                        var claim1 = Claims[c1];
                        overlapsFound = claim0.Rectangle.Overlap(claim1.Rectangle);
                    }

                    //found = claim0.Rectangle.Overlap(claim1.Rectangle);
                }

                if (!overlapsFound)
                {
                    found = true;
                    id = claim0.Id;
                }
            }
            System.Console.WriteLine("Fabric that isn't overlapping has id: " + id);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Day03Tasks dayInst = new Day03Tasks();
            dayInst.Exec(false, true);
            
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
