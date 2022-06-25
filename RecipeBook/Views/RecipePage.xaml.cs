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
    [QueryProperty(nameof(SetRecipeByIdString), nameof(SetRecipeByIdString))]
    public partial class RecipePage : ContentPage
    {
        public Recipe currentRecipe { get; set; }

        public string SetRecipeByIdString
        {
            set
            {
                Recipe recipe = DataStore.Source.GetRecipeById(int.Parse(value));
                currentRecipe = new Recipe(recipe);
                BindingContext = currentRecipe;
            }
        }

        public RecipePage()
        {
            InitializeComponent();

        }

        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(EditingRecipePage)}?{nameof(EditingRecipePage.SetRecipeIdAsString)}={currentRecipe.ID}");
        }

    }
}