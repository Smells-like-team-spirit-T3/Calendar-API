using CalendarAPI.Models.CalendarModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarAPI.Models
{
    public class CalendarContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Trip> Trips { get; set; }

        public CalendarContext(DbContextOptions<CalendarContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
