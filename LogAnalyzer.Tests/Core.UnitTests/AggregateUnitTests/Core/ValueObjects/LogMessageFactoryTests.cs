using FluentAssertions;
using LogAnalyzer.Core.Entities.LogEntryAggregate;
namespace LogAnalyzer.Tests.Core.UnitTests.AggregateUnitTests.Core.ValueObjects
{
    public class LogMessageFactoryTests
    {
        private const int MessageSanityLimit = 1000;

        [Fact]
        public void Create_WithValidString_ShouldReturnSuccess()
        {
            // Arrange
            var validText = "Database connection failed";

            // Act
            var result = LogMessage.Create(validText);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value!.Message.Should().Be(validText);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Create_WithEmptyOrNullString_ShouldReturnFailure(string? input)
        {
            // Act
            var result = LogMessage.Create(input!);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("message is empty");
        }

        [Fact]
        public void Create_WithTooLongString_ShouldReturnFailure()
        {
            // Arrange
            var longText = new string('a', MessageSanityLimit + 1);

            // Act
            var result = LogMessage.Create(longText);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("message is too long");
        }

        [Fact]
        public void Equals_WithSameContent_ShouldBeTrue()
        {
            // Arrange
            var text = "Error 404";
            var msg1 = LogMessage.Create(text).Value!;
            var msg2 = LogMessage.Create(text).Value!;

            // Act & Assert
            (msg1 == msg2).Should().BeTrue();
            msg1.Equals(msg2).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithDifferentContent_ShouldBeFalse()
        {
            // Arrange
            var msg1 = LogMessage.Create("Error A").Value!;
            var msg2 = LogMessage.Create("Error B").Value!;

            // Act & Assert
            (msg1 == msg2).Should().BeFalse();
            msg1.Equals(msg2).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_WithSameContent_ShouldBeIdentical()
        {
            // Arrange
            var text = "Consistent Hash";
            var msg1 = LogMessage.Create(text).Value!;
            var msg2 = LogMessage.Create(text).Value!;

            // Act & Assert
            msg1.GetHashCode().Should().Be(msg2.GetHashCode());
        }
    }
}
