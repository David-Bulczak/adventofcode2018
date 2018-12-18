using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018Lib
{
    public abstract class DayTask<OutputType> where OutputType : IComparable
    {
        public DayTask()
        {
            InputForPart01 = ReadInputLines(GetPart01InputPath());
            inputForPart01Valid = InputForPart01.Length > 0;
            InputForPart02 = ReadInputLines(GetPart02InputPath());
            inputForPart02Valid = InputForPart02.Length > 0;
            InputForTest01 = ReadInputLines(GetTest01InputPath());
            inputForTest01Valid = InputForTest01.Length > 0;
            InputForTest02 = ReadInputLines(GetTest02InputPath());
            inputForTest02Valid = InputForTest02.Length > 0;

            IsTesting = false;
        }

        protected string[] InputForPart01 { get; set; }
        private bool inputForPart01Valid;
        protected string[] InputForPart02 { get; set; }
        private bool inputForPart02Valid;
        protected string[] InputForTest01 { get; set; }
        private bool inputForTest01Valid;
        protected string[] InputForTest02 { get; set; }
        private bool inputForTest02Valid;

        protected bool IsTesting { get; set; } // aux variable that is true when one of the task functions is running in testmode. can be used for test parameters etc.

        // -----------
        // Day project specific path functions
        // -----------

        private string GetAppPath()
        {
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.AppContext.BaseDirectory, "..//.."));
        }

        private string GetPart01InputPath()
        {
            return System.IO.Path.Combine(GetAppPath(), "input-part-01.txt");
        }

        private string GetPart02InputPath()
        {
            return System.IO.Path.Combine(GetAppPath(), "input-part-02.txt");
        }

        private string GetTest01InputPath()
        {
            return System.IO.Path.Combine(GetAppPath(), "input-test-01.txt");
        }
        private string GetTest02InputPath()
        {
            return System.IO.Path.Combine(GetAppPath(), "input-test-02.txt");
        }
        
        private string[] ReadInputLines(string inputFilePath)
        {
            string[] inputData = Array.Empty<string>();
            if (System.IO.File.Exists(inputFilePath))
                inputData = System.IO.File.ReadAllLines(inputFilePath);
            return inputData;
        }

        /// <summary>
        /// Has to implement day's 1st part
        /// </summary>
        public abstract OutputType Part01(string[] input = null);

        /// <summary>
        /// Has to implement day's 2nd part
        /// </summary>
        public abstract OutputType Part02(string[] input = null);

        private OutputType StringToOutType(string resultString)
        {
            if (typeof(OutputType) == typeof(string))
                return (OutputType)(object)resultString;
            else if (typeof(OutputType) == typeof(int))
                return (OutputType)(object)int.Parse(resultString);
            else if (typeof(OutputType) == typeof(long))
                return (OutputType)(object)long.Parse(resultString);
            else
                return (OutputType)(object)double.Parse(resultString);
        }
        //private int TestResultToOutType(string resultString)
        //{
        //    return int.Parse(resultString);
        //}
        //private string TestResultToOutType(string resultString)
        //{
        //    return resultString;
        //}

        public bool RunTestPart01(string[] input = null)
        {
            IsTesting = true;
            var result = StringToOutType(input[input.Length - 1]);
            Array.Resize(ref input, input.Length - 1);
            var testResult = Part01(input);
            IsTesting = false;
            //return (new Comparer<OutputType>).Compare(result, testResult);
            return result.CompareTo(testResult) == 0;
        }

        public bool RunTestPart02(string[] input = null)
        {
            IsTesting = true;
            var result = StringToOutType(input[input.Length -1]);
            Array.Resize(ref input, input.Length - 1);
            var testResult = Part02(input);
            IsTesting = false;
            return result.CompareTo(testResult) == 0;
        }

        public void Exec(bool runPart01 = true, bool runPart02 = true)
        {
            if (runPart01 && inputForTest01Valid && RunTestPart01(InputForTest01))
            {
                System.Console.WriteLine("Test part 01 passed!");
            }
            else
            {
                System.Console.WriteLine("ERROR: Test part 01");
                return;
            }
            if (runPart01)
            {
                if (inputForPart01Valid)
                {
                    System.Console.WriteLine("----------------");
                    System.Console.WriteLine("    Part 01");
                    System.Console.WriteLine("----------------");

                    var res = Part01(InputForPart01);

                    System.Console.WriteLine("Result: " + res);
                }
                else
                    System.Console.WriteLine("ERROR: No input for part 01!");
            }


            if (runPart02 && inputForTest02Valid && RunTestPart02(InputForTest02))
            {
                System.Console.WriteLine("Test part 02 passed!");
            }
            else
            {
                System.Console.WriteLine("ERROR: Test part 02");
                return;
            }
            if (runPart02)
            {
                if (inputForPart02Valid)
                {
                    System.Console.WriteLine("----------------");
                    System.Console.WriteLine("    Part 02");
                    System.Console.WriteLine("----------------");

                    var res = Part02(InputForPart02);

                    System.Console.WriteLine("Result: " + res);
                }
                else
                    System.Console.WriteLine("ERROR: No input for part 02!");
            } 
        }
    }
}
