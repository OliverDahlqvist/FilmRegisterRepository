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


            string text = " Title, Genre, Rating, Length";
            string[] textSplit = text.Split(",");

            ConsoleKey consoleKey;

            while (playing)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("[1. Add movie] [2. Remove movie]");

                Console.ForegroundColor = ConsoleColor.Yellow;


                SetupTitles();

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
                    while (addingMovie)
                    {
                        SetupTitles();
                        
                        for (int i = 0; i < textSplit.Length; i++)
                        {
                            Console.SetCursorPosition(0, i);
                            Console.WriteLine(textSplit[i] + ":");
                        }
                        consoleKey = Console.ReadKey().Key;
                        if (consoleKey == ConsoleKey.DownArrow)
                        {
                            selection.Value++;
                        }
                        else if (consoleKey == ConsoleKey.UpArrow)
                        {
                            selection.Value--;
                        }

                    }

                    Console.ReadLine();



                    /*//Console.WriteLine(string.Format("{0," + (-spacingTitle) + "}" + "{1," + (-spacingOther) + "}" + "{2," + -spacingOther + "}" + "{3}", "Title", "Genre", "Rating", "Length"));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.CursorVisible = true;

                    string title = Console.ReadLine();//Set title
                    if(title.Length > spacingTitle)
                    {
                        spacingTitle = title.Length + 2;
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(string.Format("{0," + (-spacingTitle) + "}" + "{1," + (-spacingOther) + "}" + "{2," + -spacingOther + "}" + "{3}", "Title", "Genre", "Rating", "Length"));//Update header if the title is too long.
                    }

                    string[] genreList = Enum.GetNames(typeof(Genres));
                    

                    bool chosingGenre = true;
                    Genres genre = default;
                    int selection = 0;
                    Console.CursorVisible = false;
                    while (chosingGenre)
                    {
                        for (int i = 0; i < genreList.Length; i++)//Display all possible genres to chose from
                        {
                            Console.SetCursorPosition(spacingTitle - 1, 1 + i);
                            if (i == selection)
                                Console.WriteLine(">" + genreList[i]);
                            else
                                Console.WriteLine(" " + genreList[i]);

                        }

                        consoleKey = Console.ReadKey().Key;

                        if(consoleKey == ConsoleKey.UpArrow)
                        {
                            selection--;
                        }
                        else if (consoleKey == ConsoleKey.DownArrow)
                        {
                            selection++;
                        }
                        else if(consoleKey == ConsoleKey.Enter)
                        {
                            genre = (Genres)selection;
                            chosingGenre = false;
                            Console.SetCursorPosition(spacingTitle, 1);
                            Console.Write(genre);

                            for (int i = 0; i < genreList.Length - 1; i++)
                            {
                                Console.SetCursorPosition(spacingTitle - 1, 2 + i);
                                Console.Write(new string(' ', spacingOther));
                            }
                            Console.SetCursorPosition(spacingTitle + spacingOther, 1);
                            Console.CursorVisible = true;
                        }
                    }
                    

                    double rating = GetInputDouble(0, 10);

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
            void SetupTitles()
            {
                for (int i = 0; i < textSplit.Length; i++)
                {
                    textSplit[i] = textSplit[i].Replace('>', ' ');
                }
                textSplit[selection.Value] = textSplit[selection.Value].Replace(' ', '>');
            }
        }
        /// <summary>
        /// Reads and parses input from user.
        /// Displays error messages relevant to the error the user made. 
        /// Returns parsed double.
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns></returns>
        public static double GetInputDouble(int min, int max)
        {
            bool tryParse = false;
            double output = 0;
            while (!tryParse)
            {
                Console.ForegroundColor = ConsoleColor.White;
                tryParse = double.TryParse(Console.ReadLine().Replace('.', ','), out output);

                if (!tryParse)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(0, 3);
                    Console.WriteLine("Error: input numbers only.");
                }

                if(output > max || output < min)
                {
                    tryParse = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: input not in range {0} - {1}.", min, max);
                }
            }
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
