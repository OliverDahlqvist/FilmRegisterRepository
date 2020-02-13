using System;
using System.Collections.Generic;
using System.Text;

namespace FilmRegister
{
    public enum Genres { Action, Adventure, Animation, Biography, Crime, Drama, Horror, War }
    public class Movie
    {
        public string m_Title;
        public Genres m_Genre;
        public double m_Rating;
        public double m_Length;
        public bool m_Seen;
        public Movie(string title, Genres genre, double rating, double length, bool seen)
        {
            m_Title = title;
            m_Genre = genre;
            m_Rating = rating;
            m_Length = length;
            m_Seen = seen;
        }
    }
}