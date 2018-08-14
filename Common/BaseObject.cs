using LogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BaseObject
    {
        private readonly ILogWrite _logger;

        public BaseObject()
        {
            _logger = new NLogService();
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message, Exception exception = null)
        {
            _logger.Error(message, exception);
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
