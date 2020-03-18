using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FilmRegister
{
    public static class Enums
    {
        public static Genres[] genres = (Genres[])Enum.GetValues(typeof(Genres));
    }
    class Program
    {
        static void Main(string[] args)
        {
            bool playing = true;

            string savePath = "Filmer.txt";

            Movie[] movieList = new Movie[0];

            int spacingTitle = 14;
            int spacingOther = 14;
            int spacingMovies = 8;

            Genres[] genres = Enums.genres;

            Menu[] menus = new Menu[2];//Define how many menus there are.
            IntRange currentMenu = new IntRange(0, menus.Length);
            menus[0] = new Menu(true, true);
            menus[1] = new Menu(false);


            ConsoleColor defaultColor = ConsoleColor.Yellow;
            menus[0].AddMenuItem("Title", spacingTitle, defaultColor, defaultColor, showCursor: false, sortingType: SortingTypes.Title);
            menus[0].AddMenuItem("Genre", spacingOther, defaultColor, defaultColor, showCursor: false, sortingType: SortingTypes.Genre);
            menus[0].AddMenuItem("Rating", spacingOther, defaultColor, defaultColor, showCursor: false, sortingType: SortingTypes.Rating);
            menus[0].AddMenuItem("Length", spacingOther, defaultColor, defaultColor, showCursor: false, sortingType: SortingTypes.Length);
            menus[0].AddMenuItem("Seen", spacingOther, defaultColor, defaultColor, showCursor: false, sortingType: SortingTypes.Seen);

            menus[1].AddMenuItem("Title", spacingMovies, errorProfile: new ErrorProfileTitle());
            menus[1].AddMenuItem("Genre", spacingMovies, errorProfile: new ErrorProfileGenre());
            menus[1].AddMenuItem("Rating", spacingMovies, errorProfile: new ErrorProfileRating(0, 10));
            menus[1].AddMenuItem("Length", spacingMovies, errorProfile: new ErrorProfileLength());
            menus[1].AddMenuItem("Seen", spacingMovies, errorProfile: new ErrorProfileSeen(), showCursor: true);
            menus[1].AddMenuItem("[Done]", color: ConsoleColor.Green, showCursor: false);
            menus[1].AddMenuItem("[Cancel]", showCursor: false);

            IntRange selection = new IntRange(0, menus[0].menuItems.Length - 1);
            bool ratingSorted = false;

            movieList = FileFunctions.LoadFile(savePath, movieList, menus[currentMenu.Value].menuItems[0]);
            IntRange selectionAlt = new IntRange(0, movieList.Length);

            ConsoleKey consoleKey;
            while (playing)
            {
                currentMenu.Value = 0;
                selection.MaxValue = menus[currentMenu.Value].Amount - 1;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("[1. Add] [2. Edit] [3. Remove]");

                SetupMenus(menus[currentMenu.Value].MenuItems, 0, 2);
                DisplayMovies(movieList);

                consoleKey = Console.ReadKey().Key;
                UpdateMenu(consoleKey);
                if (consoleKey == ConsoleKey.D1)//Key 1 pressed
                {
                    Console.Clear();
                    currentMenu.Value = 1;
                    selection.Value = 0;
                    selection.MaxValue = menus[currentMenu.Value].Amount - 1;

                    string title = "";
                    Genres genre = default;
                    double length = 0;
                    bool seen = false;
                    Object[] movieVariables = new Object[5];

                    string[] userInputs = new string[selection.MaxValue + 1];//Used to store all keystrokes from corresponding selection
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
                        Console.WriteLine(i + 1 + ": " + genres[i]);
                    }

                    while (currentMenu.Value == 1)//While in adding movie menu
                    {
                        Console.CursorVisible = false;

                        Console.SetCursorPosition(0, 10);
                        Console.Write(new string(' ', Console.WindowWidth));
                        bool tryParse = false;
                        MenuItem[] menuItems = menus[currentMenu.Value].menuItems;

                        Console.SetCursorPosition(0, 10);

                        for (int i = 0; i < movieVariables.Length; i++)
                        {
                            if(menus[currentMenu.Value].menuItems[i].errorProfile != null && selection.Value == i)
                            {
                                SetInputs(menus[currentMenu.Value].menuItems[i].errorProfile.CheckError(userInputs[i], out movieVariables[i]), i);
                            }
                        }

                        if(HelpFunctions.CheckBools(inputsCorrect))
                        {
                            SetInputs(true, 5);
                        }
                        SetupMenus(menus[currentMenu.Value].menuItems, 0, 0);
                        
                        Console.SetCursorPosition(menus[currentMenu.Value].menuItems[selection.Value].Spacing + userInputs[selection.Value].Length, selection.Value);
                        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                        consoleKey = consoleKeyInfo.Key;

                        if (UpdateMenu(consoleKey))
                        {

                        }
                        else if (consoleKey == ConsoleKey.Enter)
                        {
                            allInputsCorrect = HelpFunctions.CheckBools(inputsCorrect);//Check if all inputs are correct
                            if (selection.Value == selection.MaxValue && allInputsCorrect)
                            {
                                currentMenu.Value = 0;
                                Movie newMovie = new Movie((string)movieVariables[0], (Genres)movieVariables[1], (double)movieVariables[2], (double)movieVariables[3], (bool)movieVariables[4]);
                                movieList = HelpFunctions.AddMovie(newMovie, movieList);
                                int newSpacing = movieVariables[0].ToString().Length;
                                if (newSpacing > menus[0].MenuItems[0].Spacing)//Check if the title of the movie is longer than current spacing value, if the title is longer set the spacing value to the title length + 2
                                {
                                    menus[currentMenu.Value].menuItems[0].SetSpacing(newSpacing + 2);
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
                                Console.SetCursorPosition(menus[currentMenu.Value].menuItems[selection.Value].Spacing + userInputs[selection.Value].Length, selection.Value);
                                Console.Write(" ");
                            }
                            else if (consoleKey != ConsoleKey.Backspace)
                                userInputs[selection.Value] += consoleKeyInfo.KeyChar;//Måste fixa så att den kollar om tangenten man slår in faktiskt är en karaktär som kan användas. T.ex. F1

                        }
                    }
                    void SetInputs(bool input, int index)
                    {
                        inputsCorrect[index] = input;
                        menus[currentMenu.Value].menuItems[index].Correct = input;
                    }
                }
                else if (consoleKey == ConsoleKey.D2)//Key 2 pressed
                {
                    FileFunctions.SaveFile(savePath, movieList);
                }
                else if(consoleKey == ConsoleKey.Enter)
                {
                    ratingSorted = !ratingSorted;
                    HelpFunctions.Sort(movieList, menus[currentMenu.Value].menuItems[selection.Value], 0, movieList.Length - 1, ratingSorted);
                    for (int i = 0; i < menus[currentMenu.Value].menuItems.Length; i++)
                    {
                        menus[currentMenu.Value].menuItems[i].SetSorted(ratingSorted);
                    }
                }
            }
            void SetupMenus(MenuItem[] menuItems, int left, int top)
            {
                Console.SetCursorPosition(left, top);
                if (menuItems[selection.Value].ShowCursor)
                    Console.CursorVisible = true;
                else
                    Console.CursorVisible = false;

                for (int i = 0; i < menuItems.Length; i++)
                {
                    string sb = "";
                    MenuItem menuItem = menuItems[i];
                    if (i == selection.Value && selectionAlt.Value < 1)
                        menuItem.Select(true);
                    else
                        menuItem.Select(false);

                    if (menus[currentMenu.Value].Spacing)
                        sb += string.Format("{0," + -menuItem.Spacing + "}", menuItem.Name);
                    else
                        sb += string.Format("{0}", menuItem.Name);

                    if (menus[currentMenu.Value].Horizontal == false || i == menuItems.Length - 1)
                        sb += "\n";

                    if (!menuItem.Correct)
                        Console.ForegroundColor = menuItem.ErrorColor;
                    else
                        Console.ForegroundColor = menuItem.Color;

                    Console.Write(sb.ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            bool UpdateMenu(ConsoleKey consoleKey)//Takes input (the input depends on if the current menu is horizontal) and changes selection value which in term makes the menus update
            {
                bool isHorizontal = menus[currentMenu.Value].Horizontal;
                if (isHorizontal)
                {
                    if (consoleKey == ConsoleKey.RightArrow)
                    {
                        selectionAlt.Value = 0;
                        selection.Value++;
                    }
                    else if (consoleKey == ConsoleKey.LeftArrow)
                    {
                        selectionAlt.Value = 0;
                        selection.Value--;
                    }
                    else if (consoleKey == ConsoleKey.DownArrow)
                    {
                        selection.Value = 0;
                        selectionAlt.Value++;
                    }
                    else if (consoleKey == ConsoleKey.UpArrow)
                    {
                        selection.Value = 0;
                        selectionAlt.Value--;
                    }
                }
                else
                {
                    if (consoleKey == ConsoleKey.DownArrow)
                        selection.Value++;
                    else if (consoleKey == ConsoleKey.UpArrow)
                        selection.Value--;
                    else if (consoleKey == ConsoleKey.RightArrow)
                    {
                        selectionAlt.Value++;
                    }
                    else if (consoleKey == ConsoleKey.LeftArrow)
                    {
                        selectionAlt.Value--;
                    }
                }
                if (consoleKey == ConsoleKey.DownArrow || consoleKey == ConsoleKey.UpArrow || consoleKey == ConsoleKey.LeftArrow || consoleKey == ConsoleKey.RightArrow)
                    return true;
                else
                    return false;
            }
            void DisplayMovies(Movie[] movieList)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (movieList.Length > 0)
                {
                    string s;
                    for (int i = 0; i < movieList.Length; i++)
                    {
                        string prefix;
                        if(selectionAlt.Value - 1 == i)
                            prefix = ">";
                        else
                            prefix = " ";
                        s = string.Format(prefix + "{0," + -menus[0].menuItems[0].Spacing + "}" + 
                                        "{1," + -menus[0].menuItems[1].Spacing + "}" + 
                                        "{2," + -menus[0].menuItems[2].Spacing + "}" + 
                                        "{3:0}h{4:00}m", 
                                        movieList[i].title, 
                                        movieList[i].genre, 
                                        movieList[i].rating, 
                                        Math.Floor(movieList[i].length / 60), 
                                        movieList[i].length % 60);
                        s = s.PadRight(s.Length + 9) + (movieList[i].seen ? "x" : " ");
                        Console.WriteLine(s);
                    }
                }
            }
        }
    }
    public static class HelpFunctions
    {
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
        private static int Partition(Movie[] movies, MenuItem menuItem, int low, int high, bool ascending)
        {
            double pivotDouble = 0;
            string pivotString;
            bool pivotBool = false;

            switch (menuItem.sortingType)
            {
                case SortingTypes.None:
                    break;
                case SortingTypes.Title:
                    break;
                case SortingTypes.Genre:
                    break;
                case SortingTypes.Rating:
                    pivotDouble = movies[high].rating;
                    break;
                case SortingTypes.Length:
                    pivotDouble = movies[high].length;
                    break;
                case SortingTypes.Seen:

                    break;
                default:

                    break;
            }
            
            int lowIndex = (low - 1);

            for (int j = low; j < high; j++)
            {
                bool variableDifference = false;
                
                switch (menuItem.sortingType)
                {
                    case SortingTypes.None:
                        break;
                    case SortingTypes.Title:
                        break;
                    case SortingTypes.Genre:
                        break;
                    case SortingTypes.Rating:
                        variableDifference = (ascending ? movies[j].rating >= pivotDouble : movies[j].rating <= pivotDouble);
                        break;
                    case SortingTypes.Length:
                        variableDifference = (ascending ? movies[j].length >= pivotDouble : movies[j].length <= pivotDouble);
                        break;
                    case SortingTypes.Seen:
                            variableDifference = (ascending ? movies[j].seen != pivotBool : movies[j].seen == pivotBool);
                        break;
                }

                /*if (ascending)
                    variableDifference = movies[j].rating >= pivot;
                else
                    variableDifference = movies[j].rating <= pivot;*/

                if (variableDifference)
                {
                    lowIndex++;

                    Movie temp = movies[lowIndex];
                    movies[lowIndex] = movies[j];
                    movies[j] = temp;
                }
            }

            Movie temp1 = movies[lowIndex + 1];
            movies[lowIndex + 1] = movies[high];
            movies[high] = temp1;

            return lowIndex + 1;
        }
        public static void Sort(Movie[] movies, MenuItem menuItem, int low, int high, bool ascending)
        {
            if(low < high)
            {
                switch (menuItem)
                {
                    default:
                        break;
                }
                int partitionIndex = Partition(movies, menuItem, low, high, ascending);
                Sort(movies, menuItem, low, partitionIndex - 1, ascending);
                Sort(movies, menuItem, partitionIndex + 1, high, ascending);
            }
        }

        /// <summary>
        /// Tries to parse double within given min and max value, returns true if parse succeded and out double variable
        /// </summary>
        /// <param name="input">String input</param>
        /// <param name="min">Minimum desired value</param>
        /// <param name="max">Maximum desired value</param>
        /// <param name="results">Variable to set value to</param>
        /// <returns></returns>
        public static bool TryParseDouble(string input, int min, int max, out double results)
        {
            bool tryParse = double.TryParse(input.Replace('.', ','), out results);
            if (tryParse && results >= min && results <= max)
            {
                return true;
            }
            else if(input.Length > 0)
            {
                Console.Write("Input number between {0} - {1}", min, max);
            }
            return false;
        }
        /// <summary>
        /// Tries to parse time from string input, returns true if parse succeded and sets the output to results
        /// </summary>
        /// <param name="input">String input</param>
        /// <param name="results">Reference variable that gets the parsed value</param>
        /// <returns></returns>
        public static bool TryParseTime(string input, out double results)
        {
            if (input.Length > 0)
            {
                string[] splitInput = input.Split('h');
                if (splitInput.Length > 0 && splitInput.Length <= 2)
                {
                    double output;
                    double addedOutput;
                    bool tryParse = false;
                    tryParse = double.TryParse(splitInput[0], out output);
                    addedOutput = output * 60;
                    if (splitInput.Length > 1)
                    {
                        splitInput[1] = Regex.Replace(splitInput[1], "[^0-9.]", "");
                        tryParse = double.TryParse(splitInput[1], out output);
                        addedOutput += output;
                    }
                    results = addedOutput;
                    return true;
                }
            }
            results = 0;
            return false;
        }
        /// <summary>
        /// Checks if all bools in a bool array are true, returns true or false
        /// </summary>
        /// <param name="bools">Input bool array</param>
        /// <returns></returns>
        public static bool CheckBools(bool[] bools)
        {
            for (int i = 0; i < bools.Length-2; i++)
            {
                if (!bools[i])
                    return false;
            }
            return true;
        }
    }
    public static class FileFunctions
    {
        public static void SaveFile(string filePath, Movie[] movies)
        {
            StreamWriter file = new StreamWriter(filePath);
            for (int i = 0; i < movies.Length; i++)
            {
                file.WriteLine(movies[i].title + "\t" + movies[i].genre + "\t" + movies[i].rating + "\t" + movies[i].length + "\t" + movies[i].seen);
            }
            file.Close();
        }
        public static Movie[] LoadFile(string filePath, Movie[] movies, MenuItem menuItem)
        {
            if (File.Exists(filePath))
            {
                StreamReader file = new StreamReader(filePath);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] lines = line.Split('\t');
                    if(lines.Length <= 0)
                    {
                        file.Close();
                        File.WriteAllText(filePath, string.Empty);
                        break;
                    }
                    string title = lines[0];
                    Genres genre;
                    double rating;
                    double length;
                    bool seen;

                    if (!Enum.TryParse(lines[1], out genre) && !double.TryParse(lines[2], out rating))
                    {
                        Console.WriteLine("Couldn't parse genre");
                        break;
                    }

                    if (!double.TryParse(lines[2], out rating))
                    {
                        Console.WriteLine("Couldn't parse rating");
                        break;
                    }

                    if (!double.TryParse(lines[3], out length))
                    {
                        Console.WriteLine("Couldn't parse length");
                        break;
                    }

                    if (!bool.TryParse(lines[4], out seen))
                    {
                        Console.WriteLine("Couldn't parse seen");
                        break;
                    }

                    Movie newMovie = new Movie(title, genre, rating, length, seen);
                    movies = HelpFunctions.AddMovie(newMovie, movies);
                    if(title.Length > menuItem.Spacing)
                    {
                        menuItem.SetSpacing(title.Length + 2);
                    }
                }
                
                file.Close();

                Console.WriteLine("Loaded file");
                return movies;
            }
            else
            {
                File.Create(filePath);
                return null;
            }
        }
    }
}