using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace CodeSCAN
{
    class QueueControl
    {
        public string ConnectionString
        {
            get;
            set;
        }

        //public TicketInfo ticketinfo = new TicketInfo();


        public QueueControl(string conStr)
        {
            ConnectionString = conStr;
        }

        public TicketInfo InsertToQueue()
        {
            TicketInfo ticketinfo = new TicketInfo();
            if (!InsertToQueue(out ticketinfo))
            {
                ticketinfo.Header = "预约号插入失败！";
                ticketinfo.QueueString = "ERR";
                return ticketinfo;
            }

            return ticketinfo; ;
        }

        private bool InsertToQueue(out TicketInfo ticketinfo)
        {
            ticketinfo = new TicketInfo();
            ticketinfo.QueueString = "前面还有" + GetLeftCount().ToString() + "人";
            //ticketinfo.OrderString = "G" + GetNewOrder();

            //return true;

            ticketinfo.OrderString = GetNewOrder();
            StringBuilder insertSB = new StringBuilder();
            string dateStr = DateTime.Now.ToString();

            insertSB.Append("insert into queue(Q_Serial,Q_number,Q_Counter,Q_cometime,Q_mobile,Q_issms) values(7,@OrderString,0,@cometime,'',0);");
            insertSB.Append("insert into QueueHist(H_Serial,H_number,H_counter,H_cometime,H_servetime,H_serveno,H_endtime,H_isdo,H_issend) values(7,@OrderString,0,@cometime,@cometime,'0000',@cometime,0,0)");
            //string insertSql = "insert into queue(Q_Serial,Q_number,Q_Counter,Q_cometime,Q_mobile,Q_issms) values(1,{@OrderString},1,getdate(),'',0)";
            //ticketinfo.OrderString
            SqlParameter[] para = { 
                                      new SqlParameter("@OrderString", SqlDbType.VarChar, ticketinfo.OrderString.Length), 
                                      new SqlParameter("@cometime",SqlDbType.DateTime,dateStr.Length)
                                  };

            para[0].Value = ticketinfo.OrderString;
            para[1].Value = Convert.ToDateTime(dateStr);

            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();
                using (SqlCommand insertCmd = new SqlCommand(insertSB.ToString(), cnn))
                {
                    insertCmd.Parameters.AddRange(para);
                    if (insertCmd.ExecuteNonQuery() == 2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool InsertToHist(string name,string idcard)
        {
            string insertSQL = @"insert into GRYUHistory (user_name,idcard,printtime) values('" + name + "','" + idcard + "',getdate();";

            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();
                using (SqlCommand insertCmd = new SqlCommand(insertSQL, cnn))
                {
                    insertCmd.CommandType = CommandType.Text;
                    if (insertCmd.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //return true;
        }

        public int GetTodayPrintCount(string idcard)
        {
            string sql = @"select count(GUID) from GRYUHistory where idcard='" + idcard + "'";
            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();
                using (SqlCommand selectCmd = new SqlCommand(sql, cnn))
                {
                    selectCmd.CommandType = CommandType.Text;
                    int count = Convert.ToInt16(selectCmd.ExecuteScalar());

                    return count;
                }
            }
        }
        //private void 

        /// <summary>
        /// Get the count of order left
        /// </summary>
        /// <returns>the left count</returns>
        private int GetLeftCount()
        {
            string leftSql = "select count(H_number) from queuehist where H_Serial=7 and H_counter=0 and datediff(day,getdate(),H_cometime)=0";
            //string insertSql = "insert into queue(Q_Serial,Q_number,Q_Counter,Q_cometime,Q_mobile,Q_issms) values(5,";
            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();
                int leftcount = 0;

                using (SqlCommand leftcmd = new SqlCommand(leftSql, cnn))
                {
                    var left = leftcmd.ExecuteScalar();
                    leftcount = Convert.ToInt16(left);
                }
                if (leftcount == -1)
                {
                    leftcount = 0;
                }
                return leftcount;
            }
        }

        /// <summary>
        /// Get the latest Order 
        /// </summary>
        /// <returns>The lastest order string</returns>
        private string GetNewOrder()
        {
            string newOrder = "";
            string maxOrder = "";
            string maxSql = @"select H_number from QueueHist where H_number like 'G%' and H_cometime=(select max(H_cometime) from QueueHist where H_number like 'G%' and datediff(day,h_cometime,getdate())=0)";

            using (SqlConnection cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();

                using (SqlCommand maxcmd = new SqlCommand(maxSql, cnn))
                {
                    maxOrder = (string)maxcmd.ExecuteScalar();
                }
            }

            if (maxOrder != null)
            {
                int orderNum = int.Parse(maxOrder.Substring(1)) + 1;

                if (orderNum < 100)
                {
                    newOrder = maxOrder.Substring(0, 1) + (1000 + orderNum).ToString().Substring(1);
                }
            }
            else
            {
                newOrder = "G001";
            }

            return newOrder;
        }

    }
}
