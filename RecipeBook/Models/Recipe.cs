using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook
{
    public class Recipe
    {
        public int ID { get; set; }

        public string Image { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string AsString()
        {
            return $"{ID},{Image},{Title},{Description}";
        }
    }
}
