using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RecipeBook.Data
{
    public interface IDataSource
    {
        ObservableCollection<Recipe> GetAllRecipes();
        Recipe GetRecipeById(int id);
        void SaveOrUpdateRecipe(Recipe recipe);
    }
}
