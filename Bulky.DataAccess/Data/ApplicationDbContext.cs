using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.Design;

namespace BulkyBook.DataAcess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        //2025.05.15 10:18 宗祠-祖先牌位管理
        public DbSet<AncestralPosition> AncestralPositions { get; set; }
        //2025.05.15 10:18 懷恩塔-祖先塔位管理
        public DbSet<KindnessPosition> KindnessPositions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        //2025.03.30 16:59 活動報名
        public DbSet<EventRegistration> EventRegistrations { get; set; }

        //2025.08.21 14:06 問卷調查
        public DbSet<SurveyResponse> SurveyResponses { get; set; }

                    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "祭祖、團拜 (新年、清明、中秋、冬至)", DisplayOrder = 1 },
                new Category { Id = 2, Name = "年度大掃除 (懷恩塔、純篤公墓園、宗祠)", DisplayOrder = 2 },
                new Category { Id = 3, Name = "參訪、春酒、尾牙", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "台中市銀同碧湖陳氏宗親會",
                    StreetAddress = "123 Tech St",
                    City = "Taichung City",
                    PostalCode = "12121",
                    State = "ROC",
                    PhoneNumber = "6669990000"
                },
                new Company
                {
                    Id = 2,
                    Name = "金門湖前陳氏宗親會",
                    StreetAddress = "999 Vid St",
                    City = "KinMen",
                    PostalCode = "66666",
                    State = "ROC",
                    PhoneNumber = "7779990000",
                },
                new Company
                {
                    Id = 3,
                    Name = "金門塔后陳氏宗親會\n",
                    StreetAddress = "999 Main St",
                    City = "KinMen",
                    PostalCode = "99999",
                    State = "ROC",
                    PhoneNumber = "1113335555"
                },
                new Company
                {
                    Id = 4,
                    Name = "台南學甲中洲陳桂記大宗祠\n",
                    StreetAddress = "999 Main St",
                    City = "Taina",
                    PostalCode = "99999",
                    State = "ROC",
                    PhoneNumber = "1113335555"
                },
                new Company
                {
                    Id = 5,
                    Name = "高雄巿學甲中洲陳桂記宗親會\n",
                    StreetAddress = "999 Main St",
                    City = "Kao-chung",
                    PostalCode = "99999",
                    State = "Kaoh",
                    PhoneNumber = "1113335555"
                }
                );


            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "正月初一新春祭祖團拜\r\n",
                    CompanyId = 1,
                    Description = "正月初一新春祭祖團拜\r\n\r\n(農曆正月初一,宗祠會館三樓). ",
                    ISBN = "正月初一新春祭祖團拜",
                    ListPrice = 0,
                    //Price = 0,
                    //Price50 = 0,
                    //Price100 = 0,

                    CategoryId = 1,
                    HDate = "正月初一"

                },
                new Product
                {
                    Id = 2,
                    Title = "清明節祭祖大典及聯誼餐會",
                    CompanyId = 1,
                    Description = "清明節祭祖大典及聯誼餐會.\r\n\r\n(清明節,宗祠會館). ",
                    ISBN = "清明節祭祖大典及聯誼餐會",
                    ListPrice = 600,
                    //Price = 600,
                    //Price50 = 600,
                    //Price100 = 600,

                    CategoryId = 1,
                    HDate = "清明節"

                },
                new Product
                {
                    Id = 3,
                    Title = "中秋節祭祖大典及聯誼餐會",
                    CompanyId = 1,
                    Description = "中秋節祭祖大典及聯誼餐會.\r\n\r\n(中秋節前一週日,宗祠會館). ",
                    ISBN = "中秋節祭祖大典及聯誼餐會",
                    ListPrice = 0,
                    //Price = 0,
                    //Price50 = 0,
                    //Price100 = 0,

                    CategoryId = 1,
                    HDate = "中秋節前一週日"

                },
                new Product
                {
                    Id = 4,
                    Title = "冬至搓湯圓活動",
                    CompanyId = 1,
                    Description = "冬至搓湯圓活動.\r\n\r\n(冬至前一週日或週末,宗祠會館). ",
                    ISBN = "冬至搓湯圓活動",
                    ListPrice = 0,
                    //Price = 0,
                    //Price50 = 0,
                    //Price100 = 0,

                    CategoryId = 1,
                    HDate = "冬至前一週日或週末"
                }
                ,
                new Product
                {
                    Id = 5,
                    Title = "台南學甲中洲大桂記大宗祠祭祖大典",
                    CompanyId = 4,
                    Description = "台南學甲中洲大桂記大宗祠祭祖大典.\r\n\r\n(中秋節後週日或週末). ",
                    ISBN = "台南學甲中洲大桂記大宗祠祭祖大典",
                    ListPrice = 0,
                    //Price = 0,
                    //Price50 = 0,
                    //Price100 = 0,
                    CategoryId = 1,
                    HDate = "中秋節後週日或週末"
                }
                ,
                new Product
                {
                    Id = 6,
                    Title = "金門祭祖大典",
                    CompanyId = 2,
                    Description = "金門祭祖大典.\r\n\r\n(冬至前一週日或週末). ",
                    ISBN = "金門祭祖大典",
                    ListPrice = 0,
                    //Price = 0,
                    //Price50 = 0,
                    //Price100 = 0,
                    CategoryId = 1,
                    HDate = "冬至前一週日或週末"

                },
                new Product
                {
                    Id = 7,
                    Title = "志工隊年度旅遊活動",
                    CompanyId = 1,
                    Description = "志工隊年度旅遊活動.\r\n\r\n(活動日期及費用依主辦單位公佈為主). ",
                    ISBN = "志工隊年度旅遊活動",
                    ListPrice = 0,
                    //Price = 0,
                    //Price50 = 0,
                    //Price100 = 0,
                    CategoryId = 1,
                    HDate = "依主辦單位公佈為主"

                }
                );

            modelBuilder.Entity<ProductImage>().HasData(
              new ProductImage
              { Id = 1, ImageUrl = "\\images\\products\\product-4\\caca0d62-34b0-47e4-bd22-3d4f439367cf.jpg", ProductId = 4 },
              new ProductImage
              { Id = 2, ImageUrl = "\\images\\products\\product-1\\06e30a51-4abe-4e4b-9f98-f4235bbd0ac9.jpg", ProductId = 1 },
              new ProductImage
              { Id = 3, ImageUrl = "\\images\\products\\product-2\\3dcd5a56-fac1-4730-a2f4-7c04baf99689.jpg", ProductId = 2 },
              new ProductImage
              { Id = 4, ImageUrl = "\\images\\products\\product-3\\142d5816-7199-491c-b606-ca3d1ed5d976.jpg", ProductId = 3 },
              new ProductImage
              { Id = 5, ImageUrl = "\\images\\products\\product-4\\bc8e2d1b-25d6-4174-b687-a556d871d0d5.jpg", ProductId = 4 },
              new ProductImage
              { Id = 6, ImageUrl = "\\images\\products\\product-2\\f8bb9335-585b-4ae1-9e3c-6f2cd0af5b4e.jpg", ProductId = 2 },
              new ProductImage
              { Id = 7, ImageUrl = "\\images\\products\\product-1\\cef81a2a-95fa-4c01-8afb-4cfabba061db.jpg", ProductId = 1 },
               new ProductImage
               { Id = 8, ImageUrl = "\\images\\products\\product-5\\12eb4930-cf74-4f69-9092-c10e07703d77.jpg", ProductId = 5 },
               new ProductImage
               { Id = 9, ImageUrl = "\\images\\products\\product-5\\eb17ea7a-91f4-43b5-bc47-58b77559fd8c.jpg", ProductId = 5 },
              new ProductImage
              { Id = 10, ImageUrl = "\\images\\products\\product-5\\80534b19-a460-49da-b989-cdaabeb322f2.jpg", ProductId = 5 },
              new ProductImage
              { Id = 12, ImageUrl = "\\images\\products\\product-6\\8951128a-5f5a-40cd-bb98-7a44eace559c.jpg", ProductId = 6 },
               new ProductImage
               { Id = 13, ImageUrl = "\\images\\products\\product-6\\f207d7b2-bef7-47a4-b92e-d5e064d07969.jpg", ProductId = 6 },
               new ProductImage
               { Id = 14, ImageUrl = "\\images\\products\\product-6\\b24faf65-a7a8-4a7f-a6d6-111596bfe469.jpg", ProductId = 6 },
               new ProductImage
               { Id = 15, ImageUrl = "\\images\\products\\product-7\\5be2d6ab-cddf-40fe-b56d-7bf4df9a7744.jpg", ProductId = 7 },
               new ProductImage
               { Id = 16, ImageUrl = "\\images\\products\\product-7\\8434a454-da5b-496f-8bfc-773e2b9df6d7.jpg", ProductId = 7 }
             );
        }
       }
}
