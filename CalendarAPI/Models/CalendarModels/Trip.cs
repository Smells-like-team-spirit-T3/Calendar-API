using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarAPI.Models.CalendarModels
{
    public class Trip
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public int AmountParticipants { get; set; }
        public List<Event> Events { get; set; }
    }
}
