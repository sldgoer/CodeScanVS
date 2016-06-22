using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using IDScanAndLoad;
using System.Runtime.InteropServices;

namespace CodeSCAN
{
    public partial class GetTicket : Form
    {
        public GetTicket()
        {
            InitializeComponent();
        }

        //IDInfo idinfo = null;

        private void GetTicket_Load(object sender, EventArgs e)
        {
            label1.Text = "预约取号系统";
            //Font f = new System.Drawing.Font("黑体", 40);
            //label1.Font = f;
            //SetButtonPos();
            //SetHintsPos();

            //idinfo = new IDInfo();
            //iniHotKey();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //label1.Text = "预约取号系统";
            //Font f = new System.Drawing.Font("黑体", 40);
            //label1.Font = f;
            //SetButtonPos();
            //SetHintsPos();
        }

        private void SetButtonPos()
        {
            int wndWidth = this.Width;
            int wndHeight = this.Height;

            int btnPosX = (wndWidth - btn_GetTicket.Width) / 2;
            int btnPosY = (wndHeight - btn_GetTicket.Height) / 2 + 100;

            Point p = new Point(btnPosX, btnPosY);
            btn_GetTicket.Location = p;
        }

        private void SetHintsPos()
        {
            int wndWidth = this.Width;
            int wndHeight = this.Height;

            int lblPosX = (wndWidth - label1.Width) / 2;
            int lblPosY = (wndHeight - label1.Height - 200) / 2;

            Point p = new Point(lblPosX, lblPosY);
            label1.Location = p;
        }

        private void btn_GetTicket_Click(object sender, EventArgs e)
        {
            label1.Text = @"请把二代居民身份证放在“证件阅读区”";
            //label1.res
            //LoopToTest();
            Form1 form1 = new Form1();
            form1.PrintTicketThrouthIDCard();                 
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            //SetHintsPos();
        }

        private void label1_Resize(object sender, EventArgs e)
        {
            SetHintsPos();
            //MessageBox.Show(label1.Width.ToString());
        }
        
    }
}
