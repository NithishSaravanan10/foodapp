using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FoodOrderingSystem.Models;
using System.Data;
using System.Collections;
using System.Threading;
using System.Dynamic;
using Newtonsoft.Json;
using System.Text;

namespace FoodOrderingSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
     private readonly ApplicationDbContext _database;
      public HomeController(ApplicationDbContext database)
    {
       _database=database; 
    }

  
    public IActionResult Index()
    {
        HttpContext.Session.SetString("UserName", "");
        return View();
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(LoginUser loginuser)
    {
        DatabaseConnection databaseConnection = new DatabaseConnection();
        string info = databaseConnection.loginValidation(loginuser);
        if(info=="admin")
        {  
         HttpContext.Session.SetString("UserName", loginuser.UserName);
           return RedirectToAction("Index","Admin");
        }
      
        if (info == "done")
        {
            HttpContext.Session.SetString("UserName", loginuser.UserName);
            ViewData["Info"] = "Login succesfull";
            return RedirectToAction("FoodPage", "Home");

        }

        ViewData["Info"] = "Invalid Credentials";
        return View("Login");
    }

[HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

[HttpPost]
    public IActionResult Contact(FeedBack user)
    {
       DatabaseConnection databaseConnection = new DatabaseConnection();
        string message = databaseConnection.FeedBackConnection(user);
         if (message == "ok")
        {
            ViewData["Mess"] = "Feedback submitted successfully";
            return View("Contact");
        }
        else if (message=="Invalid Password"){
            ViewData["Mess"] = "Give Proper Username and mail id";
            return View("Contact");

        }
        
         else if (message == "empty")
        {
            ViewData["Mess"] = "Username should not be empty";
        }
        return View("Contact");
    }



    [HttpGet]
    public IActionResult Registeration()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Registeration(User user)
    {

        
        DatabaseConnection databaseConnection = new DatabaseConnection();
        string message = databaseConnection.UserDataConnection(user);
        if (message == "ok")
        {
            ViewData["Notify"] = "User added successfully";
            return View("Login");
        }
        else if (message == "Invalid Password")
        {
            ViewData["Notify"] = "Credentials dosent meet the requirements";
            return View("Registeration");
        }
        else if (message == "Password dosen't match")
        {
            ViewData["Notify"] = "Password Mismatched";
            return View("Registeration");
        }
        else if (message == "empty")
        {
            ViewData["Notify"] = "Username should not be empty";
        }
        return View("Registeration");
    }



    public IActionResult ForgotPassword()
    {
        return View();
    }




    public IActionResult foodPage()
    {
        
        // if (!string.IsNullOrEmpty(HttpContext.Session.GetString("session")))
        // {
        //     ViewBag.Session = HttpContext.Session.GetString("session");
            DataTable datatable = DatabaseConnection.getMenu();

            // foreach (DataRow obj1 in datatable.Rows)
            // {
            //     //Console.WriteLine(obj1[2]);
            // }
            return View("foodPage", datatable);
            // return View();
        
        // return RedirectToAction("Login", "Home");


    }


    public IActionResult HomePage()
    {
        return View();
    }

    public IActionResult about()
    {
        return View();
    }

  

    // public IActionResult Privacy()
    // {
    //     return View();
    // }
    public IActionResult Logout()
    {
        
        HttpContext.Session.SetString("CouponDiscount", "0");
        return RedirectToAction("Index");
    }
    [HttpGet]
   public IActionResult CartPage()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")) && HttpContext.Session.GetString("UserName") != "Admin123")
        {
            string name = HttpContext.Session.GetString("UserName");
            ViewBag.UserName = name;
            DatabaseConnection databaseConnection = new DatabaseConnection();
            List<Cart> details = databaseConnection.FetchCartItems(name);
            if(!string.IsNullOrEmpty(HttpContext.Session.GetString("CouponStatus")) || HttpContext.Session.GetString("CouponStatus") == "true"){
                ViewBag.discount=HttpContext.Session.GetString("CouponDiscount");
            }
            else{
                ViewBag.discount = 0;
            }
            
            return View(details);
        }

        string? username = HttpContext.Session.GetString("UserName");
        ViewBag.UserName=username;
        
        return RedirectToAction("Login", "Home");
    }
    

    
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


   


    //Add Items to Cart
    public IActionResult AddToCart( string ItemName,string ItemPrice){
        //Check for Session
        if(HttpContext.Session.GetString("UserName") == ""){
            return RedirectToAction("Login","Home");
        }

        string? name = HttpContext.Session.GetString("UserName");


        DatabaseConnection databaseConnection = new DatabaseConnection();
        if(databaseConnection.AddToCart(ItemName,ItemPrice,name))
        {
            return RedirectToAction("foodPage","Home");
        }
        else{
             ViewData["Alert"] = "You have already added";
        }
        
     return RedirectToAction("foodPage");
    
    }

    public IActionResult DeleteCart(string ItemName){
         DatabaseConnection databaseConnection = new DatabaseConnection();
        databaseConnection.DeleteCart(ItemName);
        return RedirectToAction("CartPage");
    }

    public IActionResult UpdatePrice(string ItemName,int Quantity,string buttn)
    {
            DatabaseConnection databaseConnection = new DatabaseConnection();
            databaseConnection.UpdateQuantity(ItemName,HttpContext.Session.GetString("UserName"),Quantity,buttn);    
            return RedirectToAction("CartPage");
    }

    public IActionResult Payment()
    {
       ViewBag.Bay = HttpContext.Session.GetString("Bay");
       ViewBag.Total = HttpContext.Session.GetString("Total");
        return View();
    }

    public IActionResult SavePayment(IFormCollection form)
    {

        string Bay = form["Bay"];
        string Total = form["Total"];
        string Username = HttpContext.Session.GetString("UserName");

        //Fetch cart Items of particular user
        DatabaseConnection databaseConnection = new DatabaseConnection();
        IEnumerable<Cart> items =  databaseConnection.FetchCartItems(Username);
        //Add Cart Items to History table -->step2
        string id = Convert.ToString(DateTime.Now);
        databaseConnection.addOrderId(Username,id);
       foreach(var item in items){
            OrderHistory order = new OrderHistory();
            order.OrderId = id;
            order.UserName = Username;
            order.ItemName = item.ItemName;
            order.Quantity = item.Quantity;
            order.Total = Total;
            order.ItemPrice = item.ItemPrice;
            order.Location = Bay;
            //Adding in databse
            _database.OrderHistory.Add(order);
            //Save changes to database
            _database.SaveChanges();

       }
        //Delete all the items of cart for a particular User -->step3
        databaseConnection.DeleteCartItemsByName(Username);


        //Clearing the coupon Session
        HttpContext.Session.Remove("CouponStatus");
        HttpContext.Session.Remove("CouponDiscount");

        return RedirectToAction("finalpage","Home");
    }


    public IActionResult finalpage()
    {
        return View();
    }


    public IActionResult Pay(IFormCollection form){
        HttpContext.Session.SetString("Bay",form["Bay"]);
        HttpContext.Session.SetString("Total",form["Total"]);
        return RedirectToAction("Payment","Home");
    }

    public IActionResult ViewOrders()
    {
        string name = HttpContext.Session.GetString("UserName");
        ViewBag.UserName = name;
        DatabaseConnection databaseConnection = new DatabaseConnection();
        IEnumerable<OrderHistory> details = databaseConnection.ViewOrderHistoryofUser(name);
        IEnumerable<Orders> orders = databaseConnection.FetchOrders();

        dynamic mymodel = new ExpandoObject();
        mymodel.Details = details;
        mymodel.Orders = orders;

        return View(mymodel);
    }

     public IActionResult OrderReady()
    {
        return View();
    }

   public IActionResult Coupon(IFormCollection form)
    {

       if( HttpContext.Session.GetString("CouponStatus")=="true")
       {
        return RedirectToAction("CartPage","Home");
       }
        DatabaseConnection databaseConnection = new DatabaseConnection();
        string UserName = HttpContext.Session.GetString("UserName");
        string CouponCode = form["couponcode"];
        int total = Convert.ToInt32(form["total"]);

        int discount=databaseConnection.ApplyCoupon(CouponCode,UserName,total);
        Console.WriteLine(discount);
        TempData["discount"]= discount;
    
        HttpContext.Session.SetString("CouponStatus","true");
        HttpContext.Session.SetString("CouponDiscount",discount.ToString());

        return RedirectToAction("CartPage","Home");
    }
    [HttpGet]
    public IActionResult PutFeedback()
    {
        // if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        // {
            //  ViewBag.Session=HttpContext.Session.GetString("Session");
             return View();
        // }
        // return RedirectToAction("Index", "Home");
    }
    [HttpPost]
    public async Task<IActionResult> PutFeedback(FeedBack feedback)
    {
        // if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Session")))
        // {
        //     ViewBag.Session=HttpContext.Session.GetString("Session");
           
           
            HttpClient httpClient = new HttpClient();
            string apiUrl = "http://localhost:5278/api/Rating";

            var jsondata = JsonConvert.SerializeObject(feedback);

            var data1 = new StringContent(jsondata, Encoding.UTF8, "application/json");

            var httpresponse = httpClient.PostAsync(apiUrl, data1);
            var result = await httpresponse.Result.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            if (result == "true")
            {
                ViewBag.Message = "Feedback submited Successful";
                return View("PutFeedback");
            }
            return View("Index");
    //     }
    //     return RedirectToAction("Index", "Home");
     }
}

