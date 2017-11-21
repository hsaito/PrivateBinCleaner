using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace PrivateBinCleaner
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Logger
    {
        private static bool _initialized;
        private readonly ILog _log;

        public Logger(Type type)
        {
            if (!_initialized)
            {
                // Configuration for logging
                var log4NetConfig = new XmlDocument();

                using (var reader = new StreamReader(new FileStream("log4net.config", FileMode.Open, FileAccess.Read)))
                {
                    log4NetConfig.Load(reader);
                }

                var rep = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Hierarchy));
                XmlConfigurator.Configure(rep, log4NetConfig["log4net"]);
                _initialized = true;
            }

            _log = LogManager.GetLogger(type);
        }

        public void Debug(string message)
        {
            _log.Debug(message);
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Warn(string message)
        {
            _log.Warn(message);
        }

        public void Error(string message)
        {
            _log.Error(message);
        }

        public void Fatal(string message)
        {
            _log.Fatal(message);
        }

        public void Exception(Exception e)
        {
            _log.Error(e.Message);
            _log.Debug(e.StackTrace);
        }
    }
}