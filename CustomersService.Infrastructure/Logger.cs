using System;
using System.Diagnostics;
using Caliburn.Micro;

namespace CustomersService.Infrastructure
{
    public class Logger : ILog
    {
      //  private readonly AsteaLogViewer.ILogger _log;

        public Logger()
        {
          //  LogTraceListener 
        }
        public void Error(Exception exception)
        {
            Trace.WriteLine(exception);
        }

        public void Info(string format, params object[] args)
        {
            Trace.WriteLine(format);
        }

        public void Warn(string format, params object[] args)
        {
            Trace.WriteLine(format);
        }
    }
}