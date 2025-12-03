using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    //2025.08.21 16:20 問卷調查
    class SurveyResponseRepository : Repository<SurveyResponse>, ISurveyResponseRepository
    {
        private ApplicationDbContext _db;
        public SurveyResponseRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SurveyResponse obj)
        {
            _db.SurveyResponses.Update(obj);
        }
    }
}
