using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018Lib
{
    public class DayTask
    {
        // -----------
        // Day project specific path functions
        // -----------

        public string GetAppPath()
        {
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(System.AppContext.BaseDirectory, "..//.."));
        }

        public string GetPart01InputPath()
        {
            return System.IO.Path.Combine(GetAppPath(), "input-part-01.txt");
        }

        public string GetPart02InputPath()
        {
            return System.IO.Path.Combine(GetAppPath(), "input-part-02.txt");
        }

        public string[] ReadInputLines(string inputFilePath)
        {
            string[] tmpFrequencyString = System.IO.File.ReadAllLines(inputFilePath);
            return tmpFrequencyString;
            //return Array.ConvertAll(tmpFrequencyString, int.Parse);
        }
    }
}
