using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarAPI.Models.CalendarModels
{
    public class Trip
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Cost { get; set; }
        public int AmountParticipants { get; set; }
        public virtual List<Event> Events { get; set; } = new List<Event>();
    }
}
