using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using RecipeBook.Models;
using System.Diagnostics;

namespace RecipeBook.Data
{
    public class Mock : IDataSource
    {
        private ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();
        private ObservableCollection<Step> steps = new ObservableCollection<Step>();

        private string[][] recipesTable = new string[][]
        {
            new string[] {"1", "recipe_1", "first recipe", "description of first recipe", "1,2" },
            new string[] {"2", "", "second recipe", "description of second recipe", "3,4,5" },
            new string[] {"3", "", "third recipe", "deescription of third recipe", "6,7,8,9" }
        };

        private string[][] stepsTable = new string[][]
        {
            new string[] {"1", "", "1 step description"},
            new string[] {"2", "", "2 step description"},
            new string[] {"3", "", "3 step description"},
            new string[] {"4", "", "4 step description"},
            new string[] {"5", "", "5 step description"},
            new string[] {"6", "", "6 step description"},
            new string[] {"7", "", "7 step description"},
            new string[] {"8", "", "8 step description"},
            new string[] {"9", "", "9 step description"}
        };

        public Mock()
        {
            LoadData();
        }

        private void LoadData()
        {
            foreach (var stepArray in stepsTable)
            {
                int stepID;
                if (!int.TryParse(stepArray[0], out stepID)) continue;
                Step step = new Step
                {
                    ID = stepID,
                    Image = ImageStorage.GetImagePathForStep(stepID),
                    Description = stepArray[2]
                };
                steps.Add(step);
            }

            foreach (var recipeArray in recipesTable)
            {
                int recipeID;
                if (!int.TryParse(recipeArray[0], out recipeID)) continue;
                Recipe recipe = new Recipe
                {
                    ID = recipeID,
                    Image = ImageStorage.GetImagePathForRecipe(recipeID),
                    Title = recipeArray[2],
                    Description = recipeArray[3],
                    Steps = GetStepsById(recipeID)
                };
                recipes.Add(recipe);
            }
        }

        public ObservableCollection<Recipe> GetAllRecipes()
        {
            return recipes;
        }

        public ObservableCollection<Step> GetStepsById(int recipeId)
        {
            ObservableCollection<Step> result = new ObservableCollection<Step>();

            string[] stepsAsString = recipesTable.FirstOrDefault(x => x[0] == recipeId.ToString());
            if (stepsAsString == null) throw new ArgumentNullException("Рецепт с ID: {recipeId} не найден");
            string[] stepsIdAsStringArray = stepsAsString[4].Split(',');
            foreach (var stepIdAsString in stepsIdAsStringArray)
            {
                int stepID;
                if (!int.TryParse(stepIdAsString, out stepID)) continue;
                Step step = GetStepById(stepID);
                if (step == null)
                {
                    Debug.WriteLine($"Нужна синхронизация с бд. Запрашиваемый ID шага не найден ({stepID})");
                    continue;
                }
                result.Add(GetStepById(stepID));
            }
            return result;
        }

        public Recipe GetRecipeById(int id)
        {
            foreach (var recipe in recipes)
                if (recipe.ID == id) return new Recipe(recipe);

            return null;
        }

        public Step GetStepById(int id)
        {
            foreach (var step in steps)
                if (step.ID == id) return new Step(step);

            return null;
        }

        public void AddOrUpdateRecipe(Recipe recipe)
        {
            ImageStorage.SaveImageForRecipe(recipe.ID, recipe.Image);
            AddOrUpdateStepsFromRecipe(recipe);
            if (recipe.ID == 0)
            {
                //добавить новый рецепт
                recipe.ID = recipes.LastOrDefault().ID + 1;
                recipes.Add(recipe);
            }
            else
            {
                //обновить рецепт
                Recipe recipeForUpdate = recipes.First(x => x.ID == recipe.ID);
                recipes[recipes.IndexOf(recipeForUpdate)] = recipe;
            }
        }

        public void AddOrUpdateStepsFromRecipe(Recipe recipe)
        {
            foreach (var step in recipe.Steps)
            { 
                if (step.ID == 0)
                {
                    //добавляем новый шаг
                    step.ID = steps.LastOrDefault().ID + 1;
                    steps.Add(step);
                }
                else
                {
                    //обновляем старый шаг
                    Step stepForUpdate = steps.First(x => x.ID == step.ID);
                    steps[steps.IndexOf(stepForUpdate)] = step;
                }
            }
        }
    }
}
