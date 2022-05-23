using CalendarAPI.Controllers;
using CalendarAPI.Models;
using CalendarAPI.Models.CalendarModels;
using CalendarAPI.Models.Interfaces;
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
            var controller = new TripsController(mockUnitOfWork.Object);

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
            var controller = new TripsController(mockUnitOfWork.Object);

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
            var controller = new TripsController(mockUnitOfWork.Object);

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
            var controller = new TripsController(mockUnitOfWork.Object);

            // Act
            var result = controller.GetTripById(0).Result as NotFoundResult;

            // Assert
            Assert.Equal(controller.NotFound().StatusCode, result.StatusCode);
        }

        [Fact]
        public void AddTripGetsTripAddsToDbReturnsAddedTrip()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.SetupGet(unit => unit.Trips).Returns(new Mock<ITripRepository>().Object);
            var controller = new TripsController(mockUnitOfWork.Object);

            // Act
            var result = controller.AddTrip(trips[0]).Result as OkObjectResult;
            var expected = trips[0];

            // Assert
            Assert.Equal(expected, result.Value);
            mockUnitOfWork.Verify(mock => mock.Trips.Add(trips[0]));
            mockUnitOfWork.Verify(mock => mock.Save());
        }

        [Fact]
        public void AddTripGetsNullReturnsCode400()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTripRepository = new Mock<ITripRepository>();
            mockUnitOfWork.SetupGet(unit => unit.Trips).Returns(mockTripRepository.Object);

            var controller = new TripsController(mockUnitOfWork.Object);

            // Act
            var result = controller.AddTrip(null).Result as BadRequestResult;
            var expected = controller.BadRequest();

            // Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public void ChangeTripGetsTripReturnsChangedTrip()
        {
            // Arrange
            trips[1].Id = 0;

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Trips.GetById(trips[1].Id)).Returns(trips[0]);

            var controller = new TripsController(mockUnitOfWork.Object);

            // Act
            var okObject = controller.ChangeTrip(trips[1]).Result as OkObjectResult;
            var result = okObject.Value as Trip;
            var expected = trips[1];

            // Assert
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Title, result.Title);
            Assert.Equal(expected.Cost, result.Cost);
            Assert.Equal(expected.AmountParticipants, result.AmountParticipants);
            Assert.Equal(expected.EndDate, result.EndDate);
            Assert.Equal(expected.StartDate, result.StartDate);
        }

        [Fact]
        public void ChangeTripGetsNullReturnsCode400()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockTripRepository = new Mock<ITripRepository>();
            mockUnitOfWork.SetupGet(unit => unit.Trips).Returns(mockTripRepository.Object);

            var controller = new TripsController(mockUnitOfWork.Object);

            // Act
            var result = controller.ChangeTrip(null).Result as BadRequestResult;
            var expected = controller.BadRequest();

            // Assert
            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public void DeleteTripGetsExistingIdReturnsCode204()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Trips.GetById(0)).Returns(trips[0]);

            var controller = new TripsController(mockUnitOfWork.Object);

            // Act
            var result = controller.DeleteTrip(0) as NoContentResult;
            var expected = controller.NoContent();

            // Assert
            Assert.Equal(expected.StatusCode, expected.StatusCode);
        }

        [Fact]
        public void DeleteTripGetsWrongIdReturnsCode400()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(unit => unit.Trips.GetById(10)).Throws(new NullReferenceException());

            var controller = new TripsController(mockUnitOfWork.Object);

            // Act
            var result = controller.DeleteTrip(10) as BadRequestResult;
            var expected = controller.BadRequest();

            // Assert
            Assert.Equal(expected.StatusCode, expected.StatusCode);
        }
    }
}
