using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodFinder.Models.FoodModels
{
    public class NewReviews
    {

        public int Id { get; set; }
        public string Review1 { get; set; }
        public bool Like {get;set;}
        public DateTime ReviewDate { get; set; }
    


        public virtual NewFoodTable newFoodTable { get; set; }
        public virtual ApplicationUser newUser { get; set; }
        public virtual ICollection<ReviewReply> newReplies { get; set; }

    }
}
