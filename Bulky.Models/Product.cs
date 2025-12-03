using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyBook.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("名稱")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [DisplayName("說明")]
        public string Description { get; set; } = string.Empty;
        [Required]
        [DisplayName("活動簡介")]
        public string ISBN { get; set; } = string.Empty;
        [Required]
        [DisplayName("主辧單位")]
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company? Company { get; set; }
        [Display(Name = "活動日期")]
        public string? HDate { get; set; }
        [Display(Name = "是否舉辦(是Y/否N)")]
        public char HeldYN { get; set; } = 'N';
        [Required]
        [Display(Name = "一般費用")]
        [Range(0, 1000000)]
        public double ListPrice { get; set; }
        [Display(Name = "活動類別")]     
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }

        [Display(Name = "上傳DM照片")]

        [ValidateNever]
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
