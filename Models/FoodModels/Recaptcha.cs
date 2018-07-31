using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodFinder.Models.FoodModels
{
    public class Recaptcha
    {
        
        public bool success { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }

    }
}
