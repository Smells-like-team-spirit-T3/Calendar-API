using CalendarAPI.Controllers;
using CalendarAPI.Models;
using CalendarAPI.Models.CalendarModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CalendarAPI.Tests
{
    public class EventControllerTests
    {
        private readonly List<Event> events;
        private readonly Trip trip;

        public EventControllerTests()
        {
            events = new List<Event>()
            {
                new Event()
                {
                    Id = 0,
                    Description = "Description-0",
                    Duration = 5,
                    Cost = 1000,
                    Title = "Event-0",
                    Type = "Type-0",
                    StartDate = new DateTime(2001, 11, 21, 12, 0, 0),
                    EndDate = new DateTime(2001, 11, 21, 13, 0, 0)
                },
                new Event()
                {
                    Id = 1,
                    Description = "Description-1",
                    Duration = 3,
                    Cost = 500,
                    Title = "Event-0",
                    Type = "Type-0",
                    StartDate = new DateTime(2001, 11, 22, 12, 0, 0),
                    EndDate = new DateTime(2001, 11, 22, 13, 0, 0)
                }
            };

            trip = new Trip()
            {
                Events = events,
                AmountParticipants = 6,
                Cost = 2000,
                Id = 1,
                Title = "Trip-1",
                StartDate = new DateTime(2002, 12, 21),
                EndDate = new DateTime(2003, 1, 25)
            };
        }

        [Fact]
        public void GetEventsByGroupIdReturnsListOfEvents()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Trips.GetById(0)).Returns(trip);

            var controller = new EventController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetEventsByTripId(0).Result as OkObjectResult;
            var expected = events;

            // Assert
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void GetEventsByGroupIdReturnsCode404()
        {
            // Arrange
            trip.Events = new List<Event>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Trips.GetById(0)).Returns(trip);

            var controller = new EventController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetEventsByTripId(0).Result as NotFoundResult;
            var expected = controller.NotFound();

            // Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public void GetEventByIdReturnsEvent()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Events.GetById(0)).Returns(events[0]);

            var controller = new EventController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetEventById(0).Result as OkObjectResult;
            var expected = events[0];

            // Assert
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void GetEventByIdReturnsCode404()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Events.GetById(0)).Throws(new NullReferenceException());

            var controller = new EventController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetEventById(0).Result as NotFoundResult;
            var expected = controller.NotFound();

            // Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }
    }
}
