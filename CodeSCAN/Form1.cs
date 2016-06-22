using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Threading;
using IDScanAndLoad;
using System.Runtime.InteropServices;
using LXB.Loger;


namespace CodeSCAN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SerialPort serialPort;
        public delegate void HandleInterfaceReadDelegate(string text);
        private HandleInterfaceReadDelegate handleinterfacereaddelegate;

        BookInfo bookinfo;
        IDInfo idinfo;

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort = new SerialPort();

            bookinfo = new BookInfo();
            idinfo = new IDInfo();

            cb_Baud.SelectedText = "9600";
            cb_COMS.DataSource = SerialPort.GetPortNames();

            btn_StartCheck.Enabled = true;
            btn_StopCheck.Enabled = false;

            SetSplitContainer();
        }

        private void btn_StartCheck_Click(object sender, EventArgs e)
        {
            //printtest();
            //GetTicket gt = new GetTicket();
            //gt.MdiParent = this;
            //gt.Show();

            booklisttest();

            serialPort.PortName = cb_COMS.Text;
            serialPort.BaudRate = int.Parse(cb_Baud.Text);
            serialPort.StopBits = StopBits.One;
            //serialPort.
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.DtrEnable = true;

            handleinterfacereaddelegate = new HandleInterfaceReadDelegate(DisplayBookInfo);
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort_DataRecieved);
            serialPort.ReceivedBytesThreshold = 1;

            try
            {
                if (serialPort.IsOpen == false)
                {
                    serialPort.Open();
                }
                btn_StartCheck.Enabled = false;
                btn_StopCheck.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("端口打开失败：" + ex.Message.ToString());
            }
        }

        private void serialPort_DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] readBuffer = new byte[serialPort.ReadBufferSize];
            serialPort.Read(readBuffer, 0, readBuffer.Length);
            this.Invoke(handleinterfacereaddelegate, new string[] { Encoding.UTF8.GetString(readBuffer) });

            BookCheck bc = new BookCheck(bookinfo);
            string checkMSG = "";
            if (bc.Check(out checkMSG))
            {
                //MessageBox.Show("预约校验成功");
                TicketInfo ticketinfo = new TicketInfo();
                AssemblyTicketInfo(out ticketinfo);
            }
            else
            {
                MessageBox.Show(checkMSG);
            }
        }

        private void DisplayBookInfo(string bookinfoString)
        {
            //MessageBox.Show(bookinfoString);
            //bookinfo = new BookInfo();
            string[] info = bookinfoString.Split('|');
            AssemblyBookInfo(info, out bookinfo);

            //tb_Name.Text = bookinfo.Name;
            //tb_ID.Text = bookinfo.IdCard;
            //tb_Booktime.Text = bookinfo.BookTime;
            //tb_BookSerial.Text = bookinfo.BookSerial;
        }

        private void btn_StopCheck_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen == true)
            {
                serialPort.Close();
            }
            btn_StartCheck.Enabled = true;
            btn_StopCheck.Enabled = false;
        }

        /// <summary>
        /// 二维码信息转预约数据模型BookInfo
        /// </summary>
        /// <param name="info">二维码字符串</param>
        /// <param name="bookinfo">BookInfo</param>
        private void AssemblyBookInfo(string[] info, out BookInfo bookinfo)
        {
            bookinfo = new BookInfo();
            bookinfo.IdCard = info[0].ToString();
            bookinfo.Name = info[1].ToString();
            bookinfo.OrderID = int.Parse(info[2].ToString());
            bookinfo.BookDate = DateTime.Parse(info[3].ToString());
            bookinfo.BookTime = info[4].ToString();
            bookinfo.BusinessID = info[5].ToString();
            bookinfo.BookSerial = info[6].ToString();
        }

        /// <summary>
        /// print ticket if the idinfo is valid
        /// </summary>
        /// <param name="idinfo"></param>
        public void PrintTicketThrouthIDCard()
        {
            BookCheck bc = new BookCheck(bookinfo);
            string checkMSG = "";
            if (!InitialCVRDevice())
            {
                MessageBox.Show("身份证阅读仪初始化失败，请检查！");
                return;
                //this.Close();
            }
            else
            {
                LoopToTest();
            }

            if (bc.Check(idinfo, out checkMSG))
            {
                //MessageBox.Show("预约校验成功");
                TicketInfo ticketinfo = new TicketInfo();
                try
                {
                    AssemblyTicketInfo(out ticketinfo);
                }
                catch (Exception ex)
                {
                    Log log = new Log(Environment.CurrentDirectory);
                    log.AddLog(ex.Message, Log.LogType.Execution);
                }
            }
            else
            {
                MessageBox.Show(checkMSG);
            }
        }

        /// <summary>
        /// Prepair the Ticket model "TicketInfo"
        /// </summary>
        /// <param name="ti"></param>
        private void AssemblyTicketInfo(out TicketInfo ti)
        {
            ti = new TicketInfo();
            TicketPrint tp = null;
            string queueConStr = "Data Source=192.168.199.250;Initial Catalog=queue;User ID=gryy;Password=#*@(*^^";
            QueueControl queuecontrol = new QueueControl(queueConStr);

            ti = queuecontrol.InsertToQueue();
            //ti.OrderString = "G" + bookinfo.OrderID.ToString().PadLeft(3, '0');

            if (ti.QueueString != "ERR" && queuecontrol.GetTodayPrintCount(bookinfo.IdCard) == 0)
            {
                ti.Header = "江门市住房公积金预约业务\n";
                ti.Context = "预约人：" + bookinfo.Name + "\n身份证：" + bookinfo.IdCard.Substring(0, 6) + "********" + bookinfo.IdCard.Substring(13, 4) + "\n业务名：" + bookinfo.BusinessName;
                ti.Endding = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                try
                {
                    tp = new TicketPrint(ti);
                    tp.PrintTicket();
                    queuecontrol.InsertToHist(bookinfo.Name, bookinfo.IdCard);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (queuecontrol.GetTodayPrintCount(bookinfo.IdCard) > 0)
            {
                MessageBox.Show("您已经取过号了，不能重复取号，如遗失号票请联系我们的工作人员！");
            }
            else
            {
                tp = new TicketPrint(ti);
                tp.PrintTicket();
            }
        }

        /// <summary>  
        /// 时间戳转为C#格式时间  
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>C#格式时间</returns>  
        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        private void iniHotKey()
        {
            HotKey.RegisterHotKey(this.Handle, 101, HotKey.MODKEY.ALT, Keys.Q);
            //HotKey.RegisterHotKey(this.Handle, 102, HotKey.MODKEY.ALT, Keys.F);
            //RegisterHotKey(this.Handle, 11, MODKEY.ALT, Keys.S);
            //this.TopMost = true;
            this.Opacity = 0.9;
            this.Text = "身份证扫描程序";
            if (!InitialCVRDevice())
            {
                MessageBox.Show("身份证阅读仪初始化失败，请检查！");
                //this.Close();
            }
        }

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <returns>true:初始化成功；false:初始化失败</returns>
        int iRetUSB = 0, iRetCOM = 0;
        private bool InitialCVRDevice()
        {
            //int iRetUSB = 0, iRetCOM = 0;
            try
            {

                int iPort;
                for (iPort = 1001; iPort <= 1016; iPort++)
                {
                    iRetUSB = CVRSDK.CVR_InitComm(iPort);
                    if (iRetUSB == 1)
                    {
                        break;
                    }
                }
                if (iRetUSB != 1)
                {
                    for (iPort = 1; iPort <= 4; iPort++)
                    {
                        iRetCOM = CVRSDK.CVR_InitComm(iPort);
                        if (iRetCOM == 1)
                        {
                            break;
                        }
                    }
                }

                if ((iRetCOM == 1) || (iRetUSB == 1))
                {
                    //this.label9.Text = "初始化成功！";
                    return true;
                }
                else
                {
                    //this.label9.Text = "初始化失败！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// 读身份证
        /// </summary>
        private void LoopToTest()
        {
            if ((iRetCOM == 1) || (iRetUSB == 1))
            {
                int authenticate = 0;

                authenticate = CVRSDK.CVR_Authenticate();
                if (authenticate == 1)
                {

                    int readContent = CVRSDK.CVR_Read_Content(4);
                    if (readContent == 1)
                    {
                        //this.label10.Text = "读卡操作成功！";
                        FillData();
                        return;
                        //SendIDNumAndLoad(idinfo.Number);
                    }
                    else
                    {
                        //Thread.Sleep(1000);
                        MessageBox.Show("读卡操作失败！");
                        //Thread.Sleep(2000);
                    }

                }
                else
                {
                    //Thread.Sleep(1000);
                }

            }
            else
            {
                MessageBox.Show("设备初始化失败！");
            }
        }

        /// <summary>
        /// 获取份证信息并显示
        /// </summary>
        public void FillData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { FillData(); }));
            }
            else
            {
                try
                {
                    //pictureBox1.ImageLocation = Application.StartupPath + "\\zp.bmp";
                    byte[] name = new byte[30];
                    int length = 30;
                    CVRSDK.GetPeopleName(ref name[0], ref length);
                    //MessageBox.Show();
                    byte[] number = new byte[30];
                    length = 36;
                    CVRSDK.GetPeopleIDCode(ref number[0], ref length);
                    byte[] people = new byte[30];
                    length = 3;
                    CVRSDK.GetPeopleNation(ref people[0], ref length);
                    byte[] validtermOfStart = new byte[30];
                    length = 16;
                    CVRSDK.GetStartDate(ref validtermOfStart[0], ref length);
                    byte[] birthday = new byte[30];
                    length = 16;
                    CVRSDK.GetPeopleBirthday(ref birthday[0], ref length);
                    byte[] address = new byte[30];
                    length = 70;
                    CVRSDK.GetPeopleAddress(ref address[0], ref length);
                    byte[] validtermOfEnd = new byte[30];
                    length = 16;
                    CVRSDK.GetEndDate(ref validtermOfEnd[0], ref length);
                    byte[] dept = new byte[30];
                    length = 30;
                    CVRSDK.GetDepartment(ref dept[0], ref length);
                    byte[] sex = new byte[30];
                    length = 3;
                    CVRSDK.GetPeopleSex(ref sex[0], ref length);

                    byte[] samid = new byte[32];
                    CVRSDK.CVR_GetSAMID(ref samid[0]);

                    string validDateEndString = System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfEnd).Replace("\0", "").Trim();
                    DateTime validDateEnd = new DateTime();
                    DateTime.TryParse(validDateEndString, out validDateEnd);
                    if (DateTime.Compare(validDateEnd, DateTime.Now.Date) < 0)
                    {
                        MessageBox.Show("身份证已经过期，请核对");
                    }

                    idinfo.Address = System.Text.Encoding.GetEncoding("GB2312").GetString(address).Replace("\0", "").Trim();
                    idinfo.Birthday = System.Text.Encoding.GetEncoding("GB2312").GetString(birthday).Replace("\0", "").Trim();
                    idinfo.Name = System.Text.Encoding.GetEncoding("GB2312").GetString(name).Replace("\0", "").Trim();
                    idinfo.Number = System.Text.Encoding.GetEncoding("GB2312").GetString(number).Replace("\0", "").Trim();
                    idinfo.People = System.Text.Encoding.GetEncoding("GB2312").GetString(people).Replace("\0", "").Trim();
                    idinfo.Sex = System.Text.Encoding.GetEncoding("GB2312").GetString(sex).Replace("\0", "").Trim();
                    idinfo.Dept = System.Text.Encoding.GetEncoding("GB2312").GetString(dept).Replace("\0", "").Trim();
                    idinfo.ValidDate = System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfStart).Replace("\0", "").Trim() + "-" + System.Text.Encoding.GetEncoding("GB2312").GetString(validtermOfEnd).Replace("\0", "").Trim();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

        private const int ERR_SUCCESS = 1;       //执行成功
        private const int ERR_MAX_BUFFER = -1;       //超出最大缓存
        private const int ERR_NO_DATA_TWO = -2;       //2轨道无数据
        private const int ERR_NO_DATA_THREE = -3;       //3轨道无数据
        private const int ERR_NO_DATA_TWO_AND_THREE = -4;       //2、3轨道都无数据
        private const int ERR_NO_DATA_TWO_BUT_THREE = -5;       //2轨有数据，但3轨无数据
        private const int ERR_NO_DATA_THREE_BUT_TWO = -6;       //2轨无数据，但3轨有数据
        private const int ERR_INTPUT_PARAMETER = -7;       //输入参数有误
        private const int ERR_STATUS_FAIL_TWO = -8;       //2磁道操作状态失败
        private const int ERR_STATUS_FAIL_THREE = -9;       //3磁道操作状态失败
        private const int ERR_STATUS_FAIL_TWO_AND_THREE = -10;       //2、3磁道操作状态失败
        private const int ERR_UNKNOW = -99;       //未知错误

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //isClose = true;
            HotKey.UnregisterHotKey(this.Handle, 101);
            HotKey.UnregisterHotKey(this.Handle, 102);
            CVRSDK.CVR_CloseComm();
            //ZT606.ZtDevice_CR_Close();
            //this.Close();
        }

        /// <summary>
        /// 热键处理
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                if (m.WParam.ToInt32() == 101)
                {
                    //FillData();
                    //SendIDNumAndLoad(idinfo.Number);
                    LoopToTest();
                    //SwitchToThisWindow(this.Handle, true);
                }
                //else if (m.WParam.ToInt32() == 102)
                //{
                //    //MessageBox.Show(ReadBankCardNumber());
                //    string cardnum = ReadBankCardNumber();
                //    if (cardnum.Length > 0)
                //    {
                //        try
                //        {
                //            Clipboard.SetDataObject(cardnum, true);
                //            CopyOkDialog copyokdialog = new CopyOkDialog();
                //            copyokdialog.ShowDialog();
                //            //copyokdialog.Opacity=  0.8;                          
                //            //Thread.Sleep(3000);
                //            //copyokdialog.Close();
                //        }
                //        catch (Exception ex)
                //        {
                //            MessageBox.Show("复制帐号出错！", ex.Message);
                //        }
                //    }
                //}
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 设置SplitContainer
        /// </summary>
        private void SetSplitContainer()
        {
            //splitContainer1.Panel1.Width = 300;
            splitContainer1.FixedPanel = FixedPanel.Panel1;


        }

        /// <summary>
        /// 设置Panel2的布局
        /// </summary>
        private void SetPanel2Layout()
        {
            label2.Font = new Font("黑体", 50);
            btn_PrintTicket.Width = 400;
            btn_PrintTicket.Height = 200;
            btn_PrintTicket.Font = new Font("黑体", 40);

            int btnPosX = (splitContainer1.Panel2.Width - btn_PrintTicket.Width) / 2;
            int btnPosY = (splitContainer1.Panel2.Height - btn_PrintTicket.Height) / 2;

            int lblPosX = (splitContainer1.Panel2.Width - label2.Width) / 2;
            int lblPosY = (splitContainer1.Panel2.Height - label2.Height) / 2 - 200;

            Point btnP = new Point(btnPosX, btnPosY);
            btn_PrintTicket.Location = btnP;

            Point lblP = new Point(lblPosX, lblPosY);
            label2.Location = lblP;

        }

        private void printtest()
        {
            TicketInfo ti = new TicketInfo();
            ti.Header = "江门市住房公积金预约业务\n";
            ti.Context = "姓名：主*中\n证件：4407**********5117\n预约号：GZ5552525252552252225\n";
            ti.Endding = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
            TicketPrint tp = new TicketPrint(ti);
            try
            {
                tp.PrintTicket();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void booklisttest()
        {
            //var bookinfo=new BookInfo();
            string msg;
            BookCheck bookcheck = new BookCheck(bookinfo);
            bookcheck.Check(idinfo, out msg);
            //string url = "http://weixin.cnw-highlights.org/Home/Appointment/listApm";
            //string postDataStr = "sid=zj56d02564b7f1e";
            //bookcheck.HttpGet(url, postDataStr);
        }

        private void btn_ShowHide_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel1Collapsed == false)
            {
                splitContainer1.Panel1Collapsed = true;
                btn_ShowHide.Text = ">>";
            }
            else
            {
                splitContainer1.Panel1Collapsed = false;
                btn_ShowHide.Text = "<<";
            }


        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            SetPanel2Layout();
        }

        private void btn_PrintTicket_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(2000);
            //Thread t = new Thread(new Program());
            TipsForm tf = new TipsForm();
            if (tf.ShowDialog() == DialogResult.OK)
            {
                PrintTicketThrouthIDCard();
            }
            else
            {
                return;
            }

        }
    }



}
