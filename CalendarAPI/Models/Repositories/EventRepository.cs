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
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private CalendarContext Context { get => context as CalendarContext; }

        public EventRepository(DbContext context) : base(context) { }
    }
}
