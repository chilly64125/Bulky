using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models {
    public class ShoppingCart {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }  
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Count { get; set; }
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Table { get; set; }
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Senior80 { get; set; }
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Volunteer { get; set; }
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Vegetarian { get; set; }
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int PreAdult { get; set; }

        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser? ApplicationUser { get; set; }

		[NotMapped]
		public double Price { get; set; }
	}
}
