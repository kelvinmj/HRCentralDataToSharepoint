using System;
using System.Configuration;

namespace HRCentralDataToSharePoint
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LogHelper.InfoLog("Job Start *****");

                Configer.SetGlobalProxy();
                var data = ExcelHelper.GetDataBytesFromDB();

                var token = SharePointHelper.GetAccessToken();
                var message = SharePointHelper.PostDataViaRestAPI(token.access_token, data);
                LogHelper.InfoLog(message);

                LogHelper.InfoLog("Job End   *****");
            }
            catch (Exception ex)
            {
                LogHelper.ErrorLog(ex.ToString());
            }
        }
    }
}
