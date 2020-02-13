using System;
using System.Collections.Generic;
using System.Text;

namespace FilmRegister
{
    public enum Genres { Action, Adventure, Animation, Biography, Crime, Drama, Horror, War }
    public class Movie
    {
        public string title;
        public Genres genre;
        public double rating;
        public double length;
        public bool seen;
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