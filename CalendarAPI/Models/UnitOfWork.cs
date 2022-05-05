using CalendarAPI.Models.Interfaces;
using CalendarAPI.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarAPI.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private CalendarContext Context { get; }
        public ITripRepository Trips { get; }
        public IEventRepository Events { get; }
        public UnitOfWork(CalendarContext context)
        {
            Context = context;
            Trips = new TripRepository(context);
            Events = new EventRepository(context);
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
