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

        private string image;
        public string Image 
        {
            get => image;
            set
            {
                image = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
            }
        }

        public ImageSource ImageSource => string.IsNullOrEmpty(image) ? ImageStorage.DefaultImage : image;

        public string Title { get; set; }

        public string Description { get; set; }

        public ObservableCollection<Step> Steps { get; set; } = new ObservableCollection<Step>();


        public Recipe() 
        {
            ID = 0;
            Title = "Название";
            Description = "Описание";
            Steps = new ObservableCollection<Step>() { new Step() };
        }

        public Recipe(Recipe recipeToCopy)
        {
            ID =  recipeToCopy.ID;
            Image = recipeToCopy.Image;
            Title = recipeToCopy.Title;
            Description = recipeToCopy.Description;
            Steps = new ObservableCollection<Step>(recipeToCopy.Steps.Select(x => new Step(x)));
        }

    }
}
