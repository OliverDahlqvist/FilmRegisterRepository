using System;
using System.Collections.Generic;
using System.Text;

namespace FilmRegister
{
    public enum SortingTypes { None, Title, Genre, Rating, Length, Seen}
    // This class includes everything ui related such as spacing, colours, menus etc.
    public class MenuItem
    {
        private string name;
        private int spacing;
        private bool selected;
        private bool showCursor;
        private bool correct;
        private ConsoleColor color;
        private ConsoleColor errorColor;
        private bool sorted;
        private bool sortedAscending;
        private string suffix;
        public ErrorProfile errorProfile;
        public SortingTypes sortingType;

        public string Name
        {
            get { return name + suffix; }
        }
        public int Spacing
        {
            get { return spacing; }
            set 
            { 
                spacing = value;
                if(spacing < 0)
                    spacing = 0;
            }
        }
        public bool Selected
        {
            get { return selected; }
        }
        public bool Correct
        {
            get { return correct; }
            set { correct = value; }
        }
        public ConsoleColor Color
        {
            get { return color; }
        }
        public ConsoleColor ErrorColor
        {
            get { return errorColor; }
        }
        public bool ShowCursor
        {
            get { return showCursor; }
        }
        public bool Sorted
        {
            get { return sorted; }
            set { sorted = value; }
        }
        public bool SortedAscending
        {
            get { return sortedAscending; }
        }
        public MenuItem(string name, ConsoleColor color, ConsoleColor errorColor, int spacing, bool showCursor, ErrorProfile errorProfile, SortingTypes sortingType)
        {
            this.name = name;
            this.spacing = spacing;
            this.showCursor = showCursor;
            this.color = color;
            this.errorColor = errorColor;
            this.errorProfile = errorProfile;
            this.sortingType = sortingType;
        }
        public MenuItem(int spacing = 14, bool showCursor = true)//If no error handling is required
        {
            this.name = name;
            this.spacing = spacing;
            this.showCursor = showCursor;
        }

        /// <summary>
        /// Changes the name prefix depending on input value.
        /// </summary>
        /// <param name="select">True for '>' false for blank.</param>
        public void Select(bool select)
        {
            selected = select;
            if (selected)
                name = name.Replace(' ', '>');
            else
                name = name.Replace('>', ' ');
        }
        /// <summary>
        /// Changes suffix depending on sorted and ascending.
        /// </summary>
        public void SetSorted()
        {
            if (selected)
            {
                sorted = true;
                sortedAscending = !sortedAscending;
                
                suffix = (sortedAscending ? " ^" : " v");
            }
            else
            {
                sorted = false;
                sortedAscending = false;
                suffix = " ";
            }
        }
        /// <summary>
        /// Sets new spacing
        /// </summary>
        /// <param name="newSpacing">New spacing</param>
        public void SetSpacing(int newSpacing)
        {
            Spacing = newSpacing;
        }
    }
    public class Menu
    {
        public MenuItem[] menuItems = new MenuItem[0];
        private int menuItemsAmount;
        private bool horizontal;
        private bool spacing;
        public MenuItem[] MenuItems { get { return menuItems; } }
        public int Amount { get { return menuItemsAmount; } }
        public bool Horizontal { get { return horizontal; } }
        public bool Spacing { get { return spacing; } }

        public Menu(bool horizontal, bool spacing = false)
        {
            this.horizontal = horizontal;
            if(horizontal)
                this.spacing = true;
        }
        /// <summary>
        /// Creates menu item and adds it to menu item array in the menu it was called from.
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="spacing">Spacing</param>
        /// <param name="color">Color when correct</param>
        /// <param name="errorColor">Color when incorrect</param>
        /// <param name="showCursor">Show cursor or not</param>
        /// <param name="errorProfile">Error profile to check for errors</param>
        /// <param name="sortingType">Sorting type used for sorting</param>
        public void AddMenuItem(string title, int spacing, ConsoleColor color = ConsoleColor.White, ConsoleColor errorColor = ConsoleColor.Red, bool showCursor = true, ErrorProfile errorProfile = null, SortingTypes sortingType = default)
        {
            MenuItem newItem = new MenuItem(" " + title, color, errorColor, spacing, showCursor, errorProfile, sortingType);
            menuItems = AddItemToArray(newItem, menuItems);
            menuItemsAmount = menuItems.Length;
        }
        public void AddMenuItem(string title, ConsoleColor color = ConsoleColor.White, ConsoleColor errorColor = ConsoleColor.Red, int spacing = 14, bool showCursor = true, ErrorProfile errorProfile = null, SortingTypes sortingType = default)
        {
            MenuItem newItem = new MenuItem(" " + title, color, errorColor, spacing, showCursor, errorProfile, sortingType);
            menuItems = AddItemToArray(newItem, menuItems);
            menuItemsAmount = menuItems.Length;
        }
        /// <summary>
        /// Adds menu item to MenuItem array dynamically. Doesn't matter the size of the array initially.
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="array">Array to add to</param>
        /// <returns></returns>
        private MenuItem[] AddItemToArray(MenuItem item, MenuItem[] array)
        {
            if (array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == null)
                    {
                        array[i] = item;
                    }
                    else if (i == array.Length - 1)
                    {
                        MenuItem[] newArray = new MenuItem[array.Length + 1];
                        for (int j = 0; j < array.Length; j++)
                        {
                            newArray[j] = array[j];
                        }
                        newArray[i + 1] = item;
                        return newArray;
                    }
                }
            }
            else
            {
                MenuItem[] newArray = new MenuItem[1];
                newArray[0] = item;
                return newArray;
            }
            Console.WriteLine("Error, couldn't create new array");
            return null;
        }
    }
}
