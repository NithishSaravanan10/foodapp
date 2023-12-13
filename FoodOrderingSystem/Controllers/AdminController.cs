using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FoodOrderingSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Threading;
using Microsoft.EntityFrameworkCore;


namespace FoodOrderingSystem.Controllers;

public class AdminController : Controller
{
    // private readonly ILogger<HomeController> _logger;
    // private readonly FoodApp _database;
    // public AdminController(FoodApp database)
    // {
    //    _database=database; 
    // }


    // public AdminController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }

     public IActionResult Index()
    {
        return View();
    }


    // public IActionResult Feedbacks()
    // {
    //     DatabaseConnection databaseConnection = new DatabaseConnection();
    //     IEnumerable feedback = databaseConnection.FeedBackDetails();
    //     return View(feedback);
    // }

    public IActionResult RegisteredUsers()
    {
        DatabaseConnection databaseConnection = new DatabaseConnection();
        IEnumerable userdetails = databaseConnection.UserDetails();
        return View(userdetails);
    }

    public IActionResult ViewOrders()
    {
        DatabaseConnection databaseConnection = new DatabaseConnection();
        IEnumerable adminOrderHistory = databaseConnection.AdminOrder();
        return View(adminOrderHistory);
    }

    

     public IActionResult ManageMenu()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AddItems()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddItems(AddItems items)
    {
        DatabaseConnection databaseConnection = new DatabaseConnection();
        databaseConnection.AddItem(items);
        return RedirectToAction("ManageMenu","Admin");
    }

    [HttpGet]
    public IActionResult DeleteItems()
    {
        return View();
    }

     [HttpPost]
    public IActionResult DeleteItems(string ItemName)
    {
        DatabaseConnection databaseConnection = new DatabaseConnection();
        databaseConnection.DeleteItem(ItemName);
        return RedirectToAction("ManageMenu","Admin");
    }
   
    [HttpGet]
     public IActionResult UpdateItems()
    {
        return View();
    }
   [HttpPost]
    public IActionResult UpdateItems(string ItemName,string ItemPrice,string ItemImage)
    {
        DatabaseConnection databaseConnection = new DatabaseConnection();
        databaseConnection.UpdateItem(ItemName,ItemPrice,ItemImage);
        return RedirectToAction("ManageMenu","Admin");
    }

// public IActionResult ViewOrders(string sortOrder, string filterLocation, string filterDate)
// {
//     DatabaseConnection databaseConnection = new DatabaseConnection();
//     IEnumerable adminOrderHistory = databaseConnection.AdminOrder();
    
//     // Sorting
//     ViewBag.UserNameSort = String.IsNullOrEmpty(sortOrder) ? "UserNameDesc" : "";
//     ViewBag.ItemNameSort = sortOrder == "ItemName" ? "ItemNameDesc" : "ItemName";
//     ViewBag.QuantitySort = sortOrder == "Quantity" ? "QuantityDesc" : "Quantity";
//     ViewBag.ItemPriceSort = sortOrder == "ItemPrice" ? "ItemPriceDesc" : "ItemPrice";
//     ViewBag.TotalSort = sortOrder == "Total" ? "TotalDesc" : "Total";
//     ViewBag.LocationSort = sortOrder == "Location" ? "LocationDesc" : "Location";
//     ViewBag.DateTimeSort = sortOrder == "DateTime" ? "DateTimeDesc" : "DateTime";
    
//     switch (sortOrder)
//     {
//         case "UserNameDesc":
//             adminOrderHistory = adminOrderHistory.OrderByDescending(a => a.UserName);
//             break;
//         case "ItemName":
//             adminOrderHistory = adminOrderHistory.OrderBy(a => a.ItemName);
//             break;
//         case "ItemNameDesc":
//             adminOrderHistory = adminOrderHistory.OrderByDescending(a => a.ItemName);
//             break;
//         case "Quantity":
//             adminOrderHistory = adminOrderHistory.OrderBy(a => a.Quantity);
//             break;
//         case "QuantityDesc":
//             adminOrderHistory = adminOrderHistory.OrderByDescending(a => a.Quantity);
//             break;
//         case "ItemPrice":
//             adminOrderHistory = adminOrderHistory.OrderBy(a => a.ItemPrice);
//             break;
//         case "ItemPriceDesc":
//             adminOrderHistory = adminOrderHistory.OrderByDescending(a => a.ItemPrice);
//             break;
//         case "Total":
//             adminOrderHistory = adminOrderHistory.OrderBy(a => a.Total);
//             break;
//         case "TotalDesc":
//             adminOrderHistory = adminOrderHistory.OrderByDescending(a => a.Total);
//             break;
//         case "Location":
//             adminOrderHistory = adminOrderHistory.OrderBy(a => a.Location);
//             break;
//         case "LocationDesc":
//             adminOrderHistory = adminOrderHistory.OrderByDescending(a => a.Location);
//             break;
//         case "DateTime":
//             adminOrderHistory = adminOrderHistory.OrderBy(a => a.DateTime);
//             break;
//         case "DateTimeDesc":
//             adminOrderHistory = adminOrderHistory.OrderByDescending(a => a.DateTime);
//             break;
//         default:
//             adminOrderHistory = adminOrderHistory.OrderBy(a => a.UserName);
//             break;
//     }
    
//     // Filtering
//     if (!String.IsNullOrEmpty(filterLocation))
//     {
//         adminOrderHistory = adminOrderHistory.Where(a => a.Location == filterLocation);
//     }
//     if (!String.IsNullOrEmpty(filterDate))
//     {
//         adminOrderHistory = adminOrderHistory.Where(a => a.DateTime.StartsWith(filterDate));
//     }
    
//     return View(adminOrderHistory);
// }
public async Task<IActionResult> GetFeedbacks()
    {

        // if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        // {
        //     ViewBag.Session = HttpContext.Session.GetString("Session");
            HttpClient httpClient = new HttpClient();
            string apiUrl = ("http://localhost:5278/api/Rating");

            var responseofapi = httpClient.GetAsync(apiUrl).Result;

            var listoffeedback = responseofapi.Content.ReadAsAsync<IEnumerable<FeedBack>>().Result;
            return View(listoffeedback);
    //     }
    //     return RedirectToAction("Index", "Home");
     }

}

