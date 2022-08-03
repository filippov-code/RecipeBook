using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using RecipeBook.Models;
using System.Diagnostics;

namespace RecipeBook.Data
{
    public class DataBaseMock : DataSource
    {
        private string[][] recipesTable = new string[][]
        {
            new string[] {"1", "first recipe", "description of first recipe", "ingredients of first recipe", "1,2" },
            new string[] {"2", "second recipe", "description of second recipe", "ingredients of second recipe", "3,4,5" },
            new string[] {"3", "third recipe", "deescription of third recipe", "ingredients of third recipe", "6,7,8,9" }
        };

        private string[][] stepsTable = new string[][]
        {
            new string[] {"1", "1 step description"},
            new string[] {"2", "2 step description"},
            new string[] {"3", "3 step description"},
            new string[] {"4", "4 step description"},
            new string[] {"5", "5 step description"},
            new string[] {"6", "6 step description"},
            new string[] {"7", "7 step description"},
            new string[] {"8", "8 step description"},
            new string[] {"9", "9 step description"}
        };

        public override void Save(List<Recipe> recipes, List<Step> steps)
        {
            
        }

        protected override List<RecipeData> GetRecipesData()
        {
            List<RecipeData> result = new List<RecipeData>();
            foreach (string[] recipeRow in recipesTable)
            {
                RecipeData recipeData = new RecipeData
                {
                    ID = int.Parse(recipeRow[0]),
                    Title = recipeRow[1],
                    Description = recipeRow[2],
                    Ingredients = recipeRow[3],
                    StepsID = recipeRow[4]
                };
                result.Add(recipeData);
            }

            return result;
        }

        protected override List<StepData> GetStepsData()
        {
            List<StepData> result = new List<StepData>();
            foreach (string[] stepRow in stepsTable)
            {
                StepData stepData = new StepData
                {
                    ID = int.Parse(stepRow[0]),
                    Description = stepRow[1]
                };
                result.Add(stepData);
            }

            return result;
        }
    }
}
