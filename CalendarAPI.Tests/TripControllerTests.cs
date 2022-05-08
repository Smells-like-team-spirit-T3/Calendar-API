using CalendarAPI.Controllers;
using CalendarAPI.Models;
using CalendarAPI.Models.CalendarModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CalendarAPI.Tests
{
    public class TripControllerTests
    {
        private readonly List<Trip> trips;

        public TripControllerTests()
        {
            trips = new List<Trip>()
            {
                new Trip()
                {
                    Events = new List<Event>(),
                    AmountParticipants = 5,
                    Cost = 1000,
                    Id = 0,
                    Title = "Trip-0",
                    StartDate = new DateTime(2001, 11, 21),
                    EndDate = new DateTime(2001, 12, 25)
                },
                new Trip()
                {
                    Events = new List<Event>(),
                    AmountParticipants = 6,
                    Cost = 2000,
                    Id = 1,
                    Title = "Trip-1",
                    StartDate = new DateTime(2002, 12, 21),
                    EndDate = new DateTime(2003, 1, 25)
                }
            };
        }

        [Fact]
        public void GetAllTripsReturnsAllTrips()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(unit => unit.Trips.GetAll()).Returns(trips);
            var controller = new TripController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetAllTrips().Result as OkObjectResult;
            var expected = trips;

            // Assert
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void GetAllTripsReturnsCode404()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(unit => unit.Trips.GetAll()).Returns(new List<Trip>());
            var controller = new TripController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetAllTrips().Result as NotFoundResult;

            // Assert
            Assert.Equal(controller.NotFound().StatusCode, result.StatusCode);
        }

        [Fact]
        public void GetTripByIdReturnsRequestedTrip()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(unit => unit.Trips.GetById(0)).Returns(trips[0]);
            var controller = new TripController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetTripById(0).Result as OkObjectResult;
            var expected = trips[0];

            // Assert
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void GetTripByIdReturnsCode404()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(unit => unit.Trips.GetById(0)).Throws(new NullReferenceException());
            var controller = new TripController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetTripById(0).Result as NotFoundResult;

            // Assert
            Assert.Equal(controller.NotFound().StatusCode, result.StatusCode);
        }
    }
}
