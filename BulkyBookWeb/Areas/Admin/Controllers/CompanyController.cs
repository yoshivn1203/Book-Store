using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]


public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Company obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.Company.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }


    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return Notfound();
        }

        var obj = _unitOfWork.Company.FindID(id);
        if (obj == null)
        {
            return Notfound();

        }
        return View(obj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Company obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.Company.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company updated successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }


    private IActionResult Notfound()
    {
        throw new NotImplementedException();
    }


    #region API CALLS   
    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList = _unitOfWork.Company.GetAll();
        return Json(new { data = companyList });
    }

    [HttpDelete]

    public IActionResult Delete(int? id)
    {

        var obj = _unitOfWork.Company.GetFirstOrDefault(d => d.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });

        }
       
        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Successful" });


    }
    #endregion

}

