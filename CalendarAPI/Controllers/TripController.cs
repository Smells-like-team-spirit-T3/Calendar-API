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
    public class TripController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; set; }

        public TripController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        // GET: api/<TripController>
        [HttpGet]
        public ActionResult<IEnumerable<Trip>> GetAllTrips()
        {
            var result = UnitOfWork.Trips.GetAll();
            if (result.ToList().Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET api/<TripController>/5
        [HttpGet("{id}")]
        public ActionResult<Trip> GetTripById(int id)
        {
            try
            {
                return Ok(UnitOfWork.Trips.GetById(id));
            }
            catch (NullReferenceException e)
            {
                return NotFound();
            } 
        }

        // POST api/<TripController>
        [HttpPost]
        public ActionResult<Trip> AddTrip([FromBody] Trip trip)
        {
            throw new NotImplementedException();
        }

        // PUT api/<TripController>/5
        [HttpPut("{id}")]
        public ActionResult<Trip> ChangeTrip([FromBody] Trip trip)
        {
            throw new NotImplementedException();
        }

        // DELETE api/<TripController>/5
        [HttpDelete("{id}")]
        public ActionResult DeleteTrip(int id)
        {
            throw new NotImplementedException();
        }
    }
}
