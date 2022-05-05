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
    public class TripRepository : Repository<Trip>, ITripRepository
    {
        private CalendarContext Context { get => context as CalendarContext; }

        public TripRepository(DbContext context) : base(context) { }
    }
}
