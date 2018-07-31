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

namespace FoodFinder.Controllers
{
    public class NewFoodTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewFoodTablesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: NewFoodTables
        public void Index()
        {

            List<NewFoodTable> foodList =_context.newFoodTable.ToList();

            foreach(NewFoodTable place in foodList)
            {
                Console.WriteLine(place.FoodName);
            }
            
        }

        // GET: NewFoodTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newFoodTable = await _context.newFoodTable.SingleOrDefaultAsync(m => m.Id == id);
            if (newFoodTable == null)
            {
                return NotFound();
            }

            return View(newFoodTable);
        }

        // GET: NewFoodTables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewFoodTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,FoodName,PicturesPath,Rating")] NewFoodTable newFoodTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newFoodTable);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(newFoodTable);
        }

        public void Create2([Bind("Id,Address,FoodName,PicturesPath,Rating")]NewFoodTable newFoodTable)
        {
            Console.WriteLine("The Object:" );
            Console.WriteLine("Name: " + newFoodTable.FoodName + " Address: " + newFoodTable.Address + 
                " Rating:" + newFoodTable.Rating.ToString() +"Picture: " +newFoodTable.PicturesPath);
            newFoodTable.Rating = Math.Ceiling(newFoodTable.Rating);
            if (ModelState.IsValid)
            {
                 
                
                _context.Add(newFoodTable);
                _context.SaveChanges();
                Console.WriteLine("Added");
            }
            
            
        }


        // GET: NewFoodTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newFoodTable = await _context.newFoodTable.SingleOrDefaultAsync(m => m.Id == id);
            if (newFoodTable == null)
            {
                return NotFound();
            }
            return View(newFoodTable);
        }

        public void Search(NewFoodTable searchstring)
        {
            var places = from p in _context.newFoodTable
                         select p;

            Console.WriteLine("Something: " + searchstring.FoodName + " Rating: " + searchstring.Rating);



            
   
            if (!String.IsNullOrEmpty(searchstring.FoodName))
            {
                places = places.Where(p => p.FoodName.Contains(searchstring.FoodName));
                
                if (places.Count() == 0)
                {
                    Create2(searchstring);
                }

                foreach (NewFoodTable table in places)
                {
                    Console.WriteLine(table.FoodName);
                
            }

        }

      }

        // POST: NewFoodTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,FoodName,PicturesPath,Rating")] NewFoodTable newFoodTable)
        {
            if (id != newFoodTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newFoodTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewFoodTableExists(newFoodTable.Id))
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
            return View(newFoodTable);
        }

        // GET: NewFoodTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newFoodTable = await _context.newFoodTable.SingleOrDefaultAsync(m => m.Id == id);
            if (newFoodTable == null)
            {
                return NotFound();
            }

            return View(newFoodTable);
        }

        // POST: NewFoodTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newFoodTable = await _context.newFoodTable.SingleOrDefaultAsync(m => m.Id == id);
            _context.newFoodTable.Remove(newFoodTable);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NewFoodTableExists(int id)
        {
            return _context.newFoodTable.Any(e => e.Id == id);
        }

    }
}
