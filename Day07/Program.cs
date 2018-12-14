using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;

namespace Day07
{
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
            IsFinished = false;
            Id = newId;
            NextNodes = new List<GraphNode>();
            PrevNodes = new List<GraphNode>();
        }
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
            throw new NotImplementedException();
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
