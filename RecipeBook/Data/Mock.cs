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
            new string[] {"1", "first recipe", "description of first recipe", "1,2" },
            new string[] {"2", "second recipe", "description of second recipe", "3,4,5" },
            new string[] {"3", "third recipe", "deescription of third recipe", "6,7,8,9" }
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
                    Title = recipeArray[1],
                    Description = recipeArray[2],
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

            string[] stepAsString = recipesTable.FirstOrDefault(x => x[0] == recipeId.ToString());
            if (stepAsString == null) throw new ArgumentNullException($"Рецепт с ID: {recipeId} не найден");
            string[] stepsIdAsStringArray = stepAsString[3].Split(',');
            foreach (var stepIdAsString in stepsIdAsStringArray)
            {
                int stepID;
                if (!int.TryParse(stepIdAsString, out stepID)) continue;
                Step step = steps.FirstOrDefault(x => x.ID == stepID);
                if (step == null)
                {
                    Debug.WriteLine($"Нужна синхронизация с бд. Запрашиваемый ID шага не найден ({stepID})");
                    continue;
                }
                result.Add(step);
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
            recipe = new Recipe(recipe);

            AddOrUpdateStepsFromRecipe(recipe);

            if (recipe.ID == 0)
            {
                AddNewRecipe(recipe);
            }
            else
            {
                ReplaceRecipe(recipe);
            }
        }

        private void AddOrUpdateStepsFromRecipe(Recipe recipe)
        {
            if (recipe.ID == 0)
            {
                foreach (var newStep in recipe.Steps)
                {
                    AddNewStep(newStep);
                }
            }
            else
            {
                Recipe originalRecipe = recipes.FirstOrDefault(x => x.ID == recipe.ID);
                if (originalRecipe == null) throw new Exception("Редактируемый рецепт не найден в данных");
                foreach (var oldStep in originalRecipe.Steps)
                {
                    bool thisStepDeleted = recipe.Steps.FirstOrDefault(x => x.ID == oldStep.ID) == null;
                    if (thisStepDeleted)
                    {
                        DeleteStep(oldStep);
                    }
                }
                foreach (var newStep in recipe.Steps)
                {
                    if (newStep.ID == 0)
                    {
                        AddNewStep(newStep);
                    }
                    else
                    {
                        ReplaceStep(newStep);
                    }
                }
            }
            
        }

        private int GetNextIdForRecipe()
        {
            Recipe lastRecipe = recipes.LastOrDefault();
            return lastRecipe == null ? 1 : lastRecipe.ID + 1;
        }

        private int GetNextIdForStep()
        {
            Step lastStep = steps.LastOrDefault();
            return lastStep == null ? 1 : lastStep.ID + 1;
        }

        private void AddNewRecipe(Recipe recipe)
        {
            recipe.ID = GetNextIdForRecipe();
            recipes.Add(recipe);

            if (!string.IsNullOrEmpty(recipe.LoadedImagePath)) 
                ImageStorage.SaveImageForRecipe(recipe.ID, recipe.LoadedImagePath);

            recipe.LoadedImagePath = "";
        }

        private void ReplaceRecipe(Recipe recipe)
        {
            Recipe recipeForUpdate = recipes.FirstOrDefault(x => x.ID == recipe.ID);
            if (recipeForUpdate == null) throw new Exception("Заменяемый рецепт не найден");
            recipes[recipes.IndexOf(recipeForUpdate)] = recipe;

            if (!string.IsNullOrEmpty(recipe.LoadedImagePath))
                ImageStorage.SaveImageForRecipe(recipe.ID, recipe.LoadedImagePath);

            recipe.LoadedImagePath = "";
        }

        public void DeleteRecipeById(int id)
        {
            Recipe recipeToDelete = recipes.FirstOrDefault(x => x.ID == id);
            if (recipeToDelete == null) throw new Exception("Попытка удалить несуществующий рецепт");

            foreach (var stepToDelete in recipeToDelete.Steps)
            {
                Step originalStep = steps.FirstOrDefault(x => x.ID == stepToDelete.ID);
                if (originalStep == null) throw new Exception("Попытка удалить несуществующий шаг");
                DeleteStep(originalStep);
            }
            recipes.Remove(recipeToDelete);

            ImageStorage.DeleteImageForRecipe(recipeToDelete.ID);
        }

        private void AddNewStep(Step step)
        {
            if (step.ID != 0) throw new Exception("В новом рецепте есть не новые шаги");

            step.ID = GetNextIdForStep();
            steps.Add(step);

            if (!string.IsNullOrEmpty(step.Image))
                ImageStorage.SaveImageForStep(step.ID, step.Image);
        }

        private void ReplaceStep(Step step)
        {
            Step originalStep = steps.FirstOrDefault(x => x.ID == step.ID);
            if (originalStep == null) throw new Exception("Заменяемый шаг не найден в данных");
            int index = steps.IndexOf(originalStep);
            steps[index] = step;

            if (!string.IsNullOrEmpty(step.Image))
                ImageStorage.SaveImageForStep(step.ID, step.Image);
        }

        private void DeleteStep(Step step)
        {
            Step stepToDelete = steps.FirstOrDefault(x => x.ID == step.ID);
            if (stepToDelete == null) throw new Exception("Попытка удалить несуществующий шаг");

            steps.Remove(stepToDelete);

            ImageStorage.DeleteImageForRecipe(stepToDelete.ID);
        }
    }
}
