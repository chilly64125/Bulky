using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
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
    /// <summary>
    /// 活動報名基本檔
    /// 2025.03.30 16:26
    /// </summary>

    [Index(nameof(ProductId), nameof(UserId), IsUnique = true)]
    public class EventRegistration
    {

        [Key]
        [Display(Name = "報名編號")]
        public int Id { get; set; }
        [Display(Name = "活動名稱")]
        [Required]
        public int EventId { get; set; }

        [Display(Name = "報名人")]
        public string? UserId { get; set; }

        ////[Display(Name = "報名姓名")]
        //public string UserName { get; set; }
        //[Display(Name = "活動日期")]
        //public DateTime HDate { get; set; }

        [Display(Name = "付款日")]
        public DateTime ?PaymentTime { get; set; }
        [Display(Name = "報名日")]
        public string RegistrationTime { get; set; } =default(DateTime).ToString("yyyy-mm-dd HH:mm:ss");
        [Display(Name = "個人")]
        public int Count { get; set; }
        [Display(Name = "桌數")]
        [Range(0, 10, ErrorMessage = "請輸入0~10之間數字")]
        public int Table { get; set; }

        [Display(Name = "80歲以上")]
        [Range(0, 10, ErrorMessage = "請輸入0~10之間數字")]
        public int Senior80 { get; set; }

        [Display(Name = "素食")]
        [Range(0, 10, ErrorMessage = "請輸入0~10之間數字")]
        public int Vegetarian { get; set; }

        [Display(Name = "志工")]
        [Range(0, 10, ErrorMessage = "請輸入0~10之間數字")]
        public int Volunteer { get; set; }

        [Display(Name = "成年禮")]
        [Range(0, 10, ErrorMessage = "請輸入0~10之間數字")]
        public int PreAdult { get; set; }

        [Display(Name = "總人數")]
        public int TotalNumberJoined { get; set; }

        [Display(Name = "總費用")]
        public int PaymentAmount { get; set; }
        [Display(Name = "活動")]
        [Required]

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

    }
}
