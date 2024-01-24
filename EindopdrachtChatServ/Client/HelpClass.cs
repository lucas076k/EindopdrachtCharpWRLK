using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class HelpClass
    {
        public bool ReturnTrueIfNotEmpty(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
