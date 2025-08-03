using Microsoft.AspNetCore.Mvc;
using Project_Equinox.Models;

namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    public class AdminCategoryController : Controller
    {
        private readonly EquinoxContext _context;

        public AdminCategoryController(EquinoxContext context)
    {
        _context = context;
    }

    [Route("")]
    [Route("Index")]
    public IActionResult Index() => View(_context.ClassCategories.ToList());

    [Route("Edit")]
    [Route("Edit/{id?}")]
    public IActionResult Edit(int? id)
    {
        ClassCategory model;
        if (id == null)
        {
            model = new ClassCategory();
        }
        else
        {
            model = _context.ClassCategories.Find(id)!;
            if (model == null) return NotFound();
        }
        return View(model);
    }

    [HttpPost]
    [Route("Edit")]
    public IActionResult Edit(ClassCategory category)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please fix the error";
            return View(category);
        }

        if (category.ClassCategoryId == 0)
        {
            _context.ClassCategories.Add(category); // Create
        }
        else
        {
            _context.ClassCategories.Update(category); // Edit
        }
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [Route("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var cat = _context.ClassCategories.Find(id);
        if (cat == null) return NotFound();
        _context.ClassCategories.Remove(cat);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
}
