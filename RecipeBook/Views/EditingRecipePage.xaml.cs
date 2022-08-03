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
                editingRecipe = DataStore.GetRecipeById(int.Parse(value)) ?? new Recipe();

                if (editingRecipe.ID == 0)
                {
                    Title = "Новый рецепт";
                    ToolbarItems.RemoveAt(0);
                }
                
                BindingContext = editingRecipe;
            }
        }

        public EditingRecipePage()
        {
            InitializeComponent();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            DataStore.AddOrUpdateRecipe(editingRecipe);
            DataStore.SaveData();

            await Shell.Current.GoToAsync($"..?{nameof(RecipePage.SetRecipeByIdString)}={editingRecipe.ID}");
        }


        private async void OnChangeRecipeImageButtonClicked(object sender, EventArgs e)
        {
            string pickPhotoMethod = await DisplayActionSheet("Сменить фото", "Отмена", null, "Сделать фото", "Выбрать фото");
            FileResult photo = null;
            switch (pickPhotoMethod)
            {
                case "Сделать фото":
                    photo = await MediaPicker.CapturePhotoAsync();
                    break;
                case "Выбрать фото":
                    photo = await MediaPicker.PickPhotoAsync();
                    break;
                default:
                    return;
            }

            if (photo != null)
            {
                editingRecipe.LoadedImagePath = photo.FullPath;
            }
        }

        private async void OnChangeStepImageButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            Step step = (Step)button.CommandParameter;

            string pickPhotoMethod = await DisplayActionSheet("Сменить фото", "Отмена", null, "Сделать фото", "Выбрать фото");
            FileResult photo = null;
            switch (pickPhotoMethod)
            {
                case "Сделать фото":
                    photo = await MediaPicker.CapturePhotoAsync();
                    break;
                case "Выбрать фото":
                    photo = await MediaPicker.PickPhotoAsync();
                    break;
                default:
                    return;
            }

            if (photo != null)
            {
                step.LoadedImagePath = photo.FullPath;
            }
        }

        private async void OnDeleteRecipeButtonClicked(object sender, EventArgs e)
        {
            bool accept = await DisplayAlert("Удаление", "Вы уверены?", "Удалить", "Отменить");
            if (!accept) return;

            DataStore.DeleteRecipeById(editingRecipe.ID);

            await Shell.Current.GoToAsync("../..");
        }

        private async void OnDeleteStepButtonClicked(object sender, EventArgs e)
        {
            bool accept = await DisplayAlert("Удаление", "Вы уверены?", "Удалить", "Отменить");
            if (!accept) return;

            var button = (Button)sender;
            Step step = (Step)button.CommandParameter;
            editingRecipe.Steps.Remove(step);
            //DataStore.Source.DeleteStepFromRecipeById(editingRecipe.ID, step.ID);
        }

        private async void OnAddStepButtonClicked(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                editingRecipe.Steps.Add(new Step());
                //DataStore.Source.AddOrUpdateStepsFromRecipe(editingRecipe);
            });
        }
    }
}