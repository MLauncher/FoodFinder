using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodFinder.Data;
using FoodFinder.Models.FoodModels;
using FoodFinder.Models;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.IO;
using System.Net;

using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace FoodFinder.Controllers
{
    public class NewReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        public NewReviewsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;   
        }

        // GET: NewReviews
        public async Task<IActionResult> Index()
        {
           
            return View(await _context.newReviews.ToListAsync());
        }

        public List<ReturnedReview> getReviews2(NewFoodTable table)
        {

            if (ModelState.IsValid)
            {

                Dictionary<int, ReturnedReview> myReplyHolder = new Dictionary<int, ReturnedReview>();
                //Check to see if the database contains the place
                var dplace = from p in _context.newFoodTable
                             where p.FoodName == table.FoodName
                             select p;
                NewFoodTable foundplace;
                //If not make the place
                if (dplace.Count() == 0)
                {
                    NewFoodTable newplace = table;
                    _context.Add(newplace);
                    foundplace = newplace;
                }
                else
                {
                    foundplace = dplace.First();
                  
                   
                }

               

                var nReviews = from r in _context.newReviews
                               join f2 in _context.newFoodTable on r.newFoodTable.Id equals f2.Id
                               join f3 in _context.newReviewReply on r.Id equals f3.newReview.Id
                               into myList
                               from x in myList.DefaultIfEmpty()
                               where f2.FoodName == foundplace.FoodName
                               select new
                               {
                                   r.Review1,
                                   r.Id,
                                   r.newUser.UserName,
                                   r.ReviewDate,
                                   r.Like,
                                   f2.FoodName,
                                   ReplyId = (x == null ? 0 : x.Id),
                                   Reply = (x == null ? String.Empty : x.reply),
                                   ReplyUser = (x == null ? String.Empty : x.newUser.UserName)
                                   
                               };


              

               
                

                List<ReturnedReview> newList = new List<ReturnedReview>();
                foreach (var item in nReviews.ToList())
                {
                    Console.WriteLine("FoodName: " + item.FoodName +
                        " User " + item.UserName + " Review " + item.Review1);

                    Console.WriteLine("Reply Id" + item.ReplyId);
                    ReturnedReview thisReview = new ReturnedReview();
                    thisReview.FoodName = item.FoodName;
                    thisReview.rContents = item.Review1;
                    thisReview.Username = item.UserName;
                    thisReview.rLike = item.Like;
                   
                    thisReview.rId = item.Id;
                    thisReview.rDate = item.ReviewDate.ToString();
              
                   
                   if( item.ReplyId != 0)
                    {

                        if (myReplyHolder.ContainsKey(item.Id))
                        {

                            Console.WriteLine("Found the key");
                            Console.WriteLine("The Review " + item.Review1);
                            ReturnedReview foundReply = myReplyHolder[item.Id];
                            ReturnedReply uReply = new ReturnedReply();

                            
                            uReply.user = item.ReplyUser;
                            uReply.rpContents = item.Reply;
                            thisReview.rpList = new List<ReturnedReply>();
                            thisReview.rpList.Add(uReply);
                            
                           
                            foundReply.rpList.Add(uReply);

                            foreach (var reply in foundReply.rpList)
                            {
                                Console.WriteLine("The Replies so far " + reply.rpContents);
                            }


                        }

                        else
                        {
                            Console.WriteLine("Adding new key");

                            ReturnedReply uReply = new ReturnedReply();
                            uReply.user = item.ReplyUser;
                            uReply.rpContents = item.Reply;

                          
                            thisReview.rpList = new List<ReturnedReply>();
                            thisReview.rpList.Add(uReply);

                            Console.WriteLine(uReply.rpContents);
                            myReplyHolder.Add(item.Id, thisReview);

                        }
                    }
                    else
                    {
                       
                        newList.Add(thisReview);
                    }
             

                    
                }

                foreach(var repItem in myReplyHolder)
                {
                    Console.WriteLine("Adding Review with Replies to List");
                    newList.Add(repItem.Value);
                }

                return newList;

            }
            else
            {
                List<ReturnedReview> emptyList = new List<ReturnedReview>();

                return emptyList;
            }

        }
        public async Task<List<NewReviews>> getReviews()
        {
           
            return await _context.newReviews.ToListAsync();
        }

 
     
        // GET: NewReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newReviews = await _context.newReviews.SingleOrDefaultAsync(m => m.Id == id);
            if (newReviews == null)
            {
                return NotFound();
            }

            return View(newReviews);
        }

        // GET: NewReviews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Review1")] NewReviews newReviews)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newReviews);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newReviews);
        }

  
        // GET: NewReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newReviews = await _context.newReviews.SingleOrDefaultAsync(m => m.Id == id);
            if (newReviews == null)
            {
                return NotFound();
            }
            return View(newReviews);
        }

        // POST: NewReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Review1")] NewReviews newReviews)
        {
            if (id != newReviews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newReviews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewReviewsExists(newReviews.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(newReviews);
        }

        // GET: NewReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newReviews = await _context.newReviews.SingleOrDefaultAsync(m => m.Id == id);
            if (newReviews == null)
            {
                return NotFound();
            }

            return View(newReviews);
        }

        // POST: NewReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newReviews = await _context.newReviews.SingleOrDefaultAsync(m => m.Id == id);
            _context.newReviews.Remove(newReviews);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NewReviewsExists(int id)
        {
            return _context.newReviews.Any(e => e.Id == id);
        }

        public async Task<ApplicationUser> getUser()
        {


            //var user = _userManager.GetUserName(HttpContext.User);
            var user =  await GetCurrentUserAsync();
            return user;
            

                
                
          
            

            
            
            
        }
        
        public void printUserName()
        {
            var user = getUser();

            Console.WriteLine("The User " + user.Result.UserName);
        }


        public async Task<List<NewReviews>> makeUserReview(NewFoodTable place, string content, string date,bool like, string captcha)
        {

            await IsValidCaptcha(captcha);
            if (ModelState.IsValid)
            {
                Console.WriteLine("The Review " + content);
                //Console.WriteLine("The Review like " + like);
                Console.WriteLine("The Review Date" + date);
                CultureInfo eng = new CultureInfo("en-US");
                DateTime newDate = DateTime.Parse(date);
                Console.WriteLine("New Review Date " + newDate);
                //Check to see if the database contains the place
                var dplace = from p in _context.newFoodTable
                             where p.FoodName == place.FoodName
                             select p;
                NewFoodTable foundplace;
                //If not make the place
                if (dplace.Count() == 0)
                {
                    NewFoodTable newplace = place;
                    _context.Add(newplace);
                    foundplace = newplace;
                }
                else
                {
                    foundplace = dplace.First();
                }





                //Gets the Current User
                var user = getUser().Result;

               
                //Create a new review

                NewReviews review = new NewReviews();
                review.Review1 = content;
                review.newUser = user;
                review.newFoodTable = foundplace;

              
                
                review.ReviewDate = newDate;
                review.Like = like;
                
                //Add it to the database
                _context.Add(review);
                await _context.SaveChangesAsync();
                return getFoodReviews(review.newFoodTable);
            }

            else
            {
                Console.WriteLine("Model State was not valid!");

                Console.WriteLine(place.FoodName);
                Console.WriteLine(place.Address);
                Console.WriteLine(place.PicturesPath);
                Console.WriteLine(place.Rating);
                
                Console.WriteLine("The Review " + content);
                //Console.WriteLine("The Review like " + like);
                Console.WriteLine("The Review Date" + date);
                DateTime newDate = DateTime.Parse(date);
                Console.WriteLine("New Review Date " + newDate);

                return getFoodReviews(place);






            }

        }

        public List<NewReviews> getFoodReviews(NewFoodTable place)
        {
            var freviews = from r in _context.newReviews
                           where r.Id == place.Id
                           select r;

            return freviews.ToList();
        }

        public async Task<bool> IsValidCaptcha(string resp)
        {

            using(var client = new HttpClient()) {


                try
                {
                    resp = resp.Replace(" ", String.Empty);
                    client.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify?secret=6LcX1RgUAAAAANIoqVQ8B-WArlrzJ7Lb6EpOFiwB&response=" +resp+ "&remoteip=user_ip_address");
                    string gResp = await client.GetStringAsync(client.BaseAddress);
                    Console.WriteLine("Google's Response: " + gResp);
                    //var response = await client.GetAsync("/posts");
                    //response.EnsureSuccessStatusCode(); // Throw in not success

                    //var stringResponse = await response.Content.ReadAsStringAsync();
                    var posts = JsonConvert.DeserializeObject<Recaptcha>(gResp);
                    
                    Console.WriteLine("Json Post");
                    
                   

                    Console.WriteLine($"Got " + posts.success + " " + posts.hostname + " " + posts.challenge_ts);
                   
                    return true;


                }

                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                    return false;
                }
            }
           
           
        }

        public void SearchandGet(NewFoodTable searchstring,string content)
        {



            //Task<ApplicationUser> user = GetCurrentUserAsync();
            //Console.WriteLine("The id of the current user " + user.Id);
            var place = from p in _context.newFoodTable
                        select p;

            
            
            place = place.Where(p => p.FoodName.Contains(searchstring.FoodName));

          

            NewFoodTable foundplace = place.First();
           
         
            //If the search query string is not empty
            if (!String.IsNullOrEmpty(searchstring.FoodName))
            {
                //Search for the place in the database

                Console.WriteLine("Searching for " + foundplace.Id + " in reviews" + foundplace.FoodName );
                var allreviews = from r in _context.newReviews
                                 where r.newFoodTable.Id == foundplace.Id
                                 select r;
        
                //No Reviews were found 
                if (allreviews.Count() == 0)
                {
                    Console.WriteLine("Not Found");
                    //r
                    //Create2(searchstring);
                }

                //Get the reviews from the database
                else
                {
                    Console.WriteLine("Found");
                    List<NewReviews> pReviews = allreviews.ToList();

                    foreach(NewReviews r in pReviews)
                    {
                        Console.WriteLine("The review " + r.Review1);
                    }
                }
           
            }

        }


        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            
            return _userManager.GetUserAsync(HttpContext.User);

        }

    }
}
