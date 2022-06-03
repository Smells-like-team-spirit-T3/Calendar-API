using CalendarAPI.Models.CalendarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarAPI.Models.Interfaces
{
    public interface ITripRepository : IRepository<Trip, string>
    {
    }
}
