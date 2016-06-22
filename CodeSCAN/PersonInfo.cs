using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSCAN
{
    class PersonInfo
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string idcard;
        public string IdCard
        {
            get { return idcard; }
            set { idcard = value; }
        }

        private string orderid;
        public string OrderID
        {
            get { return orderid; }
            set { orderid = value; }
        }
    }
}
