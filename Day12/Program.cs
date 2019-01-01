using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;

namespace Day12
{
    //class PotParser : InputParser<>
    //{

    //}

    class Pot : ICloneable
    {
        public Pot(int id, bool plant)
        {
            Id = id;
            Plant = plant;
            //Next = null;
            //Previous = null;
        }

        public bool FulfillsRule(PotRule rule)
        {
            return false;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int Id { get; set; }
        public bool Plant { get; set; }

        //public Pot Next { get; set; }
        //public Pot Previous { get; set; }
    }

    class PotRule
    {
        public PotRule(bool leftTwo, bool leftOne, bool current, bool rightOne, bool rightTwo, bool result)
        {
            LeftTwo = leftTwo;
            LeftOne = leftOne;
            Current = current;
            RightOne = rightOne;
            RightTwo = rightTwo;
            Result = result;
        }

        public bool LeftTwo { get; set; }
        public bool LeftOne { get; set; }
        public bool Current { get; set; }
        public bool RightOne { get; set; }
        public bool RightTwo { get; set; }
        public bool Result { get; set; }
    }

    class PotParser : InputParser<PotRule>
    {
        public PotParser(string[] input) : base(input)
        {
            PotList = new LinkedList<Pot>();
        }

        public Pot RootPot { get; set; }
        public LinkedList<Pot> PotList { get; set; }


        public override PotRule ParseInputEntry(string inputEntry)
        {
            if (inputEntry != null && inputEntry.Length > 0)
            {
                // generate initial pot setting
                if (inputEntry.Contains("initial state"))
                {
                    var initialRowString = inputEntry.Split(' ');
                    var potString = initialRowString[2];

                    if (potString.Length > 0)
                    {
                        //int potCounter = 0;

                        //// generate root/0th pot and initially 3 empty pots left of it (for rule testing)
                        //RootPot = new Pot(0, potString[0] == '#');
                        //RootPot.Previous = new Pot(-1, false);
                        //RootPot.Previous.Previous = new Pot(-1, false);
                        //RootPot.Previous.Previous.Previous = new Pot(-1, false);

                        //var CurrentPot = RootPot;
                        //for (int s = 1; s < potString.Length; ++s)
                        //{
                        //    ++potCounter;
                        //    CurrentPot.Next = new Pot(potCounter, potString[s] == '#');
                        //    CurrentPot.Next.Previous = CurrentPot;
                        //    CurrentPot = CurrentPot.Next;
                        //}

                        var IdCounter = 0;
                        for (int s = 0; s < potString.Length; ++s)
                        {
                            PotList.AddLast(new Pot(IdCounter, potString[s] == '#'));
                            ++IdCounter;
                        }
                    }
                }
                else // parse an actual rule
                {
                    var ruleString = inputEntry.Replace(" ", "");
                    var rulePartString = ruleString.Split(new string[] { "=>" }, StringSplitOptions.None);
                    var rule = new PotRule(rulePartString[0][0] == '#',
                                           rulePartString[0][1] == '#',
                                           rulePartString[0][2] == '#',
                                           rulePartString[0][3] == '#',
                                           rulePartString[0][4] == '#',
                                           rulePartString[1][0] == '#');
                    Results.Add(rule);
                }
            }

            return null;
        }
    }

    class Day12Tasks : DayTask<string>
    {
        public bool RuleIsFulfilled(LinkedListNode<Pot> potNode, PotRule rule)
        {
            var currentPlantState   = potNode.Value.Plant;
            var left1PlantState     = false;
            var left2PlantState     = false;
            if (potNode.Previous != null)
            {
                left1PlantState = potNode.Previous.Value.Plant;
                if (potNode.Previous.Previous != null)
                {
                    left2PlantState = potNode.Previous.Previous.Value.Plant;
                }
            }
            var right1PlantState = false;
            var right2PlantState = false;
            if (potNode.Next != null)
            {
                right1PlantState = potNode.Next.Value.Plant;
                if (potNode.Next.Next != null)
                {
                    right2PlantState = potNode.Next.Next.Value.Plant;
                }
            }

            return (left2PlantState == rule.LeftTwo &&
                    left1PlantState == rule.LeftOne &&
                    currentPlantState == rule.Current &&
                    right1PlantState == rule.RightOne &&
                    right2PlantState == rule.RightTwo);
        }

