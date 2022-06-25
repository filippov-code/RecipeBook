using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeBook.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeBook
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));

            Routing.RegisterRoute(nameof(EditingRecipePage), typeof(EditingRecipePage));
        }
    }
}