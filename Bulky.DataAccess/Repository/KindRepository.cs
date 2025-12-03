using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{

    //懷恩塔-塔位管理 2025 05 15 14:14
    public class KindnessRepository : Repository<KindnessPosition>, IKindnessRepository
    {
        private ApplicationDbContext _db;
        public KindnessRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(KindnessPosition obj)
        {
            var objFromDb = _db.KindnessPositions.FirstOrDefault(u => u.KindnessPositionId == obj.KindnessPositionId);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Applicant = obj.Applicant;
                if(obj.Section != null)
                {
                    objFromDb.Section = obj.Section;
                }
                if(obj.Floor != null)
                {
                    objFromDb.Floor = obj.Floor;
                }
                if(obj.Level != null)
                {
                    objFromDb.Level = obj.Level;
                }
                if(obj.Position != null)
                {
                    objFromDb.Position = obj.Position;
                }
                if (obj.PositionId !="0樓-0區-0層:000")
                {
                    objFromDb.PositionId = obj.PositionId;
                }              
                objFromDb.Mobile_Tel = obj.Mobile_Tel;
                objFromDb.Relation = obj.Relation;               
            }
        }


    }
}
