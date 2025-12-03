using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.ViewComponents {
    public class ShoppingCartViewComponent : ViewComponent {

        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            var claimsIdentity = User?.Identity as ClaimsIdentity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(claim?.Value)) {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null) {
                    var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")?.Count() ?? 0;
                    HttpContext.Session.SetInt32(SD.SessionCart, count);
                }

                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else {
                HttpContext.Session.Clear();
                return View(0);
            }
        }

    }
}
