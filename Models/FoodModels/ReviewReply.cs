using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodFinder.Models.FoodModels
{
    public class ReviewReply
    {
        public int Id { get; set; }
        public string reply { get; set; }
        public bool agree { get; set; }


        public virtual ApplicationUser newUser { get; set; }
        public virtual NewReviews newReview { get; set; }
    }
}
