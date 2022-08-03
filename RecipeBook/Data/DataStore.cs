using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using RecipeBook.Models;
using System.Diagnostics;

namespace RecipeBook.Data
{
    public static class DataStore
    {
        private static ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();
        private static ObservableCollection<Step> steps = new ObservableCollection<Step>();
        public static readonly IDataSource dataSource;

        static DataStore()
        {
            dataSource = new LocalDataBase();

            LoadData();
        }

        private static void LoadData()
        {
            recipes = new ObservableCollection<Recipe>( dataSource.GetRecipes() );
            steps = new ObservableCollection<Step>( dataSource.GetSteps() );
        }

        public static void SaveData()
        {
            dataSource.Save(recipes.ToList(), steps.ToList());
        }

        public static ObservableCollection<Recipe> GetAllRecipes()
        {
            return recipes;
            //return copy?
        }

        public static Recipe GetRecipeById(int id)
        {
            foreach (var recipe in recipes)
                if (recipe.ID == id) return new Recipe(recipe);

            return null;
        }

        public static Step GetStepById(int id)
        {
            foreach (var step in steps)
                if (step.ID == id) return new Step(step);

            return null;
        }

        public static void AddOrUpdateRecipe(Recipe recipe)
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

        private static void AddOrUpdateStepsFromRecipe(Recipe recipe)
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

        private static int GetNextIdForRecipe()
        {
            Recipe lastRecipe = recipes.LastOrDefault();
            return lastRecipe == null ? 1 : lastRecipe.ID + 1;
        }

        private static int GetNextIdForStep()
        {
            Step lastStep = steps.LastOrDefault();
            return lastStep == null ? 1 : lastStep.ID + 1;
        }

        private static int GetNextId(Collection<int> collection)
        {
            if (collection.Count == 0) return 1;

            collection.OrderBy(x => x); //
            for (int i = 1; i<collection.Count; i++)
            {
                if (collection[i] != collection[i] - 1) return collection[i] - 1;
            }

            return collection.Count + 1;
        }

        private static void AddNewRecipe(Recipe recipe)
        {
            recipe.ID = GetNextIdForRecipe();
            recipes.Add(recipe);

            if (!string.IsNullOrEmpty(recipe.LoadedImagePath))
                ImageStorage.SaveImageForRecipe(recipe.ID, recipe.LoadedImagePath);

            recipe.LoadedImagePath = "";
        }

        private static void ReplaceRecipe(Recipe recipe)
        {
            Recipe recipeForUpdate = recipes.FirstOrDefault(x => x.ID == recipe.ID);
            if (recipeForUpdate == null) throw new Exception("Заменяемый рецепт не найден");
            recipes[recipes.IndexOf(recipeForUpdate)] = recipe;

            if (!string.IsNullOrEmpty(recipe.LoadedImagePath))
                ImageStorage.SaveImageForRecipe(recipe.ID, recipe.LoadedImagePath);

            recipe.LoadedImagePath = "";
        }

        public static void DeleteRecipeById(int id)
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

        private static void AddNewStep(Step step)
        {
            if (step.ID != 0) throw new Exception("В новом рецепте есть не новые шаги");

            step.ID = GetNextIdForStep();
            steps.Add(step);

            if (!string.IsNullOrEmpty(step.LoadedImagePath))
                ImageStorage.SaveImageForStep(step.ID, step.LoadedImagePath);
        }

        private static void ReplaceStep(Step step)
        {
            Step originalStep = steps.FirstOrDefault(x => x.ID == step.ID);
            if (originalStep == null) throw new Exception("Заменяемый шаг не найден в данных");
            int index = steps.IndexOf(originalStep);
            steps[index] = step;

            if (!string.IsNullOrEmpty(step.LoadedImagePath))
                ImageStorage.SaveImageForStep(step.ID, step.LoadedImagePath);
        }

        private static void DeleteStep(Step step)
        {
            Step stepToDelete = steps.FirstOrDefault(x => x.ID == step.ID);
            if (stepToDelete == null) throw new Exception("Попытка удалить несуществующий шаг");

            steps.Remove(stepToDelete);

            ImageStorage.DeleteImageForStep(stepToDelete.ID);
        }
    }
}