        public override string Part01(string[] input = null)
        {
            var parser = new PotParser(input);
            parser.ExecParsing();

            var rules = parser.Results;
            var rootPot = parser.RootPot;
            var potList = parser.PotList;

            //var currentPot
            const int generations = 20;
            var currentPotList = potList;
            for (int g = 0; g < generations; ++g)
            {
                // attach helpers to the right and left (unneccessary memory consumption, should be done cleaner)
                int leftmostId = currentPotList.First.Value.Id;
                int rightmostId = currentPotList.Last.Value.Id;
                currentPotList.AddFirst(new Pot(leftmostId - 1, false));
                currentPotList.AddFirst(new Pot(leftmostId - 2, false));
                currentPotList.AddLast(new Pot(rightmostId + 1, false));
                currentPotList.AddLast(new Pot(rightmostId + 2, false));
                
                var nextPotList = new LinkedList<Pot>();

                // iterate through elements, clone for new list and apply rules
                var potNodeIterator = currentPotList.First;
                while (potNodeIterator != null)
                {
                    var pot = (Pot)potNodeIterator.Value.Clone();
                    //pot.Id = potNodeIterator.Value.Id;

                    var ruleFulfilled = false;
                    for (int r = 0; r < rules.Count && !ruleFulfilled; ++r)
                    {
                        //var ruleResult = CheckRule(potNodeIterator, rule);
                        if (RuleIsFulfilled(potNodeIterator, rules[r]))
                        {
                            ruleFulfilled = true;
                            pot.Plant = rules[r].Result;
                        }
                    }
                    if (!ruleFulfilled)
                    {
                        pot.Plant = false;
                    }
                    nextPotList.AddLast(pot);
                    potNodeIterator = potNodeIterator.Next;
                }

                //// we need to check if some new pots have to be added left from current list due to rules.
                ////var leftId = currentPotList.First().Id;
                //var leftmostId = nextPotList.First.Value.Id;
                //var l1Node = new LinkedListNode<Pot>(new Pot(leftmostId - 1, false));
                //nextPotList.AddFirst(l1Node);
                //var aRuleFulfilled = false;
                //for (int r = 0; r < rules.Count && !aRuleFulfilled; ++r)
                //{
                //    //var ruleResult = CheckRule(potNodeIterator, rule);
                //    if (RuleIsFulfilled(l1Node, rules[r]))
                //    {
                //        aRuleFulfilled = true;
                //        l1Node.Value.Plant = rules[r].Result;
                //    }
                //}
                //if (!aRuleFulfilled)
                //    l1Node.Value.Plant = false;

                //var l2Node = new LinkedListNode<Pot>(new Pot(leftmostId - 2, false));
                //nextPotList.AddFirst(l2Node);
                //aRuleFulfilled = false;
                //for (int r = 0; r < rules.Count && !aRuleFulfilled; ++r)
                //{
                //    //var ruleResult = CheckRule(potNodeIterator, rule);
                //    if (RuleIsFulfilled(l2Node, rules[r]))
                //    {
                //        aRuleFulfilled = true;
                //        l2Node.Value.Plant = rules[r].Result;
                //    }
                //}
                //if (!aRuleFulfilled)
                //    l2Node.Value.Plant = false;

                //// same for right end
                //int rightmostId = nextPotList.Last.Value.Id;
                //var r1Node = new LinkedListNode<Pot>(new Pot(rightmostId + 1, false));
                //nextPotList.AddLast(r1Node);
                //aRuleFulfilled = false;
                //for (int r = 0; r < rules.Count && !aRuleFulfilled; ++r)
                //{
                //    //var ruleResult = CheckRule(potNodeIterator, rule);
                //    if (RuleIsFulfilled(r1Node, rules[r]))
                //    {
                //        aRuleFulfilled = true;
                //        r1Node.Value.Plant = rules[r].Result;
                //    }
                //}
                //if (!aRuleFulfilled)
                //    r1Node.Value.Plant = false;

                //var r2Node = new LinkedListNode<Pot>(new Pot(rightmostId + 2, false));
                //nextPotList.AddLast(r2Node);
                //aRuleFulfilled = false;
                //for (int r = 0; r < rules.Count && !aRuleFulfilled; ++r)
                //{
                //    //var ruleResult = CheckRule(potNodeIterator, rule);
                //    if (RuleIsFulfilled(r2Node, rules[r]))
                //    {
                //        aRuleFulfilled = true;
                //        r2Node.Value.Plant = rules[r].Result;
                //    }
                //}
                //if (!aRuleFulfilled)
                //    r2Node.Value.Plant = false;

                currentPotList = nextPotList;

                //var auxNode1 = new LinkedListNode<Pot>(new Pot(leftId - 1, false));
                //var clonedList = potList.Select(x => (Pot)x.Clone()).ToList();
            //var clonedList = LightPoints.Select(x => (LightPoint)x.Clone()).ToList();

            }

            // sum up plant containing pots
            int count = 0;
            foreach (var pot in currentPotList)
            {
                if (pot.Plant)
                    count += pot.Id;
            }

            return count.ToString();
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
            var tasks = new Day12Tasks();
            tasks.Exec();
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
