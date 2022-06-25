using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RecipeBook.Data;

namespace RecipeBook.Views
{
    [QueryProperty(nameof(SetRecipeIdAsString), nameof(SetRecipeIdAsString))]
    public partial class EditingRecipePage : ContentPage
    {
        public Recipe editingRecipe;

        public string SetRecipeIdAsString
        {
            set
            {
                Recipe recipe = DataStore.Source.GetRecipeById(int.Parse(value));
                editingRecipe = recipe == null ? null : new Recipe(recipe);
                BindingContext = editingRecipe;
            }
        }

        public EditingRecipePage()
        {
            InitializeComponent();

        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (editingRecipe == null)
            {
                //добавляем новый рецепт
                DataStore.Source.SaveOrUpdateRecipe(
                    new Recipe
                    {
                        ID = 0,
                        Image = "",
                        Title = titleEntry.Text,
                        Description = descriptionEntry.Text
                    });

                await Shell.Current.GoToAsync($"..");
            }
            else
            {
                //обновляем существующий
                DataStore.Source.SaveOrUpdateRecipe(
                    new Recipe
                    {
                        ID = editingRecipe.ID,
                        Image = editingRecipe.Image,
                        Title = titleEntry.Text,
                        Description = descriptionEntry.Text
                    });

                await Shell.Current.GoToAsync($"..?{nameof(RecipePage.SetRecipeByIdString)}={editingRecipe.ID}");
            }
        }
    }
}