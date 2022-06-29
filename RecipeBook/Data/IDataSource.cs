using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using RecipeBook.Models;

namespace RecipeBook.Data
{
    public interface IDataSource
    {
        ObservableCollection<Recipe> GetAllRecipes();
        ObservableCollection<Step> GetStepsById(int recipeId);
        Recipe GetRecipeById(int id);
        Step GetStepById(int id);
        void AddOrUpdateRecipe(Recipe recipe);
        void AddOrUpdateStepsFromRecipe(Recipe recipe);
    }
}
