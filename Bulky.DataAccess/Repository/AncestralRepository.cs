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

    //宗祠-牌位管理 2025 05 15 14:15
    public class AncestralRepository : Repository<AncestralPosition>, IAncestralRepository 
        {
        private ApplicationDbContext _db;
        public AncestralRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        

        public void Update(AncestralPosition obj)
        {
            var objFromDb = _db.AncestralPositions.FirstOrDefault(u => u.AncestralPositionId == obj.AncestralPositionId);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Applicant = obj.Applicant;
                if (obj.Section != null)
                {
                    objFromDb.Section = obj.Section;
                }
                if (obj.Side != null)
                {
                    objFromDb.Side = obj.Side;
                }
                if (obj.Level != null)
                {
                    objFromDb.Level = obj.Level;
                }
                if (obj.Position != null)
                {
                    objFromDb.Position = obj.Position;
                }
                if (obj.PositionId != "0側-0區-0層:000")
                {
                    objFromDb.PositionId = obj.PositionId;
                }
                objFromDb.Mobile_Tel = obj.Mobile_Tel;
                objFromDb.Relation = obj.Relation;
            }
        }
    }
}
