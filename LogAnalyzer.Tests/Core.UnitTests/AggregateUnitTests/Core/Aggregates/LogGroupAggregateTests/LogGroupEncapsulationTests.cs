using FluentAssertions;
using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.LogEntryAggregate;
using LogAnalyzer.Core.Entities.LogGroupAggregate;

namespace LogAnalyzer.Tests.Core.UnitTests.AggregateUnitTests.Core.Aggregates.LogGroupAggregateTests
{
    public class LogGroupEncapsulationTests
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
        public void LogsCollection_ShouldBeReadOnly()
        {
            var logGroup = CreateLogGroup().Value!;
            var logs = logGroup.Logs;

            Action act = () => ((IList<Guid>)logs).Add(Guid.NewGuid());

            act.Should().Throw<NotSupportedException>();
        }
        [Fact]
        public void LogsCollection_ShouldOnlyBeModifiedByAddLogMethod()
        {
            var logGroup = CreateLogGroup().Value!;
            var externalLogsReference = logGroup.Logs;
            try
            {
                if (externalLogsReference is IList<Guid> mutableList)
                {
                    mutableList.Add(Guid.NewGuid());
                }
            }
            catch { }
            logGroup.count.Should().Be(1);
            logGroup.Logs.Should().HaveCount(1);
        }
    }
}
