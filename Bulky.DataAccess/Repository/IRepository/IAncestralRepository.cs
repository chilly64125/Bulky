using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    //宗祠-牌位管理 2025 05 14 12:01
    public interface IAncestralRepository : IRepository<AncestralPosition>
    {
        void Update(AncestralPosition obj);
    }
}
