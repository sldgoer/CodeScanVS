﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDScanAndLoad
{
    class IDInfo
    {
        private String name;     //姓名
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private String sex;      //性别
        public String Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        private String people;    //民族，护照识别时此项为空
        public String People
        {
            get { return people; }
            set { people = value; }
        }
        private String birthday;   //出生日期
        public String Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }
        private String address;  //地址，在识别护照时导出的是国籍简码
        public String Address
        {
            get { return address; }
            set { address = value; }
        }
        private string number;  //地址，在识别护照时导出的是国籍简码
        public string Number
        {
            get { return number; }
            set { number = value; }
        }
        private string dept;   //签发日期，在识别护照时导出的是有效期至 
        public string Dept
        {
            get { return dept; }
            set { dept = value; }
        }
        private string validDate;
        public string ValidDate
        {
            get { return validDate; }
            set { validDate = value; }
        }
        //private string validtermOfStart;  //有效起始日期，在识别护照时为空
        //public string ValidtermOfStart
        //{
        //    get { return validtermOfStart; }
        //    set { validtermOfStart = value; }
        //}
        //private string validtermOfEnd;  //有效截止日期，在识别护照时为空
        //public string ValidtermOfEnd
        //{
        //    get { return validtermOfEnd; }
        //    set { validtermOfEnd = value; }
        //}
        
    }
}
