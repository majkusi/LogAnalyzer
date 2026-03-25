using FluentAssertions;
using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.LogEntryAggregate;
using LogAnalyzer.Core.Entities.LogGroupAggregate;
using LogAnalyzer.Core.Events;

namespace LogAnalyzer.Tests.Core.UnitTests.AggregateUnitTests.Core.Aggregates.LogGroupAggregateTests
{
    public class LogGroupBusinessLogicTests
    {
        private readonly LogMessage message = LogMessage.Create("Database connection failed").Value!;
        private readonly Guid firstLogId = Guid.NewGuid();
        private readonly DateTime timestamp = new DateTime(2026, 03, 25, 10, 0, 0);
        private readonly Guid secondLogId = Guid.NewGuid();
        public Result<LogGroup> CreateLogGroup()
        {
            var result = LogGroup.Create(message, firstLogId, timestamp);

            return result;
        }
        [Fact]
        public void AddLog_WithMatchingMessage_ShouldRaiseLogAddedEventToLogGroupEvent()
        {
            // Arrange
            var logGroup = CreateLogGroup().Value!;
            logGroup.ClearDomainEvents();
            //Act 
            logGroup.AddLog(secondLogId, message, timestamp.AddMinutes(5));

            //Assert
            logGroup.DomainEvents.Should().ContainSingle(e => e is LogAddedToLogGroupEvent)
                .Which.Should().BeEquivalentTo(new LogAddedToLogGroupEvent(secondLogId, logGroup.Id));
        }

        [Fact]
        public void AddLog_WithMatchingMessage_ShouldIncreaseCount()
        {
            var logGroup = CreateLogGroup().Value!;

            var newMessage = LogMessage.Create("Database connection failed").Value!;
            var newGuid = Guid.NewGuid();
            logGroup.AddLog(newGuid, newMessage, timestamp.AddMinutes(5));

            logGroup.count.Should().Be(2);
        }

        [Fact]
        public void AddLog_WithMatchingMessage_ShouldUpdateLastOccurrence()
        {
            var logGroup = CreateLogGroup().Value!;
            var newMessage = LogMessage.Create("Database connection failed").Value!;
            var newGuid = Guid.NewGuid();
            var newTimestamp = timestamp.AddMinutes(5);
            logGroup.AddLog(newGuid, newMessage, newTimestamp);

            logGroup.lastOccurence.Should().Be(newTimestamp);
        }

        [Fact]
        public void AddLog_WithMatchingMessage_ShouldAddLogIdToCollection()
        {
            var logGroup = CreateLogGroup().Value!;
            var newMessage = LogMessage.Create("Database connection failed").Value!;
            var newGuid = Guid.NewGuid();
            var newTimestamp = timestamp.AddMinutes(5);
            logGroup.AddLog(newGuid, newMessage, newTimestamp);

            logGroup.Logs.Should().EndWith(newGuid);
        }

        [Fact]
        public void Add_Log_WithEmptyLogId_ShouldThrowArgumentException()
        {
            var logGroup = CreateLogGroup().Value!;
            var newMessage = LogMessage.Create("Database connection failed").Value!;
            var newGuid = Guid.Empty;
            var newTimestamp = timestamp.AddMinutes(5);
            Action act = () => logGroup.AddLog(newGuid, newMessage, newTimestamp);

            act.Should().Throw<ArgumentNullException>().WithParameterName("logId");
        }
        [Fact]
        public void AddLog_WithMismatchedMessage_ShouldThrowArgumentException()
        {
            var logGroup = CreateLogGroup().Value!;
            var newMessage = LogMessage.Create("Missmatched message").Value!;
            var newGuid = Guid.NewGuid();
            var newTimestamp = timestamp.AddMinutes(5);
            Action act = () => logGroup.AddLog(newGuid, newMessage, newTimestamp);
            act.Should().Throw<ArgumentException>().WithMessage("Log message does not match LogGroup message!");
        }
    }
}
