using Microsoft.AspNetCore.Mvc;
using Project_Equinox.Models.DomainModels;
using Project_Equinox.Models.Infrastructure;

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
            var foundCategory = _context.ClassCategories.Find(id);
            if (foundCategory == null)
            {
                return NotFound();
            }
            model = foundCategory;
        }
        return View(model);
    }

    [HttpPost]
    [Route("Edit")]
    [Route("Edit/{id?}")]
    public IActionResult Edit(ClassCategory category)
    {
        // Server-side validation: Check for duplicate category name
        var nameExists = _context.ClassCategories.Any(c => c.Name == category.Name && c.ClassCategoryId != category.ClassCategoryId);
        if (nameExists)
        {
            ModelState.AddModelError("Name", "A category with this name already exists.");
        }

        if (!ModelState.IsValid)
        {
            // Use TempData to coordinate with client-side validation
            TempData["ValidationError"] = "Please correct the errors below and try again.";
            return View(category);
        }

        // Clear any previous validation messages on successful validation
        TempData.Remove("ValidationError");

        if (category.ClassCategoryId == 0)
        {
            _context.ClassCategories.Add(category);
        }
        else
        {
            _context.ClassCategories.Update(category);
        }
        
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [Route("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var cat = _context.ClassCategories.Find(id);
        if (cat == null) return NotFound();
        _context.ClassCategories.Remove(cat);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
}
