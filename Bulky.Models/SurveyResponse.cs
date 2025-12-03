using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Graph.Models.TermStore;
using Microsoft.Kiota.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class SurveyResponse
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "請選擇登入頁出現時間")]
        public string LoginTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "請選擇使用次數")]
        public string UsageCount { get; set; } = string.Empty;

        [Required(ErrorMessage = "請選擇遇到訊息次數")]
        public string ErrorCount { get; set; } = string.Empty;

        [Required(ErrorMessage = "請選擇您所在洲別(亞洲、歐洲、北美洲,中美洲,南美洲,非洲,澳洲")]
        public string? Continent { get; set; }
        //public DateTime? SubmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAt { get; set; } =
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
        //•	Store UTC in the database.
        //•	Convert to China Standard Time using TimeZoneInfo when displaying or processing.
    }
}