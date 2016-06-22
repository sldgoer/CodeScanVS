using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSCAN
{
    class TicketInfo
    {
        private string header;
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        private string context;
        public string Context
        {
            get { return context; }
            set { context = value; }
        }

        private string orderstring;
        public string OrderString
        {
            get { return orderstring; }
            set { orderstring = value; }
        }

        private string queuestring;
        public string QueueString
        {
            get { return queuestring; }
            set { queuestring = value; }
        }

        private string endding;
        public string Endding
        {
            get { return endding; }
            set { endding = value; }
        }
    }
}
