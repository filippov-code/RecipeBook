using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeBook.Views
{
    [QueryProperty(nameof(CurrentRecipeFromString), nameof(CurrentRecipeFromString))]
    public partial class RecipePage : ContentPage
    {
        private Recipe currentRecipe;
        public Recipe CurrentRecipe 
        {
            get => currentRecipe;
            set
            {
                currentRecipe = value;
                BindingContext = currentRecipe;
            }
        }

        public string CurrentRecipeFromString
        {
            set => LoadRecipeFromString(value);
        }


        public RecipePage()
        {
            InitializeComponent();

            BindingContext = CurrentRecipe;
        }

        private void LoadRecipeFromString(string data)
        {
            string[] components = data.Split(',');
            CurrentRecipe = new Recipe
            {
                ID = int.Parse(components[0]),
                Image = components[1],
                Title = components[2],
                Description = components[3]
            };
        }

    }
}