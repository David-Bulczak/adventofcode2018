﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018Lib
{
    public abstract class DayTask
    {
        public DayTask()
        {
            InputForPart01 = ReadInputLines(GetPart01InputPath());
            inputForPart01Valid = InputForPart01.Length > 0;
            InputForPart02 = ReadInputLines(GetPart02InputPath());
            inputForPart02Valid = InputForPart02.Length > 0;
        }
        
        protected string[] InputForPart01 { get; set; }
        private bool inputForPart01Valid;
        protected string[] InputForPart02 { get; set; }
        private bool inputForPart02Valid;

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
        public abstract void Part01();
        
        /// <summary>
        /// Has to implement day's 2nd part
        /// </summary>
        public abstract void Part02();

        public void Exec(bool runPart01 = true, bool runPart02 = true)
        {
            if (inputForPart01Valid && runPart01)
            {
                System.Console.WriteLine("----------------");
                System.Console.WriteLine("    Part 01");
                System.Console.WriteLine("----------------");
                
                Part01();
            }

            if (inputForPart02Valid && runPart02)
            {
                System.Console.WriteLine("----------------");
                System.Console.WriteLine("    Part 02");
                System.Console.WriteLine("----------------");

                Part02();
            }
        }

    }
}