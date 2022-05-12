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
    public class EventController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; set; }

        public EventController(IUnitOfWork unitOfWork)
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

        // PUT api/<EventController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
