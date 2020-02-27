using System;
using System.Collections.Generic;
using System.Text;

namespace FilmRegister
{
    public enum Genres { Action, Adventure, Animation, Biography, Crime, Drama, Horror, War, Scifi }
    public class Movie
    {
        public string title;
        public Genres genre;
        public double rating;
        public double length;
        public bool seen;
        private bool selected;

        public string[,] variableStrings = new string[5,2];
        public Movie(string title, Genres genre, double rating, double length, bool seen)
        {
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.length = length;
            this.seen = seen;
        }
    }
}