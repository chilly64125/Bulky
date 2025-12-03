using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{

    //懷恩塔-塔位管理 2025 05 15 12:21
    public interface IKindnessRepository : IRepository<KindnessPosition>
    {
        void Update(KindnessPosition obj);
    }
}
