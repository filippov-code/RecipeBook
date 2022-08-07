using RecipeBook.Models;
using RecipeBook.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace RecipeBook.Data
{
    public abstract class DataSource : IDataSource
    {
        private List<Recipe> recipes = new List<Recipe>();
        private List<Step> steps = new List<Step>();


        public DataSource()
        {
            Init();
        }

        protected virtual void Init()
        {
            InitSteps();
            InitRecipes();
        }

        private void InitSteps()
        {
            List<StepData> stepsData = GetStepsData();
            foreach (StepData stepData in stepsData)
            {
                Step step = new Step
                {
                    ID = stepData.ID,
                    Description = stepData.Description
                };
                steps.Add(step);
            }
        }

        private void InitRecipes()
        {
            List<RecipeData> recipesData = GetRecipesData();
            foreach (RecipeData recipeData in recipesData)
            {
                Recipe recipe = new Recipe(recipeData);

                try
                {
                    int[] stepsId = recipeData.StepsID.Trim().Split(',').Select(x => int.Parse(x)).ToArray();
                    recipe.Steps = new ObservableCollection<Step>( GetStepsByIds(stepsId) );
                    recipes.Add(recipe);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка при чтении рецепта: {ex.Message}");
                    continue;
                }
            }
        }

        public List<Recipe> GetRecipes()
        {
            return recipes;
        }

        public List<Step> GetSteps()
        {
            return steps;
        }

        protected List<Step> GetStepsByIds(params int[] stepsId)
        {
            List<Step> result = new List<Step>();
            foreach (int stepID in stepsId)
            {
                try
                {
                    result.Add( steps.Where(x=> x.ID == stepID).First() );
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Шага рецепта с ID:{stepID} нет. Ex.Message:{ex.Message}");
                    continue;
                }
            }

            return result;
        }

        protected abstract List<RecipeData> GetRecipesData();

        protected abstract List<StepData> GetStepsData();

        public abstract void Save(List<Recipe> recipes, List<Step> steps);
    }
}
