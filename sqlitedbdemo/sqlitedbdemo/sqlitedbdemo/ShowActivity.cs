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
using sqlitedbdemo.SQLItedb;
using sqlitedbdemo.Adapter;
using Android.Database;
using Android.Support.V7.App;

namespace sqlitedbdemo
{
    [Activity(Label = "Users",Theme = "@style/MyCustomTheme1")]
    public class ShowActivity : Activity
    {
        ListView lstData;
        List<Person> lstSource = new List<Person>();
        ListViewAdapter adp;
        //int id;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Users);
            lstData = FindViewById<ListView>(Resource.Id.listView);
            loaddata();
            lstData.ItemClick += LstData_ItemClick;

           // RegisterForContextMenu(lstData);
        }

        private void LstData_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            PopupMenu menu = new PopupMenu(this,lstData.GetChildAt(e.Position));
            menu.Inflate(Resource.Menu.mymenu);
            menu.MenuItemClick += (s, a) =>
            {
                switch (a.Item.ItemId)
                {
                    case Resource.Id.update:
                        var id = lstSource[e.Position].sid;
                        Intent intent = new Intent(this, typeof(UpdateActivity));
                        intent.PutExtra("id", id);
                        StartActivity(intent);
                       // Toast.MakeText(this, "Update " + id, ToastLength.Short).Show();
                        break;
                       
                    case Resource.Id.delete:
                        var sid = lstSource[e.Position].sid;
                        // Toast.MakeText(this, "Delete " + sid, ToastLength.Short).Show();
                        DBAdapter db = new DBAdapter(this);
                        db.openDb();
                        int i = db.delete(sid);
                        if (i > 0)
                        {
                            Toast.MakeText(this, "Deleted Successfully ", ToastLength.Short).Show();
                            lstSource.Remove(new Person() { sid = sid });
                            loaddata();
                            adp.NotifyDataSetChanged();

                        }
                        else
                        {
                            Toast.MakeText(this, "Delete failed...", ToastLength.Short).Show();
                        }
                        break;
                }
            };

            menu.Show();
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            MenuInflater.Inflate(Resource.Menu.mymenu, menu);
           
           
        }
        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            switch (item.ItemId)
            {
                case Resource.Id.update:
                 
                    var id = lstSource[info.Position].sid;
                    Intent intent = new Intent(this, typeof(UpdateActivity));
                    intent.PutExtra("id", id);
                    StartActivity(intent);
                   // Toast.MakeText(this, "Update " + id, ToastLength.Short).Show();
                    break;

                case Resource.Id.delete:
                    var sid = lstSource[info.Position].sid;
                   // Toast.MakeText(this, "Delete " + sid, ToastLength.Short).Show();
                    DBAdapter db = new DBAdapter(this);
                    db.openDb();
                    int i= db.delete(sid);
                    if(i>0)
                    {
                        Toast.MakeText(this, "Deleted Successfully ", ToastLength.Short).Show();
                        lstSource.Remove(new Person() { sid = sid });
                        loaddata();
                        adp.NotifyDataSetChanged();
                       
                    }
                    else
                    {
                        Toast.MakeText(this, "Delete failed...", ToastLength.Short).Show();
                    }

                    break;
            }
            return true;
        }

        private void loaddata()
        {
            DBAdapter db = new DBAdapter(this);
            db.openDb();
            lstSource = db.Show();
            adp = new ListViewAdapter(this, lstSource);
            lstData.Adapter = adp;
           
            

        }
    }
}