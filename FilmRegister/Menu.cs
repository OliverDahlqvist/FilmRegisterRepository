using System;
using System.Collections.Generic;
using System.Text;

namespace FilmRegister
{
    class MenuItem
    {
        private string name;
        private int spacing;
        private bool selected;
        private bool showCursor;
        private bool correct;
        private ConsoleColor color;
        private ConsoleColor errorColor;

        public string Name
        {
            get { return name; }
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
        public MenuItem(string name, ConsoleColor color, ConsoleColor errorColor, int spacing, bool showCursor)
        {
            this.name = name;
            this.spacing = spacing;
            this.showCursor = showCursor;
            this.color = color;
            this.errorColor = errorColor;
        }
        public MenuItem(string name, int spacing = 14, bool showCursor = true)
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
    }
    class Menu
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

        public void AddMenuItem(string title, int spacing, ConsoleColor color = ConsoleColor.White, ConsoleColor errorColor = ConsoleColor.Red, bool showCursor = true)
        {
            MenuItem newItem = new MenuItem(" " + title, color, errorColor, spacing, showCursor);
            menuItems = AddItemToArray(newItem, menuItems);
            menuItemsAmount = menuItems.Length;
        }
        public void AddMenuItem(string title, ConsoleColor color = ConsoleColor.White, ConsoleColor errorColor = ConsoleColor.Red, int spacing = 14, bool showCursor = true)
        {
            MenuItem newItem = new MenuItem(" " + title, color, errorColor, spacing, showCursor);
            menuItems = AddItemToArray(newItem, menuItems);
            menuItemsAmount = menuItems.Length;
        }
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
