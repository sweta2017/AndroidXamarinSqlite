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
using sqlitedbdemo.SQLItedb;
using Android.Database;

namespace sqlitedbdemo.Adapter
{
    class DBAdapter
    {
        private Context c;
        private SQLiteDatabase db;
        public MyHelper helper;

        public DBAdapter(Context c)
        {
            this.c = c;
            helper = new MyHelper(c);
        }
        public DBAdapter openDb()
        {
            try
            {
                db = helper.WritableDatabase;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return this;

        }
        public DBAdapter closeDb()
        {
            try
            {
                helper.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return this;

        }

        public List<Person> Show()
        {
            ICursor c= db.Query(MyHelper.TBLNAME, null, null, null, null, null, null);
            var list = new List<Person>();
            while (c.MoveToNext())
            {
                list.Add(new Person() { sid = c.GetInt(0), name = c.GetString(1), email = c.GetString(2) ,image=c.GetBlob(7)});
            }
            return list;

        }
        public ICursor showall()
        {
           return db.Query(MyHelper.TBLNAME, null, null, null, null, null, null);
        }
        public ICursor showbyId(int sid)
        {
            string ssid = sid.ToString();
            String[] whereArgs = { ssid };
            return db.Query(MyHelper.TBLNAME, null, "sid = ?", whereArgs, null, null, null);
        }

        internal long insert(string name, string email, string pwd, string date, string city, string g, byte[] img)
        {
            
           ContentValues cv = new ContentValues();
            cv.Put("name", name);
            cv.Put("email", email);
            cv.Put("password", pwd);
            cv.Put("dob", date);
            cv.Put("city", city);
             cv.Put("gender", g);
            cv.Put("image",img);


            return db.Insert(MyHelper.TBLNAME, null, cv);
        }
        internal long update(int sid,string name, string email, string pwd, string date, string city, string g, byte[] img)
        {
            string ssid = sid.ToString();
            String[] whereArgs = { ssid };
            ContentValues cv = new ContentValues();
            cv.Put("name", name);
            cv.Put("email", email);
            cv.Put("password", pwd);
            cv.Put("dob", date);
            cv.Put("city", city);
            cv.Put("gender", g);
            cv.Put("image", img);


            return db.Update(MyHelper.TBLNAME,cv, "sid = ?", whereArgs);
        }

        internal int delete(int sid)
        {
            string ssid = sid.ToString();
            String[] whereArgs = { ssid };
            return db.Delete(MyHelper.TBLNAME,"sid = ?",whereArgs);
           
        }


        //public ICursor showImage()
        //{
        //    string id = "4";
        //    String where = MyHelper.col_id + " = ?";
        //    String[] whereArgs = { id };
        //    return db.Query(MyHelper.TBLNAME2, null, where, whereArgs, null, null, null);

        //}

        //internal long insertImage(string bal)
        //{
        //    ContentValues cv = new ContentValues();
        //    cv.Put("imagename", bal);
        //    return db.Insert(MyHelper.TBLNAME2, null, cv);
        //}
    }
}