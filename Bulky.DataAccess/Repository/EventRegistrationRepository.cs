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
    class EventRegistrationRepository : Repository<EventRegistration>, IEventRegistrationRepository
    {
        private ApplicationDbContext _db;
        public EventRegistrationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(EventRegistration obj)
        {
            _db.EventRegistrations.Update(obj);
        }
    }
}
