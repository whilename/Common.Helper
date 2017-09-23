using log4net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Common.Utility
{
    /// <summary>系统日志处理类</summary>
    public class Log
    {
        #region  变量

        private static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>分隔行</summary>
        private static string DIVIDE_LINE = "".PadRight(30, '*') + Environment.NewLine;
        /// <summary>明细分隔行</summary>
        private static string DETAIL_DIVIDE_LINE = "".PadRight(30, '-') + Environment.NewLine;
        /// <summary>TAB(4空格)</summary>
        private static string TAB_STRING = "".PadRight(4, ' ');
        /// <summary>使用消息队列处理日志信息</summary>
        static Queue<LogInfo> LogQueue = new Queue<LogInfo>();

        #endregion

        /// <summary>注册应用程序系统日志记录</summary>
        public static void Register()
        {
            // log4net配置文件自定义变量设置
            //log4net.GlobalContext.Properties["sitename"] = System.Web.Hosting.HostingEnvironment.SiteName;
            // 解注以下两行代码自动识别C/S或B/S程序加载log4net所在目录配置文件
            //string config_file = System.AppDomain.CurrentDomain.SetupInformation.ApplicationName.Contains(".exe") ? "log4net.config" : "bin\\log4net.config";
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + config_file));
            ThreadPool.QueueUserWorkItem(o =>
            {
                while (true)
                {
                    try
                    {
                        if (LogQueue.Count > 0)
                        {
                            LogInfo log = LogQueue.Dequeue(); // 从消息队列中获取日志
                            // 记录错误日志
                            if (log.iserr) { _log.Error(log.msg); /*_log.Error(log.msg, log.ex); */ }
                            // 记录普通日志
                            else { _log.Info(log.msg); }
                        }
                    }
                    catch (Exception ex) { LogQueue.Enqueue(new LogInfo(true, GetAllExceptionText(ex))); }
                    // 为避免CPU空转，在队列为空时休息2秒
                    finally { if (LogQueue.Count <= 0) { Thread.Sleep(2000); } }
                }
            });
        }

        #region 获取异常信息

        /// <summary>获取异常及其内部异常的描述</summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetAllExceptionText(Exception e)
        {
            StringBuilder text = new StringBuilder(4096);
            List<Exception> errors = new List<Exception>();

            Exception err = e;
            while (err.InnerException != null)
            {
                err = err.InnerException;
                errors.Add(err);
            }

            //AppUser user = AppUser.Current;
            //if (user != null)
            //{
            //    text.Append("操作者：" + user.Usercd
            //    + "," + user.Name
            //    + Environment.NewLine);
            //}

            //text.Append(DIVIDE_LINE);
            int j = 1;
            for (int i = errors.Count - 1; i >= 0; i--)
            {
                text.Append(TAB_STRING);
                text.Append("InnerException" + Convert.ToString(j) + ":" + GetExceptionText(errors[i]));
                text.Append(DETAIL_DIVIDE_LINE);
                j++;
            }
            text.Append(TAB_STRING);
            text.Append("Exception:" + GetExceptionText(e));

            //////用户操作信息
            //if (user != null &&
            //    user.Action.GetLog().Count > 0)
            //{
            //    text.Append(DIVIDE_LINE);
            //    text.Append("附加信息如下：" + Environment.NewLine);
            //    foreach (ActionMessage x in user.Action.GetLog())
            //    {
            //        text.Append(x.Text + Environment.NewLine);
            //    }
            //}

            return text.ToString();
        }

        /// <summary>获取异常描述</summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string GetExceptionText(Exception e)
        {
            StringBuilder sb = new StringBuilder(4096);

            sb.Append(e.Message + Environment.NewLine);
            //sb.Append(TAB_STRING);
            sb.Append("Type:" + e.GetType().AssemblyQualifiedName + Environment.NewLine);
            //sb.Append(TAB_STRING);
            sb.Append("source:" + e.Source + Environment.NewLine);
            //sb.Append(TAB_STRING);
            sb.Append("StackTrace:" + e.StackTrace + Environment.NewLine);
            //Console.WriteLine("Line:"+Environment.NewLine);
            foreach (string k in e.Data.Keys)
            {
                sb.Append(TAB_STRING + TAB_STRING);
                sb.Append(string.Format("{0}:{1}", k, e.Data[k]) + Environment.NewLine);
            }

            return sb.ToString();
        }

        #endregion

        #region 输出日志

        /// <summary>输出Debug信息</summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            if (_log.IsDebugEnabled)
            {
                LogQueue.Enqueue(new LogInfo() { iserr = false, msg = message });
                //_log.Debug(message);
            }
        }

        /// <summary>输出Error信息</summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            if (_log.IsErrorEnabled)
            {
                LogQueue.Enqueue(new LogInfo() { iserr = true, msg = message });
                //_log.Error(message);
            }
        }

        /// <summary>输出异常信息</summary>
        /// <param name="e"></param>
        public static void Error(Exception e)
        {
            if (_log.IsErrorEnabled)
            {
                string msg = GetAllExceptionText(e);
                LogQueue.Enqueue(new LogInfo() { iserr = true, msg = msg, ex = e });
                //_log.Error(s);
            }
        }

        /// <summary>输出日志消息</summary>
        /// <param name="message"></param>
        public static void Warning(string message)
        {
            if (_log.IsWarnEnabled)
            {
                LogQueue.Enqueue(new LogInfo() { iserr = false, msg = message });
                //_log.Warn(message);
            }
        }

        /// <summary>记录日志信息</summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            if (_log.IsInfoEnabled)
            {
                LogQueue.Enqueue(new LogInfo() { iserr = false, msg = message });
                //_log.Info(message);
            }
        }

        /// <summary>记录日志信息</summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Info(string message, params object[] args)
        {
            if (_log.IsInfoEnabled)
            {
                LogQueue.Enqueue(new LogInfo() { iserr = false, msg = String.Format(message, args) });
                //_log.Info(message);
            }
        }

        #endregion

        /// <summary>日志信息</summary>
        private class LogInfo
        {
            /// <summary>日志信息</summary>
            public LogInfo() { }
            /// <summary>日志信息</summary>
            public LogInfo(bool iserr, string msg) { this.iserr = iserr; this.msg = msg; }
            /// <summary>日志信息</summary>
            public LogInfo(bool iserr, Exception ex) { this.iserr = iserr; this.ex = ex; }
            /// <summary>是否错误消息</summary>
            public bool iserr { get; set; }
            /// <summary>消息描述</summary>
            public string msg { get; set; }
            /// <summary>异常错误</summary>
            public Exception ex { get; set; }
        }

    }
}
