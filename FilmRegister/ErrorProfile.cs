using System;
using System.Collections.Generic;
using System.Text;

namespace FilmRegister
{
    // This class handles wrong input values and prevents the program to crash.
    public class ErrorProfile
    {
        public virtual bool CheckError(string input, out object results, int left, int top)
        {
            results = default;
            return false;
        }
    }
    public class ErrorProfileTitle : ErrorProfile
    {
        public override bool CheckError(string input, out object results, int left, int top)
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
        public override bool CheckError(string input, out object results, int left, int top)
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
                Console.SetCursorPosition(left, top);
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
        public override bool CheckError(string input, out object results, int left, int top)
        {
            double output;
            bool returnBool = EssentialFunctions.TryParseDouble(input, min, max, out output);
            if (!returnBool && input.Length > 0)
            {
                Console.SetCursorPosition(left, top);
                Console.Write("Input number between {0} - {1}", min, max);
            }
            results = output;
            return returnBool;
        }
    }
    public class ErrorProfileLength : ErrorProfile
    {
        public override bool CheckError(string input, out object results, int left, int top)
        {
            double output;
            bool returnBool = EssentialFunctions.TryParseTime(input, out output);
            Console.SetCursorPosition(left, top);
            Console.Write("Format: 2h30m");
            results = output;
            return returnBool;
        }
    }
    public class ErrorProfileSeen : ErrorProfile
    {
        public override bool CheckError(string input, out object results, int left, int top)
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
