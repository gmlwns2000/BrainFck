using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainFck
{
    public class Compiler
    {
        static char[] Operators = { '>', '<', '+', '-', '.', ',', '[', ']' };

        public enum Operator
        {
            Nah = -1,
            AddPtr = 0,
            SubPtr = 1,
            Add = 2,
            Sub = 3,
            Print = 4,
            Get = 5,
            BraceOpen = 6,
            BraceClose = 7
        }

        StringBuilder output = new StringBuilder();

        readonly string OpMemory;
        int OpPtr = 0;
        Operator OpCurrent { get => Convert(OpMemory[OpPtr]); }

        long[] Memory;
        long memPtr = 0;
        long MemPtr
        {
            get => memPtr;
            set
            {
                if (value >= Memory.Length)
                {
                    int newsize = Memory.Length + heapStep;

                    Array.Resize(ref Memory, newsize);
                }

                memPtr = value;
            }
        }
        long MemCurrent { get => Memory[MemPtr]; set => Memory[MemPtr] = value; }
        int heapStep;

        long Input = 0;

        public Compiler(string str, int initialHeapSize = 4096 * 4, int heapStep = 4096)
        {
            OpMemory = str;
            this.heapStep = heapStep;
            Memory = new long[initialHeapSize];
        }

        private void Run(Operator op)
        {
            switch (op)
            {
                case Operator.SubPtr:
                    MemPtr--;
                    OpPtr++;
                    break;
                case Operator.AddPtr:
                    MemPtr++;
                    OpPtr++;
                    break;
                case Operator.BraceClose:
                    if (MemCurrent == 0)
                    {
                        OpPtr++;
                    }
                    else
                    {
                        while (OpCurrent != Operator.BraceOpen)
                        {
                            OpPtr--;
                            if (OpPtr < 0)
                                throw new BrainException("StackOverflow", OpPtr);
                        }
                    }
                    break;
                case Operator.BraceOpen:
                    if (MemCurrent == 0)
                    {
                        while (OpCurrent != Operator.BraceClose)
                        {
                            OpPtr++;
                            if (OpPtr >= OpMemory.Length)
                                throw new BrainException("StackOverflow", OpPtr);
                        }
                    }
                    else
                    {
                        OpPtr++;
                    }
                    break;
                case Operator.Sub:
                    MemCurrent--;
                    OpPtr++;
                    break;
                case Operator.Add:
                    MemCurrent++;
                    OpPtr++;
                    break;
                case Operator.Get:
                    MemCurrent = (byte)Console.Read();
                    OpPtr++;
                    break;
                case Operator.Print:
                    string printBuff = $"{((char)MemCurrent)}";
                    output.Append(printBuff);
                    Console.Write(printBuff);
                    Input = MemCurrent;
                    OpPtr++;
                    break;
                case Operator.Nah:
                    OpPtr++;
                    break;
                default:
                    throw new BrainException("Unknown Operator");
            }
        }

        public string Run()
        {
            output.Clear();

            OpPtr = 0;
            while (OpPtr < OpMemory.Length)
            {
                Run(OpCurrent);
            }

            return output.ToString();
        }

        private Operator Convert(char c)
        {
            int ind = -1;
            for (int i = 0; i < Operators.Length; i++)
            {
                if (c == Operators[i])
                {
                    ind = i;
                    break;
                }
            }
            return (Operator)ind;
        }
    }
}
