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

            while (playing)
            {
                Console.Clear();
                Console.WriteLine("1. Add movie");
                if (Console.ReadLine() == "1")
                {
                    Console.Write("Titel: ");
                    string title = Console.ReadLine();
                    
                    Console.Write("Genre: ");
                    string[] genreList = Enum.GetNames(typeof(Genres));
                    
                    for (int i = 0; i < genreList.Length; i++)
                    {
                        Console.WriteLine(i + ": " + genreList[i]);
                    }
                    
                    Console.Write("Rating: ");
                    double rating = Convert.ToDouble(Console.ReadLine());
                    
                    Console.Write("Length: ");
                    double length = Convert.ToDouble(Console.ReadLine());

                    Movie newMovie = new Movie(title, Genres.Action, rating, length);
                    movieList = AddMovie(newMovie, movieList);                
                }
                for (int i = 0; i < movieList.Length; i++)
                {
                    if(movieList[i] != null)
                    Console.WriteLine(movieList[i].m_Title + " " + movieList[i].m_Genre + " " + movieList[i].m_Rating + " " + movieList[i].m_Length);
                }

                Console.WriteLine("\nAnother?");
                Console.ReadLine();
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
