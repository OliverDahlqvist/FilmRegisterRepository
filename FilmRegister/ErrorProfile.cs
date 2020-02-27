using System;
using System.Collections.Generic;
using System.Text;

namespace FilmRegister
{
    public class ErrorProfile
    {
        public virtual bool CheckError(string input, out object results)
        {
            results = default;
            return false;
        }
    }
    public class ErrorProfileTitle : ErrorProfile
    {
        public override bool CheckError(string input, out object results)
        {
            bool inputLength = input.Length > 0;
            if (inputLength)
                results = input;
            else
                results = "";
            return inputLength;
        }
    }
    public class ErrorProfileGenre : ErrorProfile
    {
        public override bool CheckError(string input, out object results)
        {
            int testVariable;
            bool tryParse = int.TryParse(input, out testVariable);

            if (tryParse && testVariable > 0 && testVariable <= Enums.genres.Length)
            {
                results = Enums.genres[testVariable - 1];
                return true;
            }
            else if (input.Length == 0)
            {
                results = default;
                return false;
            }
            else
            {
                results = default;
                Console.Write("Input number 1-" + Enums.genres.Length);
                return false;
            }
        }
    }
    public class ErrorProfileRating : ErrorProfile
    {
        int min;
        int max;
        public ErrorProfileRating(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        public override bool CheckError(string input, out object results)
        {
            double output;
            bool returnBool = HelpFunctions.TryParseDouble(input, min, max, out output);
            results = output;
            return returnBool;
        }
    }
    public class ErrorProfileLength : ErrorProfile
    {
        public override bool CheckError(string input, out object results)
        {
            double output;
            bool returnBool = HelpFunctions.TryParseTime(input, out output);
            results = output;
            return returnBool;
        }
    }
    public class ErrorProfileSeen : ErrorProfile
    {
        public override bool CheckError(string input, out object results)
        {
            if (input.Length > 0)
            {
                bool inputEquals = input[0] == 'y';
                results = inputEquals;
                return true;
            }
            else
            {
                results = false;
                return false;
            }
        }
    }
}
