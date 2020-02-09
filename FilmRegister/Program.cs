using System;

namespace FilmRegister
{
    class Program
    {
        public enum Genres { Action, Adventure, Animation, Biography, Crime, Drama, Horror, War}
        public class Movie
        {
            public string m_Title;
            public Genres m_Genre;
            public double m_Rating;
            public double m_Length;
            public Movie(string title, Genres genre, double rating, double length)
            {
                m_Title = title;
                m_Genre = genre;
                m_Rating = rating;
                m_Length = length;
            }
        }
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
        static void Main(string[] args)
        {
            bool playing = true;
            Movie[] movieList = new Movie[0];

            IntRange selection = new IntRange(0, 3);

            int column = 0;
            int columnMax = 1;

            int spacingTitle = 14;
            int spacingOther = 14;


            string text = " Title, Genre, Rating, Length, [Done]";
            string[] textSplit = text.Split(",");

            ConsoleKey consoleKey;

            while (playing)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("[1. Add movie] [2. Remove movie]");

                Console.ForegroundColor = ConsoleColor.Yellow;


                SetupCategories();

                Console.SetCursorPosition(0, 2);
                string test = string.Format("{0," + (-spacingTitle) + "}" + "{1," + (-spacingOther) + "}" + "{2," + -spacingOther + "}" + "{3}", textSplit[0], textSplit[1], textSplit[2], textSplit[3]);
                Console.WriteLine(test);

                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < movieList.Length; i++) //Write the full list of movies
                {
                    if (movieList[i] != null)
                        Console.WriteLine(" {0," + -spacingTitle + "}" + "{1," + -spacingOther + "}" + "{2," + -spacingOther + "}" + "{3:0}h{4:00}m", movieList[i].m_Title, movieList[i].m_Genre, movieList[i].m_Rating, movieList[i].m_Length / 60, movieList[i].m_Length % 60);
                }

                consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.RightArrow)//Menu Left
                {
                    selection.Value++;
                }
                else if (consoleKey == ConsoleKey.LeftArrow)//Menu Right
                {
                    selection.Value--;
                }
                else if (consoleKey == ConsoleKey.D1)
                {
                    Console.Clear();
                    
                    bool addingMovie = true;
                    selection.Value = 0;

                    string title = "";
                    Genres genre = default;
                    double rating = 0;
                    double length = 0;

                    string[] userInputs = new string[4] { "", "", "", ""};//Used to store all keystrokes from corresponding selection
                    bool[] inputsCorrect = new bool[userInputs.Length];

                    
                    while (addingMovie)
                    {
                        Console.CursorVisible = false;
                        selection.MaxValue = 4;
                        SetupCategories();

                        Console.SetCursorPosition(0, 10);
                        Console.Write(new string(' ', Console.WindowWidth));
                        switch (selection.Value)//Depending on the current selection check for errors and display them in the console.
                        {
                            case 0:
                                inputsCorrect[selection.Value] = true;
                                break;
                            case 1:
                                inputsCorrect[selection.Value] = true;
                                break;
                            case 2:
                                //rating = TryParseDouble(userInputs[selection.Value], 0, 10, 0, 10, true);
                                bool tryParse = double.TryParse(userInputs[selection.Value].Replace('.', ','), out rating);
                                inputsCorrect[2] = tryParse;
                                Console.SetCursorPosition(0, 10);
                                if (!tryParse && userInputs[selection.Value].Length > 0)
                                {
                                    Console.Write("Input numbers only");
                                }
                                else if (rating > 10)
                                {
                                    Console.Write("Pick between 0-10");
                                }

                                break;
                            case 3:
                                //length = TryParseDouble(userInputs[selection.Value], 0, 0, 0, 10);
                                bool tryParse1 = double.TryParse(userInputs[selection.Value], out length);
                                inputsCorrect[3] = tryParse1;
                                Console.SetCursorPosition(0, 10);
                                if (!tryParse1 && userInputs[selection.Value].Length > 0)
                                {
                                    Console.Write("Input numbers only");
                                }

                                break;
                            default:
                                break;
                        }

                        for (int i = 0; i < textSplit.Length - 1; i++)//Write all except the last menu item
                        {
                            Console.SetCursorPosition(0, i);
                            if (inputsCorrect[i])
                                Console.ForegroundColor = ConsoleColor.White;
                            else
                                Console.ForegroundColor = ConsoleColor.Red;

                            Console.WriteLine(textSplit[i] + ":");
                        }
                        if (CheckBools(inputsCorrect))
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                            Console.ForegroundColor = ConsoleColor.Red;

                        Console.WriteLine("\n" + textSplit[textSplit.Length - 1]);//Write last menu item which is special
                        Console.ForegroundColor = ConsoleColor.White;

                        if (userInputs.Length > selection.Value)//Check if the array is longer than the selection to avoid index out of range, else just hide the cursor
                        {
                            Console.CursorVisible = true;
                            Console.SetCursorPosition(textSplit[selection.Value].Length + 2 + userInputs[selection.Value].Length, selection.Value);
                        }
                        else
                        {
                            Console.CursorVisible = false;
                            Console.SetCursorPosition(0, selection.Value + 1);
                        }


                        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                        consoleKey = consoleKeyInfo.Key;

                        if (consoleKey == ConsoleKey.DownArrow)
                        {
                            selection.Value++;
                        }
                        else if (consoleKey == ConsoleKey.UpArrow)
                        {
                            selection.Value--;
                        }
                        else if(consoleKey == ConsoleKey.Enter)
                        {
                            selection.Value++;
                        }
                        else
                        {
                            if (consoleKey == ConsoleKey.Backspace && userInputs[selection.Value].Length > 0)
                            {
                                userInputs[selection.Value] = userInputs[selection.Value].Remove(userInputs[selection.Value].Length - 1);
                                Console.SetCursorPosition(textSplit[selection.Value].Length + 2 + userInputs[selection.Value].Length, selection.Value);
                                Console.Write(" ");
                            }
                            else if(consoleKey != ConsoleKey.Backspace)
                                userInputs[selection.Value] += consoleKeyInfo.KeyChar;//Måste fixa så att den kollar om tangenten man slår in faktiskt är en karaktär som kan användas. T.ex. F1
                            
                        }
                        

                    }

                    /*double rating = GetInputDouble(0, 10);

                    Console.Write("Length: ");
                    double length = Convert.ToDouble(Console.ReadLine());

                    Movie newMovie = new Movie(title, genre, rating, length);
                    movieList = AddMovie(newMovie, movieList);
                    Console.Clear();*/
                }
                else if (consoleKey == ConsoleKey.D2)
                {

                }
            }
            void SetupCategories()
            {
                for (int i = 0; i < textSplit.Length; i++)
                {
                    textSplit[i] = textSplit[i].Replace('>', ' ');
                }
                textSplit[selection.Value] = textSplit[selection.Value].Replace(' ', '>');
            }
        }

        public static bool CheckBools(bool[] bools)
        {
            for (int i = 0; i < bools.Length; i++)
            {
                if (!bools[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Reads and parses input from user. ÄNDRA
        /// Displays error messages relevant to the error the user made. 
        /// Returns parsed double.
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns></returns>
        public static double TryParseDouble(string input, int min, int max, int cursorX, int cursorY, bool replace = false)
        {
            
            bool tryParse = false;
            double output = 0;
            if(replace)
                tryParse = double.TryParse(input.Replace('.', ','), out output);
            else
                tryParse = double.TryParse(Console.ReadLine(), out output);

            Console.SetCursorPosition(cursorX, cursorY);
            if (!tryParse && input.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error: input numbers only.");
                output = 0;
            }
            else if(max > 0 && (output > max || output < min))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(new string(' ', Console.WindowWidth));
                Console.Write("Error: input not in range {0} - {1}.", min, max);
                output = 0;
            }
            else
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.ForegroundColor = ConsoleColor.White;
            return output;
        }
        /// <summary>
        /// Adds movie to an array. If the current array doesn't have enough space, a new array is created and replaces the old one.
        /// </summary>
        /// <param name="movieToAdd">Object to add.</param>
        /// <param name="list">Array object gets added to.</param>
        /// <returns>List with added object.</returns>
        public static Movie[] AddMovie(Movie movieToAdd, Movie[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == null)
                {
                    list[i] = movieToAdd;
                    return list;
                }
                else if (i == list.Length - 1)
                {
                    Movie[] newMovieList = new Movie[list.Length + 1];
                    for (int j = 0; j < list.Length; j++)
                    {
                        newMovieList[j] = list[j];
                    }
                    newMovieList[i + 1] = movieToAdd;
                    return newMovieList;
                }
            }
            Movie[] newList = new Movie[1];//If the list is initially set to 0 length, create new list and add new movie, return the new list.
            newList[0] = movieToAdd;
            return newList;
        }
    }
}
