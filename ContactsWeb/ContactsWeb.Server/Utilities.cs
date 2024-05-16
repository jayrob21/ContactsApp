using NLog;

namespace ContactsWeb.Server
{
    public class Utilities
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        public static void LogError(Exception ex) => logger.Error(ex);
        public static void LogInfo(string msg) => logger.Info(msg);
    }
}