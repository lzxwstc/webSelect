using System;

namespace JIYITECH.WebApi.Factories
{
    /// <summary>
    /// NLog 日志类(easy 版)
    /// </summary>
    public class LogHelper
    {
        private static NLog.Logger Nlog = NLog.LogManager.GetCurrentClassLogger();

        public static void Error(Exception exp = null)
        {
            Nlog.Error(exp);
        }

        public static void Error(object msg)
        {
            Nlog.Error(msg);
        }

        public static void Error(string msg, Exception exp = null)
        {
            Nlog.Error(exp, msg);
        }

        public static void Debug(Exception exp = null)
        {
            Nlog.Debug(exp);
        }

        public static void Debug(object msg)
        {
            Nlog.Debug(msg);
        }

        public static void Debug(string msg, Exception exp = null)
        {
            Nlog.Debug(exp, msg);
        }

        public static void Info(Exception exp = null)
        {
            Nlog.Info(exp);
        }

        public static void Info(object msg)
        {
            Nlog.Info(msg);
        }

        public static void Info(string msg, Exception exp = null)
        {
            Nlog.Info(exp, msg);
        }

        public static void Warn(Exception exp = null)
        {
            Nlog.Warn(exp);
        }

        public static void Warn(object msg)
        {
            Nlog.Warn(msg);
        }

        public static void Warn(string msg, Exception exp = null)
        {
            Nlog.Warn(exp, msg);
        }
    }
}
