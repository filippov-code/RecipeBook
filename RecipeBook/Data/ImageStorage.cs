﻿using System;
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
        }

        private async static void SaveImageByNameAsync(string imageName, string imagePath)
        {
            if (!File.Exists(imagePath)) return;

            string savedImagePath = Path.Combine(imagesFolderPath, imageName + imageFormat);

            using (var readStream = File.OpenRead(imagePath))
            using (var writeStream = File.OpenWrite(savedImagePath))
                await readStream.CopyToAsync(writeStream);
        }

        private static bool ImageExistByName(string imageName)
        {
            return File.Exists(Path.Combine(imagesFolderPath, imageName + imageFormat));
        }

        private static string GetImagePathByName(string imageName)
        {
            if (ImageExistByName(imageName)) return Path.Combine(imagesFolderPath, imageName + imageFormat);
            else return "";
        }

        public static void SaveImageForRecipe(int recipeId, string imagePath)
        {
            SaveImageByNameAsync(recipePrefix + recipeId, imagePath);
        }

        public static bool RecipeImageExist(int recipeId)
        {
            return ImageExistByName(recipePrefix + recipeId);
        }

        public static string GetImagePathForRecipe(int recipeId)
        {
            string imageName = recipePrefix + recipeId;

            return GetImagePathByName(imageName);
        }

        public static void SaveImageForStep(int stepId, string imagePath)
        {
            SaveImageByNameAsync(stepPrefix + stepId, imagePath);
        }

        public static bool StepImageExist(int stepId)
        {
            return ImageExistByName(stepPrefix + stepId);
        }

        public static string GetImagePathForStep(int stepId)
        {
            string imageName = stepPrefix + stepId;

            return GetImagePathByName(imageName);
        }
    }
}
