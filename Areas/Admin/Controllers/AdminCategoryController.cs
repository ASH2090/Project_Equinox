using Microsoft.AspNetCore.Mvc;
using Project_Equinox.Models.DomainModels;
using Project_Equinox.Models.Data.Repository;
using Project_Equinox.Models.Util;
 
namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoryController : Controller
    {
        private readonly IEquinoxRepository _repository;
 
        public AdminCategoryController(IEquinoxRepository repository)
        {
            _repository = repository;
        }
 
        [Route("Admin/Category")]
        [Route("Admin/Category/Index")]
        public IActionResult Index()
        {
            var options = new QueryOptions<ClassCategory>();
            var categories = _repository.ClassCategories.List(options);
            return View(categories.ToList());
        }
 
        [Route("Admin/Category/Edit")]
        [Route("Admin/Category/Edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            ClassCategory model;
            if (id == null)
            {
                model = new ClassCategory();
            }
            else
            {
                model = _repository.ClassCategories.Get(id.Value);
                if (model == null)
                {
                    return NotFound();
                }
            }
            return View(model);
        }
 
        [HttpPost]
        [Route("Admin/Category/Edit")]
        [Route("Admin/Category/Edit/{id?}")]
        public IActionResult Edit(ClassCategory category)
        {
            // Server-side validation: Check for duplicate category name
            var nameOptions = new QueryOptions<ClassCategory>()
                .Filter(c => c.Name == category.Name && c.ClassCategoryId != category.ClassCategoryId);
            var existingCategories = _repository.ClassCategories.List(nameOptions);
           
            if (existingCategories.Any())
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
            }
 
            if (!ModelState.IsValid)
            {
                TempData["ValidationError"] = "Please correct the errors below and try again.";
                return View(category);
            }
 
            TempData.Remove("ValidationError");
 
            if (category.ClassCategoryId == 0)
            {
                _repository.ClassCategories.Add(category);
            }
            else
            {
                _repository.ClassCategories.Update(category);
            }
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }
 
        [Route("Admin/Category/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = _repository.ClassCategories.Get(id);
            if (category == null)
                return NotFound();
           
            _repository.ClassCategories.Delete(category);
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}