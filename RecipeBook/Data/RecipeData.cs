using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace RecipeBook.Data
{
    [Table("Recipes")]
    public class RecipeData
    {
        [PrimaryKey]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public string StepsID { get; set; }
    }
}
