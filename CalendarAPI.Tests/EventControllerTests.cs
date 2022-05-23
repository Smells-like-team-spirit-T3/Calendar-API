using CalendarAPI.Controllers;
using CalendarAPI.Models;
using CalendarAPI.Models.CalendarModels;
using CalendarAPI.Models.Interfaces;
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

            var controller = new EventsController(mockUnitOfWork.Object);

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

            var controller = new EventsController(mockUnitOfWork.Object);

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

            var controller = new EventsController(mockUnitOfWork.Object);

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

            var controller = new EventsController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetEventById(0).Result as NotFoundResult;
            var expected = controller.NotFound();

            // Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public void AddEventToTripGetsEventAndTripIdAddsEventReturnsAddedEvent()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTripRepository = new Mock<ITripRepository>();
            mockTripRepository.Setup(repo => repo.GetById(0)).Returns(trip);
            mockUnitOfWork.SetupGet(unit => unit.Trips).Returns(mockTripRepository.Object);

            var controller = new EventsController(mockUnitOfWork.Object);

            // Act
            var result = controller.AddEventToTrip(0, events[0]).Result as OkObjectResult;
            var expected = events[0];

            // Assert
            Assert.Equal(expected, result.Value);
            mockTripRepository.Verify(mock => mock.GetById(0));
            mockUnitOfWork.Verify(mock => mock.Save());
        }

        [Fact]
        public void AddEventToTripGetsNullReturnsCode400()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTripRepository = new Mock<ITripRepository>();
            mockUnitOfWork.SetupGet(unit => unit.Trips).Returns(mockTripRepository.Object);

            var controller = new EventsController(mockUnitOfWork.Object);

            // Act
            var result = controller.AddEventToTrip(0,null).Result as BadRequestResult;
            var expected = controller.BadRequest();

            // Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public void ChangeEventGetsEventReturnsChangedEvent()
        {
            // Arrange
            events[1].Id = 0;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Events.GetById(events[1].Id)).Returns(events[0]);

            var controller = new EventsController(mockUnitOfWork.Object);

            // Act
            var okObject = controller.ChangeEvent(events[1]).Result as OkObjectResult;
            var result = okObject.Value as Event;
            var expected = events[1];

            // Assert
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Title, result.Title);
            Assert.Equal(expected.Cost, result.Cost);
            Assert.Equal(expected.Description, result.Description);
            Assert.Equal(expected.EndDate, result.EndDate);
            Assert.Equal(expected.StartDate, result.StartDate);
            Assert.Equal(expected.Type, result.Type);
        }

        [Fact]
        public void ChangeEventGetsNullReturnsCode400()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockEventRepository = new Mock<IEventRepository>();
            mockUnitOfWork.SetupGet(unit => unit.Events).Returns(mockEventRepository.Object);

            var controller = new EventsController(mockUnitOfWork.Object);

            // Act
            var result = controller.ChangeEvent(null).Result as BadRequestResult;
            var expected = controller.BadRequest();

            // Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public void DeleteEventGetsExistingIdReturnsCode204()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Events.GetById(0)).Returns(events[0]);

            var controller = new EventsController(mockUnitOfWork.Object);

            // Act
            var result = controller.DeleteEvent(0) as NoContentResult;
            var expected = controller.NoContent();

            // Assert
            Assert.Equal(expected.StatusCode, expected.StatusCode);
        }

        [Fact]
        public void DeleteEventGetsWrongIdReturnsCode400()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Events.GetById(10)).Throws(new NullReferenceException());

            var controller = new EventsController(mockUnitOfWork.Object);

            // Act
            var result = controller.DeleteEvent(10) as BadRequestResult;
            var expected = controller.BadRequest();

            // Assert
            Assert.Equal(expected.StatusCode, expected.StatusCode);
        }
    }
}
