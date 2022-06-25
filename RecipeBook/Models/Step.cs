using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models
{
    public class Step
    {
        public int ID { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }


        public Step() { }

        public Step(Step stepToCopy)
        {
            ID = stepToCopy.ID;
            Image = stepToCopy.Image;
            Description = stepToCopy.Description;
        }
    }
}
