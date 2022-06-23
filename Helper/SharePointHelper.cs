using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using WebProxy = System.Net.WebProxy;

namespace HRCentralDataToSharePoint
{
    public class SharePointHelper
    {
        public static AccessToken GetAccessToken()
        {
            bool getOnlineToken = true;
            string jsonString = null;
            AccessToken tokenObj = null;

            try
            {
                var tokenFilePath = Configer.BaseDirectory + @"token.json";
                var tokenDuplicateFilePath = Configer.BaseDirectory + @"TempData\Token_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";

                if (File.Exists(tokenFilePath))
                {
                    jsonString = File.ReadAllText(tokenFilePath);
                    tokenObj = JsonConvert.DeserializeObject<AccessToken>(jsonString);
                    var expires_on = new DateTime(1970, 1, 1).AddSeconds(int.Parse(tokenObj.expires_on));
                    if (expires_on.Subtract(DateTime.UtcNow).TotalSeconds > 0)
                    {
                        getOnlineToken = false;
                    }
                }

                if (getOnlineToken == true)
                {
                    string serviceURL = string.Format("https://accounts.accesscontrol.windows.net/{0}/tokens/OAuth/2", Configer.Tenant_Id);
                    string body = string.Format(@"grant_type=client_credentials&resource=00000003-0000-0ff1-ce00-000000000000/{0}.sharepoint.com@{1}&client_id={2}@{1}&client_secret= {3}",
                        "bayergroup", Configer.Tenant_Id, Configer.Client_Id, Configer.Client_Secret);

                    WebRequest webRequest = WebRequest.Create(serviceURL);
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.Method = "POST";

                    byte[] payload = System.Text.Encoding.ASCII.GetBytes(body);
                    webRequest.ContentLength = payload.Length;
                    using (System.IO.Stream outputStream = webRequest.GetRequestStream())
                    {
                        outputStream.Write(payload, 0, payload.Length);
                    }

                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (Stream stream = webResponse.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                using (var reader = new StreamReader(stream))
                                {
                                    jsonString = reader.ReadToEnd();
                                }
                                File.WriteAllText(tokenFilePath, jsonString);
                                File.WriteAllText(tokenDuplicateFilePath, jsonString);
                            }
                        }
                    }
                    tokenObj = JsonConvert.DeserializeObject<AccessToken>(jsonString);
                }
                return tokenObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string PostDataViaRestAPI(string accessToken, byte[] bytesArray)
        {
            Uri url = new Uri(string.Format(@"{0}/_api/web/GetFolderByServerRelativeUrl('{1}')/Files/add(url='{2}.xlsx',overwrite=true)",
                Configer.SharePointSiteUrl, 
                Configer.SharePointFolder, 
                Configer.SharePointExcelName));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = true;
            request.AllowWriteStreamBuffering = true;
            request.Method = "POST";
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            //request.ContentType = "application/json";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 1000 * 60 * 5;
            request.KeepAlive = true;

            byte[] postData = bytesArray;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();
            }

            try
            {
                HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    return "Excel uploaded to Sharepoint Successfully!";
                }
                else
                {
                    return $"Excel failed to be uploaded to Sharepoint. StatusDescription:{resp.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

