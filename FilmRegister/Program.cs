using System;
using System.Text;

namespace FilmRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            bool playing = true;
            Movie[] movieList = new Movie[0];
            int column = 0;
            int columnMax = 1;

            int spacingTitle = 14;
            int spacingOther = 14;

            Genres[] genres = (Genres[])Enum.GetValues(typeof(Genres));

            string text = " Title, Genre, Rating, Length, Seen, [Done]";
            string[] textSplit = text.Split(",");

            Menu[] menus = new Menu[2];//Define how many menus there are.
            IntRange currentMenu = new IntRange(0, menus.Length);
            menus[0] = new Menu(true, true);
            menus[1] = new Menu(false);

            ConsoleColor defaultColor = ConsoleColor.Yellow;
            menus[0].AddMenuItem("Title", spacingTitle, defaultColor);
            menus[0].AddMenuItem("Genre", spacingOther, defaultColor);
            menus[0].AddMenuItem("Rating", spacingOther, defaultColor);
            menus[0].AddMenuItem("Length", spacingOther, defaultColor);
            menus[0].AddMenuItem("Seen", spacingOther, defaultColor);

            menus[1].AddMenuItem("Title");
            menus[1].AddMenuItem("Genre");
            menus[1].AddMenuItem("Rating");
            menus[1].AddMenuItem("Length");
            menus[1].AddMenuItem("Seen");
            menus[1].AddMenuItem("[Done]");

            IntRange selection = new IntRange(0, menus[0].menuItems.Length - 1);

            ConsoleKey consoleKey;
            while (playing)
            {
                //Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("[1. Add] [2. Edit] [3. Remove]");

                //Console.ForegroundColor = ConsoleColor.Yellow;

                Console.SetCursorPosition(0, 2);
                SetupCategories(menus[currentMenu.Value].MenuItems);
                //Console.WriteLine(test);

                //Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < movieList.Length; i++) //Write the full list of movies
                {
                    if (movieList[i] != null)
                    {
                        string movieSeen = "";
                        if (movieList[i].seen)
                        {
                            movieSeen = "x";
                        }
                        Console.WriteLine(" {0," + -menus[0].menuItems[0].Spacing + "}" + "{1," + -menus[0].menuItems[1].Spacing + "}" + "{2," + -menus[0].menuItems[2].Spacing + "}" + "{3:0}h{4:00}m" + "{5}",
                            movieList[i].title, movieList[i].genre, movieList[i].rating, movieList[i].length / 60, movieList[i].length % 60, movieSeen);
                    }
                }

                consoleKey = Console.ReadKey().Key;
                UpdateMenu(consoleKey);
                if (consoleKey == ConsoleKey.D1)
                {
                    Console.Clear();
                    currentMenu.Value = 1;

                    bool addingMovie = true;
                    selection.Value = 0;

                    string title = "";
                    Genres genre = default;
                    double rating = 0;
                    double length = 0;
                    bool seen = false;

                    selection.MaxValue = menus[currentMenu.Value].Amount - 1;
                    string[] userInputs = new string[selection.MaxValue];//Used to store all keystrokes from corresponding selection
                    for (int i = 0; i < userInputs.Length; i++)
                    {
                        userInputs[i] = "";
                    }
                    bool[] inputsCorrect = new bool[userInputs.Length];//Bool array used to collect all errors, if there are no errors the user can press done. 
                    bool allInputsCorrect;

                    Console.SetCursorPosition(0, 12);
                    Console.WriteLine("Genres");
                    for (int i = 0; i < genres.Length; i++)
                    {
                        Console.WriteLine(i + ": " + genres[i]);
                    }

                    while (addingMovie)
                    {
                        Console.CursorVisible = false;

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
                                
                                Console.SetCursorPosition(0, 10);
                                if(!tryParse && userInputs[selection.Value].Length > 0 || testVariable > genres.Length - 1 || testVariable < 0)
                                {
                                    Console.Write("Input numbers between 0-" + genres.Length);
                                    inputsCorrect[selection.Value] = false;
                                    menus[currentMenu.Value].menuItems[selection.Value].Correct = false;
                                }
                                else if(testVariable > 0 && testVariable < genres.Length)
                                {
                                    genre = genres[testVariable];
                                    inputsCorrect[selection.Value] = true;
                                    menus[currentMenu.Value].menuItems[selection.Value].Correct = true;
                                }
                                else
                                {
                                    inputsCorrect[selection.Value] = false;
                                    menus[currentMenu.Value].menuItems[selection.Value].Correct = false;
                                }

                                break;
                            case 2:
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
                        Console.SetCursorPosition(0, 0);
                        SetupCategories(menus[currentMenu.Value].menuItems);

                        allInputsCorrect = CheckBools(inputsCorrect);//Check if all inputs are correct
                        /*if (allInputsCorrect)
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                            Console.ForegroundColor = ConsoleColor.Red;

                        Console.ForegroundColor = ConsoleColor.White;*/

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
                            selection.Value++;
                        else if (consoleKey == ConsoleKey.UpArrow)
                            selection.Value--;
                        else if (consoleKey == ConsoleKey.Enter)
                        {
                            if (selection.Value == selection.MaxValue && allInputsCorrect)
                            {
                                addingMovie = false;
                                Movie newMovie = new Movie(title, genre, rating, length, seen);
                                movieList = AddMovie(newMovie, movieList);
                                if (title.Length > menus[0].MenuItems[0].Spacing)//Check if the title of the movie is longer than current spacing value, if the title is longer set the spacing value to the title length + 2
                                {
                                    menus[0].MenuItems[0].Spacing = title.Length + 2;
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
                            else if (consoleKey != ConsoleKey.Backspace)
                                userInputs[selection.Value] += consoleKeyInfo.KeyChar;//Måste fixa så att den kollar om tangenten man slår in faktiskt är en karaktär som kan användas. T.ex. F1

                        }
                    }
                }
                else if (consoleKey == ConsoleKey.D2)
                {

                }
            }

            void SetupCategories(MenuItem[] menuItems)
            {
                
                for (int i = 0; i < menuItems.Length; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    MenuItem menuItem = menuItems[i];
                    string text = "";
                    if (i == selection.Value)
                        menuItem.Select(true);
                    else
                        menuItem.Select(false);

                    if (menus[currentMenu.Value].Spacing)
                        sb.AppendFormat("{0," + -menuItem.Spacing + "}", menuItem.Name);
                    else
                        sb.AppendFormat("{0}", menuItem.Name);

                    if (!menus[currentMenu.Value].Horizontal)
                        sb.Append("\n");

                    if (!menuItem.Correct)
                    {
                        Console.ForegroundColor = menuItem.ErrorColor;
                        Console.Write(sb.ToString());
                    }
                    else
                    {
                        Console.ForegroundColor = menuItem.Color;
                        Console.Write(sb.ToString());
                    }

                    Console.ForegroundColor = ConsoleColor.White;//Set the color to white again.
                }
            }

            void UpdateMenu(ConsoleKey consoleKey)//Takes input (the input depends on if the current menu is horizontal) and changes selection value which in term makes the menus update
            {
                bool isHorizontal = menus[currentMenu.Value].Horizontal;
                if (isHorizontal)
                {
                    if (consoleKey == ConsoleKey.RightArrow)
                        selection.Value++;
                    else if (consoleKey == ConsoleKey.LeftArrow)
                        selection.Value--;
                }
                else
                {
                    if (consoleKey == ConsoleKey.DownArrow)
                        selection.Value++;
                    else if (consoleKey == ConsoleKey.UpArrow)
                        selection.Value--;
                }
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
        /// Reads and parses input from user. ÄNDRA. Displays error messages relevant to the error the user made. Returns parsed double.
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