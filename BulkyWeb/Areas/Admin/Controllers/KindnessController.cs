using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.TermStore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Intrinsics.X86;
using static System.Collections.Specialized.BitVector32;
using WebApplication = Microsoft.Graph.Models.WebApplication;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    //懷恩塔-塔位管理 2025 05 15 12:01  KindnessPosition.cs
    [Area("Admin")]
 //   [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
    public class KindnessController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int? SelectedPoistionId { set; get; } //保存選擇的塔位 KindnessPositionID      
        private static string? SelectedName { set; get; } //保存選擇的祖先Name

        private static DateTime? SystemStartTime { get; set; }
        public KindnessController(IUnitOfWork unitOfWork)
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
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? KindnessPositionId)
        {
            ReadKindnessSetting(KindnessPositionId);
            List<KindnessPosition> objKindnessPositionList = _unitOfWork.Kindness.GetAll().ToList();
            //List<KindnessPosition> objKindnessPositionList = _unitOfWork.Kindness.GetAll(includeProperties: "ApplicationUser").ToList();
            //IEnumerable<KindnessPosition> objKindnessPositionList = _unitOfWork.Kindness.GetAll().Where(item=> item.PositionId.Length>0).ToList();

            return View(objKindnessPositionList);
        }

        /// <summary>
        /// (原本)預約:2025 05 16 16:39
        ///   塔位設定:2025 05 24 13:10
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Application(int? KindnessPositionId = 0 )
        {
            ReadKindnessSetting(KindnessPositionId);
            return DisplayKindnessObj(KindnessPositionId);
        }

        /// <summary>
        /// 預約 2025 05 16 16:39
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DisplayPosition(int? KindnessPositionId)
        {
            ReadKindnessSetting(KindnessPositionId);
            return DisplayKindnessObj(KindnessPositionId);
        }

        /// <summary>
        /// 2025 06 19:02
        /// 讀取塔位設定值: 樓層,區別,層數,編號等...
        /// </summary>
        /// <param name="KindnessPositionId"></param>
        private void ReadKindnessSetting(int? KindnessPositionId)
        {
            //record SystemStartTime
            ViewBag.SystemStartTime = SystemStartTime;
            SelectedPoistionId = KindnessPositionId; //保存選擇的塔位ID
            var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder();
            // 顯示 懷恩塔 塔位 的設定頁面 2025 05 22 16:13
            ViewBag.KindnessFloor = builder.Configuration.GetSection("Kindness:Floor").Get<int>();
            ViewBag.KindnessSection = builder.Configuration.GetSection("Kindness:Section").Get<int>();
            ViewBag.KindnessLevel_3f = builder.Configuration.GetSection("Kindness:Level_3f").Get<int>();
            ViewBag.KindnessLevel_1f_2f = builder.Configuration.GetSection("Kindness:Level_1f_2f").Get<int>();
            ViewBag.KindnessPosition = builder.Configuration.GetSection("Kindness:Position").Get<int>();

            // Kindness Layout_1F
            ViewBag.KindnessLayout_1F = builder.Configuration.GetSection("Kindness:Layout_1F").Get<string>();
            // Kindness Layout_2F
            ViewBag.KindnessLayout_2F = builder.Configuration.GetSection("Kindness:Layout_2F").Get<string>();
            // Kindness Layout_3F
            ViewBag.KindnessLayout_3F = builder.Configuration.GetSection("Kindness:Layout_3F").Get<string>();

            //1 樓: r 列數, c行數 : f1ar, f1ac, f1br, f1bc, f1cr, f1cc, f1dr, f1dc, f1er, f1ec, f1fr, f1fc
            ViewBag.kf1ar = builder.Configuration.GetSection("Kindness:kf1ar:row").Get<int>();
            ViewBag.kf1ac = builder.Configuration.GetSection("Kindness:kf1ac:col").Get<int>();

            ViewBag.kf1br = builder.Configuration.GetSection("Kindness:kf1br:row").Get<int>();
            ViewBag.kf1bc = builder.Configuration.GetSection("Kindness:kf1bc:col").Get<int>();

            ViewBag.kf1cr = builder.Configuration.GetSection("Kindness:kf1cr:row").Get<int>();
            ViewBag.kf1cc = builder.Configuration.GetSection("Kindness:kf1cc:col").Get<int>();

            ViewBag.kf1dr = builder.Configuration.GetSection("Kindness:kf1dr:row").Get<int>();
            ViewBag.kf1dc = builder.Configuration.GetSection("Kindness:kf1dc:col").Get<int>();

            ViewBag.kf1er = builder.Configuration.GetSection("Kindness:kf1er:row").Get<int>();
            ViewBag.kf1ec = builder.Configuration.GetSection("Kindness:kf1ec:col").Get<int>();

            ViewBag.kf1fr = builder.Configuration.GetSection("Kindness:kf1fr:row").Get<int>();
            ViewBag.kf1fc = builder.Configuration.GetSection("Kindness:kf1fc:col").Get<int>();

            //2 樓:  r 列數, c行數 : f2ar, f2ac, f2br, f2bc, f2cr, f2cc, f2dr, f2dc, f2er, f2ec, f2fr, f2fc
            ViewBag.kf2ar = builder.Configuration.GetSection("Kindness:kf2ar:row").Get<int>();
            ViewBag.kf2ac = builder.Configuration.GetSection("Kindness:kf2ac:col").Get<int>();

            ViewBag.kf2br = builder.Configuration.GetSection("Kindness:kf2br:row").Get<int>();
            ViewBag.kf2bc = builder.Configuration.GetSection("Kindness:kf2bc:col").Get<int>();

            ViewBag.kf2cr = builder.Configuration.GetSection("Kindness:kf2cr:row").Get<int>();
            ViewBag.kf2cc = builder.Configuration.GetSection("Kindness:kf2cc:col").Get<int>();

            ViewBag.kf2dr = builder.Configuration.GetSection("Kindness:kf2dr:row").Get<int>();
            ViewBag.kf2dc = builder.Configuration.GetSection("Kindness:kf2dc:col").Get<int>();

            ViewBag.kf2er = builder.Configuration.GetSection("Kindness:kf2er:row").Get<int>();
            ViewBag.kf2ec = builder.Configuration.GetSection("Kindness:kf2ec:col").Get<int>();

            ViewBag.kf2fr = builder.Configuration.GetSection("Kindness:kf2fr:row").Get<int>();
            ViewBag.kf2fc = builder.Configuration.GetSection("Kindness:kf2fc:col").Get<int>();

            //3 樓:  r 列數, c行數 : f3ar, f3ac, f3br, f3bc, f3cr, f3cc, f3dr, f3dc, f3er, f3ec, f3fr, f3fc
            ViewBag.kf3ar = builder.Configuration.GetSection("Kindness:kf3ar:row").Get<int>();
            ViewBag.kf3ac = builder.Configuration.GetSection("Kindness:kf3ac:col").Get<int>();

            ViewBag.kf3br = builder.Configuration.GetSection("Kindness:kf3br:row").Get<int>();
            ViewBag.kf3bc = builder.Configuration.GetSection("Kindness:kf3bc:col").Get<int>();

            ViewBag.kf3cr = builder.Configuration.GetSection("Kindness:kf3cr:row").Get<int>();
            ViewBag.kf3cc = builder.Configuration.GetSection("Kindness:kf3cc:col").Get<int>();

            ViewBag.kf3dr = builder.Configuration.GetSection("Kindness:kf3dr:row").Get<int>();
            ViewBag.kf3dc = builder.Configuration.GetSection("Kindness:kf3dc:col").Get<int>();

            ViewBag.kf3er = builder.Configuration.GetSection("Kindness:kf3er:row").Get<int>();
            ViewBag.kf3ec = builder.Configuration.GetSection("Kindness:kf3ec:col").Get<int>();

            ViewBag.kf3fr = builder.Configuration.GetSection("Kindness:kf3fr:row").Get<int>();
            ViewBag.kf3fc = builder.Configuration.GetSection("Kindness:kf3fc:col").Get<int>();

            //懷恩塔:各樓—區塔位數:
            //1 樓  甲區 4列編號(由下到上: row1,row2,....row4)
            //1 樓  1列   kf1a.row1
            ViewBag.kf1arow1 = builder.Configuration.GetSection("kf1a:row1").Get<string>();
            //1 樓  2列   kf1a.row2
            ViewBag.kf1arow2 = builder.Configuration.GetSection("kf1a:row2").Get<string>();
            //1 樓  3列   kf1a.row3
            ViewBag.kf1arow3 = builder.Configuration.GetSection("kf1a:row3").Get<string>();
            //1 樓  4列   kf1a.row4
            ViewBag.kf1arow4 = builder.Configuration.GetSection("kf1a:row4").Get<string>();

            //1 樓 乙區 4列編號(由下到上: row1,row2,....row4)
            //1 樓  1列   kf1b.row1
            ViewBag.kf1brow1 = builder.Configuration.GetSection("kf1b:row1").Get<string>();
            //1 樓  2列   kf1b.row2
            ViewBag.kf1brow2 = builder.Configuration.GetSection("kf1b:row2").Get<string>();
            //1 樓  3列   kf1b.row3
            ViewBag.kf1brow3 = builder.Configuration.GetSection("kf1b:row3").Get<string>();
            //1 樓  4列   kf1b.row4
            ViewBag.kf1brow4 = builder.Configuration.GetSection("kf1b:row4").Get<string>();

            //1 樓 丙區 4列編號(由下到上: row1,row2,....row4)
            //1 樓  1列   kf1c.row1
            ViewBag.kf1crow1 = builder.Configuration.GetSection("kf1c:row1").Get<string>();
            //1 樓  2列   kf1c.row2
            ViewBag.kf1crow2 = builder.Configuration.GetSection("kf1c:row2").Get<string>();
            //1 樓  3列   kf1c.row3
            ViewBag.kf1crow3 = builder.Configuration.GetSection("kf1c:row3").Get<string>();
            //1 樓  4列   kf1c.row4
            ViewBag.kf1crow4 = builder.Configuration.GetSection("kf1c:row4").Get<string>();

            //1 樓 丁區 4列編號(由下到上: row1,row2,....row4)
            //1 樓  1列   kf1d.row1
            ViewBag.kf1drow1 = builder.Configuration.GetSection("kf1d:row1").Get<string>();
            //1 樓  2列   kf1d.row2
            ViewBag.kf1drow2 = builder.Configuration.GetSection("kf1d:row2").Get<string>();
            //1 樓  3列   kf1d.row3
            ViewBag.kf1drow3 = builder.Configuration.GetSection("kf1d:row3").Get<string>();
            //1 樓  4列   kf1d.row4
            ViewBag.kf1drow4 = builder.Configuration.GetSection("kf1d:row4").Get<string>();

            //1 樓 戊區 4列編號(由下到上: row1,row2,....row4)
            //1 樓  1列   kf1e.row1
            ViewBag.kf1erow1 = builder.Configuration.GetSection("kf1e:row1").Get<string>();
            //1 樓  2列   kf1e.row2
            ViewBag.kf1erow2 = builder.Configuration.GetSection("kf1e:row2").Get<string>();
            //1 樓  3列   kf1e.row3
            ViewBag.kf1erow3 = builder.Configuration.GetSection("kf1e:row3").Get<string>();
            //1 樓  4列   kf1e.row4
            ViewBag.kf1erow4 = builder.Configuration.GetSection("kf1e:row4").Get<string>();


            //1 樓 己區 4列編號(由下到上: row1,row2,....row4)
            //1 樓  1列   kf1f.row1
            ViewBag.kf1frow1 = builder.Configuration.GetSection("kf1f:row1").Get<string>();
            //1 樓  2列   kf1f.row2
            ViewBag.kf1frow2 = builder.Configuration.GetSection("kf1f:row2").Get<string>();
            //1 樓  3列   kf1f.row3
            ViewBag.kf1frow3 = builder.Configuration.GetSection("kf1f:row3").Get<string>();
            //1 樓  4列   kf1f.row4
            ViewBag.kf1frow4 = builder.Configuration.GetSection("kf1f:row4").Get<string>();

            // 2 樓 甲區 4列編號(由下到上: row1,row2,....row4)
            // 1列   kf2a.row1
            ViewBag.kf2arow1 = builder.Configuration.GetSection("kf2a:row1").Get<string>();
            // 2列   kf2a.row2
            ViewBag.kf2arow2 = builder.Configuration.GetSection("kf2a:row2").Get<string>();
            // 3列   kf2a.row3
            ViewBag.kf2arow3 = builder.Configuration.GetSection("kf2a:row3").Get<string>();
            // 4列   kf2a.row4
            ViewBag.kf2arow4 = builder.Configuration.GetSection("kf2a:row4").Get<string>();

            // 2 樓 乙區 4列編號(由下到上: row1,row2,....row4)
            // 1列   kf2b.row1
            ViewBag.kf2brow1 = builder.Configuration.GetSection("kf2b:row1").Get<string>();
            // 2列   kf2b.row2
            ViewBag.kf2brow2 = builder.Configuration.GetSection("kf2b:row2").Get<string>();
            // 3列   kf2b.row3
            ViewBag.kf2brow3 = builder.Configuration.GetSection("kf2b:row3").Get<string>();
            // 4列   kf2b.row4
            ViewBag.kf2brow4 = builder.Configuration.GetSection("kf2b:row4").Get<string>();

            //懷恩塔 2 樓 丙區 4列編號(由下到上: row1,row2,....row4)
            // 1列   kf2c.row1
            ViewBag.kf2crow1 = builder.Configuration.GetSection("kf2c:row1").Get<string>();
            // 2列   kf2c.row2
            ViewBag.kf2crow2 = builder.Configuration.GetSection("kf2c:row2").Get<string>();
            // 3列   kf2c.row3
            ViewBag.kf2crow3 = builder.Configuration.GetSection("kf2c:row3").Get<string>();
            // 4列   kf2c.row4
            ViewBag.kf2crow4 = builder.Configuration.GetSection("kf2c:row4").Get<string>();

            //懷恩塔 2 樓 丁區 4列編號(由下到上: row1,row2,....row4)
            // 1列   kf2d.row1
            ViewBag.kf2drow1 = builder.Configuration.GetSection("kf2d:row1").Get<string>();
            // 2列   kf2d.row2
            ViewBag.kf2drow2 = builder.Configuration.GetSection("kf2d:row2").Get<string>();
            // 3列   kf2d.row3
            ViewBag.kf2drow3 = builder.Configuration.GetSection("kf2d:row3").Get<string>();
            // 4列   kf2d.row4
            ViewBag.kf2drow4 = builder.Configuration.GetSection("kf2d:row4").Get<string>();

            //懷恩塔 2 樓 戊區 4列編號(由下到上: row1,row2,....row4)
            // 1列   kf2e.row1
            ViewBag.kf2erow1 = builder.Configuration.GetSection("kf2e:row1").Get<string>();
            // 2列   kf2e.row2
            ViewBag.kf2erow2 = builder.Configuration.GetSection("kf2e:row2").Get<string>();
            // 3列   kf2e.row3
            ViewBag.kf2erow3 = builder.Configuration.GetSection("kf2e:row3").Get<string>();
            // 4列   kf2e.row4
            ViewBag.kf2erow4 = builder.Configuration.GetSection("kf2e:row4").Get<string>();


            //懷恩塔 2 樓 己區 4列編號(由下到上: row1,row2,....row4)
            // 1列   kf2f.row1
            ViewBag.kf2frow1 = builder.Configuration.GetSection("kf2f:row1").Get<string>();
            // 2列   kf2f.row2
            ViewBag.kf2frow2 = builder.Configuration.GetSection("kf2f:row2").Get<string>();
            // 3列   kf2f.row3
            ViewBag.kf2frow3 = builder.Configuration.GetSection("kf2f:row3").Get<string>();
            // 4列   kf2f.row4
            ViewBag.kf2frow4 = builder.Configuration.GetSection("kf2f:row4").Get<string>();

            //懷恩塔 3 樓 甲區 7列編號(由下到上: row1,row2,....row7)
            // 1列   kf3a.row1
            ViewBag.kf3arow1 = builder.Configuration.GetSection("kf3a:row1").Get<string>();
            // 2列   kf3a.row2
            ViewBag.kf3arow2 = builder.Configuration.GetSection("kf3a:row2").Get<string>();
            // 3列   kf3a.row3
            ViewBag.kf3arow3 = builder.Configuration.GetSection("kf3a:row3").Get<string>();
            // 4列   kf3a.row4
            ViewBag.kf3arow4 = builder.Configuration.GetSection("kf3a:row4").Get<string>();
            // 5列   kf3a.row5
            ViewBag.kf3arow5 = builder.Configuration.GetSection("kf3a:row5").Get<string>();
            // 6列   kf3a.row6
            ViewBag.kf3arow6 = builder.Configuration.GetSection("kf3a:row6").Get<string>();
            // 7列   kf3a.row7
            ViewBag.kf3arow7 = builder.Configuration.GetSection("kf3a:row7").Get<string>();

            //懷恩塔 3 樓 乙區 7列編號(由下到上: row1,row2,....row7)
            // 1列   kf3b.row1
            ViewBag.kf3brow1 = builder.Configuration.GetSection("kf3b:row1").Get<string>();
            // 2列   kf3b.row2
            ViewBag.kf3brow2 = builder.Configuration.GetSection("kf3b:row2").Get<string>();
            // 3列   kf3b.row3
            ViewBag.kf3brow3 = builder.Configuration.GetSection("kf3b:row3").Get<string>();
            // 4列   kf3b.row4
            ViewBag.kf3brow4 = builder.Configuration.GetSection("kf3b:row4").Get<string>();
            // 5列   kf3b.row5
            ViewBag.kf3brow5 = builder.Configuration.GetSection("kf3b:row5").Get<string>();
            // 6列   kf3b.row6
            ViewBag.kf3brow6 = builder.Configuration.GetSection("kf3b:row6").Get<string>();
            // 7列   kf3b.row7
            ViewBag.kf3brow7 = builder.Configuration.GetSection("kf3b:row7").Get<string>();

            //懷恩塔 3 樓 丙區 7列編號(由下到上: row1,row2,....row7)
            // 1列   kf3c.row1
            ViewBag.kf3crow1 = builder.Configuration.GetSection("kf3c:row1").Get<string>();
            // 2列   kf3c.row2
            ViewBag.kf3crow2 = builder.Configuration.GetSection("kf3c:row2").Get<string>();
            // 3列   kf3c.row3
            ViewBag.kf3crow3 = builder.Configuration.GetSection("kf3c:row3").Get<string>();
            // 4列   kf3c.row4
            ViewBag.kf3crow4 = builder.Configuration.GetSection("kf3c:row4").Get<string>();
            // 5列   kf3c.row5
            ViewBag.kf3crow5 = builder.Configuration.GetSection("kf3c:row5").Get<string>();
            // 6列   kf3c.row6
            ViewBag.kf3crow6 = builder.Configuration.GetSection("kf3c:row6").Get<string>();
            // 7列   kf3c.row7
            ViewBag.kf3crow7 = builder.Configuration.GetSection("kf3c:row7").Get<string>();

            //懷恩塔 3 樓 丁區 7列編號(由下到上: row1,row2,....row7)
            // 1列   kf3d.row1
            ViewBag.kf3drow1 = builder.Configuration.GetSection("kf3d:row1").Get<string>();
            // 2列   kf3d.row2
            ViewBag.kf3drow2 = builder.Configuration.GetSection("kf3d:row2").Get<string>();
            // 3列   kf3d.row3
            ViewBag.kf3drow3 = builder.Configuration.GetSection("kf3d:row3").Get<string>();
            // 4列   kf3d.row4
            ViewBag.kf3drow4 = builder.Configuration.GetSection("kf3d:row4").Get<string>();
            // 5列   kf3d.row5
            ViewBag.kf3drow5 = builder.Configuration.GetSection("kf3d:row5").Get<string>();
            // 6列   kf3d.row6
            ViewBag.kf3drow6 = builder.Configuration.GetSection("kf3d:row6").Get<string>();
            // 7列   kf3d.row7
            ViewBag.kf3drow7 = builder.Configuration.GetSection("kf3d:row7").Get<string>();

            //懷恩塔 3 樓 戊區 7列編號(由下到上: row1,row2,....row7)
            // 1列   kf3e.row1
            ViewBag.kf3erow1 = builder.Configuration.GetSection("kf3e:row1").Get<string>();
            // 2列   kf3e.row2
            ViewBag.kf3erow2 = builder.Configuration.GetSection("kf3e:row2").Get<string>();
            // 3列   kf3e.row3
            ViewBag.kf3erow3 = builder.Configuration.GetSection("kf3e:row3").Get<string>();
            // 4列   kf3e.row4
            ViewBag.kf3erow4 = builder.Configuration.GetSection("kf3e:row4").Get<string>();
            // 5列   kf3e.row5
            ViewBag.kf3erow5 = builder.Configuration.GetSection("kf3e:row5").Get<string>();
            // 6列   kf3e.row6
            ViewBag.kf3erow6 = builder.Configuration.GetSection("kf3e:row6").Get<string>();
            // 7列   kf3e.row7
            ViewBag.kf3erow7 = builder.Configuration.GetSection("kf3e:row7").Get<string>();

            //懷恩塔 3 樓 己區 7列編號(由下到上: row1,row2,....row7)
            // 1列   kf3f.row1
            ViewBag.kf3frow1 = builder.Configuration.GetSection("kf3f:row1").Get<string>();
            // 2列   kf3f.row2
            ViewBag.kf3frow2 = builder.Configuration.GetSection("kf3f:row2").Get<string>();
            // 3列   kf3f.row3
            ViewBag.kf3frow3 = builder.Configuration.GetSection("kf3f:row3").Get<string>();
            // 4列   kf3f.row4
            ViewBag.kf3frow4 = builder.Configuration.GetSection("kf3f:row4").Get<string>();
            // 5列   kf3f.row5
            ViewBag.kf3frow5 = builder.Configuration.GetSection("kf3f:row5").Get<string>();
            // 6列   kf3f.row6
            ViewBag.kf3frow6 = builder.Configuration.GetSection("kf3f:row6").Get<string>();
            // 7列   kf3f.row7
            ViewBag.kf3frow7 = builder.Configuration.GetSection("kf3f:row7").Get<string>();

            // 系統更新: @ViewBag.PublishDate
            ViewBag.PublishDate = builder.Configuration.GetValue<string>("PublishDate");

            //連線主機字串
            ViewBag.connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Auto logout settings
            float? AUTO_LOGOUT_MINUTE = builder.Configuration.GetSection("Logout_Duration:AUTO_LOGOUT_MINUTE").Get<float>();
            int? WARNING_BEFORE_LOGOUT_SECOND = builder.Configuration.GetSection("Logout_Duration:WARNING_BEFORE_LOGOUT_SECOND").Get<int>();

            // If AUTO_LOGOUT_MINUTE or WARNING_BEFORE_LOGOUT_SECOND is not set, use default values
            if (AUTO_LOGOUT_MINUTE == null || WARNING_BEFORE_LOGOUT_SECOND == null)
            {
                // Default values if not set in configuration
                AUTO_LOGOUT_MINUTE = (float?)0.5; // 30 minutes
                WARNING_BEFORE_LOGOUT_SECOND = 10; // 10 seconds                                                   
            }
            // Set ViewBag for use in the view
            ViewBag.AUTO_LOGOUT_MINUTE = AUTO_LOGOUT_MINUTE;
            ViewBag.WARNING_BEFORE_LOGOUT_SECOND = WARNING_BEFORE_LOGOUT_SECOND;


            // Work_Duration is used for the duration of work in the system
            float? Work_Duration = builder.Configuration.GetSection("Work_Duration").Get<float>();
            float? Import_Duration = builder.Configuration.GetSection("Import_Duration").Get<float>();
            int? WORK_WARNING_SECONDS = builder.Configuration.GetSection("WORK_WARNING_SECONDS").Get<int>();

            // If Work_Duration is not set, use default value
            if (Work_Duration == null)
            {
                // Default value if not set in configuration
                Work_Duration = (float?)1.0; // 1 mins          
            }
            ViewBag.Work_Duration = Work_Duration;
            // If Work_Duration is not set, use default value
            if (Import_Duration == null)
            {
                // Default value if not set in configuration
                Import_Duration = (float?)3.0; // 1 mins
            }
            ViewBag.Import_Duration = Import_Duration;

            // Set ViewBag for WORK_WARNING_SECONDS for use in the view
            if (WORK_WARNING_SECONDS == null)
            {
                // Default value if not set in configuration
                WORK_WARNING_SECONDS = 60; // 60 seconds          
            }
            ViewBag.WORK_WARNING_SECONDS = WORK_WARNING_SECONDS; // Default value will be null if not set  

        }

        private IActionResult DisplayKindnessObj(int? KindnessPositionId)
        {
            List<KindnessPosition> objPositionList = _unitOfWork.Kindness.GetAll().ToList();
            List<string> PositionIdlist = objPositionList
                .Where(u => !string.IsNullOrEmpty(u.PositionId) && u.PositionId != "0樓-0區-0層:000")
                .Select(u => u.PositionId!)
                .ToList();
            List<string> Namelist = objPositionList
                .Select(u => u.Name ?? string.Empty)
                .ToList();
            //產生已進駐祖先的塔位清單 ListPositionId
            List<string> result = new List<string>();
            string positionId = "";
            KindnessPosition KindnessCurrentPositionObj;
            for (int i = 0; i < PositionIdlist.Count; i++)
            {
                result.Add(PositionIdlist[i] + "," + Namelist[i]);
            }
            //已進駐祖先的塔位清單儲入 ViewBag.ListPositionId
            ViewBag.ListPositionId = result;
            ViewBag.OccupiedCount = result.Count; //使用的位數
            try
            {
                //目前要設定/修改 塔位的祖先資訊
                KindnessCurrentPositionObj = _unitOfWork.Kindness.Get(u => u.KindnessPositionId == KindnessPositionId);
                if (KindnessCurrentPositionObj == null)
                {
                    positionId = "0樓-0區-0層:000"; //目前查詢的塔位
                    ViewBag.PositionId = positionId; //目前查詢的塔位
                }
                else
                {
                    positionId = KindnessCurrentPositionObj.PositionId ?? "0樓-0區-0層:000"; //目前查詢的塔位
                    ViewBag.PositionId = positionId; //目前查詢的塔位
                }                            
            }
            catch (NullReferenceException)
            {
                //如果沒有找到塔位，則返回預設值
                 KindnessCurrentPositionObj = new KindnessPosition
                {
                    KindnessPositionId = 0,
                    PositionId = "0樓-0區-0層:000",
                    Name = "",
                    Floor = "1樓",
                    Section = "甲區",
                    Level = "1層",
                    Position = "000",
                    //ApplicantName = "",
                    //ApplicantPhoneNumber = "",
                    //ApplicantEmail = "",
                    //ApplicationDateTime = DateTime.Now,
                    //ApplicationStatus = "未處理"
                };
                ViewBag.PositionId = KindnessCurrentPositionObj.PositionId; //目前查詢的塔位
                return View(KindnessCurrentPositionObj);
            }
          
          
            // 讀取樓,區資訊
            string splitter_colon = ":";
            string splitter_floor = "樓";
            string splitter_section = "區";
            string splitter_level = "層";

            string floor= "1"; //樓
            string section = "甲"; //區
            string level = "1"; //層
            string position = "000"; //位
            //如果 positionId 為空，則返回預設值
            //檢查 positionId 是否包含分隔符號
            int colon_Index = positionId.IndexOf(splitter_colon); //1樓-甲區-7層:246
            int floor_Index = positionId.IndexOf(splitter_floor);
            int section_Index = positionId.IndexOf(splitter_section);
            int level_Index = positionId.IndexOf(splitter_level);

            if (colon_Index < 0 || floor_Index < 0 || section_Index < 0 || level_Index < 0)
            {
                //如果沒有找到分隔符號，則返回預設值
                ViewBag.floor = "1";
                ViewBag.section = "甲";
                ViewBag.level = "1";
                ViewBag.position = "000";
                return View(KindnessCurrentPositionObj);
            }
            else if (colon_Index < floor_Index || colon_Index < section_Index || colon_Index < level_Index)
            {
                //如果分隔符號位置不正確，則返回預設值
                ViewBag.floor = "1";
                ViewBag.section = "甲";
                ViewBag.level = "1";
                ViewBag.position = "000";
                return View(KindnessCurrentPositionObj);
            }
            else
            {
                //如果找到分隔符號，則提取相應的值
                 floor = positionId.Substring(floor_Index - 1, 1) ?? "1"; //樓
                 section = positionId.Substring(section_Index - 1, 1) ?? "1";//區
                 level = positionId.Substring(level_Index - 1, 1) ?? "1"; //層
                 position = positionId.Substring(colon_Index + 1) ?? "0";  //位
            }

            ViewBag.floor = floor; //目前查詢的-樓 
            ViewBag.section = section; //目前查詢的-區 
            ViewBag.level = level; //目前查詢的-層 
            ViewBag.position = position; //目前查詢的-位           
            return View(KindnessCurrentPositionObj);
        }

        /// <summary>
        /// 查詢 2025 05 16 16:39
        /// </summary>
        /// <returns></returns>
        public IActionResult PositionQuery()
        {
            return View();
        }

        /// <summary>
        /// 2025 07 07 14:56
        /// Imports kindness position data from an Excel file and validates the input rows.
        /// </summary>
        /// <remarks>This method processes a list of kindness position data provided in the request body,
        /// validates each row,  and saves valid rows to the database. Validation includes checking required fields,
        /// ensuring values conform  to expected formats, and verifying that position IDs are unique within the
        /// database. If any validation errors are found, the method returns a response containing the error
        /// details.</remarks>
        /// <param name="importedRows">A list of <see cref="KindnessPositionViewModel"/> objects representing the rows to be imported. Each object
        /// should contain the necessary data for an ancestral position, such as name, floor, section, level,  position,
        /// position ID, and applicant information.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the import operation.  If validation errors are
        /// found, the response includes a list of error messages and indicates failure.  If all rows are valid, the
        /// response indicates success.</returns>
        [HttpPost]
        public IActionResult ImportExcel([FromBody] List<KindnessPositionViewModel> importedRows)
        {
            var errors = new List<string>();
            var validRows = new List<KindnessPosition>();

            for (int i = 0; i < importedRows.Count; i++)
            {
                var row = importedRows[i];
                int rowNum = i + 2; // Excel row number (header is row 1)

                // Example validation rules
                if (string.IsNullOrWhiteSpace(row.Name))
                    errors.Add($"第{rowNum}行: 祖先姓名為必填");
                if (row.Floor != "1樓" && row.Floor != "2樓" && row.Floor != "3樓")
                    errors.Add($"第{rowNum}行: 樓必須為'1樓'或'2樓'或'3樓'");
                if (string.IsNullOrWhiteSpace(row.Section))
                    errors.Add($"第{rowNum}行: 區為必填");
                if (string.IsNullOrWhiteSpace(row.Level))
                    errors.Add($"第{rowNum}行: 層為必填");
                //      if (!int.TryParse(row.Level, out _))
                //      errors.Add($"第{rowNum}行: 層必須為數字");
                if (string.IsNullOrWhiteSpace(row.Position))
                    errors.Add($"第{rowNum}行: 編號為必填");
                if (string.IsNullOrWhiteSpace(row.PositionId))
                    errors.Add($"第{rowNum}行: 塔位為必填");

                // Example: check for duplicates in DB
                // commented:2025 08 25
                //if (_unitOfWork.Kindness.Get(a => a.PositionId == row.PositionId) != null)
                //    errors.Add($"第{rowNum}行: 牌位編號 [{row.PositionId}] 已存在於資料庫");
                // commented:2025 08 25

                // If no errors for this row, add to validRows
                if (!errors.Any(e => e.StartsWith($"第{rowNum}行")))
                {
                    validRows.Add(new KindnessPosition
                    {
                        Name = row.Name,
                        Floor = row.Floor,
                        Section = row.Section,
                        Level = row.Level,
                        Position = row.Position,
                        PositionId = row.PositionId,
                        Applicant = row.Applicant,
                        Relation = row.Relation,
                        Mobile_Tel = row.Mobile_Tel,
                        Note = row.Note
                    });
                }
            }

            if (errors.Count > 0)
                return Json(new { success = false, errors });

            // Save validRows to DB
            foreach (var entity in validRows)
            {
                _unitOfWork.Kindness.Add(entity);
            }
            _unitOfWork.Save();

            return Json(new { success = true });
        }

        public IActionResult Upsert(int? KindnessPositionId)
        {

            if (KindnessPositionId == null || KindnessPositionId == 0)
            {
                //create
                return View(new KindnessPosition());
            }
            else
            {
                //update
                KindnessPosition KindnessPositionObj = _unitOfWork.Kindness.Get(u => u.KindnessPositionId == KindnessPositionId);
                return View(KindnessPositionObj);
            }

        }
        [HttpPost]
        public IActionResult Upsert(KindnessPosition KindnessPositionObj)
        {
            if (ModelState.IsValid)
            {

                if (KindnessPositionObj.KindnessPositionId == 0)
                {
                    _unitOfWork.Kindness.Add(KindnessPositionObj);
                }
                else
                {
                    _unitOfWork.Kindness.Update(KindnessPositionObj);
                }

                string strResult = _unitOfWork.Save();
                TempData["success"] = "新增/更新 成功" + strResult;
                return RedirectToAction("Index");
            }
            else
            {

                return View(KindnessPositionObj);
            }
        }


        #region API CALLS

        //        2. Create a Server Endpoint to Receive the Data
        //In your controller(e.g., OrderController.cs), add an action to receive and save the data:
        [HttpPost]
        public IActionResult SavePositions([FromBody] string displaytext)  //eg: 您已選取..1樓,甲區,7層塔位編號:246
        {
            try
            {
                // 儲存到資料庫的邏輯
                string splitter_colon = ":";
                string splitter_floor = "樓";
                string splitter_section = "區";
                string splitter_level = "層";

                int colon_Index = displaytext.IndexOf(splitter_colon); //您已選取..1樓,甲區,7層塔位編號:246
                int floor_Index = displaytext.IndexOf(splitter_floor);
                int section_Index = displaytext.IndexOf(splitter_section);
                int level_Index = displaytext.IndexOf(splitter_level);
                     
                string floor = displaytext.Substring(floor_Index-1,2); //樓
                string section = displaytext.Substring(section_Index-1,2); ;//區
                string level = displaytext.Substring(level_Index-1, 2); ; //層
                string position = displaytext.Substring(colon_Index+1);  //位
                //string sPositionId = floor + "-" + section + "-" + level + ":" + position;
                string sPositionId = displaytext;
                //Select Obj of sKindnessPositionId
                KindnessPosition KindnessPositionObj = _unitOfWork.Kindness.Get(u => u.KindnessPositionId==SelectedPoistionId);
                //Update PositionId
                KindnessPositionObj.PositionId = sPositionId;
                KindnessPositionObj.Floor = floor;
                KindnessPositionObj.Section = section;
                KindnessPositionObj.Level = level;
                KindnessPositionObj.Position = position;

                //do db_change:
                _unitOfWork.Kindness.Update(KindnessPositionObj);
                string strResult = _unitOfWork.Save();
                TempData["success"] = "塔位設定成功~" + strResult;
                //return RedirectToAction("Index");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // 可記錄 log
                return Json(new { success = false, message = ex.Message });
            }
        }
        //2	Adjust the parameter type to match your data structure(e.g., List<int>, List<PositionModel>, etc.).
        //        
        //3. Save to Database
        //Inside the controller action, use your Entity Framework context or other data access code to save the received data.
        //        ---
        //Summary:
        //•	Use JavaScript to send localStorage data to the server via AJAX.
        //•	Create a controller action to receive and save the data.
        //•	Save the data to your database using your data access layer.
        //Gotcha:
        //•	Make sure your endpoint URL matches your route.
        //•	Ensure the data format sent from JS matches what your controller expects.
        //Let me know if you need a full working example for your specific data structure!
        //* 2025 08 27 01:12 搜尋祖先姓名  
        [HttpGet]
        public IActionResult GetAll(string? search)
        {
            IEnumerable<KindnessPosition> objKindnessPositionList;
            if (!string.IsNullOrWhiteSpace(search))
            {
                objKindnessPositionList = _unitOfWork.Kindness.GetAll(
                    filter: x => x.Name != null && x.Name.Contains(search)
                ).ToList();
            }
            else
            {
                objKindnessPositionList = _unitOfWork.Kindness.GetAll().ToList();
            }

            foreach(var item in objKindnessPositionList)
            {

                try
                {
                    //2025 07 16 15:20 new encrpted_name,applicant
                    if (item.Applicant == null)
                    {
                        continue; //跳過空值
                    }
                    else if (item.Applicant.Length < 2)
                    {
                        continue; //跳過長度不足的姓名
                    }
                    else if (item.Applicant.Length > 2)
                    {
                        //string encrpted_name = item.Name.Substring(0, 1) + "*" + item.Name.Substring(2, item.Name.Length - 2) ?? "";
                        string encrpted_applicant = item.Applicant.Substring(0, 1) + "*" + item.Applicant.Substring(2, item.Applicant.Length - 2) ?? ""; //加密申請人姓名
                        //item.Name = encrpted_name; //加密姓名                   
                        item.Applicant = encrpted_applicant; //加密申請人姓名
                    }
                    else if (item.Applicant.Length == 2)
                    {
                        //string encrpted_name = item.Name.Substring(0, 2) + "*"; //加密姓名
                        string encrpted_applicant = item.Applicant.Substring(0, 2) + "*"; //加密申請人姓名
                        //item.Name = encrpted_name; //加密姓名                   
                        item.Applicant = encrpted_applicant; //加密申請人姓名
                    }
                    else
                    {
                        return Json(new { success = false, message = "加密申請人姓名失敗!!" });
                    }
                }
                catch (Exception)
                {
                    continue; //跳過長度[特別]的申請人姓名
                }

            }
            return Json(new { data = objKindnessPositionList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var KindnessToBeDeleted = _unitOfWork.Kindness.Get(u => u.KindnessPositionId == id);
            if (KindnessToBeDeleted == null)
            {
                return Json(new { success = false, message = "刪除失敗" });
            }

            _unitOfWork.Kindness.Remove(KindnessToBeDeleted);
            string strResult = _unitOfWork.Save();

            return Json(new { success = true, message = "刪除成功" });
        }


        /// <summary>
        /// 刪除選取 2025.07.07 22:12
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteRange([FromBody] List<int> ids)
        {
            foreach (var id in ids)
            {
                var entity = _unitOfWork.Kindness.Get(x => x.KindnessPositionId == id);
                if (entity != null)
                    _unitOfWork.Kindness.Remove(entity);
            }
            _unitOfWork.Save();
            return Json(new { success = true });
        }

        /// <summary>
        /// 刪除全部 2025.07.07 22:12
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteAll()
        {
            var all = _unitOfWork.Kindness.GetAll().ToList();
            foreach (var entity in all)
                _unitOfWork.Kindness.Remove(entity);
            _unitOfWork.Save();
            return Json(new { success = true });
        }


        #endregion
    }
}



//using BulkyBook.Utility;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace BulkyWeb.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Roles = SD.Role_Admin)]
//    public class KindnessController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }

//        /// <summary>
//        /// 懷恩塔塔位查詢 2025.05.12 17:43
//        /// </summary>
//        /// <returns></returns>
//        public IActionResult PositionQuery()
//        {
//            return View();
//        }
//    }
//}
