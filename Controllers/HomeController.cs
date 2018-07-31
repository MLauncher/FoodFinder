using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodFinder.Models;
using FoodFinder.Data;
using Microsoft.EntityFrameworkCore;
using FoodFinder.Models.ManageViewModels;
using FoodFinder.Models.FoodModels;

namespace FoodFinder.Controllers
{
    public class HomeController : Controller
    {
        
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult UserPage()
        {
            ViewData["Message"] = "Welcome to your page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public void  OnClick()
        {
            Console.WriteLine("Button was clicked");
        }

        [HttpPost]
        public void OnClick2(FormData phrase)
        {
            
            Console.WriteLine("Second Button " + phrase.data);
           
            Console.WriteLine("The phrase " + phrase.data);
        }

        public void search()
        {
            NewFoodTable table = new NewFoodTable();
            

            //var query = from m in context.newFoodTable
            //            select m;
            //if(query == null)
            //{
            //    Console.WriteLine("Query is null");
            //}

            //IQueryable<string> foodQuery = from m in context.newFoodTable
            //                                orderby m.FoodName
            //                                select m.FoodName;

            var FoodList = new NewFoodTable();
    
            List<NewFoodTable> foodList = new List<NewFoodTable>();
            List<ApplicationUser> users = new List<ApplicationUser>();
            //foodList = query.ToList();
            //users = query.ToList();
            foreach (ApplicationUser F in users)
            {
               // Console.WriteLine("The name is " + F.UserName);
            }
        }
    }
}
