using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using RecipeBook.Models;
using System.Linq;

namespace RecipeBook
{
    public class Recipe
    {
        public int ID { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ObservableCollection<Step> Steps { get; set; } = new ObservableCollection<Step>();


        public Recipe() { }

        public Recipe(Recipe recipeToCopy)
        {
            ID =  recipeToCopy.ID;
            Image = recipeToCopy.Image;
            Title = recipeToCopy.Title;
            Description = recipeToCopy.Description;
            Steps = new ObservableCollection<Step>(recipeToCopy.Steps.Select(x => new Step(x)));
        }
    }
}
