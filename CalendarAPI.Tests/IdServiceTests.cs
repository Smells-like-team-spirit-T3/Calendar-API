using CalendarAPI.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CalendarAPI.Tests
{
    public class IdServiceTests
    {
        [Fact]
        public void Get9DigitIdReturns10UniqIds()
        {
            // Arrange
            var service = new IdService();

            // Act
            var result = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                result.Add(service.Get9DigitId());
            }

            // Assert
            for (var i = 0; i < result.Count; i++)
            {
                for (var j = i+1; j < result.Count; j++)
                {
                    Assert.NotEqual(result[i], result[j]);
                }
            }
        }
    }
}
