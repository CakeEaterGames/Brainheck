using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brainheck
{
    public class Brainfuck
    {
        public List<byte> Memory = new List<byte>();
        public char[] Code;

        public int MC = 0;
        public int PC = 0;
        public int Chain = 0;
        public char Command = '+';
        public bool IsRunning = false;

        public List<byte> Input;
        public int InputPointer = 0;

        public List<byte> Output;
        public int OutputPointer = 0;
        public string OutputString = "";

        public int StepCount = 0;
        public int MaxStepCount = 100000;

        HashSet<int> Visited = new HashSet<int>();
        public int UsedMemory { get { return Visited.Count; } }
        public State CurrentState = State.Noraml;
        public enum State
        {
            Noraml,
            Finished,
            OutOfBounds,
            NoInput,
            TooManySteps
        }

        public Brainfuck()
        {
            
        }
      
        public void Init(string code, List<byte> memory, List<byte> input)
        {
            Code = code.ToCharArray();

            IsRunning = true;
            Input = input;
            Memory = memory;

            Output = new List<byte>();
        }

        public void FastRun()
        {
            while (IsRunning)
            {
                Iterate();

            }
        }

        public void Iterate()
        {
            if (PC < Code.Length)
            {
                StepCount++;
                if (StepCount > MaxStepCount)
                {
                    CurrentState = State.TooManySteps;
                    IsRunning = false;
                    return;
                }


                while (Code[PC] == ' ')
                {
                    PC++;
                }

                char cur = Code[PC];
                if (PC != 0 && Code[PC - 1] == cur)
                {
                    Chain++;
                }
                else
                {
                    Chain = 0;
                }

                while (Memory.Count <= MC)
                {
                    Memory.Add(0);
                }

                Visited.Add(MC);
                Command = cur;
                switch (cur)
                {
                    case '+':
                        Memory[MC]++;
                        PC++;
                        break;
                    case '-':
                        Memory[MC]--;
                        PC++;
                        break;
                    case '<':
                        MC--;
                        if (MC<0)
                        {
                            CurrentState = State.OutOfBounds;
                            IsRunning = false;
                        }
                        PC++;
                        break;
                    case '>':
                        MC++;
                        PC++;
                        break;
                    case '.':
                        Output.Add(Memory[MC]);
                        OutputString += (char)Memory[MC];
                        OutputPointer++;
                        PC++;
                        break;
                    case ',':
                        if (InputPointer>=Input.Count)
                        {
                            CurrentState = State.NoInput;
                            IsRunning = false;
                        }
                        else
                        {
                            Memory[MC] = Input[InputPointer];
                            InputPointer++;
                            PC++;
                        }
                      
                        break;

                    case '[':
                        if (Memory[MC] == 0)
                        {
                            int loop = 0;
                            while (true)
                            {
                                PC++;
                                if (Code[PC] == ']')
                                {
                                    if (loop == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        loop--;
                                    }
                                }
                                if (Code[PC] == '[')
                                {
                                    loop++;
                                }
                            }
                        }

                        PC++;
                        break;
                    case ']':
                        if (Memory[MC] != 0)
                        {
                            int loop = 0;
                            while (true)
                            {
                                PC--;
                                if (Code[PC] == '[')
                                {
                                    if (loop == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        loop--;
                                    }
                                }
                                if (Code[PC] == ']')
                                {
                                    loop++;
                                }
                            }
                        }
                        PC++;
                        break;
                    default:
                        PC++;
                        break;

                }

            }
            else
            {
                CurrentState = State.Finished;
                IsRunning = false;
            }
        

        }
   
        public List<byte> GetFinalTape()
        {
            int LastNonZero = 0;
            for (int i = 0; i < Memory.Count; i++)
            {
                if (Memory[i]!=0)
                {
                    LastNonZero = i;
                }
            }
            if (LastNonZero!=Memory.Count-1)
            {
                Memory.RemoveRange(LastNonZero + 1, Memory.Count - LastNonZero-1);
            }

            return Memory;
        }

        public int SoundDuration = 10;
        public int SoundStep = 10;

        public void PlaySound()
        {
            switch (Command)
            {
                case '+':
                    Console.Beep(1000+SoundStep*Math.Min(10,Chain),SoundDuration);
                    break;
                case '-':
                    Console.Beep(200 - SoundStep * Math.Min(10, Chain), SoundDuration);
                    break;
                case '>':
                    break;
                case '<':
                    break;
                case '.':
                    break;
                case ',':
                    break;
                case '[':
                    break;
                case ']':
                    break;
                default:
                    break;
            }
        }
    }

}
