using CalendarAPI.Models;
using CalendarAPI.Models.CalendarModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CalendarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; set; }

        public EventsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("trip")]
        public ActionResult<IEnumerable<Event>> GetEventsByTripId([FromQuery]int id)
        {
            var trip = UnitOfWork.Trips.GetById(id);
            if (trip.Events.Count == 0)
            {
                return NotFound();
            }
            return Ok(trip.Events);
        }

        [HttpGet("{id}")]
        public ActionResult<Event> GetEventById(int id)
        {
            try
            {
                return Ok(UnitOfWork.Events.GetById(id));
            }
            catch (NullReferenceException e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Event> AddEventToTrip([FromQuery] int id, [FromBody] Event value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            UnitOfWork.Events.Add(value);
            var targetTrip = UnitOfWork.Trips.GetById(id);
            targetTrip.Events.Add(value);
            UnitOfWork.Save();
            return Ok(value);
        }

        [HttpPut]
        public ActionResult<Event> ChangeEvent([FromBody] Event value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var eventToChange = UnitOfWork.Events.GetById(value.Id);
            eventToChange.Title = value.Title;
            eventToChange.Description = value.Description;
            eventToChange.Cost = value.Cost;
            eventToChange.Type = value.Type;
            eventToChange.StartDate = value.StartDate;
            eventToChange.EndDate = value.EndDate;
            UnitOfWork.Save();

            return Ok(eventToChange);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEvent(int id)
        {
            Event eventToDelete;
            try
            {
                eventToDelete = UnitOfWork.Events.GetById(id);
            }
            catch (NullReferenceException e)
            {
                return BadRequest();
            }
            UnitOfWork.Events.Remove(eventToDelete);
            UnitOfWork.Save();
            return NoContent();
        }
    }
}
