using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]

public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
        return View(objCoverTypeList);
    }

  

    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new Product(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        };
        if (id == null || id == 0)
        {
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
            return View(productVM);
        }

        else
        {
            return Notfound();

        }
        
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(CoverType obj)
    {
     
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cover Type updated successfully";
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

        var coverTypefromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        if (coverTypefromDb == null)
        {
            return Notfound();

        }
        return View(coverTypefromDb);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {

        if (id == null || id == 0)
        {
            return Notfound();
        }

        var coverTypefromDb = _unitOfWork.CoverType.GetFirstOrDefault(d => d.Id == id);
        if (coverTypefromDb == null)
        {
            return Notfound();

        }
        _unitOfWork.CoverType.Remove(coverTypefromDb);
        _unitOfWork.Save();
        TempData["success"] = "Cover Type deleted successfully";
        return RedirectToAction("Index");

    }



    private IActionResult Notfound()
    {
        throw new NotImplementedException();
    }

}

