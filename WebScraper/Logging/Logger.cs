using System.Runtime.CompilerServices;

namespace WebScraperProject.Logging
{
    public static class Logger
    {
        #region Emums
        public enum LogType
        {
            Info,
            Warning,
            Error
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Log a message, automatically finding the caller name and line number
        /// </summary>
        /// <param name="message">The message for this log</param>
        /// <param name="callerName">The caller objects name</param>
        /// <param name="callerLineNum">The line in the caller where this method was called</param>
        public static void Log(string message, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLineNum = 0)
        {
            Log(LogType.Info, message, callerName, callerLineNum);
        }
        /// <summary>
        /// Log a message, automatically finding the caller name and line number
        /// </summary>
        /// <param name="type">The type of log</param>
        /// <param name="message">The message for this log</param>
        /// <param name="callerName">The caller objects name</param>
        /// <param name="callerLineNum">The line in the caller where this method was called</param>
        public static void Log(LogType type, string message, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLineNum = 0)
        {
            Log(type, null, message, callerName, callerLineNum);
        }
        /// <summary>
        /// Log a message, automatically finding the caller name and line number
        /// </summary>
        /// <param name="type">The type of log</param>
        /// <param name="source">The source of the log</param>
        /// <param name="message">The message for this log</param>
        /// <param name="callerName">The caller objects name</param>
        /// <param name="callerLineNum">The line in the caller where this method was called</param>
        public static void Log(LogType type, object? source, string message, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLineNum = 0)
        {
            Write(type, source, message, callerName, callerLineNum);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Write a message to the debug console
        /// </summary>
        /// <param name="type">The type of writing</param>
        /// <param name="source">The source of the writing</param>
        /// <param name="message">The message for this writing</param>
        /// <param name="callerName">The caller objects name</param>
        /// <param name="callerLineNum">The line in the caller where this method was called</param>
        private static void Write(LogType type, object? source, string message, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLineNum = 0)
        {
            if (source != null)
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}][{type.ToString()[0..1]}][{callerName}:{callerLineNum}][{source}] {message}.");
            else
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}][{type.ToString()[0..1]}][{callerName}:{callerLineNum}] {message}.");
        }
        #endregion
    }
}
