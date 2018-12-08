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
        }

        public override void Part02()
        {

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Day03Tasks dayInst = new Day03Tasks();
            dayInst.Exec();
            
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
