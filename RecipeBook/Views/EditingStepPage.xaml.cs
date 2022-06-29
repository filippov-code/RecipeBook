using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeBook.Models;
using RecipeBook.Data;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace RecipeBook.Views
{
    [QueryProperty(nameof(SetStepIdAsString), nameof(SetStepIdAsString))]
    public partial class EditingStepPage : ContentPage
    {
        private Step editingStep;
        public string SetStepIdAsString
        {
            set
            {
                editingStep = DataStore.Source.GetStepById(int.Parse(value)) ?? new Step();

                BindingContext = editingStep;
            }
        }

        public EditingStepPage()
        {
            InitializeComponent();
        }

        private async void OnChangeStepImageButtonClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.PickPhotoAsync();

            if (photo != null) 
                editingStep.Image = photo.FullPath;
            else 
                await DisplayAlert("Ошибка", "Изображение не выбрано", "Ок");
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            //DataStore.Source.SaveOrUpdateStep(editingStep);
        }
    }
}