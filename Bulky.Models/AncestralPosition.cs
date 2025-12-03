using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

    //宗祠-牌位管理 2025 05 14 12:01
    public class AncestralPosition
    {
        [Key]
        public int AncestralPositionId { get; set; }
        [Required]
        [DisplayName("祖先名諱")]
        public string? Name { get; set; }

        [DisplayName("側(1: 左L | 2: 右R)")]
        public string? Side { get; set; }

        [DisplayName("區(1 ~ 10)")]
        public string? Section { get; set; }

        [DisplayName("層(1 ~ 10)")]
        public string? Level { get; set; }

        [DisplayName("編號(1 ~ 10)")]
        public string? Position { get; set; }

        [DisplayName("連絡人姓名")]
        public string? Applicant { get; set; }

        [DisplayName("連絡人-關係")]
        public string? Relation { get; set; }

        [DisplayName("手機-巿話")]
        public string? Mobile_Tel { get; set; }

        [DisplayName("費用")]
        public int? Price { get; set; }
        [DisplayName("祖先牌位")]
        public string? PositionId { get; set; } = "0側-0區-0層:000";

        [DisplayName("備註")]
        public string? Note { get; set; }

    }

}
