using FluentAssertions;
using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.Enums;
using LogAnalyzer.Core.Entities.LogEntryAggregate;
using LogAnalyzer.Core.Events;

namespace LogAnalyzer.Tests.Core.UnitTests.AggregateUnitTests.Core.Aggregates.LogEntryAggregateTests
{
    public class LogEntryFactoryTests
    {
        private readonly LogMessage message = LogMessage.Create("Database connection failed").Value!;
        private readonly string stackTrace = "Connected successfully" + " " + "to the database";
        private readonly string source = "log.txt";
        private readonly LogLevelEnum level = 0;
        public Result<LogEntry> CreateLogEntry()
        {
            var logEntry = LogEntry.Create(message, stackTrace, source, level);
            return logEntry;
        }

        [Fact]
        public void Create_WithValidData_ShouldSetInitialProperties()
        {
            var result = CreateLogEntry();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();

            var logEntry = result.Value!;

            logEntry.Message.Should().Be(message);
            logEntry.StackTrace.Should().Be(stackTrace);
            logEntry.Source.Should().Be(source);
            logEntry.LogLevel.Should().Be(level);
            logEntry.Id.Should().NotBe(Guid.Empty);
        }
        [Fact]
        public void Create_WithInvalidLogLevel_ShouldReturnFailure()
        {
            var invalidLogLevel = (LogLevelEnum)999;
            var result = LogEntry.Create(message, stackTrace, source, invalidLogLevel);
            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Error.Should().Be("Inappropiate Log Level value");
        }
        [Fact]
        public void Create_WithInvalidSource_ShouldReturnFailure()
        {
            var invalidSource = string.Empty;
            var result = LogEntry.Create(message, stackTrace, invalidSource, level);
            result.IsSuccess.Should().BeFalse();
            result.Value.Should().BeNull();
            result.Error.Should().Be("Log Source is null or empty");
        }
        [Fact]
        public void Create_WithValidData_ShouldRaiseLogEntryCreatedEvent()
        {
            var result = CreateLogEntry();
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            var logEntry = result.Value!;

            logEntry.DomainEvents.Should().ContainSingle(e => e is LogEntryCreatedEvent)
                .Which.Should().BeEquivalentTo(new LogEntryCreatedEvent(logEntry.Id));
        }
    }
}
