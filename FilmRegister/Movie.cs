using System;
using System.Collections.Generic;
using System.Text;


namespace FilmRegister
{
    public enum Genres { Action, Adventure, Animation, Biography, Crime, Drama, Horror, War, SciFi }
    public class Movie
    {
        // Defining the fields of the class
        public string title;
        public Genres genre;
        public double rating;
        public double length;
        public bool seen;
        private bool selected;

        public string[,] variableStrings = new string[5,2];
      
        // Defining the Movie method and its input values
        public Movie(string title, Genres genre, double rating, double length, bool seen)
        {
            this.title = title; // Setting the fields values
            this.genre = genre;
            this.rating = rating;
            this.length = length;
            this.seen = seen;
        }
    }
}