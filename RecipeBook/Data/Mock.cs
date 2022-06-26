using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using RecipeBook.Models;

namespace RecipeBook.Data
{
    public class Mock : IDataSource
    {
        private ObservableCollection<Recipe> recipes = new ObservableCollection<Recipe>();
        

        public Mock()
        {
            recipes = new ObservableCollection<Recipe>{
                new Recipe
                {
                    ID = 1,
                    Image = "/data/user/0/com.companyname.recipebook/files/LoadedImages/recipe_1.png",//"https://cs10.pikabu.ru/post_img/big/2018/08/18/7/1534590614195235309.png",
                    Title = "first recipe",
                    Description = "description of first recipe",
                    Steps =
                    {
                        new Step
                        {
                            ID = 1,
                            Image = "https://free-png.ru/wp-content/uploads/2020/12/1-ba49575b.png",
                            Description = "1 step of first recipe"
                        },
                        new Step
                        {
                            ID = 2,
                            Image = "https://dvemorkovki.ru/upload/iblock/2a9/pink_02.jpg",
                            Description = "2 step of first recipe"
                        }
                    }
                },
                new Recipe
                {
                    ID = 2,
                    Image = "https://www.tourprom.ru/site_media/images/upload/2018/10/7/newsphoto/pinchos.jpg",
                    Title = "second recipe",
                    Description = "description of second recipe",
                    Steps =
                    {
                        new Step
                        {
                            ID = 3,
                            Image = "https://free-png.ru/wp-content/uploads/2020/12/1-ba49575b.png",
                            Description = "1 step of second recipe"
                        },
                        new Step
                        {
                            ID = 4,
                            Image = "https://dvemorkovki.ru/upload/iblock/2a9/pink_02.jpg",
                            Description = "2 step of second recipe"
                        },
                        new Step
                        {
                            ID = 5,
                            Image = "https://leopony.com/media/games/cover/20190618-004028-886684_desktop_%D1%80%D0%B0%D1%81%D0%BA%D1%80%D0%B0%D1%81%D0%BA%D0%B8-3-1.jpg",
                            Description = "3 step of second recipe"
                        }
                    }
                },
                new Recipe
                {
                    ID = 3,
                    Image = "https://img.gazeta.ru/files3/33/8135033/eda1-pic905-895x505-299.jpg",
                    Title = "third recipe",
                    Description = "description of third recipe",
                    Steps =
                    {
                        new Step
                        {
                            ID = 6,
                            Image = "https://free-png.ru/wp-content/uploads/2020/12/1-ba49575b.png",
                            Description = "1 step of third recipe"
                        },
                        new Step
                        {
                            ID = 7,
                            Image = "https://dvemorkovki.ru/upload/iblock/2a9/pink_02.jpg",
                            Description = "2 step of third recipe"
                        },
                        new Step
                        {
                            ID = 8,
                            Image = "https://leopony.com/media/games/cover/20190618-004028-886684_desktop_%D1%80%D0%B0%D1%81%D0%BA%D1%80%D0%B0%D1%81%D0%BA%D0%B8-3-1.jpg",
                            Description = "3 step of third recipe"
                        },
                        new Step
                        {
                            ID = 9,
                            Image = "https://sharik.ru/images/elements_big/1502-2790_m1.jpg",
                            Description = "4 step of third recipe"
                        }
                    }
                }
            };
        }

        public ObservableCollection<Recipe> GetAllRecipes()
        {
            return recipes;
        }

        public Recipe GetRecipeById(int id)
        {
            try
            {
                return recipes.First(x => x.ID == id);
            }
            catch
            {
                return null;
            }
        }

        public void SaveOrUpdateRecipe(Recipe recipe)
        {
            if (recipe.ID == 0)
            {
                //сохранить новый рецепт
                recipe.ID = recipes.LastOrDefault().ID + 1;
                recipes.Add(recipe);
            }
            else
            {
                //обновить рецепт
                Recipe recipeForUpdate = GetRecipeById(recipe.ID);
                recipes[recipes.IndexOf(recipeForUpdate)] = recipe;
            }
        }
    }
}
