using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LXB.Loger
{
    public class Log
    {
        private delegate void AddLogDel(string logstring);
        AddLogDel addlog = null;

        public enum LogType { Application, Execution };

        public string LogPath { get; set; }

        public Log(string logpath)
        {
            LogPath = logpath;
        }

        public void AddLog(String LogString,LogType Type) 
        {            
            switch(Type)
            {
                case LogType.Application: 
                    //AddAppLog(LogString);
                    addlog = new AddLogDel(AddAppLog);
                    break;
                case LogType.Execution:
                    //AddExtLog(LogString);
                    addlog = new AddLogDel(AddExtLog);
                    break;
                default: break;

            }
            addlog(LogString + "\r\n");

        }

        private void AddAppLog(string logstring)
        {
            string LogFileName =LogPath + @"/APP_" + DateTime.Now.ToString("yyyy-MM-dd") + @".txt";
            AddToLog(LogFileName, logstring);

        }

        private void AddExtLog(string logstring)
        {
            string LogFileName = LogPath + @"/EXT_" + DateTime.Now.ToString("yyyy-MM-dd") + @".txt";
            AddToLog(LogFileName, logstring);

        }

        private void AddToLog(string LogFileName,string logstring)
        {
            try
            {
                FileStream fs = new FileStream(LogFileName, FileMode.Append);
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(logstring);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();

            }
            catch (IOException ex)
            {
                throw ex;
            }

        }

    }
}
