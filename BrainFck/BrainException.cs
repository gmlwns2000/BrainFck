using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BrainFck
{
    public class BrainException : Exception
    {
        public BrainException(string str, int pos = 0) : base($"{str} Index:{pos}")
        {

        }
    }
}