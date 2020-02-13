using System;

namespace FilmRegister
{
    class Program
    {
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
            IntRange selection = new IntRange(0, 4);
            int column = 0;
            int columnMax = 1;

            int spacingTitle = 14;
            int spacingOther = 14;

            Genres[] genres = (Genres[])Enum.GetValues(typeof(Genres));

            string text = " Title, Genre, Rating, Length, Seen, [Done]";
            string[] textSplit = text.Split(",");

            ConsoleKey consoleKey;

            while (playing)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("[1. Add movie] [2. Edit movie] [3. Remove movie]");

                Console.ForegroundColor = ConsoleColor.Yellow;


                SetupCategories();

                Console.SetCursorPosition(0, 2);
                string test = string.Format("{0," + (-spacingTitle) + "}" + "{1," + (-spacingOther) + "}" + "{2," + -spacingOther + "}" + "{3, " + (-spacingOther) + "}" + "{4}", textSplit[0], textSplit[1], textSplit[2], textSplit[3], textSplit[4]);
                Console.WriteLine(test);

                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < movieList.Length; i++) //Write the full list of movies
                {
                    if (movieList[i] != null)
                    {
                        string movieSeen = "";
                        if (movieList[i].m_Seen)
                        {
                            movieSeen = "x";
                        }
                        Console.WriteLine(" {0," + -spacingTitle + "}" + "{1," + -spacingOther + "}" + "{2," + -spacingOther + "}" + "{3:0}h{4:00}m" + "{5}", 
                            movieList[i].m_Title, movieList[i].m_Genre, movieList[i].m_Rating, movieList[i].m_Length / 60, movieList[i].m_Length % 60, movieSeen);
                    }
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
                    bool seen = false;

                    selection.MaxValue = textSplit.Length - 1;
                    string[] userInputs = new string[selection.MaxValue];//Used to store all keystrokes from corresponding selection
                    for (int i = 0; i < userInputs.Length; i++)
                    {
                        userInputs[i] = "";
                    }
                    bool[] inputsCorrect = new bool[userInputs.Length];
                    bool allInputsCorrect;

                    
                    while (addingMovie)
                    {
                        Console.CursorVisible = false;
                        
                        SetupCategories();

                        Console.SetCursorPosition(0, 10);
                        Console.Write(new string(' ', Console.WindowWidth));
                        bool tryParse = false;
                        switch (selection.Value)//Depending on the current selection check for errors and display them in the console.
                        {
                            case 0:
                                inputsCorrect[selection.Value] = true;
                                title = userInputs[0];
                                break;
                            case 1:
                                int testVariable = 0;
                                tryParse = int.TryParse(userInputs[selection.Value], out testVariable);
                                Console.SetCursorPosition(0, 12);
                                for (int i = 0; i < genres.Length; i++)
                                {
                                    Console.WriteLine(i + ": " + genres[i]);
                                }
                                Console.SetCursorPosition(0, 10);
                                if(!tryParse && userInputs[selection.Value].Length > 0 || testVariable > genres.Length)
                                {
                                    Console.Write("Input numbers between 0-" + genres.Length);
                                    inputsCorrect[selection.Value] = false;
                                }
                                else if(testVariable > 0 && testVariable < genres.Length)
                                {
                                    genre = genres[testVariable];
                                    inputsCorrect[selection.Value] = true;
                                }

                                break;
                            case 2:
                                //rating = TryParseDouble(userInputs[selection.Value], 0, 10, 0, 10, true);
                                tryParse = double.TryParse(userInputs[selection.Value].Replace('.', ','), out rating);
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
                                tryParse = double.TryParse(userInputs[selection.Value], out length);
                                inputsCorrect[3] = tryParse;
                                Console.SetCursorPosition(0, 10);
                                if (!tryParse && userInputs[selection.Value].Length > 0)
                                {
                                    Console.Write("Input numbers only");
                                }

                                break;
                            case 4:
                                if (userInputs[selection.Value].Length > 0 && userInputs[selection.Value][0] == 'y')
                                    seen = true;
                                else
                                    seen = false;

                                inputsCorrect[selection.Value] = seen;
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

                        allInputsCorrect = CheckBools(inputsCorrect);//Check if all inputs are correct
                        if (allInputsCorrect)
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
                            if(selection.Value == selection.MaxValue && allInputsCorrect)
                            {
                                addingMovie = false;
                                Movie newMovie = new Movie(title, genre, rating, length, seen);
                                movieList = AddMovie(newMovie, movieList);
                                if(title.Length > spacingTitle)
                                {
                                    spacingTitle = title.Length + 2;
                                }
                                Console.Clear();
                            }
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
                    */
                    
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
            return true;        }

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

        public static void SortList()//Kan byta från void till något annat, lägg in funktionen på rad 
        {

        }
    }
}