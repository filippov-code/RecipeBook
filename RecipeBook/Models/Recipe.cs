using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using RecipeBook.Models;
using System.Linq;
using System.ComponentModel;
using Xamarin.Forms;
using RecipeBook.Data;

namespace RecipeBook
{
    public class Recipe : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ID { get; set; }

        private string loadedImagePath;
        public string LoadedImagePath 
        {
            get => loadedImagePath;
            set
            {
                loadedImagePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadedImagePath)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                if (!string.IsNullOrEmpty(loadedImagePath)) 
                    return loadedImagePath;

                string savedImagePath = ImageStorage.GetImagePathForRecipe(ID);
                if (string.IsNullOrEmpty(savedImagePath)) 
                    return ImageStorage.DefaultImage;
                else 
                    return savedImagePath;
            }
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public ObservableCollection<Step> Steps { get; set; } = new ObservableCollection<Step>();


        public Recipe() 
        {
            ID = 0;
            Title = "Название";
            Description = "Описание";
            Ingredients = "Ингредиенты";
            Steps = new ObservableCollection<Step>() { new Step() };
        }

        public Recipe(Recipe recipeToCopy)
        {
            ID =  recipeToCopy.ID;
            LoadedImagePath = recipeToCopy.LoadedImagePath;
            Title = recipeToCopy.Title;
            Description = recipeToCopy.Description;
            Ingredients = recipeToCopy.Ingredients;
            Steps = new ObservableCollection<Step>(recipeToCopy.Steps.Select(x => new Step(x)));
        }

        public RecipeData GetRecipeData()
        {
            return new RecipeData
            {
                ID = this.ID,
                Title = this.Title,
                Description = this.Description,
                Ingredients = this.Ingredients,
                StepsID = string.Join(",", Steps.Select(x => x.ID.ToString()))
            };
        }
    }
}
