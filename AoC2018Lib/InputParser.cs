using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018Lib
{
    /**
     * @brief This class is a generic class that helps parsing AoC input data. You can define the output type for the result as well you have to implement the abstract ParseInputEntry finction.
     * @tparam T define the data type that is stored in the output list.
     */
    public abstract class InputParser<T>
    {
        private string[] OriginalInput;
        public List<T> Results
        {
            get;
            private set;
        }

        public InputParser(string[] input)
        {
            Results = new List<T>();
            OriginalInput = input;
        }

        public abstract T ParseInputEntry(string inputEntry);

        public void ExecParsing()
        {
            foreach (string inputEntry in OriginalInput)
            {
                var tmp = ParseInputEntry(inputEntry);
                if (tmp != null)
                    Results.Add(tmp);
            }
        }
    }
}
