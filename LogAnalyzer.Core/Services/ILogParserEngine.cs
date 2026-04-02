using LogAnalyzer.Core.Entities.LogEntryAggregate;

namespace LogAnalyzer.Core.Services
{
    public interface ILogParserEngine
    {
        public List<LogEntry> ParseLogs(string logsFilePath);
    }
}
