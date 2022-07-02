using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using System.Diagnostics;

namespace RecipeBook.Data
{
    public static class ImageStorage
    {
        private const string loadedImagesFolderName = "LoadedImages";
        private const string recipePrefix = "recipe_";
        private const string stepPrefix = "step_";
        private const string imageFormat = ".png";
        private static readonly string imagesFolderPath;

        public static ImageSource DefaultImage => ImageSource.FromResource("RecipeBook.Images.DefaultImage.png");


        static ImageStorage()
        {
            string filesFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            imagesFolderPath = Path.Combine(filesFolderPath, loadedImagesFolderName);

            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            /*
            var images = Directory.GetFiles(imagesFolderPath);
            foreach (var imagePath in images)
                File.Delete(imagePath);
            */
        }

        private async static void SaveImageByNameAsync(string imageName, string imagePath)
        {

            if (!File.Exists(imagePath)) throw new Exception("Путь изображения для сохранения не найден");

            string savedImagePath = Path.Combine(imagesFolderPath, imageName + imageFormat);

            if (imagePath == savedImagePath) return;

            using (var readStream = File.OpenRead(imagePath))
            using (var writeStream = File.OpenWrite(savedImagePath))
                await readStream.CopyToAsync(writeStream);
        }

        private static bool ImageExistByName(string imageName)
        {
            return File.Exists(Path.Combine(imagesFolderPath, imageName + imageFormat));
        }

        private static bool DeleteImageByName(string imageName)
        {
            string imageToDeletePath = Path.Combine(imagesFolderPath, imageName + imageFormat);

            if (!File.Exists(imageToDeletePath)) return false;

            File.Delete(imageToDeletePath);

            return true;
        }

        private static string GetImagePathByName(string imageName)
        {
            if (ImageExistByName(imageName)) 
                return Path.Combine(imagesFolderPath, imageName + imageFormat);
            else 
                return "";
        }

        public static void SaveImageForRecipe(int recipeId, string imagePath)
        {
            if (recipeId == 0) 
                throw new Exception("Попытка сохранения изобажения для рецепта по умолчанию");

            SaveImageByNameAsync(recipePrefix + recipeId, imagePath);
        }

        public static bool RecipeImageExist(int recipeId)
        {
            return ImageExistByName(recipePrefix + recipeId);
        }

        public static bool DeleteImageForRecipe(int recipeId)
        {
            return DeleteImageByName(recipePrefix + recipeId);
        }

        public static string GetImagePathForRecipe(int recipeId)
        {
            return GetImagePathByName(recipePrefix + recipeId);
        }

        public static void SaveImageForStep(int stepId, string imagePath)
        {
            if (stepId == 0) 
                throw new Exception("Попытка сохранения изображения для шага по умолчанию");

            SaveImageByNameAsync(stepPrefix + stepId, imagePath);
        }

        public static bool StepImageExist(int stepId)
        {
            return ImageExistByName(stepPrefix + stepId);
        }

        public static bool DeleteImageForStep(int stepID)
        {
            return DeleteImageByName(stepPrefix + stepID);
        }

        public static string GetImagePathForStep(int stepId)
        {
            return GetImagePathByName(stepPrefix + stepId);
        }
    }
}
