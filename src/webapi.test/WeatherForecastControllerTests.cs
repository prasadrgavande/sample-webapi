using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using webapi.Controllers;
using Xunit;
namespace webapi.test
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void Get_ReturnWetherForcastController()
        {
            var logger = new NullLogger<WeatherForecastController>();


            //arrange
            var controller = new WeatherForecastController(logger);

            //act
            var result = controller.Get();

            //assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public void Get_ReturnsWeatherForecasts()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(mockLogger.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
            Assert.All(result, forecast =>
            {
                Assert.NotNull(forecast.Summary);
                Assert.True(forecast.TemperatureC >= -20 && forecast.TemperatureC <= 55);
            });
        }

        [Fact]
        public void Get_ReturnsCorrectNumberOfForecasts()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(mockLogger.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.Equal(5, result.Count());
        }
    }
}
