using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    //2025.08.21 16:20 問卷調查
    public interface ISurveyResponseRepository : IRepository<SurveyResponse>
    {
        void Update(SurveyResponse obj);
    }
}
