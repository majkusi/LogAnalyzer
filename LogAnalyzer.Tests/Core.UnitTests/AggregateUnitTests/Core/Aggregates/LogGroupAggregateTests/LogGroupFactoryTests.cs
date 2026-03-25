using FluentAssertions;
using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.LogEntryAggregate;
using LogAnalyzer.Core.Entities.LogGroupAggregate;
using LogAnalyzer.Core.Events;

namespace LogAnalyzer.Tests.Core.UnitTests.AggregateUnitTests.Core.Aggregates.LogGroupAggregateTests
{

    public class LogGroupFactoryTests
    {
        private readonly LogMessage message = LogMessage.Create("Database connection failed").Value!;
        private readonly Guid firstLogId = Guid.NewGuid();
        private readonly DateTime timestamp = new DateTime(2026, 03, 25, 10, 0, 0);

        public Result<LogGroup> CreateLogGroup()
        {
            var result = LogGroup.Create(message, firstLogId, timestamp);

            return result;
        }


        [Fact]
        public void Create_WithValidData_ShouldInitializeMessageCorrectly()
        {
            //Act
            var result = CreateLogGroup();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            //Assert
            var logGroup = result.Value!;
            logGroup.message.Should().Be(message);
        }

        [Fact]
        public void Create_WithValidData_ShouldSetCountToOne()
        {
            var result = CreateLogGroup();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            var logGroup = result.Value!;
            logGroup.count.Should().Be(1);
        }

        [Fact]
        public void Create_WithValidData_ShouldInitializeLogsListWithFirstLogId()
        {
            var result = CreateLogGroup();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            var logGroup = result.Value!;
            logGroup.Logs.Should().ContainSingle().Which.Should().Be(firstLogId);
        }

        [Fact]
        public void Create_WithValidData_ShouldSetFirstAndLastOccurenceToProvidedTimestamp()
        {
            var result = CreateLogGroup();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            var logGroup = result.Value!;

            logGroup.firstOccurence.Should().Be(timestamp);
            logGroup.lastOccurence.Should().Be(timestamp);
        }
        [Fact]
        public void Create_WithValidData_ShouldRaiseLogGroupCreateEventWithCorrectId()
        {
            var result = CreateLogGroup();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            var logGroup = result.Value!;

            logGroup.DomainEvents.Should().ContainSingle(e => e is LogGroupCreatedEvent)
                .Which.Should().BeEquivalentTo(new LogGroupCreatedEvent(logGroup.Id));
        }
    }
}

