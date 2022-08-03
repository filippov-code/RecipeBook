using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using RecipeBook.Models;
using System.IO;

namespace RecipeBook.Data
{
    public class LocalDataBase : DataSource
    {
        private const string dbName = "database.db3";
        private SQLiteAsyncConnection connection;


        protected override void Init()
        {
            string dbPath = Path.Combine(App.FilesFolderPath, dbName);
            //if (File.Exists(dbPath)) File.Delete(dbPath);

            connection = new SQLiteAsyncConnection(dbPath);
            
                connection.CreateTableAsync<RecipeData>().Wait();
                connection.CreateTableAsync<StepData>().Wait();
            

            base.Init();
        }

        protected override List<RecipeData> GetRecipesData()
        {
            var result = connection.Table<RecipeData>().ToListAsync();
            result.Wait();
            return result.Result;
        }

        protected override List<StepData> GetStepsData()
        {
            var result = connection.Table<StepData>().ToListAsync();
            result.Wait();
            return result.Result;
        }

        public override void Save(List<Recipe> recipes, List<Step> steps)
        {
            foreach (Recipe recipe in recipes)
                connection.InsertOrReplaceAsync(recipe.GetRecipeData()).Wait();

            foreach (Step step in steps)
                connection.InsertOrReplaceAsync(step.GetStepData()).Wait();
        }
    }
}
