using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RecipeBook.Data;
using RecipeBook.Models;
using Xamarin.Essentials;
using System.Diagnostics;
using System.IO;

namespace RecipeBook.Views
{
    [QueryProperty(nameof(SetRecipeIdAsString), nameof(SetRecipeIdAsString))]
    public partial class EditingRecipePage : ContentPage
    {
        private Recipe editingRecipe;

        public string SetRecipeIdAsString
        {
            set
            {
                editingRecipe = DataStore.Source.GetRecipeById(int.Parse(value)) ?? new Recipe();
                
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
                DataStore.Source.AddOrUpdateRecipe(new Recipe(editingRecipe) { ID = 0 });

                await Shell.Current.GoToAsync($"..");
            }
            else
            {
                //обновляем существующий
                DataStore.Source.AddOrUpdateRecipe(new Recipe(editingRecipe));

                await Shell.Current.GoToAsync($"..?{nameof(RecipePage.SetRecipeByIdString)}={editingRecipe.ID}");
            }
        }


        private async void OnChangeRecipeImageButtonClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                editingRecipe.Image = photo.FullPath;
            }
            else
            {
                await DisplayAlert("Ошибка", "Изображение не выбрано", "Ок");
            }
        }

        private async void OnEditStepButtonClicked(object sender, EventArgs e)
        {
            //var button = (Button)sender;
        }

        private async void OnAddStepButtonClicked(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                editingRecipe.Steps.Add(new Step());
                DataStore.Source.AddOrUpdateStepsFromRecipe(editingRecipe);
            });
        }
    }
}