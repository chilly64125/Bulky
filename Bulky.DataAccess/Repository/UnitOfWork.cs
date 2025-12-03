using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }

        public ICompanyRepository Company { get; private set; }
        //懷恩塔-塔位管理 2025 05 15 14:21
        public IKindnessRepository Kindness { get; private set; }
        //宗祠-牌位管理 2025 05 15 14:21
        public IAncestralRepository Ancestral { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }  
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }
        public IEventRegistrationRepository EventRegistration { get; private set; }
        public ISurveyResponseRepository SurveyResponse { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ProductImage = new ProductImageRepository(_db);
            EventRegistration = new EventRegistrationRepository(_db);
            SurveyResponse = new SurveyResponseRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            //懷恩塔-塔位管理 2025 05 15 14:28
            Kindness = new KindnessRepository(_db);
            //宗祠-牌位管理 2025 05 15 14:21
            Ancestral = new AncestralRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);   
        }

        public string Save()
        {
            try
            {
                _db.SaveChanges();
                return "存檔成功!";
            }
            catch (Exception)
            {
                return "存檔失敗! 請洽系統管理員協助! UnitOfWork.Save()";
            }
           
        }
    }
}
