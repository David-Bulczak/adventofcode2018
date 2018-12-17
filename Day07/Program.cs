using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;

namespace Day07
{
    class Worker
    {
        public Worker()
        {
            Step = null;
        }
        public GraphNode Step { get; set; }

        public bool IsAvailable()
        {
            return (Step == null || Step.SecondsPassed >= Step.SecondsNeeded);
        }
    }

    class GraphDependency
    {
        public GraphDependency(char node, char dependsOnNode)
        {
            Node = node;
            DependsOnNode = dependsOnNode;
        }

        public char Node { get; set; }
        public char DependsOnNode { get; set; }
    }

    class GraphNode
    {
        
        public bool IsFinished { get; set; }

        public char Id;

        public List<GraphNode> NextNodes { get; set; }
        public List<GraphNode> PrevNodes { get; set; }
        public Worker Worker;

        public bool IsAvailable()
        {
            if (PrevNodes.Count == 0)
                return true;
            bool result = true;
            foreach (var prev in PrevNodes)
            {
                result = result && prev.IsFinished;
            }
            return result;
        }

        public GraphNode(char newId)
        {
            SecondsNeeded = 60 + newId - 64;
            SecondsPassed = 0;
            Worker = null;
            IsFinished = false;
            Id = newId;
            NextNodes = new List<GraphNode>();
            PrevNodes = new List<GraphNode>();
        }

        public void AddSeconsPassed(int sec = 1)
        {
            SecondsPassed += sec;
        }
        public bool StepDone() { return SecondsPassed >= SecondsNeeded;  } // TODO this funcionality should be moved to IsFinished. Right now it is in it's own function because otherwise it would break logic of part 01.
        public bool StepAvailable()
        {
            if(PrevNodes.Count == 0)
                return true;
            bool result = true;
            foreach (var prev in PrevNodes)
            {
                result = result && prev.StepDone();
            }
            return result;
        }
        public int SecondsNeeded { get; set; }
        public int SecondsPassed { get; set; }
    }
    /**
     * Very simple and basic directed acyclic graph.
     */
    class SimpleDAG
    {
        public SimpleDAG()
        {
            Nodes = new List<GraphNode>();
        }

        public List<GraphNode> Nodes { get; set; }

        public GraphNode GetNode(char Id)
        {
            var index = Nodes.FindIndex(x => x.Id == Id);
            if (index >= 0)
            {
                return Nodes[index];
            }
            return null;
        }

        public void AddNode(char Id)
        {
            Nodes.Add(new GraphNode(Id));
        }

        public void AddDependency(GraphDependency dep)
        {
            var node = GetNode(dep.Node);
            if (node == null)
            {
                AddNode(dep.Node);
                node = GetNode(dep.Node);
            }
            var depOnNode = GetNode(dep.DependsOnNode);
            if (depOnNode == null)
            {
                AddNode(dep.DependsOnNode);
                depOnNode = GetNode(dep.DependsOnNode);
            }

            node.PrevNodes.Add(depOnNode);
            depOnNode.NextNodes.Add(node);
        }
    }

    class GraphParser : InputParser<GraphDependency>
    {
        public GraphParser(string[] input) : base(input)
        {
        }

        /**
         * @brief Very basic parsing. This time no fancy RegEx stuff.
         */
        public override GraphDependency ParseInputEntry(string inputEntry)
        {
            return new GraphDependency(inputEntry[36], inputEntry[5]);
        }
    }

    class Day07Tasks : DayTask<string>
    {
        public override string Part01(string[] input = null)
        {
            var parser = new GraphParser(input);
            parser.ExecParsing();
            var dependencies = parser.Results;

            var graph = new SimpleDAG();
            foreach (var dep in dependencies)
                graph.AddDependency(dep);

            // Init
            var availableNodes = graph.Nodes.FindAll(x => x.PrevNodes.Count == 0); // possibily available

            // Actual algorithm
            string result = "";
            while(availableNodes.Count > 0)
            {
                availableNodes.Sort((a, b) => a.Id > b.Id ? 1 : -1);

                // Get first available node
                GraphNode node = null;
                for (int n = 0; n < availableNodes.Count && node == null; ++n)
                {
                    if (availableNodes[n].IsAvailable())
                        node = availableNodes[n];
                }
                result = result + node.Id;
                foreach (var next in node.NextNodes)
                {
                    if (!availableNodes.Contains(next))
                    availableNodes.Add(next);
                }
                availableNodes.Remove(node);
                node.IsFinished = true;
                //var firstAvailable = availableNodes[0];
            }

            return result;
        }

        public override string Part02(string[] input = null)
        {
            var DebugTable = new List<string>();

            var parser = new GraphParser(input);
            parser.ExecParsing();
            var dependencies = parser.Results;

            var graph = new SimpleDAG();
            foreach (var dep in dependencies)
                graph.AddDependency(dep);

            if (IsTesting)
                foreach (var node in graph.Nodes)
                    node.SecondsNeeded -= 60;

            // Init
            var availableNodes = graph.Nodes.FindAll(x => x.PrevNodes.Count == 0); // possibily available
            availableNodes.Sort((a, b) => a.Id > b.Id ? 1 : -1);

            // Actual algorithm
            string result = "";
            var workers = new List<Worker>() { new Worker(), new Worker(), new Worker(), new Worker(), new Worker() };
            if (IsTesting)
                workers = new List<Worker>() { new Worker(), new Worker() };
            int timeCounter = 0;
            while (availableNodes.Count > 0) // each iteration represents a time step
            {
                // try to assign undone tasks to workers
                var tmpSteps = availableNodes.FindAll(s => s.Worker == null && !s.StepDone());
                var tmpWorkers = workers.FindAll(w => w.IsAvailable() == true);
                bool workersAvailable = tmpWorkers.Count > 0;
                for (int s = 0; s < tmpSteps.Count && workersAvailable; ++s)
                {
                    if (tmpSteps[s].Worker == null && !tmpSteps[s].StepDone() && tmpSteps[s].StepAvailable())
                    {
                        var worker = tmpWorkers[0];
                        tmpSteps[s].Worker = worker;
                        worker.Step = tmpSteps[s];
                        tmpWorkers = workers.FindAll(w => w.IsAvailable() == true);
                        workersAvailable = tmpWorkers.Count > 0;
                    }
                    
                }
                string debugRow = "";
                debugRow = debugRow + timeCounter;
                debugRow = debugRow + "\t";
                for (int w = 0; w < workers.Count; ++w)
                {
                    if (workers[w].Step != null)
                        debugRow = debugRow + workers[w].Step.Id + "\t";
                    else
                        debugRow = debugRow + ".\t";
                }
                debugRow = debugRow + result;
                Console.WriteLine(debugRow);

                // Update time worked on steps
                foreach (var w in workers)
                {
                    if (w.Step != null)
                    {
                        w.Step.AddSeconsPassed();

                        // check if step is ready. if so than add potentially new steps.
                        if (w.Step.StepDone())
                        {
                            result = result + w.Step.Id;
                            foreach (var next in w.Step.NextNodes)
                            {
                                if (!availableNodes.Contains(next) && !next.StepDone())
                                    availableNodes.Add(next);
                            }
                            availableNodes.Remove(w.Step);
                            w.Step = null;
                        }
                    }
                }
                ++timeCounter;

                

            }
            //++timeCounter;
            return "" + timeCounter;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            var tasks = new Day07Tasks();
            tasks.Exec();
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
