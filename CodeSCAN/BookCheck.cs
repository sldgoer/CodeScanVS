using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BusinessAppiontment;
using IDScanAndLoad;

namespace CodeSCAN
{
    class BookCheck
    {
        //public bool Check

        BookInfo bi = new BookInfo();
        public BookCheck(BookInfo bookinfo)
        {
            bi=bookinfo;
        }

        public bool Check(out string MSG)
        {
            MSG = "";

            if (!CheckBookValid())
            {
                MSG = "二维码校验失败";
                return false;
            }
            if(!CheckBookDate())
            {
                MSG = "预约失效或未到预约日期！/n预约日期：" + bi.BookDate;
                return false;
            }
            if (!CheckBookTime())
            {
                MSG = "预约超时或未到预约时间！/n预约时间："+bi.BookTime;
                return false;
            }
            return true;
        }

        public bool Check(IDInfo idinfo, out string MSG)
        {
            MSG = "";

            //if()
            if (!CheckBookValid(idinfo))
            {
                MSG = "预约信息校验失败,没找到您的预约信息！";
                return false;
            }
            if (!CheckBookDate())
            {
                MSG = "预约失效或未到预约日期！您的预约日期：" + bi.BookDate;
                return false;
            }
            if (!CheckBookTime())
            {
                MSG = "预约超时或未到预约时间！您的预约时间：" + bi.BookTime;
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 判断二维码的预约日期是否合法
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private bool CheckBookDate()
        {
            //TimeSpan ts = DateTime.Now.Subtract(bookinfo.BookDate);
            //int res = DateTime.Compare(DateTime.Now, bi.BookDate);
            //if (res > 0)
            //{
            //    return false;
            //}
            //else if (res < 0)
            //{
            //    return false;
            //}
            //return true;
            DateTime now = DateTime.Now;
            #region 校验是否到取号时间，最多提前5分钟取号
            TimeSpan bts = now.Subtract(bi.BookDate);
            if (bts.Days==0)
            {
                return true;
            }
            else 
            {
                return false;
            }

            #endregion
        }

        /// <summary>
        /// 判断预约时段是否合法（最多提前15分钟，最多超时10分钟）
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private bool CheckBookTime()
        {
            string[] t = bi.BookTime.Split('-');
            //string[] bt = t[0].Split(':');
            //string[] et = t[1].Split(':');
            DateTime bt = Convert.ToDateTime(t[0]);
            DateTime et = Convert.ToDateTime(t[1]);
            DateTime now = DateTime.Now;

            #region 校验是否到取号时间，最多提前5分钟取号
            TimeSpan bts = now.Subtract(bt);
            if (bts.Hours < 0)
            {
                return false;
            }
            else if (bts.Hours == 0 && bts.Minutes < -5)
            {
                return false;
            }

            #endregion

            #region 校验是否超时，超时5分钟内仍可取号
            TimeSpan ets = now.Subtract(et);
            if (ets.Hours > 0)
            {
                return false;
            }
            else if (ets.Hours == 0 && ets.Minutes > 5)
            {
                return false;

            }
            #endregion

            return true;
        }

        private bool CheckBookValid()
        {
            string url = "http://weixin.cnw-highlights.org/Home/Appointment/listApm";
            string data = "sid=" + bi.BusinessID;
            string result = HttpGet(url, data);
            
            string errMSG = "";
            if (result.IndexOf("error_code") >= 0)
            {
                //ErroInfo erroinfo = new ErroInfo();
                //erroinfo = JsonConvert.DeserializeObject<ErroInfo>(result);
                string msg = result.Substring(result.IndexOf("msg") + 3).Trim('"');
                errMSG = UnicodeToString(msg);

                return false;
            }
            else 
            {
                JObject jo=(JObject)JsonConvert.DeserializeObject(result);
                string code = jo["code"].ToString();

                JArray jar = JArray.Parse(jo["response"].ToString());
                for (int i = 0; i < jar.Count; i++)
                {
                    JObject j = JObject.Parse(jar[i].ToString());
                    if (j["idcard"].ToString() == bi.IdCard && j["user_name"].ToString()==bi.Name && j["status"].ToString() == "0")
                    {
                        bi.BusinessName = j["service_name"].ToString();
                        return true;
                    }
                }
                return false;
            }
            //return true;
        }

        private bool CheckBookValid(IDInfo idinfo)
        {
            #region Comment
            //string url = "http://weixin.cnw-highlights.org/Home/Appointment/listApm";
            //string data = "sid=" + "zj56f35c5f78352";
            //string result = HttpGet(url, data);

            //string errMSG = "";
            //if (result.IndexOf("error_code") >= 0)
            //{
            //    //ErroInfo erroinfo = new ErroInfo();
            //    //erroinfo = JsonConvert.DeserializeObject<ErroInfo>(result);
            //    string msg = result.Substring(result.IndexOf("msg") + 3).Trim('"');
            //    errMSG = UnicodeToString(msg);

            //    return false;
            //}
            //else
            //{
            //    JObject jo = (JObject)JsonConvert.DeserializeObject(result);
            //    string code = jo["code"].ToString();

            //    JArray jar = JArray.Parse(jo["response"].ToString());
            //    for (int i = 0; i < jar.Count; i++)
            //    {
            //        JObject j = JObject.Parse(jar[i].ToString());
            //        if (j["idcard"].ToString() == idinfo.Number && j["user_name"].ToString() == idinfo.Name && j["status"].ToString() == "0")
            //        {
            //            bi.Name = idinfo.Name;
            //            bi.IdCard = idinfo.Number;
            //            bi.BookTime = j["start_time"].ToString() + "-" + j["end_time"].ToString();
            //            bi.BusinessID = j["sid"].ToString();
            //            bi.BusinessName = j["service_name"].ToString();
            //            bi.BookDate = GetTime(j["day_timestamp"].ToString());
            //            bi.BookSerial = "EE";
            //            return true;
            //        }
            //    }
            //    return false;
            //}
            #endregion
                        
            var todayArray = GetTodayList();
            //for (int i = 0; i < jar.Count; i++)
            //{
            //    JObject j = JObject.Parse(jar[i].ToString());
            //    if (j["idcard"].ToString() == idinfo.Number && j["user_name"].ToString() == idinfo.Name && j["status"].ToString() == "0")
            //    {
            //        bi.Name = idinfo.Name;
            //        bi.IdCard = idinfo.Number;
            //        bi.BookTime = j["start_time"].ToString() + "-" + j["end_time"].ToString();
            //        bi.BusinessID = j["sid"].ToString();
            //        bi.BusinessName = j["service_name"].ToString();
            //        bi.BookDate = GetTime(j["day_timestamp"].ToString());
            //        bi.BookSerial = "EE";
            //        return true;
            //    }
            //}
            int validCount = todayArray.Where(
                a =>
                {
                    return a["idcard"].ToString() == idinfo.Number && a["user_name"].ToString() == idinfo.Name && a["status"].ToString() == "0";
                }).Count();

            if (validCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private JToken[] GetTodayList()
        {
            JArray jar = GetAllList();
            //for (int i=0; i < jar.Count; i++)
            //{
            //    JObject j = JObject.Parse(jar[i].ToString());
            //    DateTime BookDate = GetTime(j["day_timestamp"].ToString());
            //    if (BookDate.Year == DateTime.Now.Year && BookDate.Month == DateTime.Now.Month && BookDate.Day == DateTime.Now.Day)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        jar[i].Remove();
            //    }
            //}
            var todayArray = jar.Where(
                a =>
                {
                    return (DateTime.Now.Subtract(GetTime(a["day_timestamp"].ToString()))).Hours < 24;                    
                }).ToArray();
            return todayArray;
        }

        private JArray GetAllList()
        {
            /*
            string url = "http://weixin.cnw-highlights.org/Home/Appointment/listApm";
            string data = "sid=" + "zj56f35c5f78352";
            string result = HttpGet(url, data);

            string errMSG = "";
            if (result.IndexOf("error_code") >= 0)
            {
                //ErroInfo erroinfo = new ErroInfo();
                //erroinfo = JsonConvert.DeserializeObject<ErroInfo>(result);
                string msg = result.Substring(result.IndexOf("msg") + 3).Trim('"');
                errMSG = UnicodeToString(msg);

                JArray jar = null;
                return jar;
                //return false;
            }
            else
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(result);
                string code = jo["code"].ToString();

                JArray jar = JArray.Parse(jo["response"].ToString());
                return jar;
            }
             * */
            BusAppiont busapp = new BusAppiont();
            return busapp.GetAppiontList("zj56f35c5f78352");

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
