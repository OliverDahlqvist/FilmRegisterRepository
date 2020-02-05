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
        static void Main(string[] args)
        {
            bool playing = true;
            Movie[] movieList = new Movie[1];

            int row = 0;
            int rowMax = 3;

            int column = 0;
            int columnMax = 1;

            int spacingTitle = 14;
            int spacingGenre = 14;

            while (playing)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("1. Add movie, 2. Remove movie");

                Console.ForegroundColor = ConsoleColor.Yellow;
                string text = " Title -  Genre -  Rating -  Length";
                string[] textSplit = text.Split(" - ");
                textSplit[row] = textSplit[row].Replace(' ', '>');
                //text = String.Join(" - ", textSplit);
                Console.SetCursorPosition(0, 2);
                string test = string.Format("{0," + (-spacingTitle) + "}" + "{1," + (-spacingGenre) + "}" + "{2," + -spacingGenre + "}" + "{3," + -spacingGenre + "}", textSplit[0], textSplit[1], textSplit[2], textSplit[3]);
                Console.WriteLine(test);

                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < movieList.Length; i++) //Write the full list of movies
                {
                    if (movieList[i] != null)
                        //Console.WriteLine(movieList[i].m_Title + " " + movieList[i].m_Genre + " " + movieList[i].m_Rating + " " + movieList[i].m_Length);
                        Console.WriteLine(" {0," + -spacingTitle + "}" + "{1," + -spacingGenre + "}" + "{2," + -spacingGenre + "}" + "{3," + -spacingGenre + "}", movieList[i].m_Title, movieList[i].m_Genre, movieList[i].m_Rating, movieList[i].m_Length);
                }

                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.RightArrow)
                {
                    row++;
                    if (row > rowMax)
                        row = 0;
                }
                else if (consoleKey == ConsoleKey.LeftArrow)
                {
                    row--;
                    if (row < 0)
                        row = rowMax;
                }
                else if (consoleKey == ConsoleKey.UpArrow)
                {
                    column++;
                    if (column > columnMax)
                        column = 0;
                }
                else if (consoleKey == ConsoleKey.DownArrow)
                {
                    column--;
                    if (column < 0)
                        column = columnMax;
                }
                else if (consoleKey == ConsoleKey.D1)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.CursorVisible = true;

                    Console.Write("Titel: ");
                    string title = Console.ReadLine();
                    if(title.Length > spacingTitle)
                    {
                        spacingTitle = title.Length + 2;
                    }

                    Console.Write("\nGenre: ");
                    string[] genreList = Enum.GetNames(typeof(Genres));

                    for (int i = 0; i < genreList.Length; i++)
                    {
                        Console.WriteLine(i + ": " + genreList[i]);
                    }

                    Console.Write("\nRating: ");
                    double rating = Convert.ToDouble(Console.ReadLine());

                    Console.Write("Length: ");
                    double length = Convert.ToDouble(Console.ReadLine());

                    Movie newMovie = new Movie(title, Genres.Action, rating, length);
                    movieList = AddMovie(newMovie, movieList);
                    Console.Clear();
                }
                else if (consoleKey == ConsoleKey.D2)
                {

                }
            }
        }
        public void CreateList()
        {
            
        }
        public static Movie[] AddMovie(Movie movieToAdd, Movie[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if(list[i] == null)
                {
                    list[i] = movieToAdd;
                    return list;
                }
                else if(i == list.Length - 1)
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
            return list;
        }
    }
}
