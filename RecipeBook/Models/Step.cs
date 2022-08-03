using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using RecipeBook.Data;

namespace RecipeBook.Models
{
    public class Step : INotifyPropertyChanged
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSource)));
            }
        }

        public ImageSource ImageSource
        {
            get
            {
                if (!string.IsNullOrEmpty(loadedImagePath))
                    return loadedImagePath;

                string savedImagePath = ImageStorage.GetImagePathForStep(ID);
                if (string.IsNullOrEmpty(savedImagePath))
                    return ImageStorage.DefaultImage;
                else
                    return savedImagePath;
            }
        }

        public string Description { get; set; }


        public Step() 
        {
            ID = 0;
            Description = "Описание";
        }

        public Step(Step stepToCopy)
        {
            ID = stepToCopy.ID;
            LoadedImagePath = stepToCopy.LoadedImagePath;
            Description = stepToCopy.Description;
        }

        public StepData GetStepData()
        {
            return new StepData
            {
                ID = this.ID,
                Description = this.Description
            };
        }
    }
}
