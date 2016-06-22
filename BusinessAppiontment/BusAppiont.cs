using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusinessAppiontment
{
    public class BusAppiont
    {
        string url = "http://weixin.cnw-highlights.org/Home/Appointment/";

        public string MSG = null;

        //public void setURL(string Url)
        //{
        //    url = Url;
        //}

        /// <summary>
        /// Not Finish;
        /// </summary>
        /// <returns></returns>
        public bool AddAppointment()
        {
            string AddUrl = url + "addApm";
            
            return true;
        }

        /// <summary>
        /// Cancel Appointment
        /// </summary>
        /// <param name="apm_id"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public bool CancelAppiontment(string apm_id,string openid)
        {
            string CancelUrl = url + "cancelApm";
            string data = "apm_id=" + apm_id + "&openid=" + openid;
            var result = HttpPost(CancelUrl, data);

            if (result.IndexOf("error_code") >= 0)
            {
                string msg = result.Substring(result.IndexOf("msg") + 3).Trim('"');
                MSG = UnicodeToString(msg);

                return false;
            }
            else
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                string code = jo["code"].ToString();

                if (code == "20000")
                {
                    MSG = "SUCCEED";
                    return true;
                }

                MSG = "Unkown Erro";
                return false;
            }

        }

        /// <summary>
        /// Get all appointment list(limited at 100)
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public JArray GetAppiontList(string sid)
        {
            string GetListUrl = url + "listApm";
            string data = "sid=" + sid + "&limit=100";
            var result = HttpGet(GetListUrl, data);

            //string errMSG = "";
            if (result.IndexOf("error_code") >= 0)
            {
                //ErroInfo erroinfo = new ErroInfo();
                //erroinfo = JsonConvert.DeserializeObject<ErroInfo>(result);
                string msg = result.Substring(result.IndexOf("msg") + 3).Trim('"');
                MSG = UnicodeToString(msg);

                JArray jar = null;
                return jar;
                //return false;
            }
            else
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                string code = jo["code"].ToString();

                JArray jar = JArray.Parse(jo["response"].ToString());
                MSG = "SUCCEED";
                return jar;
            }
            //return;
        }

        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        private string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        private string UnicodeToString(string unicode)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(unicode))
            {
                string[] strlist = unicode.Replace("//", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            return outStr;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
