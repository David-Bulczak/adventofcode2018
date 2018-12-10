using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AoC2018Lib;

namespace Day04
{
    public class GuardRecord
    {

        public enum Observation
        {
            Unknown,
            BeginsShift,
            FallsAsleep,
            WakesUp
        }
        public DateTime Timestamp { get; set; }
        public int Id { get; set; }
        public Observation ObservedAction { get; set; }

        public GuardRecord(DateTime timestamp, int id, Observation observedAction)
        {
            Timestamp = timestamp;
            Id = id;
            ObservedAction = observedAction;
        }

        public GuardRecord()
        {
            Timestamp = new DateTime(1979, 07, 28, 22, 35, 5);
            Id = -1;
            ObservedAction = Observation.Unknown;
        }
    }

    public class GuardRecordParser : InputParser<GuardRecord>
    {
        public GuardRecordParser(string[] input) : base(input)
        {
        }

        public override GuardRecord ParseInputEntry(string inputEntry)
        {
            // split message into timestamp and message part
            Regex pattern = new Regex(@"(?<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2})" +
                                      @"(?<exclude>] )" +
                                      @"(?<message>.*)");
            Match matchEntry = pattern.Match(inputEntry);
            if (matchEntry.Success)
            {
                //Console.WriteLine(matchEntry.Groups["timestamp"]);
                //Console.WriteLine(matchEntry.Groups["message"]);

                DateTime entryTimestamp                  = DateTime.Parse(matchEntry.Groups["timestamp"].Value);
                int entryId                              = -1;
                GuardRecord.Observation entryObservation = GuardRecord.Observation.Unknown;

                // look for guard id in message
                Regex guardIdPattern = new Regex(@"(?<id>\d{1,4})");
                string message       = matchEntry.Groups["message"].Value;
                Match matchIdEntry   = guardIdPattern.Match(message);
                if (matchIdEntry.Success) // Id found
                {
                    //Console.WriteLine(matchIdEntry.Groups["id"]);
                    entryId = int.Parse(matchIdEntry.Groups["id"].Value);
                    entryObservation = GuardRecord.Observation.BeginsShift;
                }
                else if (message == "wakes up")
                {
                    entryObservation = GuardRecord.Observation.WakesUp;
                }
                else
                {
                    entryObservation = GuardRecord.Observation.FallsAsleep;
                }

                if (entryObservation == GuardRecord.Observation.Unknown)
                    throw new System.ArgumentException("Unkown observation must not occure!");

                return new GuardRecord(entryTimestamp, entryId, entryObservation);
            }
            else
            {
                throw new System.InvalidOperationException("Input string corrupted or wrong parsing!");
            }

            return new GuardRecord(new DateTime(0000), -1, GuardRecord.Observation.Unknown);
        }
    }

    public class Day04Tasks : DayTask
    {
        public override void Part01()
        {
            // Parse input and sort by timestamp to get valid list
            GuardRecordParser parser = new GuardRecordParser(InputForPart01);
            parser.ExecParsing();
            var guardRecordEntries = parser.Results;
            parser.Results.Sort((GuardRecord g0, GuardRecord g1) =>
            {
                if (g0.Timestamp < g1.Timestamp)
                    return -1;
                else if (g0.Timestamp == g1.Timestamp)
                    return 0;
                else
                    return 1;
            }
            );

            // find guard that has most minutes asleep
            var idToTimeMap = new Dictionary<int, int>(); // <id, minutesSpendAsleep>
            int currentId = -1;
            for (int i = 0; i < parser.Results.Count;)
            {
                var entry = parser.Results[i];
                if (entry.Id != -1 && entry.ObservedAction == GuardRecord.Observation.BeginsShift) // new guard begins shift
                {
                    currentId = entry.Id;
                    if (!idToTimeMap.ContainsKey(currentId))
                        idToTimeMap.Add(currentId, 0);
                    ++i;
                }
                else // this branch handles "falls asleep" observations and accumulates minutes to the corresponding guard 
                {
                    // get minutes spend asleep
                    TimeSpan timeAsleep = parser.Results[i + 1].Timestamp - entry.Timestamp;
                    int minutesAsleep = Convert.ToInt32(timeAsleep.TotalMinutes);

                    idToTimeMap[currentId] += minutesAsleep;

                    // HACKY: A little hack that updates entry ids to allow filtering in final step
                    parser.Results[i].Id     = currentId;
                    parser.Results[i + 1].Id = currentId;
                    i += 2;
                }
            }

            // get id of guard that slept most of the time
            var guardId = idToTimeMap.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            // find minute where guard slept most of the time
            List<GuardRecord> guardEntries = parser.Results.Where(r => r.Id == guardId).ToList(); // filtering for entries that belong to guard
            var sleepPerMinuteCounter = new Dictionary<int, int>(); // <minute, sleepCounter>
            for (int i = 0; i < guardEntries.Count;)
            {
                var entry = guardEntries[i];
                if (!(entry.ObservedAction == GuardRecord.Observation.BeginsShift))
                {
                    TimeSpan timeAsleep = guardEntries[i + 1].Timestamp - guardEntries[i].Timestamp;
                    int minutesAsleep = Convert.ToInt32(timeAsleep.TotalMinutes);
                    int fallAsleepMinute = guardEntries[i].Timestamp.Minute;

                    for (int m = fallAsleepMinute; m < fallAsleepMinute + minutesAsleep; ++m)
                    {
                        if (!sleepPerMinuteCounter.ContainsKey(m))
                            sleepPerMinuteCounter.Add(m, 1);
                        else
                            ++sleepPerMinuteCounter[m];
                    }
                    i += 2;
                }
                else
                    ++i;
                   
            }

            // get key where guard slept most often
            var minute = sleepPerMinuteCounter.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            var result = guardId * minute;

            System.Console.WriteLine("Guard " + guardId + " is mostly asleep in minunte " + minute + " thus the result is " + result);
        }

