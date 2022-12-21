using System;
using System.Diagnostics;

namespace ServiceControll
{
    public class LogEvent
    {
        const string LOG_NAME = "Application";
        const string SOURCE = "RoVerificaService";

        public LogEvent()
        {
            if (EventLog.SourceExists(SOURCE) == false)
                EventLog.CreateEventSource(SOURCE, LOG_NAME);
        }

        public void WriteEntry(string input, EventLogEntryType entryType)
        {
            EventLog.WriteEntry(SOURCE, input, entryType);
        }

        public void WriteEntry(string input)
        {
            WriteEntry(input, EventLogEntryType.Information);
        }

        public void WriteEntry(Exception ex)
        {
            WriteEntry(ex.ToString(), EventLogEntryType.Error);
        }
    }
}
