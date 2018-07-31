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

namespace FoodFinder.Controllers
{
    public class ReviewRepliesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        public ReviewRepliesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ReviewReplies
        public async Task<IActionResult> Index()
        {
            return View(await _context.newReviewReply.ToListAsync());
        }

        // GET: ReviewReplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewReply = await _context.newReviewReply.SingleOrDefaultAsync(m => m.Id == id);
            if (reviewReply == null)
            {
                return NotFound();
            }

            return View(reviewReply);
        }

        // GET: ReviewReplies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReviewReplies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,agree,reply")] ReviewReply reviewReply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reviewReply);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(reviewReply);
        }

        [HttpPost]
        public async Task<ReturnedReply> makeReply(string contents, bool agree, int reviewId )
        {
            

            ReviewReply reply = new ReviewReply();

            reply.agree = agree;
            reply.reply = contents;
            reply.newReview = getReview(reviewId);
            reply.newUser = getUser().Result;

            Console.WriteLine("The reply " + contents + " Liked or Disliked " + agree + "ReviewId" + reviewId);
            if (ModelState.IsValid)
            {
                _context.Add(reply);
                await _context.SaveChangesAsync();
                Console.WriteLine("Added a reply");

                ReturnedReply mapReply = new ReturnedReply();
                mapReply.likeordis = agree;
                mapReply.rpContents = contents;
                mapReply.user = reply.newUser.UserName;
                return mapReply;
                //var rpPlace = findPlace(place);
                
                //var uReplies = from r in _context.newReviews
                //               join rp in _context.newReviewReply on r.Id equals rp.newReview.Id
                //               where r.FoodName == foundplace.FoodName
                //               select new { f.Review1, f2.FoodName, f.newUser.UserName, f.ReviewDate, f.Like };
            }
            else {

                Console.WriteLine("Not a Valid Model State");
                ReturnedReply empty = new ReturnedReply();
                empty.rpContents = "Empty";
                return empty;
            }
            
        }

        public List<ReturnedReply> getReplies(int id)
        {
            var listOfReplies = from rp in _context.newReviewReply
                                join u in _context.newReviews on rp.newReview.Id equals u.Id
                                where u.Id == id
                                select new { rp.reply, rp.agree, rp.newUser.UserName };

            List<ReturnedReply> replies = new List<ReturnedReply>();

            foreach(var item in listOfReplies.ToList())
            {
                ReturnedReply reply = new ReturnedReply();
                reply.user = item.UserName;
                reply.likeordis = item.agree;
                reply.rpContents = item.reply;

                replies.Add(reply);
            }

            return replies;
        }

        // GET: ReviewReplies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewReply = await _context.newReviewReply.SingleOrDefaultAsync(m => m.Id == id);
            if (reviewReply == null)
            {
                return NotFound();
            }
            return View(reviewReply);
        }

        // POST: ReviewReplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,agree,reply")] ReviewReply reviewReply)
        {
            if (id != reviewReply.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviewReply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewReplyExists(reviewReply.Id))
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
            return View(reviewReply);
        }

        // GET: ReviewReplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewReply = await _context.newReviewReply.SingleOrDefaultAsync(m => m.Id == id);
            if (reviewReply == null)
            {
                return NotFound();
            }

            return View(reviewReply);
        }

        // POST: ReviewReplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviewReply = await _context.newReviewReply.SingleOrDefaultAsync(m => m.Id == id);
            _context.newReviewReply.Remove(reviewReply);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public NewFoodTable findPlace(NewFoodTable table)
        {
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
                return foundplace; 
            }
            else
            {
                foundplace = dplace.First();
                return foundplace;


            }
        }

        public NewReviews getReview(int id)
        {
            var rvTableList = from r in _context.newReviews
                         where id == r.Id
                         select r;

            return rvTableList.First();
           
        }
        private bool ReviewReplyExists(int id)
        {
            return _context.newReviewReply.Any(e => e.Id == id);
        }

        //Helpers

        public async Task<ApplicationUser> getUser()
        {


            //var user = _userManager.GetUserName(HttpContext.User);
            var user = await GetCurrentUserAsync();
            return user;




        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {

            return _userManager.GetUserAsync(HttpContext.User);

        }
    }
}
