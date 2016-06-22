using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace CodeSCAN
{
    class TicketPrint
    {
        PrintDocument pd = new PrintDocument();
        PrintPreviewDialog ppd = new PrintPreviewDialog();
        TicketInfo ti = new TicketInfo();

        //readonly string headerString = "江门市住房公积金管理中心";
        //StringBuilder sb = new StringBuilder();

        public TicketPrint(TicketInfo ticketinfo)
        {
            ti = ticketinfo;
        }

        private void PrintSetting()
        {
            //margin setting
            Margins margin = new Margins(0, 0, 0, 20);
            pd.DefaultPageSettings.Margins = margin;
            PaperSize ps = new PaperSize("CUSTOM", getInch(80), 600);
            pd.DefaultPageSettings.PaperSize = ps;
            pd.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);
            //sb = ticketinfo;
            ppd.Document = pd;

            //DialogResult rs = ppd.ShowDialog();
            
        }

        public void PrintTicket() 
        {
            PrintSetting();
            pd.Print();
            
        }

        private void MyPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(ti.Header, new System.Drawing.Font(new System.Drawing.FontFamily("黑体"), 16), System.Drawing.Brushes.Black, 10, 0);
            //e.Graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Brushes.Black),);
            e.Graphics.DrawString(ti.Context, new System.Drawing.Font(new System.Drawing.FontFamily("宋体"), 11), System.Drawing.Brushes.Black, 10, 30);
            e.Graphics.DrawString(ti.OrderString, new System.Drawing.Font(new System.Drawing.FontFamily("黑体"), 18), System.Drawing.Brushes.Black, 110, 90);
            e.Graphics.DrawString(ti.QueueString, new System.Drawing.Font(new System.Drawing.FontFamily("宋体"), 14), System.Drawing.Brushes.Black, 10, 120);
            e.Graphics.DrawString(ti.Endding, new System.Drawing.Font(new System.Drawing.FontFamily("宋体"), 11), System.Drawing.Brushes.Black, 50, 150);
            //e.Graphics.DrawLine()
        }

        private int getInch(double cm)
        {
            return (int)(cm / 25.4) * 100;
        }
    }
}
