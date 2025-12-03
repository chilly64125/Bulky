using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace BulkyBookWeb.Areas.Identity.Pages.Survey
{
    public class SurveyListModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public SurveyListModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<SurveyResponse> Responses { get; set; } = new List<SurveyResponse>();

        public void OnGet()
        {
            Responses = _db.SurveyResponses
                .OrderByDescending(r => r.Id)
                .ToList();
        }
    }
}