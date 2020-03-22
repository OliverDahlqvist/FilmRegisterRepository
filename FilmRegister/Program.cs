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
            Movie[] searchedMovies = new Movie[0];

            int spacingTitle = 14;//Variables that dictates spacing between menu items
            int spacingOther = 14;
            int spacingMovies = 8;

            Genres[] genres = Enums.genres;
            ConsoleColor defaultColor = ConsoleColor.Yellow;

            Menu[] menus = new Menu[3];//Define how many menus there are.
            IntRange currentMenu = new IntRange(0, menus.Length);
            menus[0] = new Menu(true, true);
            menus[1] = new Menu(false);

            menus[0].AddMenuItem("Title", spacingTitle, defaultColor, defaultColor, showCursor: true, sortingType: SortingTypes.Title);
            menus[0].AddMenuItem("Genre", spacingOther, defaultColor, defaultColor, showCursor: true, sortingType: SortingTypes.Genre);
            menus[0].AddMenuItem("Rating", spacingOther, defaultColor, defaultColor, showCursor: true, sortingType: SortingTypes.Rating);
            menus[0].AddMenuItem("Length", spacingOther, defaultColor, defaultColor, showCursor: true, sortingType: SortingTypes.Length);
            menus[0].AddMenuItem("Seen", spacingOther, defaultColor, defaultColor, showCursor: true, sortingType: SortingTypes.Seen);

            menus[1].AddMenuItem("Title", spacingMovies, errorProfile: new ErrorProfileTitle());
            menus[1].AddMenuItem("Genre", spacingMovies, errorProfile: new ErrorProfileGenre());
            menus[1].AddMenuItem("Rating", spacingMovies, errorProfile: new ErrorProfileRating(0, 10));
            menus[1].AddMenuItem("Length", spacingMovies, errorProfile: new ErrorProfileLength());
            menus[1].AddMenuItem("Seen", spacingMovies, errorProfile: new ErrorProfileSeen(), showCursor: true);
            menus[1].AddMenuItem("[Done]", color: ConsoleColor.Green, showCursor: false);
            menus[1].AddMenuItem("[Cancel]", showCursor: false);

            IntRange selection = new IntRange(0, menus[0].menuItems.Length - 1);
            IntRange selectionAlt = new IntRange(0, movieList.Length);
            bool ratingSorted = false;
            bool editMovie = false;

            movieList = FileFunctions.LoadFile(savePath, movieList, menus[currentMenu.Value].menuItems[0]);
            ConsoleKey consoleKey;
            ConsoleKeyInfo searchKeyInfo;

            string headerText = "[1. Add] [Enter. Edit] [Delete. Remove] Search: ";
            string searchString = "";
            while (playing)
            {
                selection.MaxValue = menus[currentMenu.Value].Amount - 1;
                selectionAlt.MaxValue = movieList.Length;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(headerText + searchString);

                SetupMenus(menus[currentMenu.Value].MenuItems, 0, 2);
                if (searchString.Length > 0)
                {
                    DisplayMovies(searchedMovies);
                    Console.SetCursorPosition(0, 5 + movieList.Length);
                    for (int i = 5 + searchedMovies.Length; i < 5 + movieList.Length; i++)//
                    {
                        Console.SetCursorPosition(0, i);
                        Console.Write(new string(' ', Console.WindowWidth));
                    }
                }
                else
                    DisplayMovies(movieList);
                Console.SetCursorPosition(headerText.Length + searchString.Length, 0);

                searchKeyInfo = Console.ReadKey();
                consoleKey = searchKeyInfo.Key;
                UpdateMenu(consoleKey);
                if (consoleKey == ConsoleKey.D1 || (consoleKey == ConsoleKey.Enter && selectionAlt.Value > 0)) //Key 1 pressed
                {
                    int movieIndex = 0;
                    if (selectionAlt.Value > 0) //Check if movie is being edited
                    {
                        editMovie = true;
                        movieIndex = selectionAlt.Value - 1;
                    }
                    ChangeMenu(1);
                    if(editMovie)
                        Console.WriteLine("Edit movie");
                    else
                        Console.WriteLine("Add movie");

                    selection.MaxValue = menus[currentMenu.Value].Amount - 1;
                    selectionAlt.MaxValue = 0;

                    int topOffset = 2;//Used to add offset from top line
                    int errorOffset = 9;//Used to position error line

                    Object[] movieVariables = new Object[5];//Create array that is going to hold input variables, the reason it's an object array is because the variable types are not the same
                    string[] userInputs = new string[selection.MaxValue + 1]; //Used to store all keystrokes from corresponding selection
                    bool[] inputsCorrect = new bool[userInputs.Length];//Bool array used to collect all errors, if there are no errors the user can press done. 

                    for (int i = 0; i < userInputs.Length; i++)
                    {
                        userInputs[i] = "";
                    }
                    for (int i = 0; i < menus[currentMenu.Value].menuItems.Length; i++)//Resets all menu items
                    {
                        menus[currentMenu.Value].menuItems[i].Correct = false;
                    }

                    DisplayGenres(genres, 0, 12 + topOffset, "Genres");

                    while (currentMenu.Value == 1)//While in adding movie menu
                    {
                        Console.SetCursorPosition(0, errorOffset + topOffset);
                        Console.Write(new string(' ', Console.WindowWidth));//Clear the error line

                        Console.SetCursorPosition(0, errorOffset + topOffset);
                        for (int i = 0; i < movieVariables.Length; i++)
                        {
                            if(menus[currentMenu.Value].menuItems[i].errorProfile != null && selection.Value == i)
                            {
                                SetInputs(menus[currentMenu.Value].menuItems[i].errorProfile.CheckError(userInputs[i], out movieVariables[i], 0, errorOffset + topOffset), i);
                            }
                        }

                        if(EssentialFunctions.CheckBools(inputsCorrect))//Sets the done button to true or false depending on if all other buttons are correct or not
                        {
                            SetInputs(true, 5);
                        }
                        SetupMenus(menus[currentMenu.Value].menuItems, 0, topOffset);
                        Console.SetCursorPosition(menus[currentMenu.Value].menuItems[selection.Value].Spacing + userInputs[selection.Value].Length, selection.Value + topOffset);

                        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                        consoleKey = consoleKeyInfo.Key;
                        if (UpdateMenu(consoleKey))
                        {

                        }
                        else if (consoleKey == ConsoleKey.Enter)
                        {
                            if (selection.Value == selection.MaxValue - 1 && EssentialFunctions.CheckBools(inputsCorrect))//Add movie
                            {
                                Movie newMovie = new Movie((string)movieVariables[0], (Genres)movieVariables[1], (double)movieVariables[2], (double)movieVariables[3], (bool)movieVariables[4]);
                                if (editMovie)
                                {
                                    movieList = EssentialFunctions.ReplaceMovie(movieIndex, newMovie, movieList);
                                    FileFunctions.SaveFile(savePath, movieList);
                                    editMovie = false;
                                }
                                else
                                {
                                    movieList = EssentialFunctions.AddMovie(newMovie, movieList);
                                    FileFunctions.SaveFile(savePath, movieList);
                                }

                                int newSpacing = movieVariables[0].ToString().Length;
                                EssentialFunctions.UpdateSpacing(newSpacing, 2, menus[0].MenuItems[0]);
                                ChangeMenu(0);
                            }
                            else if(selection.Value == selection.MaxValue)//Cancel
                            {
                                ChangeMenu(0);
                            }
                            else
                            {
                                selection.Value++;
                            }
                        }
                        else
                        {
                            if (consoleKey == ConsoleKey.Backspace && userInputs[selection.Value].Length > 0)
                            {
                                userInputs[selection.Value] = userInputs[selection.Value].Remove(userInputs[selection.Value].Length - 1);
                                Console.SetCursorPosition(menus[currentMenu.Value].menuItems[selection.Value].Spacing + userInputs[selection.Value].Length, selection.Value + topOffset);
                                Console.Write(" ");
                            }
                            else if (consoleKey != ConsoleKey.Backspace)
                                userInputs[selection.Value] += consoleKeyInfo.KeyChar;

                        }
                    }
                    void SetInputs(bool input, int index)
                    {
                        inputsCorrect[index] = input;
                        menus[currentMenu.Value].menuItems[index].Correct = input;
                    }
                }
                else if(consoleKey == ConsoleKey.Enter)//Enter pressed
                {
                    if(selectionAlt.Value == 0)//If the selection is on a menu item, Sort list
                    {
                        for (int i = 0; i < menus[currentMenu.Value].menuItems.Length; i++)
                        {
                            menus[currentMenu.Value].menuItems[i].SetSorted();
                        }
                        EssentialFunctions.Sort(movieList, menus[currentMenu.Value].menuItems[selection.Value], 0, movieList.Length - 1, menus[currentMenu.Value].menuItems[selection.Value].SortedAscending);
                    }
                }
                else if(consoleKey == ConsoleKey.Delete)//Delete pressed
                {
                    if (selectionAlt.Value > 0)
                    {
                        movieList = EssentialFunctions.RemoveMovie(selectionAlt.Value - 1, ref selectionAlt, movieList);
                        FileFunctions.SaveFile(savePath, movieList);
                    }
                }
                else//Search
                {
                    if (consoleKey == ConsoleKey.Backspace && searchString.Length > 0)
                    {
                        searchString = searchString.Remove(searchString.Length - 1);
                        Console.SetCursorPosition(headerText.Length + searchString.Length, 0);
                        Console.Write(" ");
                    }
                    else if(consoleKey != ConsoleKey.Backspace)
                    {
                        char inputKey = searchKeyInfo.KeyChar;
                        if(char.IsLetterOrDigit(inputKey) || consoleKey == ConsoleKey.Spacebar)
                            searchString += inputKey;
                        
                    }
                    if (searchString.Length > 0)
                    {
                        searchedMovies = EssentialFunctions.Search(movieList, searchString);
                    }
                }
            }
            void ChangeMenu(int menuIndex)//Changes the menu variables and clears the console.
            {
                Console.Clear();
                currentMenu.Value = menuIndex;
                selection.Value = 0;
                selectionAlt.Value = 0;
            }
            void SetupMenus(MenuItem[] menuItems, int left, int top)//Displays menu items with their corresponding options such as color, name, cursor, spacing.
            {
                Console.SetCursorPosition(left, top);
                if (menuItems[selection.Value].ShowCursor)
                    Console.CursorVisible = true;
                else
                    Console.CursorVisible = false;

                int lineLength = 0;
                for (int i = 0; i < menuItems.Length; i++)
                {
                    string menuItemString = "";
                    MenuItem menuItem = menuItems[i];
                    if (i == selection.Value && selectionAlt.Value < 1)
                        menuItem.Select(true);
                    else
                        menuItem.Select(false);

                    if (menus[currentMenu.Value].Spacing)
                        menuItemString += string.Format("{0," + -menuItem.Spacing + "}", menuItem.Name);
                    else
                        menuItemString += string.Format("{0}", menuItem.Name);

                    if (menus[currentMenu.Value].Horizontal == false || i == menuItems.Length - 1)
                        menuItemString += "\n";

                    if (!menuItem.Correct)
                        Console.ForegroundColor = menuItem.ErrorColor;
                    else
                        Console.ForegroundColor = menuItem.Color;
                    lineLength += menuItemString.Length;
                    Console.Write(menuItemString);
                }
                Console.Write(new string('_', lineLength - 8) + "\n\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            bool UpdateMenu(ConsoleKey consoleKey)//Takes input (the input depends on if the current menu is horizontal) and changes selection value which in term makes the menus update, returns true if correct key was pressed
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
            void DisplayMovies(Movie[] movieList)//Writes all movies in movie array.
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (movieList.Length > 0)
                {
                    string movieString;
                    for (int i = 0; i < movieList.Length; i++)
                    {
                        string prefix;
                        if (selectionAlt.Value - 1 == i && selection.Value == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                            prefix = ">";
                        }
                        else
                        {
                            prefix = " ";
                        }

                        movieString = string.Format(prefix + "{0," + -menus[0].menuItems[0].Spacing + "}" + //Takes all variables from movie object and formats them to have spacing between them.
                                        "{1," + -menus[0].menuItems[1].Spacing + "}" +
                                        "{2," + -menus[0].menuItems[2].Spacing + "}" +
                                        "{3:0}h{4:00}m",
                                        movieList[i].title,
                                        movieList[i].genre,
                                        movieList[i].rating,
                                        Math.Floor(movieList[i].length / 60),
                                        movieList[i].length % 60);
                        movieString = movieString.PadRight(movieString.Length + 9) + (movieList[i].seen ? "x" : " ");
                        
                        Console.WriteLine(movieString);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            void DisplayGenres(Genres[] genres, int left, int top, string description)//Writes all genres in a genre array
            {
                Console.SetCursorPosition(left, top);
                Console.WriteLine(description);
                for (int i = 0; i < genres.Length; i++)
                {
                    Console.WriteLine(i + 1 + ": " + genres[i]);
                }
            }
        }
    }
    public static class EssentialFunctions //Static class that can be used from anywhere to perform essential tasks such as sorting, removing and adding movies.
    {
        /// <summary>
        /// Adds movie to an array. If the current array doesn't have enough space, a new array is created and replaces the old one.
        /// </summary>
        /// <param name="movieToAdd">Object to add.</param>
        /// <param name="list">Array object gets added to.</param>
        /// <param name="selection">Updates the IntRange to reflect the new list.</param>
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
        /// <summary>
        /// Removes a movie from a movie array at given index, and updates the selection.
        /// </summary>
        /// <param name="index">Index in array</param>
        /// <param name="selection">Selection variable to update</param>
        /// <param name="list">List to remove movie from</param>
        /// <returns>Array with given movie removed</returns>
        public static Movie[] RemoveMovie(int index, ref IntRange selection, Movie[] list)
        {
            Movie[] newList = new Movie[list.Length - 1];
            if(list.Length > index)
            {
                list[index] = null;
                int indexOffset = 0;
                for (int i = 0; i < newList.Length; i++)//Adds elements to new array, increments indexOffset by 1 if the element is null
                {
                    if (list[i] == null)
                    {
                        indexOffset++;
                    }
                    newList[i] = list[i + indexOffset];
                }
            }
            if(index == 0 && newList.Length > 0)
                index = 1;
            else if(index == 0 && newList.Length == 0)
                index = 0;

            selection.UpdateRange(index, newList.Length);

            Console.Clear();
            return newList;
        }
        /// <summary>
        /// Replaces a movie from a movie array at given index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="movie"></param>
        /// <param name="movieArray"></param>
        /// <returns></returns>
        public static Movie[] ReplaceMovie(int index, Movie movie, Movie[] movieArray)//Replaces index with new movie in movie array
        {
            if (movieArray.Length > index) 
            {
                movieArray[index] = movie;
            }
            return movieArray;
        }
        /// <summary>
        /// Sorts partitions of movie array. This function uses several different statements depending on input variable "SortingType"
        /// </summary>
        /// <param name="movies">Movie array</param>
        /// <param name="menuItem">Menu item that shall be used to check which variable to sort after</param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="ascending">Sort ascending or descending</param>
        /// <returns></returns>
        private static int Partition(Movie[] movies, MenuItem menuItem, int low, int high, bool ascending)
        {
            double pivotDouble = 0;
            string pivotString = "";
            bool pivotBool = false;

            switch (menuItem.sortingType)
            {
                case SortingTypes.Title:
                    pivotString = movies[high].title;
                    break;
                case SortingTypes.Genre:
                    pivotString = movies[high].genre.ToString();
                    break;
                case SortingTypes.Rating:
                    pivotDouble = movies[high].rating;
                    break;
                case SortingTypes.Length:
                    pivotDouble = movies[high].length;
                    break;
            }
            
            int lowIndex = (low - 1);

            for (int j = low; j < high; j++)
            {
                bool variableDifference = false;
                
                switch (menuItem.sortingType)
                {
                    case SortingTypes.Title:
                        variableDifference = (ascending ? movies[j].title.CompareTo(pivotString) < 0 : movies[j].title.CompareTo(pivotString) > 0);
                        break;
                    case SortingTypes.Genre:
                        variableDifference = (ascending ? movies[j].genre.ToString().CompareTo(pivotString) < 0 : movies[j].genre.ToString().CompareTo(pivotString) > 0);
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
        /// <summary>
        /// Repetitive function which sorts movie arrays depending on if they are previously sorted or not.
        /// </summary>
        /// <param name="movies">Movie array to sort</param>
        /// <param name="menuItem"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="ascending"></param>
        public static void Sort(Movie[] movies, MenuItem menuItem, int low, int high, bool ascending)
        {
            if(low < high)
            {
                int partitionIndex = Partition(movies, menuItem, low, high, ascending);
                Sort(movies, menuItem, low, partitionIndex - 1, ascending);
                Sort(movies, menuItem, partitionIndex + 1, high, ascending);
            }
        }
        /// <summary>
        /// Searches for string in movie array
        /// </summary>
        /// <param name="movies">Movie array</param>
        /// <param name="key">Search key</param>
        /// <returns>Movies with search term</returns>
        public static Movie[] Search(Movie[] movies, string key)
        {
            Movie[] searchedMovies = new Movie[0];
            for (int i = 0; i < movies.Length; i++)
            {
                if (movies[i].title.ToUpper().Contains(key.ToUpper()))
                {
                    searchedMovies = AddMovie(movies[i], searchedMovies);
                }
                
            }
            return searchedMovies;
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
                    if (addedOutput > 0)
                    {
                        results = addedOutput;
                        return true;
                    }
                }
            }
            results = 0;
            return false;
        }
        /// <summary>
        /// Checks if all bools in a bool array are true, returns true or false
        /// </summary>
        /// <param name="bools">Input bool array</param>
        /// <returns>True or false depending on if all bools are true or false</returns>
        public static bool CheckBools(bool[] bools)
        {
            for (int i = 0; i < bools.Length-2; i++)
            {
                if (!bools[i])
                    return false;//If false; Stops right away to avoid unnecessary calls
            }
            return true;
        }
        /// <summary>
        /// Updates spacing.
        /// </summary>
        /// <param name="newSpacing">New spacing length</param>
        /// <param name="increment">How much to increase</param>
        /// <param name="menuItem">Which menu item to update</param>
        public static void UpdateSpacing(int newSpacing, int increment, MenuItem menuItem)
        {
            if (newSpacing > menuItem.Spacing)//Update spacing
            {
                menuItem.SetSpacing(newSpacing + increment);
            }
        }
    }
    public static class FileFunctions //Static class that can be accessed from wherever, used for saving and loading.
    {
        /// <summary>
        /// Saves movie array to file.
        /// </summary>
        /// <param name="filePath">Save filepath</param>
        /// <param name="movies">Movie array</param>
        public static void SaveFile(string filePath, Movie[] movies)
        {
            StreamWriter file = new StreamWriter(filePath);
            for (int i = 0; i < movies.Length; i++)
            {
                file.WriteLine(movies[i].title + "\t" + movies[i].genre + "\t" + movies[i].rating + "\t" + movies[i].length + "\t" + movies[i].seen);
            }
            file.Close();
        }
        /// <summary>
        /// Loads from file to movie array.
        /// </summary>
        /// <param name="filePath">Load filepath</param>
        /// <param name="movies">Movie array to add movies to</param>
        /// <param name="menuItem">MenuItem to update with new spacing</param>
        /// <returns>Movie array with loaded entries</returns>
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
                    movies = EssentialFunctions.AddMovie(newMovie, movies);//Uses add movie function to dynamically add movies to an array (doesn't matter the size of the array)
                    EssentialFunctions.UpdateSpacing(title.Length, 2, menuItem);
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