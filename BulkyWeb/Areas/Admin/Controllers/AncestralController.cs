using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    //宗祠-牌位管理 2025 05 15 12:01 AncestralPosition.cs
    [Area("Admin")]
 //   [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
    public class AncestralController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int? SelectedPoistionId { set; get; } //保存選擇的牌位ID
        private static DateTime? SystemStartTime { get; set; }
        public AncestralController(IUnitOfWork unitOfWork)
        {
            if (SystemStartTime != null && (DateTime.Now - SystemStartTime.Value).TotalMinutes > 3 )
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
            ReadAncestralSetting(KindnessPositionId);
            List<AncestralPosition> objAncestralList = _unitOfWork.Ancestral.GetAll().ToList();
            //IEnumerable<AncestralPosition> objAncestralList = _unitOfWork.Ancestral.GetAll().Where(static item => item.PositionId.Length>0).ToList();

            return View(objAncestralList);
        }

        /// <summary>
        /// 預約 2025 05 16 16:39
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Application(int? AncestralPositionId)
        {
            ReadAncestralSetting(AncestralPositionId);
            return DisplayAncestralObj(AncestralPositionId);
        }

        /// <summary>
        /// 預約 2025 05 16 16:39
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DisplayPosition(int? AncestralPositionId = 0)
        {
            ReadAncestralSetting(AncestralPositionId);
            return DisplayAncestralObj(AncestralPositionId);
        }

        private IActionResult DisplayAncestralObj(int? AncestralPositionId)
        {
            //record SystemStartTime
            ViewBag.SystemStartTime = SystemStartTime;
            List<AncestralPosition> objPositionList = _unitOfWork.Ancestral.GetAll().ToList();
            List<string> PositionIdlist = objPositionList
                .Where(u => !string.IsNullOrEmpty(u.PositionId) && u.PositionId != "0側-0區-0層:000")
                .Select(u => u.PositionId!)
                .ToList();
            List<string> Namelist = objPositionList
                .Select(u => u.Name ?? string.Empty)
                .ToList();
            List<string> result = new List<string>();
            for (int i = 0; i < PositionIdlist.Count; i++)
            {
                result.Add(PositionIdlist[i] + "," + Namelist[i]);
            }
            //已進駐祖先的塔位清單儲入 ViewBag.ListPositionId
            ViewBag.ListPositionId = result;
            ViewBag.OccupiedCount = result.Count; //使用的位數
            AncestralPosition PositionObj;
            string positionId = ""; //預設牌位
            try
            {
                PositionObj = _unitOfWork.Ancestral.Get(u => u.AncestralPositionId == AncestralPositionId);

                if (PositionObj == null)
                {
                    positionId = "0側-0區-0層:000"; //目前查詢的牌位
                    ViewBag.PositionId = positionId; //目前查詢的牌位
                }
                else
                {
                    positionId = PositionObj.PositionId ?? "0側-0區-0層:000"; //目前查詢的牌位
                    ViewBag.PositionId = positionId; //目前查詢的牌位
                }
            }
            catch (NullReferenceException)
            {
                //如果沒有找到牌位，則返回預設值
                PositionObj = new AncestralPosition
                {
                     AncestralPositionId = 0,
                     PositionId = "0樓-0區-0層:000",
                     Name = "",
                     Side = "1樓",
                     Section = "甲區",
                     Level = "1層",
                     Position = "000",
                     //ApplicantName = "",
                     //ApplicantPhoneNumber = "",
                     //ApplicantEmail = "",
                     //ApplicationDateTime = DateTime.Now,
                     //ApplicationStatus = "未處理"
                 };
            ViewBag.PositionId = PositionObj.PositionId; //目前查詢的牌位
                return View(PositionObj);
        }

            // 讀取側,區資訊
            string splitter_colon = ":";
            string splitter_side = "側";
            string splitter_section = "區";
            string splitter_level = "層";

            int colon_Index = positionId.IndexOf(splitter_colon); //1側-甲區-7層:246
            int side_Index = positionId.IndexOf(splitter_side);
            int section_Index = positionId.IndexOf(splitter_section);
            int level_Index = positionId.IndexOf(splitter_level);
            string side = "左"; //側
            string section = "甲"; //區
            string level = "1"; //層
            string position = "000"; //位
            //如果沒有找到分隔符號，則返回預設值
            if (colon_Index < 0 || side_Index < 0 || section_Index < 0 || level_Index < 0)
            {
                //如果沒有找到分隔符號，則返回預設值
                ViewBag.side = "左"; //側
                ViewBag.section = "甲"; //區
                ViewBag.level = "1"; //層
                ViewBag.position = "000"; //位
                return View(PositionObj);
            }
            else if (colon_Index == 0 || side_Index == 0 || section_Index == 0 || level_Index == 0)
            {
                //如果分隔符號在開頭，則返回預設值
                ViewBag.side = "左"; //側
                ViewBag.section = "甲"; //區
                ViewBag.level = "1"; //層
                ViewBag.position = "000"; //位
                return View(PositionObj);
            }
            else
            {
                side = positionId.Substring(side_Index - 1, 1) ?? "1"; //側
                section = positionId.Substring(section_Index - 1, 1) ?? "1";//區
                level = positionId.Substring(level_Index - 1, 1) ?? "1"; //層
                position = positionId.Substring(colon_Index + 1) ?? "0";  //位
            }

            ViewBag.side = side; //目前查詢的-側 
            ViewBag.section = section; //目前查詢的-區 
            ViewBag.level = level; //目前查詢的-層 
            ViewBag.position = position; //目前查詢的-位
            return View(PositionObj);
        }

        private void ReadAncestralSetting(int? AncestralPositionId)
        {
            SelectedPoistionId = AncestralPositionId;  //保存選擇的牌位ID
            ViewBag.SystemStartTime = SystemStartTime;
            var config = HttpContext.RequestServices.GetService<IConfiguration>();
            // 這些變數是用來顯示 宗祠 祖先牌位 的設定頁面 2025 05 22 16:13
            ////////////////////////////
            //(1)
            //Side: l:左側  // 乙區 // 丁區 // 己區 // 辛區 
            //      2:右側, // 甲區 // 丙區 // 戊區 // 庚區
            //Section: 區: 甲,乙,丙,丁 區
            //Level: 層(列): 10~1
            //Position: 編號 1~10 (每層10個)
            ////////////////////////////
            ViewBag.AncestralSide = config.GetSection("Ancestral:Side").Get<int>();
            ViewBag.AncestralSection = config.GetSection("Ancestral:Section").Get<int>();
            ViewBag.AncestralLevel = config.GetSection("Ancestral:Level").Get<int>();
            ViewBag.AncestralPosition = config.GetSection("Ancestral:Position").Get<int>();

            // Ancestral Layout_L  (左)
            ViewBag.AncestralLayout_L = config.GetSection("Ancestral:Layout_L").Get<string>();
            // Ancestral Layout_R  (右)
            ViewBag.AncestralLayout_R = config.GetSection("Ancestral:Layout_R").Get<string>();
            // Ancestral Layout
            ViewBag.AncestralLayout = config.GetSection("Ancestral:Layout").Get<string>();

            ////////////////////////////
            //(2)
            //l:左側  r:右側 row: r列 col: c行
            ////////////////////////////
            // l:左側  // 乙區 // 丁區 // 己區 // 辛區

            ViewBag.lar = config.GetSection("Ancestral:la:row").Get<int>();
            ViewBag.lac = config.GetSection("Ancestral:la:col").Get<int>();
           
            ViewBag.lbr = config.GetSection("Ancestral:lb:row").Get<int>();
            ViewBag.lbc = config.GetSection("Ancestral:lb:col").Get<int>();
        
            ViewBag.lcr = config.GetSection("Ancestral:lc:row").Get<int>();
            ViewBag.lcc = config.GetSection("Ancestral:lc:col").Get<int>();
         
            ViewBag.ldr = config.GetSection("Ancestral:ld:row").Get<int>();
            ViewBag.ldc = config.GetSection("Ancestral:ld:col").Get<int>();

            ViewBag.lmr = config.GetSection("Ancestral:lm:row").Get<int>();
            ViewBag.lmc = config.GetSection("Ancestral:lm:col").Get<int>();


            ////////////////////////////
            //(3)
            // r:右側 
            // r:右側 // 甲區 // 丙區 // 戊區 // 庚區
            ////////////////////////////

            ViewBag.rar = config.GetSection("Ancestral:ra:row").Get<int>();
            ViewBag.rac = config.GetSection("Ancestral:ra:col").Get<int>();
          
            ViewBag.rbr = config.GetSection("Ancestral:rb:row").Get<int>();
            ViewBag.rbc = config.GetSection("Ancestral:rb:col").Get<int>();
          
            ViewBag.rcr = config.GetSection("Ancestral:rc:row").Get<int>();
            ViewBag.rcc = config.GetSection("Ancestral:rc:col").Get<int>();
           
            ViewBag.rdr = config.GetSection("Ancestral:rd:row").Get<int>();
            ViewBag.rdc = config.GetSection("Ancestral:rd:col").Get<int>();

            ViewBag.rmr = config.GetSection("Ancestral:rm:row").Get<int>();
            ViewBag.rmc = config.GetSection("Ancestral:rm:col").Get<int>();

            ////////////////////////////
            //(4)
            //l:左側 各區/各層(列)編號
            ////////////////////////////
            //    (1)牌位,新的編排
            // l:左側 // 辛區 // 己區// 丁區// 乙區
            //===============
            // 辛區         
            ViewBag.alarow1 = config.GetSection("ala:row1").Get<string>();
            ViewBag.alarow2 = config.GetSection("ala:row2").Get<string>();
            ViewBag.alarow3 = config.GetSection("ala:row3").Get<string>();
            ViewBag.alarow4 = config.GetSection("ala:row4").Get<string>();
            ViewBag.alarow5 = config.GetSection("ala:row5").Get<string>();
            ViewBag.alarow6 = config.GetSection("ala:row6").Get<string>();
            ViewBag.alarow7 = config.GetSection("ala:row7").Get<string>();
            ViewBag.alarow8 = config.GetSection("ala:row8").Get<string>();
            ViewBag.alarow9 = config.GetSection("ala:row9").Get<string>();
            ViewBag.alarow10 = config.GetSection("ala:row10").Get<string>();

            // 己區         
            ViewBag.albrow1 = config.GetSection("alb:row1").Get<string>();
            ViewBag.albrow2 = config.GetSection("alb:row2").Get<string>();
            ViewBag.albrow3 = config.GetSection("alb:row3").Get<string>();
            ViewBag.albrow4 = config.GetSection("alb:row4").Get<string>();
            ViewBag.albrow5 = config.GetSection("alb:row5").Get<string>();
            ViewBag.albrow6 = config.GetSection("alb:row6").Get<string>();
            ViewBag.albrow7 = config.GetSection("alb:row7").Get<string>();
            ViewBag.albrow8 = config.GetSection("alb:row8").Get<string>();
            ViewBag.albrow9 = config.GetSection("alb:row9").Get<string>();
            ViewBag.albrow10 = config.GetSection("alb:row10").Get<string>();

            // 丁區     
            ViewBag.alcrow1 = config.GetSection("alc:row1").Get<string>();
            ViewBag.alcrow2 = config.GetSection("alc:row2").Get<string>();
            ViewBag.alcrow3 = config.GetSection("alc:row3").Get<string>();
            ViewBag.alcrow4 = config.GetSection("alc:row4").Get<string>();
            ViewBag.alcrow5 = config.GetSection("alc:row5").Get<string>();
            ViewBag.alcrow6 = config.GetSection("alc:row6").Get<string>();
            ViewBag.alcrow7 = config.GetSection("alc:row7").Get<string>();
            ViewBag.alcrow8 = config.GetSection("alc:row8").Get<string>();
            ViewBag.alcrow9 = config.GetSection("alc:row9").Get<string>();
            ViewBag.alcrow10 = config.GetSection("alc:row10").Get<string>();

            // 乙區           
            ViewBag.aldrow1 = config.GetSection("ald:row1").Get<string>();
            ViewBag.aldrow2 = config.GetSection("ald:row2").Get<string>();
            ViewBag.aldrow3 = config.GetSection("ald:row3").Get<string>();
            ViewBag.aldrow4 = config.GetSection("ald:row4").Get<string>();
            ViewBag.aldrow5 = config.GetSection("ald:row5").Get<string>();
            ViewBag.aldrow6 = config.GetSection("ald:row6").Get<string>();
            ViewBag.aldrow7 = config.GetSection("ald:row7").Get<string>();
            ViewBag.aldrow8 = config.GetSection("ald:row8").Get<string>();
            ViewBag.aldrow9 = config.GetSection("ald:row9").Get<string>();
            ViewBag.aldrow10 = config.GetSection("ald:row10").Get<string>();

            // 中區           
            ViewBag.almrow1 = config.GetSection("alm:row1").Get<string>();
            ViewBag.almrow2 = config.GetSection("alm:row2").Get<string>();
            ViewBag.almrow3 = config.GetSection("alm:row3").Get<string>();
            ViewBag.almrow4 = config.GetSection("alm:row4").Get<string>();
            ViewBag.almrow5 = config.GetSection("alm:row5").Get<string>();
            ViewBag.almrow6 = config.GetSection("alm:row6").Get<string>();
            ViewBag.almrow7 = config.GetSection("alm:row7").Get<string>();
            ViewBag.almrow8 = config.GetSection("alm:row8").Get<string>();
            ViewBag.almrow9 = config.GetSection("alm:row9").Get<string>();
            ViewBag.almrow10 = config.GetSection("alm:row10").Get<string>();


            ////////////////////////////
            //(5)
            //l:右側 各區/各層(列)編號
            ////////////////////////////
            // r:右側 // 甲區 // 丙區 // 戊區 //庚區

            //甲區 
            ViewBag.ararow1 = config.GetSection("ara:row1").Get<string>();
            ViewBag.ararow2 = config.GetSection("ara:row2").Get<string>();
            ViewBag.ararow3 = config.GetSection("ara:row3").Get<string>();
            ViewBag.ararow4 = config.GetSection("ara:row4").Get<string>();
            ViewBag.ararow5 = config.GetSection("ara:row5").Get<string>();
            ViewBag.ararow6 = config.GetSection("ara:row6").Get<string>();
            ViewBag.ararow7 = config.GetSection("ara:row7").Get<string>();
            ViewBag.ararow8 = config.GetSection("ara:row8").Get<string>();
            ViewBag.ararow9 = config.GetSection("ara:row9").Get<string>();
            ViewBag.ararow10 = config.GetSection("ara:row10").Get<string>();

            //丙區  
            //陳氏宗祠-祖先版位-編號
            ViewBag.arbrow1 = config.GetSection("arb:row1").Get<string>();
            ViewBag.arbrow2 = config.GetSection("arb:row2").Get<string>();
            ViewBag.arbrow3 = config.GetSection("arb:row3").Get<string>();
            ViewBag.arbrow4 = config.GetSection("arb:row4").Get<string>();
            ViewBag.arbrow5 = config.GetSection("arb:row5").Get<string>();
            ViewBag.arbrow6 = config.GetSection("arb:row6").Get<string>();
            ViewBag.arbrow7 = config.GetSection("arb:row7").Get<string>();
            ViewBag.arbrow8 = config.GetSection("arb:row8").Get<string>();
            ViewBag.arbrow9 = config.GetSection("arb:row9").Get<string>();
            ViewBag.arbrow10 = config.GetSection("arb:row10").Get<string>();

            //戊區
            //陳氏宗祠-祖先版位-編號
            ViewBag.arcrow1 = config.GetSection("arc:row1").Get<string>();
            ViewBag.arcrow2 = config.GetSection("arc:row2").Get<string>();
            ViewBag.arcrow3 = config.GetSection("arc:row3").Get<string>();
            ViewBag.arcrow4 = config.GetSection("arc:row4").Get<string>();
            ViewBag.arcrow5 = config.GetSection("arc:row5").Get<string>();
            ViewBag.arcrow6 = config.GetSection("arc:row6").Get<string>();
            ViewBag.arcrow7 = config.GetSection("arc:row7").Get<string>();
            ViewBag.arcrow8 = config.GetSection("arc:row8").Get<string>();
            ViewBag.arcrow9 = config.GetSection("arc:row9").Get<string>();
            ViewBag.arcrow10 = config.GetSection("arc:row10").Get<string>();

            //庚區
            //陳氏宗祠-祖先版位-編號
            ViewBag.ardrow1 = config.GetSection("ard:row1").Get<string>();
            ViewBag.ardrow2 = config.GetSection("ard:row2").Get<string>();
            ViewBag.ardrow3 = config.GetSection("ard:row3").Get<string>();
            ViewBag.ardrow4 = config.GetSection("ard:row4").Get<string>();
            ViewBag.ardrow5 = config.GetSection("ard:row5").Get<string>();
            ViewBag.ardrow6 = config.GetSection("ard:row6").Get<string>();
            ViewBag.ardrow7 = config.GetSection("ard:row7").Get<string>();
            ViewBag.ardrow8 = config.GetSection("ard:row8").Get<string>();
            ViewBag.ardrow9 = config.GetSection("ard:row9").Get<string>();
            ViewBag.ardrow10 = config.GetSection("ard:row10").Get<string>();

            // 中區           
            ViewBag.armrow1 = config.GetSection("arm:row1").Get<string>();
            ViewBag.armrow2 = config.GetSection("arm:row2").Get<string>();
            ViewBag.armrow3 = config.GetSection("arm:row3").Get<string>();
            ViewBag.armrow4 = config.GetSection("arm:row4").Get<string>();
            ViewBag.armrow5 = config.GetSection("arm:row5").Get<string>();
            ViewBag.armrow6 = config.GetSection("arm:row6").Get<string>();
            ViewBag.armrow7 = config.GetSection("arm:row7").Get<string>();
            ViewBag.armrow8 = config.GetSection("arm:row8").Get<string>();
            ViewBag.armrow9 = config.GetSection("arm:row9").Get<string>();
            ViewBag.armrow10 = config.GetSection("arm:row10").Get<string>();


            // 讀取 PublishDate
            ViewBag.PublishDate = config.GetValue<string>("PublishDate");

            //連線主機字串
            ViewBag.connectionString = config.GetConnectionString("DefaultConnection");

            // Auto logout settings
            float? AUTO_LOGOUT_MINUTE = config.GetSection("Logout_Duration:AUTO_LOGOUT_MINUTE").Get<float>();
            int? WARNING_BEFORE_LOGOUT_SECOND = config.GetSection("Logout_Duration:WARNING_BEFORE_LOGOUT_SECOND").Get<int>();

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
            float? Work_Duration = config.GetSection("Work_Duration").Get<float>();
            float? Import_Duration = config.GetSection("Import_Duration").Get<float>();
            int? WORK_WARNING_SECONDS = config.GetSection("WORK_WARNING_SECONDS").Get<int>();

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
        /// Imports ancestral position data from an Excel file and validates the input rows.
        /// </summary>
        /// <remarks>This method processes a list of ancestral position data provided in the request body,
        /// validates each row,  and saves valid rows to the database. Validation includes checking required fields,
        /// ensuring values conform  to expected formats, and verifying that position IDs are unique within the
        /// database. If any validation errors are found, the method returns a response containing the error
        /// details.</remarks>
        /// <param name="importedRows">A list of <see cref="AncestralPositionViewModel"/> objects representing the rows to be imported. Each object
        /// should contain the necessary data for an ancestral position, such as name, side, section, level,  position,
        /// position ID, and applicant information.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the import operation.  If validation errors are
        /// found, the response includes a list of error messages and indicates failure.  If all rows are valid, the
        /// response indicates success.</returns>
        [HttpPost]
        public IActionResult ImportExcel([FromBody] List<AncestralPositionViewModel> importedRows)
        {
            var errors = new List<string>();
            var validRows = new List<AncestralPosition>();

            for (int i = 0; i < importedRows.Count; i++)
            {
                var row = importedRows[i];
                int rowNum = i + 2; // Excel row number (header is row 1)

                // Example validation rules
                if (string.IsNullOrWhiteSpace(row.Name))
                    errors.Add($"第{rowNum}行: 祖先姓名為必填");
                if (row.Side != "左側" && row.Side != "右側" && row.Side != "中間")
                    errors.Add($"第{rowNum}行: 側必須為'左側'或'右側'或'中間");
                if (string.IsNullOrWhiteSpace(row.Section))
                    errors.Add($"第{rowNum}行: 區為必填");
                if (string.IsNullOrWhiteSpace(row.Level))
                    errors.Add($"第{rowNum}行: 層為必填");
                //      if (!int.TryParse(row.Level, out _))
                //      errors.Add($"第{rowNum}行: 層必須為數字");
                if (string.IsNullOrWhiteSpace(row.Position))
                    errors.Add($"第{rowNum}行: 編號為必填");
                if (string.IsNullOrWhiteSpace(row.PositionId))
                    errors.Add($"第{rowNum}行: 牌位為必填");

                // Example: check for duplicates in DB
                if (_unitOfWork.Ancestral.Get(a => a.PositionId == row.PositionId) != null)
                    errors.Add($"第{rowNum}行: 牌位編號 [{row.PositionId}] 已存在於資料庫");

                // If no errors for this row, add to validRows
                if (!errors.Any(e => e.StartsWith($"第{rowNum}行")))
                {
                    validRows.Add(new AncestralPosition
                    {
                        Name = row.Name,
                        Side = row.Side,
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
                _unitOfWork.Ancestral.Add(entity);
            }
            _unitOfWork.Save();

            return Json(new { success = true });
        }


        public IActionResult Upsert(int? AncestralPositionId)
        {

            if (AncestralPositionId == null || AncestralPositionId == 0)
            {
                //create
                return View(new AncestralPosition());
            }
            else
            {
                //update
                AncestralPosition AncestralObj = _unitOfWork.Ancestral.Get(u => u.AncestralPositionId == AncestralPositionId);
                return View(AncestralObj);
            }

        }
        [HttpPost]
        public IActionResult Upsert(AncestralPosition AncestralObj)
        {
            if (ModelState.IsValid)
            {

                if (AncestralObj.AncestralPositionId == 0)
                {
                    _unitOfWork.Ancestral.Add(AncestralObj);
                }
                else
                {
                    _unitOfWork.Ancestral.Update(AncestralObj);
                }

                string strResult = _unitOfWork.Save();
                TempData["success"] = "新增/更新 成功" + strResult;
                return RedirectToAction("Index");
            }
            else
            {

                return View(AncestralObj);
            }
        }


        #region API CALLS


        // 2. Create a Server Endpoint to Receive the Data
        //In your controller(e.g., OrderController.cs), add an action to receive and save the data:
        [HttpPost]
        public IActionResult SavePositions([FromBody] string displaytext)
        {
            try
            {
                // 儲存到資料庫的邏輯
                string splitter_colon = ":";
                string splitter_side = "側";
                string splitter_section = "區";
                string splitter_level = "層";

                int colon_Index = displaytext.IndexOf(splitter_colon); //您已選取..左側-甲區-7層:099
                int sider_Index = displaytext.IndexOf(splitter_side);
                int section_Index = displaytext.IndexOf(splitter_section);
                int level_Index = displaytext.IndexOf(splitter_level);

                string side = displaytext.Substring(sider_Index - 1, 2); //側
                string section = displaytext.Substring(section_Index - 1, 2); ;//區
                string level = displaytext.Substring(section_Index+2,level_Index - 1- section_Index); ; //層
                string position = displaytext.Substring(colon_Index + 1);  //位
                //string sPositionId = floor + "-" + section + "-" + level + ":" + position;
                string sPositionId = displaytext;
                AncestralPosition AncestralPositionObj_chk = _unitOfWork.Ancestral.Get(u => u.PositionId == displaytext);
                //已存在
                if(AncestralPositionObj_chk != null)
                {
                    ViewBag.PostionExisted = "[新選取]的塔位已被使用,無法被變更";
                    TempData["success"] = "[新選取]的塔位已被使用,無法被變更";
                    //return RedirectToAction("Index");
                    return Json(new { success = false, message = "[新選取]的塔位已被使用,無法被變更!!" });
                }
                //未設定.可修改
                else
                {
                    //Select Obj of sAncestralPositionId
                    AncestralPosition AncestralPositionObj = _unitOfWork.Ancestral.Get(u => u.AncestralPositionId == SelectedPoistionId);
                    //Update PositionId
                    AncestralPositionObj.PositionId = sPositionId;
                    AncestralPositionObj.Side = side;
                    AncestralPositionObj.Section = section;
                    AncestralPositionObj.Level = level;
                    AncestralPositionObj.Position = position;

                    //do db_change:
                    _unitOfWork.Ancestral.Update(AncestralPositionObj);
                    string strResult = _unitOfWork.Save();
                    TempData["success"] = "牌位設定成功~" + strResult;
                    //return RedirectToAction("Index");
                    return Json(new { success = true });
                }
                    
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
            IEnumerable<AncestralPosition> objAncestralList;
            if (!string.IsNullOrWhiteSpace(search))
            {
                objAncestralList = _unitOfWork.Ancestral.GetAll(
                    filter: x => x.Name != null && x.Name.Contains(search)
                ).ToList();
            }
            else
            {
                objAncestralList = _unitOfWork.Ancestral.GetAll().ToList();
            }

            foreach (var item in objAncestralList)
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
            return Json(new { data = objAncestralList });
        }


        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var AncestralToBeDeleted = _unitOfWork.Ancestral.Get(u => u.AncestralPositionId == Id);
            if (AncestralToBeDeleted == null)
            {
                return Json(new { success = false, message = "刪除失敗" });
            }

            _unitOfWork.Ancestral.Remove(AncestralToBeDeleted);
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
                var entity = _unitOfWork.Ancestral.Get(x => x.AncestralPositionId == id);
                if (entity != null)
                    _unitOfWork.Ancestral.Remove(entity);
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
            var all = _unitOfWork.Ancestral.GetAll().ToList();
            foreach (var entity in all)
                _unitOfWork.Ancestral.Remove(entity);
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
//    public class AncestralController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }

//        /// <summary>
//        /// 宗祠牌位查詢 2025.05.12 17:43
//        /// </summary>
//        /// <returns></returns>
//        public IActionResult PositionQuery()
//        {
//            return View();
//        }
//    }
//}
