using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;



namespace BulkyBookWeb.Controllers;



[Area("Customer")]

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;


    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index(string bookname, string category, string covertype, string orderBy)
    {
        ProductVM productVM = new()
        {
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Name
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Name
            }),
            ProductList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType").OrderBy(u=>u.ListPrice),

        };

        if (!String.IsNullOrEmpty(bookname))
        {
            productVM.ProductList = productVM.ProductList.Where(u => u.Title.Contains(bookname) || u.Title.ToLower().Contains(bookname));
            return View(productVM);
        }

        if (!String.IsNullOrEmpty(category))

        {
            productVM.ProductList = productVM.ProductList.Where(u=> u.Category.Name == category);
        
        }
        if (!String.IsNullOrEmpty(covertype))
        {
                productVM.ProductList = productVM.ProductList.Where(u => u.CoverType.Name == covertype);
        }
        if (!String.IsNullOrEmpty(orderBy))
        {
            if(orderBy == "Price: Low to High")
            {
                productVM.ProductList = productVM.ProductList.OrderBy(u=>u.Price100);
            }
            else if (orderBy == "Price: High to Low")
            {
                productVM.ProductList = productVM.ProductList.OrderByDescending(u => u.Price100);

            }
            else if (orderBy == "Name: Ascending")
            {
                productVM.ProductList = productVM.ProductList.OrderBy(u => u.Title);

            }
            else if (orderBy == "Name: Descending")
            {
                productVM.ProductList = productVM.ProductList.OrderByDescending(u => u.Title);

            }
        }
        return View(productVM);


        //IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        //if (String.IsNullOrEmpty(bookname))
        //{
        //    return View(productList);
        //}
        //else
        //{
        //    productList = productList.Where(u => u.Title.Contains(bookname) || u.Title.ToLower().Contains(bookname));
        //    return View(productList);
        //}
    }

    public IActionResult Details(int productId)
    {

        ShoppingCart cartObj = new()
        {
            Count = 1,
            ProductId = productId,
            Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperties: "Category,CoverType")
        };

        return View(cartObj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        shoppingCart.ApplicationUserId = claim.Value;

        ShoppingCart cartfromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
            u => u.ApplicationUserId == shoppingCart.ApplicationUserId && u.ProductId == shoppingCart.ProductId);

        if (cartfromDb == null)
        {
            _unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart,
                 _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
        }
        else
        {
            _unitOfWork.ShoppingCart.IncrementCount(cartfromDb, shoppingCart.Count);
            _unitOfWork.Save();

        }
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
