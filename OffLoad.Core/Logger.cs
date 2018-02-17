using log4net;
using System;
using System.Runtime.CompilerServices;

namespace OffLoad.Core
{
    public static class Logger
    {
        #region Properties

        public static ILog Log { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Wraps up the error message with the CallerMemberName
        /// </summary>
        /// <param name="data"></param>
        /// <param name="memberName"></param>
        public static void Debug(string data, [CallerMemberName]string memberName = "") => Log?.Debug($"[{memberName}]: {data}");

        /// <summary>
        /// Wraps up the error message with the CallerMemberName
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="ex"></param>
        public static void Error(Exception ex, [CallerMemberName]string memberName = "") => Log?.Error($"[{memberName}]: {ex.Message} {ex.InnerException}", ex);

        /// <summary>
        /// Wraps up the error message with the CallerMemberName
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ex"></param>
        /// <param name="memberName"></param>
        public static void Error(string data, Exception ex = null, [CallerMemberName]string memberName = "")
        {
            if (ex != null)
            {
                Log?.Error($"[{memberName}]: {data} {ex.InnerException}", ex);
            }
            else
            {
                Log?.Error($"[{memberName}]: {data}");
            }
        }

        /// <summary>
        /// Wraps up the fatal message with the CallerMemberName
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ex"></param>
        /// <param name="memberName"></param>
        public static void Fatal(string data, Exception ex = null, [CallerMemberName]string memberName = "")
        {
            if (ex != null)
            {
                Log?.Fatal($"[{memberName}]: {data} {ex.InnerException}", ex);
            }
            else
            {
                Log?.Fatal($"[{memberName}]: {data}");
            }
        }

        /// <summary>
        /// Wraps up the info message with the CallerMemberName
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ex"></param>
        /// <param name="memberName"></param>
        public static void Info(string data, Exception ex = null, [CallerMemberName]string memberName = "")
        {
            if (ex != null)
            {
                Log?.Info($"[{memberName}]: {data} {ex.InnerException}", ex);
            }
            else
            {
                Log?.Info($"[{memberName}]: {data}");
            }
        }

        public static void InitializeLogger(ILog log) => Log = log;

        /// <summary>
        /// Wraps up the warn message with the CallerMemberName
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ex"></param>
        /// <param name="memberName"></param>
        public static void Warn(string data, Exception ex = null, [CallerMemberName]string memberName = "")
        {
            if (ex != null)
            {
                Log?.Warn($"[{memberName}]: {data} {ex.InnerException}", ex);
            }
            else
            {
                Log?.Warn($"[{memberName}]: {data}");
            }
        }

        #endregion
    }
}