using Microsoft.AspNetCore.Mvc;
using Project_Equinox.Models.DomainModels;
using Project_Equinox.Models.Data.Repository;
using Project_Equinox.Models.Util;
using System.Linq; // needed for Any(), ToList()

namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    public class AdminCategoryController : Controller
    {
        private readonly IEquinoxRepository _repository;

        public AdminCategoryController(IEquinoxRepository repository)
        {
            _repository = repository;
        }

        // GET /Admin/Category and /Admin/Category/Index
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var categories = _repository.ClassCategories.List(new QueryOptions<ClassCategory>()).ToList();
            return View(categories);
        }


        // GET /Admin/Category/Edit or /Admin/Category/Edit/5
        [HttpGet("Edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            ClassCategory? model = id == null ? new ClassCategory() : _repository.ClassCategories.Get(id.Value);
            if (id != null && model == null) return NotFound();
            return View(model);
        }

        // POST /Admin/Category/Edit or /Admin/Category/Edit/5
        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ClassCategory category)
        {
            // Server-side uniqueness check (optional, but OK to keep)
            var nameOptions = new QueryOptions<ClassCategory>()
                .Filter(c => c.Name == category.Name && c.ClassCategoryId != category.ClassCategoryId);

            if (_repository.ClassCategories.List(nameOptions).Any())
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
            }

            if (!ModelState.IsValid)
            {
                TempData["ValidationError"] = "Please correct the errors below and try again.";
                return View(category);
            }

            TempData.Remove("ValidationError");

            if (category.ClassCategoryId == 0) _repository.ClassCategories.Add(category);
            else _repository.ClassCategories.Update(category);

            _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        // Prefer POST delete + anti-forgery; client does GET pre-check with CanDelete
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // Phase-4 business rule
            if (!_repository.CanDeleteClassCategory(id))
            {
                TempData["ErrorMessage"] = "Cannot delete this category because it has classes or bookings.";
                return RedirectToAction(nameof(Index));
            }

            var category = _repository.ClassCategories.Get(id);
            if (category == null) return NotFound();

            _repository.ClassCategories.Delete(category);
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
