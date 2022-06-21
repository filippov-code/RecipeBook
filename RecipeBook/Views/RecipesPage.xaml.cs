using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

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

            Recipes = new ObservableCollection<Recipe>{
                new Recipe
                {
                    ID = 1,
                    Image = "https://cs10.pikabu.ru/post_img/big/2018/08/18/7/1534590614195235309.png",
                    Title = "first recipe",
                    Description = "description of first recipe"
                },
                new Recipe
                {
                    ID = 2,
                    Image = "https://www.tourprom.ru/site_media/images/upload/2018/10/7/newsphoto/pinchos.jpg",
                    Title = "second recipe",
                    Description = "description of second recipe"
                },
                new Recipe
                {
                    ID = 3,
                    Image = "https://img.gazeta.ru/files3/33/8135033/eda1-pic905-895x505-299.jpg",
                    Title = "third recipe",
                    Description = "description of third recipe"
                }
            };

            BindingContext = this;
        }

        private async void OnRecipeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView collectionView = (CollectionView)sender;
            if (collectionView.SelectedItem == null) return;
            Recipe selectedRecipe = (Recipe)collectionView.SelectedItem;
            await Shell.Current.GoToAsync($"{nameof(RecipePage)}?{nameof(RecipePage.CurrentRecipeFromString)}={selectedRecipe.AsString()}");
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}