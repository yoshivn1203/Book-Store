using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin

{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;


        public UserController(
           UserManager<IdentityUser> userManager,
           RoleManager<IdentityRole> roleManager,
           IUnitOfWork unitOfWork,
            ApplicationDbContext db)

        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _db = db;
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangeRole(string id)
        {
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            user.RoleList = _db.Roles.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Name
            });

            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ApplicationUser.Update(user);

                var userRole = _db.UserRoles.FirstOrDefault(u => u.UserId == user.Id);
                if (userRole != null)
                {
                    var previousRoleName = _db.Roles.Where(u => u.Id == userRole.RoleId).Select(u => u.Name).FirstOrDefault();
                    var newRoleName = _db.Roles.FirstOrDefault(u => u.Name == user.Role).Name;

                    await _userManager.RemoveFromRoleAsync(user, previousRoleName);
                    await _userManager.AddToRoleAsync(user, newRoleName);


                }

                _unitOfWork.Save();
                TempData["success"] = "User updated successfully";
                return RedirectToAction("Index");
            }


            return View(user);

        }


        #region API CALLS  


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _unitOfWork.ApplicationUser.GetAll();
            return Json(new { data = users });
        }

        [HttpDelete]
        public IActionResult Delete(string? id)
        {

            var obj = _unitOfWork.ApplicationUser.GetFirstOrDefault(d => d.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

            }

            _unitOfWork.ApplicationUser.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });


        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var obj = _unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if(obj.LockoutEnd!=null && obj.LockoutEnd > DateTime.Now)
            {
                obj.LockoutEnd = DateTime.Now;
            }
            else
            {
                obj.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful" });
        }


        #endregion







    }
}