        public override void Part02()
        {
            // Parse input and sort by timestamp to get valid list
            GuardRecordParser parser = new GuardRecordParser(InputForPart02);
            parser.ExecParsing();
            var guardRecordEntries = parser.Results;
            parser.Results.Sort((GuardRecord g0, GuardRecord g1) =>
            {
                if (g0.Timestamp < g1.Timestamp)
                    return -1;
                else if (g0.Timestamp == g1.Timestamp)
                    return 0;
                else
                    return 1;
            }
            );

            // minute per guard counter TODO: Refactor into own class e.g. RecordProcessor?
            var guardMinuteCounter = new Dictionary<int, Dictionary<int, int>>();
            int currentGuardId = -1;
            for (int i = 0; i < parser.Results.Count;)
            {
                var entry = parser.Results[i];
                if (entry.Id != -1 && entry.ObservedAction == GuardRecord.Observation.BeginsShift) // new guard begins shift
                {
                    // if guard hasn't been "visited" yet, add empty minute counter
                    if (!guardMinuteCounter.ContainsKey(entry.Id))
                        guardMinuteCounter.Add(entry.Id, new Dictionary<int, int>());
                    currentGuardId = entry.Id;
                    ++i;
                }
                else // this branch handles "falls asleep" observations and counts minutes 
                {
                    TimeSpan timeAsleep = parser.Results[i + 1].Timestamp - parser.Results[i].Timestamp;
                    int minutesAsleep = Convert.ToInt32(timeAsleep.TotalMinutes);
                    int fallAsleepMinute = parser.Results[i].Timestamp.Minute;

                    var sleepPerMinuteCounter = guardMinuteCounter[currentGuardId];
                    for (int m = fallAsleepMinute; m < fallAsleepMinute + minutesAsleep; ++m)
                    {
                        if (!sleepPerMinuteCounter.ContainsKey(m))
                            sleepPerMinuteCounter.Add(m, 1);
                        else
                            ++sleepPerMinuteCounter[m];
                    }

                    i += 2;
                }
            }

            // get max entry per guard and than max of these intermediate results
            var maxCountersPerGuard = new Dictionary<Tuple<int, int>, int>(); // parameters from left to right: guardId, minute, counter
            foreach (KeyValuePair<int, Dictionary<int, int>> entry in guardMinuteCounter)
            {
                var minutesCounter = entry.Value;
                if (entry.Value.Count > 0)
                {
                    var maxCounter = minutesCounter.Values.Max();
                    var maxMinute = minutesCounter.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                    //maxCountersPerGuard.Add(entry.Key, new Tuple<int, int>(maxMinute, maxCounter));
                    maxCountersPerGuard.Add(new Tuple<int, int>(entry.Key, maxMinute), maxCounter);
                }

                //var minute = sleepPerMinuteCounter.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                //foreach (var minute in minutesCounter)
                //{

                //}
            };

            var maxTuple = maxCountersPerGuard.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            System.Console.WriteLine("Guard " + maxTuple.Item1 + " is mostly frequently asleep in minunte " + maxTuple.Item2 + " which results in " + maxTuple.Item1 * maxTuple.Item2);
        }
    }

        class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Day04Tasks dayInst = new Day04Tasks();
            dayInst.Exec(false, true);

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
