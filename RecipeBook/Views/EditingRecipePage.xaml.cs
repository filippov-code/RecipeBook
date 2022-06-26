using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RecipeBook.Data;
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
                DataStore.Source.SaveOrUpdateRecipe(new Recipe(editingRecipe) { ID = 0 });

                await Shell.Current.GoToAsync($"..");
            }
            else
            {
                //обновляем существующий
                DataStore.Source.SaveOrUpdateRecipe(new Recipe(editingRecipe));

                await Shell.Current.GoToAsync($"..?{nameof(RecipePage.SetRecipeByIdString)}={editingRecipe.ID}");
            }
        }


        private async void OnChangeImageButtonClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                string filesFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string imagesFolderPath = Path.Combine(filesFolderPath, "LoadedImages");
                if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }
                string imageName = $"recipe_{editingRecipe.ID}.png";
                string imagePath = Path.Combine(imagesFolderPath, imageName);

                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(imagePath))
                    await stream.CopyToAsync(newStream);

                editingRecipe.Image = imagePath;
                Debug.WriteLine(imagePath);
            }
            else
            {
                await DisplayAlert("Ошибка", "Изображение не выбрано", "Ок");
            }
        }
    }
}