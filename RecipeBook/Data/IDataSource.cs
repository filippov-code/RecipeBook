using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using RecipeBook.Models;

namespace RecipeBook.Data
{
    public interface IDataSource
    {
        //IDataSource Singletone { get; }
        List<Recipe> GetRecipes();
        List<Step> GetSteps();
        void Save(List<Recipe> recipes, List<Step> steps);
    }
}
