using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class SessionController : Controller
    //{

    //    private readonly IUnitOfWork _unitOfWork;
    //    public SessionController(IUnitOfWork unitOfWork )
    //    {
    //        _unitOfWork = unitOfWork;

    //    }

    //    [HttpPost("Start")]
    //    public IActionResult StartSession()
    //    {
    //        var userId = User.Identity.Name ?? "Anonymous";
    //        var startTime = DateTime.UtcNow;

    //        // Save to DB: (pseudo-code)
    //        string result=SaveStartTime(userId, startTime, _unitOfWork);

    //        return Ok(new { startTime });
    //    }
    //    public string SaveStartTime(string userId, DateTime startTime, IUnitOfWork _unitOfWork)
    //    {            
    //        try
    //        {               
    //            ApplicationUser user = _unitOfWork.ApplicationUser.Get(u => u.UserName == userId);
    //            //已存在
    //            if (user != null)
    //            {
    //               user.StartTime= startTime;
    //                //do db_change:
    //                _unitOfWork.ApplicationUser.Update(user);
    //                string strResult = _unitOfWork.Save();
    //                return "OK,"+ strResult;
    //            }
    //            else
    //            {
    //                //不存在，新增
    //                ApplicationUser newUser = new ApplicationUser
    //                {
    //                    UserName = userId,
    //                    StartTime = startTime
    //                };
    //                _unitOfWork.ApplicationUser.Add(newUser);
    //                string strResult = _unitOfWork.Save();
    //                return "OK," + strResult;
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            // 可記錄 log
    //            return "Fail,"+ex.Message;
    //        }
    //    }
    //}


    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private static DateTime? SystemStartTime { get; set; }
        public UserController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager) {
            // System start time 2025 07 15 22:17
            if (SystemStartTime == null)
            {
                SystemStartTime = DateTime.Now;
            }
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;             
        }
        public IActionResult Index() 
        {         
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
            return View();
        }

        public IActionResult RoleManagment(string userId) {
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperties: "Company");
            if (user == null)
            {
                return NotFound();
            }

            RoleManagmentVM RoleVM = new RoleManagmentVM() {
                ApplicationUser = user,
                RoleList = _roleManager.Roles.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();
            RoleVM.ApplicationUser.Role = roles.FirstOrDefault();
            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagment(RoleManagmentVM roleManagmentVM) {

                var appUserId = roleManagmentVM.ApplicationUser?.Id;
                if (string.IsNullOrEmpty(appUserId)) return RedirectToAction("Index");

                ApplicationUser? applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == appUserId);
                if (applicationUser == null) return RedirectToAction("Index");

                string oldRole = _userManager.GetRolesAsync(applicationUser).GetAwaiter().GetResult().FirstOrDefault();

                if (!(roleManagmentVM.ApplicationUser.Role == oldRole)) {
                //a role was updated
                if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company) {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company) {
                    applicationUser.CompanyId = null;
                }
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();

                if (!string.IsNullOrEmpty(oldRole)) {
                    _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                }
                var newRole = roleManagmentVM.ApplicationUser.Role;
                if (!string.IsNullOrEmpty(newRole)) {
                    _userManager.AddToRoleAsync(applicationUser, newRole).GetAwaiter().GetResult();
                }

            }
            else {
                if(oldRole==SD.Role_Company && applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId) {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(applicationUser);
                    _unitOfWork.Save();
                }
            }

            return RedirectToAction("Index");
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();

            foreach(var user in objUserList) {

                user.Role=  _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

                if (user.Company == null) {
                    user.Company = new Company() {
                        Name = ""
                    };
                }
            }

            return Json(new { data = objUserList });
        }


        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {

            var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if (objFromDb == null) 
            {
                TempData["Success"] = "帳號 鎖定/開啟 成功";
                return Json(new { success = false, message = "帳號 鎖定/開啟 成功" });
            }

            if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now) {
                //user is currently locked and we need to unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.ApplicationUser.Update(objFromDb);
            string strResult = _unitOfWork.Save();
            TempData["Success"] = "帳號 變更-成功" + strResult;
            return Json(new { success = true, message = "帳號 變更-成功" });
        }

        #endregion
    }
}
