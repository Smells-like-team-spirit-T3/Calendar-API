﻿using CalendarAPI.Models;
using CalendarAPI.Models.CalendarModels;
using CalendarAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CalendarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private IIdService IdGenerator { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }

        public TripsController(IUnitOfWork unitOfWork, IIdService idGenerator)
        {
            UnitOfWork = unitOfWork;
            IdGenerator = idGenerator;
        }

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

        [HttpGet("{id}")]
        public ActionResult<Trip> GetTripById(string id)
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

        [HttpPost]
        public ActionResult<Trip> AddTrip([FromBody] Trip trip)
        {
            if (trip == null)
            {
                return BadRequest();
            }
            trip.Id = IdGenerator.Get9DigitId();
            UnitOfWork.Trips.Add(trip);
            UnitOfWork.Save();
            return Ok(trip);
        }

        [HttpPut]
        public ActionResult<Trip> ChangeTrip([FromBody] Trip trip)
        {
            if (trip == null)
            {
                return BadRequest();
            }
            var tripToChange = UnitOfWork.Trips.GetById(trip.Id);
            tripToChange.Cost = trip.Cost;
            tripToChange.StartDate = trip.StartDate;
            tripToChange.EndDate = trip.EndDate;
            tripToChange.AmountParticipants = trip.AmountParticipants;
            tripToChange.Title = trip.Title;
            UnitOfWork.Save();

            return Ok(tripToChange);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTrip(string id)
        {
            Trip trip;
            try
            {
                trip = UnitOfWork.Trips.GetById(id);
            }
            catch (NullReferenceException e)
            {
                return BadRequest();
            }
            UnitOfWork.Trips.Remove(trip);
            UnitOfWork.Save();

            return NoContent();
        }
    }
}
