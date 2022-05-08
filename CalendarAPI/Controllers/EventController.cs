using CalendarAPI.Models;
using CalendarAPI.Models.CalendarModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<EventController>
        [HttpGet("{id}")]
        [Route("trip")]
        public ActionResult<IEnumerable<Event>> GetEventsByTripId(int id)
        {
            var trip = UnitOfWork.Trips.GetById(id);
            if (trip.Events.Count == 0)
            {
                return NotFound();
            }
            return Ok(trip.Events);
        }

        // GET api/<EventController>/5
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

        // POST api/<EventController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
