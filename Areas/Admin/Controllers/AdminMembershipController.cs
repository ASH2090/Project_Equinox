using Microsoft.AspNetCore.Mvc;
using Project_Equinox.Models.Data.Repository;
using Project_Equinox.Models.DomainModels;

namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Membership")]
    public class AdminMembershipController : Controller
    {
        private readonly IEquinoxRepository _repository;
        public AdminMembershipController(IEquinoxRepository repository)
        {
            _repository = repository;
        }

        // GET: /Admin/Membership
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var memberships = _repository.Memberships.ListAll();
            return View(memberships);
        }

        // GET: /Admin/Membership/Create
        [HttpGet("Create")]
        public IActionResult Create() => View();

        // POST: /Admin/Membership/Create
        [HttpPost("Create")]
      
        public IActionResult Create(Membership membership)
        {
            if (!ModelState.IsValid) return View(membership);
            _repository.Memberships.Add(membership);
            _repository.Save();
            return RedirectToAction("Index");
        }


        // GET: /Admin/Membership/Edit or /Admin/Membership/Edit/5
        [HttpGet("Edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            Membership model = id == null ? new Membership() : _repository.Memberships.Get(id.Value);
            if (id != null && model == null) return NotFound();
            return View(model);
        }

        // POST: /Admin/Membership/Edit or /Admin/Membership/Edit/5
        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Membership membership)
        {
            if (!ModelState.IsValid) return View(membership);
            if (membership.MembershipId == 0) _repository.Memberships.Add(membership);
            else _repository.Memberships.Update(membership);
            _repository.Save();
            return RedirectToAction("Index");
        }

        // GET: /Admin/Membership/Delete/5
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var membership = _repository.Memberships.Get(id);
            if (membership == null) return NotFound();
            return View(membership);
        }

        // POST: /Admin/Membership/Delete/5
        [HttpPost("Delete/{id}")]
        
        public IActionResult DeleteConfirmed(int id)
        {
            var membership = _repository.Memberships.Get(id);
            if (membership == null) return NotFound();
            _repository.Memberships.Delete(membership);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}
