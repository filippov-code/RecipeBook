using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace RecipeBook.Data
{
    [Table("Steps")]
    public class StepData
    {
        [PrimaryKey]
        public int ID { get; set; }

        public string Description { get; set; }
    }
}
