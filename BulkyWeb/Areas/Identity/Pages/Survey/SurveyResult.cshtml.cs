using BulkyBook.Models;
using BulkyBook.DataAcess.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Identity.Pages.Survey
{
    public class SurveyResultModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public SurveyResultModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<SurveyResponse> Responses { get; set; } = new List<SurveyResponse>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10; // Results per page

        [TempData]
        public string SuccessMessage { get; set; }

        public void OnGet(int page = 1)
        {
            SuccessMessage = TempData["Success"] as string ?? "問卷提交成功！";
        }
        // DataTables AJAX endpoint
        public JsonResult OnGetSurveyData()
        {
            var data = _db.SurveyResponses
                .OrderByDescending(r => r.SubmittedAt)
                .Select(r => new {
                    r.Id,
                    r.LoginTime,
                    r.UsageCount,
                    r.ErrorCount,
                    r.Continent,
                    SubmittedAt = r.SubmittedAt.HasValue ? r.SubmittedAt.Value.ToString("yyyy-MM-dd HH:mm") : ""
                })
                .ToList();

            return new JsonResult(new { data });
        }
    }
}

