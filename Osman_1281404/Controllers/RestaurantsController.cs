using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using Osman_1281404.Models;
using Osman_1281404.ViewModels;

namespace Osman_1281404.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly RestaurantDbContext db;
        private readonly IWebHostEnvironment env;
        public RestaurantsController(RestaurantDbContext db, IWebHostEnvironment env) { this.db = db; this.env = env; }
        public IActionResult Index()
        {
            //Eager loading
            var data = db.Restaurants.Include(x=>x.Foods).ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            var model = new RestaurantInputModel();
            model.Foods.Add(new Food { });
            ViewBag.Foods = db.Customers.ToList();
            return View(model);
        }
        public IActionResult Aggregates()
        {
            return View(db.Foods.ToList());
        }
        public IActionResult Grouping()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Grouping(string groupby)
        {
            if (groupby == "type")
            {
                var data = db.Restaurants.GroupBy(x => x.RestaurantType).ToList().Select(g => new GroupedData<Restaurant> { Key = g.Key.ToString(), Items = g }).ToList();
                return View("GroupingResult", data);
            }
            else if (groupby == "year-month")
            {
                var data = db.Restaurants.ToList().GroupBy(x => new { x.OpeningDate.Value.Year, x.OpeningDate.Value.Month }).Select(g => new GroupedData<Restaurant> { Key = $"{g.Key.Month}/{g.Key.Year}", Items = g }).ToList();
                return View("GroupingResult", data);
            }
            return NoContent();
        }
        [HttpPost]
        public IActionResult Create(RestaurantInputModel model, string operation = "")
        {
            if (operation == "add")
            {
                model.Foods.Add(new Food { });
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                    item.RawValue = null;
                }
            }
            if (operation.StartsWith("remove"))
            {
                //int i = operation.IndexOf("_");
                //int index = int.Parse(operation.Substring(i+1));
                int index = int.Parse(operation.Substring(operation.IndexOf("_") + 1));
                model.Foods.RemoveAt(index);
                foreach (var item in ModelState.Values)
                {
                    item.Errors.Clear();
                    item.RawValue = null;
                }
            }
            if (operation == "insert")
            {
                if (ModelState.IsValid)
                {
                    var r = new Restaurant
                    {
                        RestaurantName = model.RestaurantName,
                        RestaurantType = model.RestaurantType,
                        OpeningDate = model.OpeningDate,
                        IsAvailable = model.IsAvailable,
                    };
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fk = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string filePath = Path.Combine(env.WebRootPath, "Pictures", fk);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    model.Picture.CopyTo(fs);
                    fs.Close();
                    r.Picture = fk;

                    db.Restaurants.Add(r);
                    db.SaveChanges();
                    foreach (var f in model.Foods)
                    {
                       
                        db.Database.ExecuteSqlInterpolated($"EXECUTE InsertFood {f.FoodName}, {f.Price}, {r.RestaurantId}");
                    }
                    return RedirectToAction("Index");
                }


            }
            ViewBag.Foods = db.Customers.ToList();
            return View(model);
        }
        public IActionResult Delete(int id)
        {

            if (!db.Restaurants.Any(x => x.RestaurantId == id)) return NotFound();
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteRestaurant {id}");
            return Json(new { success = true });
        }
    }
}
