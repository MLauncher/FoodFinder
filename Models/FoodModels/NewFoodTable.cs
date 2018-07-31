using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodFinder.Models.FoodModels
{
    public class NewFoodTable
    {
        public int Id { get; set; }
        public string FoodName { get; set; }
        public string Address { get; set; }
        public string PicturesPath { get; set; }
        public decimal Rating { get; set; }

        





        public virtual ICollection<NewReviews> newreviews { get; set; }
    }
}
