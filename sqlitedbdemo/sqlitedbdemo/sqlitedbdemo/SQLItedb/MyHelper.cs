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
using Android.Database.Sqlite;

namespace sqlitedbdemo.SQLItedb
{
    class MyHelper : SQLiteOpenHelper
    {

        public static string DBNAME = "dbstud";
        public static string TBLNAME = "tblstud";
        public static int VERSION = 1;
        public static string TBLQUERY = "create table " + TBLNAME + "(sid integer primary key autoincrement,name text,email text,password text,dob text,city text,gender text,image blob);";
        public static String col_id = "sid";
        public static string DROPQUERY = "drop table if exists " + TBLNAME;

        public MyHelper(Context context) : base(context, DBNAME, null, VERSION) { }
       
        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(TBLQUERY);
            //db.ExecSQL(TBLQUERY2);
        }


        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL(DROPQUERY);
            OnCreate(db);
        }

    }
}