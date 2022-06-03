using CalendarAPI.Models.CalendarModels;
using CalendarAPI.Models.Interfaces;
using CalendarAPI.Models.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarAPI.Models.Repositories
{
    public class TripRepository : Repository<Trip, string>, ITripRepository
    {
        private CalendarContext Context { get => context as CalendarContext; }

        public TripRepository(DbContext context) : base(context) { }

        public override IEnumerable<Trip> GetAll()
        {
            return context.Set<Trip>().Include(trip => trip.Events).ToList();
        }

        public override Trip GetById(string id)
        {
            var result = context.Set<Trip>().Include(trip => trip.Events).ToList().Find(t => t.Id == id);
            if (result == null)
            {
                throw new NullReferenceException();
            }
            return result;
        }
    }
}
