using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        //懷恩塔-塔位管理 2025 05 14 12:01
        IKindnessRepository Kindness { get; }
        //宗祠-牌位管理 2025 05 14 12:01
        IAncestralRepository Ancestral { get; }

        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IProductImageRepository ProductImage { get; }
        IEventRegistrationRepository EventRegistration { get; }

        //2025.08.21 16:20 問卷調查
        ISurveyResponseRepository SurveyResponse { get; }
        string Save();

    }
}
