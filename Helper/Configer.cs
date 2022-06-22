using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace HRCentralDataToSharePoint
{
    public class Configer
    {
        public static string BaseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

        public static string MACHINE_ID = ConfigurationManager.AppSettings["MACHINE_ID"];

        public static string PROXY_HOST = ConfigurationManager.AppSettings["SFTP_PROXY_HOST"];
        public static string PROXY_PORT = ConfigurationManager.AppSettings["SFTP_PROXY_PORT"];


        public static string Tenant_Id = ConfigurationManager.AppSettings["SP_TENANT_ID"];
        public static string Client_Id = ConfigurationManager.AppSettings["SP_CLIENT_ID"];
        public static string Client_Secret = ConfigurationManager.AppSettings["SP_CLIENT_SECRET"];

        public static string SharePointSiteUrl = ConfigurationManager.AppSettings["SP_Site_Url"];
        public static string SharePointFolder = ConfigurationManager.AppSettings["SP_Folder"];
        public static string SharePointExcelName = ConfigurationManager.AppSettings["SP_Excel_Name"];


        public static string DaysToKeepFiles = ConfigurationManager.AppSettings["DaysToKeepFiles"];

        public static void SetGlobalProxy()
        {
            // 设置全局代理
            WebRequest.DefaultWebProxy = new WebProxy($"{PROXY_HOST}:{PROXY_PORT}")
            {
                //IF need  can apply 
                //Credentials = new NetworkCredential(PROXY_NAME, PROXY_PWD)
            };
            // 设置接受不安全证书
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, errors) => true);

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)0x300 | (SecurityProtocolType)0xC00 | System.Net.SecurityProtocolType.Ssl3
            | System.Net.SecurityProtocolType.Tls12
            | System.Net.SecurityProtocolType.Tls11
            | System.Net.SecurityProtocolType.Tls;
        }

        public static void UpdateConfig(string appKey, string appValue)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[appKey].Value = appValue;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

    }
}
