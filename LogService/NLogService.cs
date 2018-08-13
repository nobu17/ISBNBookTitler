using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogService
{
    /// <summary>
    /// NLogによるサービス実装
    /// </summary>
    public class NLogService : ILogWrite
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message, Exception exception = null)
        {
            _logger.Error(exception, message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }
    }
}
