using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The Name and the Display Order can not be the same");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        { 
            if(id==null||id==0)
            {
                return Notfound();
            }
       
            var categoryfromDb = _unitOfWork.Category.FindID(id);
            if(categoryfromDb==null)
            {
                return Notfound();

            }
            return View(categoryfromDb);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The Name and the Display Order can not be the same");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return Notfound();
            }

            var categoryfromDb = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id==id);
            if (categoryfromDb == null)
            {
                return Notfound();

            }
            return View(categoryfromDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {

            if (id == null || id == 0)
            {
                return Notfound();
            }

            var categoryfromDb = _unitOfWork.Category.GetFirstOrDefault(d=>d.Id == id);
            if (categoryfromDb == null)
            {
                return Notfound();

            }
            _unitOfWork.Category.Remove(categoryfromDb);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }



        private IActionResult Notfound()
        {
            throw new NotImplementedException();
        }

    }

