using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2018Lib;

namespace Day08
{
    class TreeNode
    {
        public TreeNode()
        {
            Children = new List<TreeNode>();
            MetaData = new List<int>();
        }
        public List<TreeNode> Children;
        public List<int> MetaData;
    }
    class TreeParser : InputParser<TreeNode>
    {
        public TreeNode TreeRoot;
        public TreeParser(string[] input) : base(input)
        {
            TreeRoot = null;
        }

        public TreeNode ParseNode(ref List<int> entries)
        {
            int countOfChildren = entries[0];
            entries.RemoveAt(0);
            int countOfMetaData = entries[0];
            entries.RemoveAt(0);

            var node = new TreeNode();
            for (int c = 0; c < countOfChildren; ++c)
            {
                var child = ParseNode(ref entries);
                node.Children.Add(child);
            }
            for (int m = 0; m < countOfMetaData; ++m)
            {
                var tmpMeta = entries[0];
                entries.RemoveAt(0);
                node.MetaData.Add(tmpMeta);
            }
            return node;
        }

        public override TreeNode ParseInputEntry(string inputEntry)
        {
            var inputString = inputEntry; // Actually in this taskt all the input is in one line.
            var entryStrings = inputString.Split(' ');
            var entries = entryStrings.Select(int.Parse).ToList();
            TreeRoot = ParseNode(ref entries);
            return TreeRoot;
        }
    }

    class Day08Tasks : DayTask<int>
    {
        public int AddMetaDataPart01(TreeNode node)
        {
            int tmpResult = 0;
            foreach (var child in node.Children)
                tmpResult += AddMetaDataPart01(child);
            foreach (var md in node.MetaData)
                tmpResult += md;
            return tmpResult;
        }

        public int NodeValue(TreeNode node)
        {
            int tmpResults = 0;
            if (node.Children.Count > 0)
            {
                foreach (var m in node.MetaData)
                {
                    if (m > 0 && node.Children.Count > m-1) // is the corresponding child index available?
                    {
                        tmpResults += NodeValue(node.Children[m-1]);
                    }
                }
            }
            else
            {
                foreach (var m in node.MetaData)
                    tmpResults += m;
            }
            return tmpResults;
        }

        public override int Part01(string[] input = null)
        {
            var parser = new TreeParser(input);
            parser.ExecParsing();
            var tree = parser.TreeRoot;

            // traverse tree and add up all metadata entries
            int result = AddMetaDataPart01(tree);
            return result;
        }

        public override int Part02(string[] input = null)
        {
            var parser = new TreeParser(input);
            parser.ExecParsing();
            var tree = parser.TreeRoot;

            int result = NodeValue(tree);
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            var tasks = new Day08Tasks();
            tasks.Exec();
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
