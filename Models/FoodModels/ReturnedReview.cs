using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodFinder.Models.FoodModels
{
    public class ReturnedReview
    {
        public string FoodName;
        public string rContents;
        public int rId;
        public string Username;
        public bool rLike;
        public string rDate;

        public List<ReturnedReply> rpList { get; set; }
        
    }
}
