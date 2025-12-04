using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{      
    [Area("Customer")]  
   // [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private static DateTime? SystemStartTime { get; set; }
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {

            if (SystemStartTime != null && (DateTime.Now - SystemStartTime.Value).TotalMinutes > 3)
            {
                SystemStartTime = DateTime.Now;
            }
            else
            {
                SystemStartTime = DateTime.Now;
            }
            //redcord sytem start time
            ViewBag.SystemStartTime = SystemStartTime;
            _logger = logger;
           _unitOfWork = unitOfWork;
           //TempData["Role"] = "尚未登入";
        }

        public IActionResult Index1()
        {
            ////2025.07.02 11:36 取得系統時間
            //ViewBag.SystemStartingTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // or any format you prefer 您本次開始使用時間
            //ViewBag.SystemTimeMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); //record the system time in milliseconds

            // System start time 2025 07 26 15:17
            if (SystemStartTime == null)
            {
                SystemStartTime = DateTime.Now;
            }
            //redcord sytem start time
            ViewBag.SystemStartTime = SystemStartTime;

            var builder = WebApplication.CreateBuilder();
            // Work_Duration is used for the duration of work in the system
            float? Work_Duration = builder.Configuration.GetSection("Work_Duration").Get<float>();        
            int? WORK_WARNING_SECONDS = builder.Configuration.GetSection("WORK_WARNING_SECONDS").Get<int>();

            // If Work_Duration is not set, use default value
            if (Work_Duration == null)
            {
                // Default value if not set in configuration
                Work_Duration = (float?)1.0; // 1 mins
            }
            ViewBag.Work_Duration = Work_Duration;         
            // Set ViewBag for WORK_WARNING_SECONDS for use in the view
            if (WORK_WARNING_SECONDS == null)
            {
                // Default value if not set in configuration
                WORK_WARNING_SECONDS = 60; // 60 seconds
            }
            ViewBag.WORK_WARNING_SECONDS = WORK_WARNING_SECONDS; // Default value will be null if not set

            // 系統更新: @ViewBag.PublishDate
            ViewBag.PublishDate = builder.Configuration.GetValue<string>("PublishDate");

            //2025.03.28 21:29 取得權限
            if (!User.IsInRole(SD.Role_Admin))
            {
                if (User.IsInRole(SD.Role_Company))
                {
                    TempData["Role"] = SD.Role_Company;
                }
                else if (User.IsInRole(SD.Role_Customer))
                {
                    TempData["Role"] = SD.Role_Customer;
                }
                else if (User.IsInRole(SD.Role_Employee))
                {
                    TempData["Role"] = SD.Role_Employee;
                }
                else
                {
                    TempData["Role"] = "尚未登入";
                }
            }
            else
            {
                TempData["Role"] = SD.Role_Customer;
            }

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,Company,ProductImages");

            // If a built SPA exists under wwwroot/spa, serve its index.html and embed server data
            try
            {
                var webRoot = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var spaIndexPath = System.IO.Path.Combine(webRoot, "spa", "index.html");
                if (System.IO.File.Exists(spaIndexPath))
                {
                    var html = System.IO.File.ReadAllText(spaIndexPath);

                    // Prepare a lightweight product DTO to serialize into the page
                    var productsForClient = productList.Select(p => new
                    {
                        id = p.Id,
                        title = p.Title,
                        company = p.Company?.Name,
                        image = p.ProductImages != null && p.ProductImages.Any() ? p.ProductImages.FirstOrDefault().ImageUrl : null
                    }).ToList();

                    var serverData = new
                    {
                        publishDate = ViewBag.PublishDate,
                        autoLogoutMinutes = ViewBag.AUTO_LOGOUT_MINUTE,
                        warningBeforeLogoutSeconds = ViewBag.WARNING_BEFORE_LOGOUT_SECOND,
                        workDurationMinutes = ViewBag.Work_Duration,
                        workWarningSeconds = ViewBag.WORK_WARNING_SECONDS,
                        systemStartTime = ViewBag.SystemStartTime,
                        role = TempData["Role"],
                        products = productsForClient
                    };

                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase };
                    var serverJson = System.Text.Json.JsonSerializer.Serialize(serverData, options);

                    var inject = $"<script>window.__INITIAL_SERVER_DATA__ = {serverJson};</script>";

                    // Insert just before closing </body>
                    if (html.Contains("</body>"))
                    {
                        html = html.Replace("</body>", inject + "</body>");
                    }
                    else
                    {
                        html += inject;
                    }

                    return Content(html, "text/html");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while attempting to serve SPA index.html");
            }

            // Fall back to the original MVC view if SPA not present
            return View(productList);
        }

        //public IActionResult Survey()
        //{
        //    List<SurveyResponse> objSurveyResponseList = _unitOfWork.SurveyResponse.GetAll().ToList();
        //    return View(objSurveyResponseList);
        //}
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new() {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,Company,ProductImages"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart) 
        {
            string strResult = string.Empty;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }
            shoppingCart.ApplicationUserId= userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId == userId &&
            u.ProductId==shoppingCart.ProductId);

            if (cartFromDb != null) {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                 strResult = _unitOfWork.Save();
            }
            else {
                //add cart record
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                strResult = _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["Success"] = "報名表內容-更新完成" + strResult;
            return RedirectToAction(nameof(Index1));
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
}