using System;
using System.Collections.Generic;
using System.Text;

namespace FilmRegister
{
    // This class limits the max value in the program.
    public class IntRange
    {
        private int rangeValue;
        private int rangeMax;
        public int Value
        {
            get { return rangeValue; }
            set
            {
                rangeValue = value;
                if (rangeValue > rangeMax)
                    rangeValue = 0;
                else if (rangeValue < 0)
                    rangeValue = rangeMax;
            }
        }
        public int MaxValue
        {
            get { return rangeMax; }
            set { rangeMax = value; }
        }
        public IntRange(int value, int valueMax)
        {
            rangeValue = value;
            rangeMax = valueMax;
        }
    }
}
