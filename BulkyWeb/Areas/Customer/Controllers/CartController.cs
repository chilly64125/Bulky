using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers {

    [Area("customer")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CartController : Controller {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; } = new ShoppingCartVM();
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender) {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }


        public IActionResult Index() {

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            ShoppingCartVM = new() {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,includeProperties: "Product"),
                OrderHeader= new()
            };

            IEnumerable<ProductImage> productImages = _unitOfWork.ProductImage.GetAll();

            foreach (var cart in ShoppingCartVM.ShoppingCartList) {
                if (cart.Product != null) {
                    cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
                }
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            //2025.03.28 21:29 取得權限
            if (User.IsInRole(SD.Role_Admin))
            {
                TempData["Role"] = SD.Role_Admin;
            }
            else if (User.IsInRole(SD.Role_Company))
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
            return View(ShoppingCartVM);
        }

        public IActionResult Summary() {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            ShoppingCartVM = new() {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,includeProperties: "Product"),
                OrderHeader = new()
            };

            var appUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            ShoppingCartVM.OrderHeader.ApplicationUser = appUser;
            if (appUser != null) {
                ShoppingCartVM.OrderHeader.Name = appUser.Name;
                ShoppingCartVM.OrderHeader.PhoneNumber = appUser.PhoneNumber;
                ShoppingCartVM.OrderHeader.StreetAddress = appUser.StreetAddress;
                ShoppingCartVM.OrderHeader.City = appUser.City;
                ShoppingCartVM.OrderHeader.State = appUser.State;
                ShoppingCartVM.OrderHeader.PostalCode = appUser.PostalCode;
            } else {
                ShoppingCartVM.OrderHeader.Name = string.Empty;
                ShoppingCartVM.OrderHeader.PhoneNumber = string.Empty;
                ShoppingCartVM.OrderHeader.StreetAddress = string.Empty;
                ShoppingCartVM.OrderHeader.City = string.Empty;
                ShoppingCartVM.OrderHeader.State = string.Empty;
                ShoppingCartVM.OrderHeader.PostalCode = string.Empty;
            }



            foreach (var cart in ShoppingCartVM.ShoppingCartList) {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
		public IActionResult SummaryPOST() {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }
            string strResult = string.Empty;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,includeProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser? applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            if (applicationUser == null)
            {
                applicationUser = new ApplicationUser { CompanyId = 0, Email = string.Empty, Name = string.Empty, PhoneNumber = string.Empty, StreetAddress = string.Empty, City = string.Empty, State = string.Empty, PostalCode = string.Empty };
            }


			foreach (var cart in ShoppingCartVM.ShoppingCartList) {
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

            if (applicationUser.CompanyId.GetValueOrDefault() == 0) {
				//it is a regular customer 
				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
			}
            else {
				//it is a company user
				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
			}

            //2025.03.28 15:05
            if (ShoppingCartVM.ShoppingCartList.Count() > 0)
            {
                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                strResult = _unitOfWork.Save();
                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    OrderDetail orderDetail = new()
                    {
                        ProductId = cart.ProductId,
                        OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                        Price = cart.Price,
                        Count = cart.Count
                    };
                    _unitOfWork.OrderDetail.Add(orderDetail);
                    strResult = _unitOfWork.Save();
                }

                if (applicationUser.CompanyId.GetValueOrDefault() == 0)
                {
                    //it is a regular customer account and we need to capture payment
                    //stripe logic
                    var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                    var options = new SessionCreateOptions
                    {
                        SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                        CancelUrl = domain + "customer/cart/index",
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                    };

                    foreach (var item in ShoppingCartVM.ShoppingCartList)
                    {
                        var sessionLineItem = new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                        Name = item.Product?.Title ?? string.Empty
                                }
                            },
                            Quantity = item.Count
                        };
                        options.LineItems.Add(sessionLineItem);
                    }
                    var service = new SessionService();
                    Session session = service.Create(options);
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                    strResult = _unitOfWork.Save();
                    Response.Headers.Add("Location", session.Url);                 
                    return new StatusCodeResult(303);
                }

            }
            else
            {
                TempData["Success"] = "報名表-無參加活動,請回首頁!" + strResult;
                return RedirectToAction(nameof(Index));                             
            }
            HttpContext.Session.Clear();//reset ShoppingCart Quantity 2025.03.26 10:26
            return RedirectToAction(nameof(OrderConfirmation),new { id=ShoppingCartVM.OrderHeader.Id });
		}


        public IActionResult OrderConfirmation(int id) {
            string strResult = _unitOfWork.Save();
            OrderHeader? orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            if (orderHeader == null) {
                return View(id);
            }
            if(orderHeader.PaymentStatus!= SD.PaymentStatusDelayedPayment) {
                //this is an order by customer

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session?.PaymentStatus?.ToLower() == "paid") {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                     strResult = _unitOfWork.Save();
                }
                HttpContext.Session.Clear();

			}

            TempData["Success"] = "報名完成!" + strResult  ;
            var recipient = orderHeader.ApplicationUser?.Email;
            if (!string.IsNullOrEmpty(recipient))
            {
                _emailSender.SendEmailAsync(recipient, "報名表 - 台中市銀同碧湖陳氏宗親會",
                    $"<p>報名完成 - {orderHeader.Id}</p>");
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            strResult = _unitOfWork.Save();

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
                TempData["Role"] = SD.Role_Admin;
            }
            return View(id);
		}


		public IActionResult Plus(int cartId) {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb == null) return RedirectToAction(nameof(Index));
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId) {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb == null) return RedirectToAction(nameof(Index));
            if (cartFromDb.Count <= 1) {
                //remove that from cart
                
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            }
            else {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            string  strResult=_unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId) {
                        var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
                        if (cartFromDb == null) return RedirectToAction(nameof(Index));

                        _unitOfWork.ShoppingCart.Remove(cartFromDb);

                        HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
                            .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            string strResult = _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }



        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart) {
            if (shoppingCart.Count <= 50) {
                return shoppingCart.Product?.ListPrice ?? 0.0;
                //return shoppingCart.Product.Price;
            }
            else {
                if (shoppingCart.Count <= 100) {
                    return shoppingCart.Product?.ListPrice ?? 0.0;
                    //return shoppingCart.Product.Price50;
                }
                else {
                    return shoppingCart.Product?.ListPrice ?? 0.0;
                    //return shoppingCart.Product.Price100;
                }
            }
        }
    }
}
