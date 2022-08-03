using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using RecipeBook.Data;

namespace RecipeBook
{
    public partial class App : Application
    {
        public static string FilesFolderPath => Environment.GetFolderPath(Environment.SpecialFolder.Personal);


        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            DataStore.SaveData();
        }

        protected override void OnResume()
        {
        }

    }
}
