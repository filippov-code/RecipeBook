using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RecipeBook.Data;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeBook.Views
{
    public partial class RecipesPage : ContentPage
    {
        public ObservableCollection<Recipe> Recipes { get; set; }


        public RecipesPage()
        {
            InitializeComponent();

            Recipes = DataStore.Source.GetAllRecipes();

            BindingContext = this;
        }

        private async void OnRecipeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView collectionView = (CollectionView)sender;
            if (collectionView.SelectedItem == null) return;
            Recipe selectedRecipe = (Recipe)collectionView.SelectedItem;
            await Shell.Current.GoToAsync($"{nameof(RecipePage)}?{nameof(RecipePage.SetRecipeByIdString)}={selectedRecipe.ID}");
            collectionView.SelectedItem = null;
        }

        private async void OnAddRecipeButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(EditingRecipePage)}?{nameof(EditingRecipePage.SetRecipeIdAsString)}=0");
        }
    }
}