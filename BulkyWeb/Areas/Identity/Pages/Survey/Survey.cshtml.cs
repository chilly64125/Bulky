using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBookWeb.Areas.Identity.Pages.Survey
{
    public class SurveyModel : PageModel
    {

        private readonly ApplicationDbContext _db;
   
        public SurveyModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public new SurveyResponse Response { get; set; } = new();

        public void OnGet() {
        
          }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
         
            TempData["Success"] = "";
            _db.SurveyResponses.Add(Response);
            _db.SaveChanges();
            TempData["Success"] = "問題填寫成功! 謝謝幫忙!";
            return Redirect("/Identity/Survey/SurveyResult"); // Internal URL
            //return RedirectToPage("/Index1");
        }
    }
}