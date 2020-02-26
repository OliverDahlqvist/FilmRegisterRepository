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

        public string[] variableStrings = new string[5];
        public Movie(string title, Genres genre, double rating, double length, bool seen)
        {
            this.title = title;
            this.genre = genre;
            this.rating = rating;
            this.length = length;
            this.seen = seen;
            variableStrings[0] = title;
            variableStrings[1] = genre.ToString();
            variableStrings[2] = rating.ToString();
            variableStrings[3] = length.ToString();
            
            if (seen)
                variableStrings[4] = "x";
            else
                variableStrings[4] = "";
        }
    }
}