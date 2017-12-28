using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace sqlitedbdemo.SQLItedb
{
    class Person
    {
        public int sid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string dob { get; set; }
        public string city { get; set; }
        public string gender { get; set; }
        public byte[] image { get; set; }
    }
}