using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models {
    public class OrderDetail {
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        [ValidateNever]
        public OrderHeader? OrderHeader { get; set; }


        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

        [DisplayName("80歲以上長者")]
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Senior80 { get; set; }

        [DisplayName("志工")]
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Volunteer { get; set; }

        [DisplayName("素食")]
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Vegetarian { get; set; }

        [DisplayName("成年禮")]
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int PreAdult { get; set; }

        [DisplayName("桌")]
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Table { get; set; }

        [DisplayName("單人")]
        [Range(1, 10, ErrorMessage = "請輸入1~10之間數字")]
        public int Count { get; set; }
        [DisplayName("報名費")]
        [Range(1, 10, ErrorMessage = "請輸入1~10000之間數字")]
        public double Price { get; set; }

    }
}
