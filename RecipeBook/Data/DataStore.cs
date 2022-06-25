using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace RecipeBook.Data
{
    public static class DataStore
    {
        public static readonly IDataSource Source;

        static DataStore()
        {
            Source = new Mock();
        }

        //public static ObservableCollection<Recipe> GetAllRecipes => Source.GetAllRecipes();
    }
}
