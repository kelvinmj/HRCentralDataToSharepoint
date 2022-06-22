using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRCentralDataToSharePoint
{
    /// <summary>
    /// LogHelper的摘要说明。
    /// </summary>
    public class LogHelper
    {

        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void InfoLog(string info)
        {

            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }
        public static void InfoLog(string info,Exception ex)
        {

            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info,ex);
            }
        }

        public static void ErrorLog(string info, Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, ex);
            }
        }

        public static void ErrorLog(string info)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info);
            }
        }
    }
}
