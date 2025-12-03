using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("customer")]  
    [Authorize(Roles = SD.Role_Admin)]
    public class EventRegistrationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventRegistrationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<EventRegistration> objEventRegistrationList = _unitOfWork.EventRegistration.GetAll(includeProperties: "Product").ToList();

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
            return View(objEventRegistrationList);
        }     

        public IActionResult Upsert(int? id)
        {
            EventRegistrationVM eventRegistrationVM = new()
            {
                //選擇報名活動 有籌備中的活動 HeldYN='Y' 2025.04.03
                ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Title + u.HeldYN,
                    Value = u.Id.ToString()
                }).Where(t => t.Text.Contains("Y")),
                EventRegistration = new EventRegistration()
            };
            if (id == null || id == 0)
            {
                //create
                return View(eventRegistrationVM);
            }
            else
            {
                //update
                eventRegistrationVM.EventRegistration = _unitOfWork.EventRegistration.Get(u => u.Id == id);
                return View(eventRegistrationVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(EventRegistrationVM eventRegistrationVM)
        {
            string sResult = string.Empty;

            if (ModelState.IsValid)
            {
                if (eventRegistrationVM.EventRegistration.Id == 0)
                {

                    //2025.04.05 15:54 檢查新增記錄是否已存在?
                    var eventRegistrationToBeInserted = _unitOfWork.EventRegistration.Get
                            (u => u.UserId.ToUpper() + u.ProductId ==
                            eventRegistrationVM.EventRegistration.UserId.ToUpper() + eventRegistrationVM.EventRegistration.ProductId);
                    if (eventRegistrationToBeInserted == null)  //不存在,可以新增
                    {
                        _unitOfWork.EventRegistration.Add(eventRegistrationVM.EventRegistration);
                    }
                    else  //否則,顯示"資料已新增過了! 請確認!"
                    {
                        sResult = "資料已新增過了! 請確認!!";
                        TempData["Success"] = sResult + "~ [系統通知] HttpPost] Upsert()";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    _unitOfWork.EventRegistration.Update(eventRegistrationVM.EventRegistration);
                }
                sResult = _unitOfWork.Save();

                TempData["Success"] = sResult + "~ [系統通知] [HttpPost] Upsert() "; ;
                return RedirectToAction("Index");


            }
            else
            {
                eventRegistrationVM.ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Title,
                    Value = u.Id.ToString()
                });
                return View(eventRegistrationVM);
            }
        }

        /// <summary>
        /// 2025.3.31 23:50 取得登入帳號
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public string getUserName(string user_name)
        {
            TempData["UserName"] = user_name;
            return "GetUserName, 報名記錄新增完成.";
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<EventRegistration> objEventRegistrationList = _unitOfWork.EventRegistration.GetAll(includeProperties: "Product").ToList();
            return Json(new { data = objEventRegistrationList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var eventRegistrationToBeDeleted = _unitOfWork.EventRegistration.Get(u => u.Id == id);
            if (eventRegistrationToBeDeleted == null)
            {
                return Json(new { success = false, message = "刪除失敗!" });
            }

            _unitOfWork.EventRegistration.Remove(eventRegistrationToBeDeleted);
            string strResult = _unitOfWork.Save();

            return Json(new { success = true, message = "[活動報名]刪除成功" });
        }

        #endregion
    }
}
