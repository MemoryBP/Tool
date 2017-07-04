namespace DataBaseTool
{
    using System;
    using log4net;

    public enum LogType
    {
        DEBUG = 1,
        INFO = 2,
        WARN = 4,
        ERROR = 8,
        FATAL = 16
    }

    public class Logout
    {

        private static readonly ILog logger;
        private static readonly ILog consoleLogger;

        static Logout()
        {
            logger = LogManager.GetLogger("File.Logging");
            consoleLogger = LogManager.GetLogger("Console.Logging");
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="msg">输出信息</param>
        /// <param name="ex">输出异常</param>
        /// <param name="logType"><see cref="LogType"/>日志类型</param>
        /// <param name="isToScreen">是否输出到屏幕</param>
        public static void WriteLog(string msg, Exception ex, LogType logType, bool isToScreen = false)
        {
            if (logType == LogType.DEBUG)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(msg, ex);
                }
            }
            else if (logType == LogType.INFO)
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(msg, ex);
                }
            }
            else if (logType == LogType.WARN)
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(msg, ex);
                }
            }
            else if (logType == LogType.ERROR)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(msg, ex);
                }
            }
            else if (logType == LogType.FATAL)
            {
                if (logger.IsFatalEnabled)
                {
                    logger.Fatal(msg, ex);
                }
            }

            if (isToScreen)
            {
                consoleLogger.Info(msg);
            }
        }

        /// <summary>
        /// 输出日志信息
        /// </summary>
        /// <param name="msg">输出信息</param>
        /// <param name="logType"><see cref="LogType"/>日志类型</param>
        /// <param name="isToScreen">是否输出到屏幕</param>
        public static void WriteLog(string msg, LogType logType, bool isToScreen = false)
        {
            WriteLog(msg, null, logType, isToScreen);
        }
    }
}
