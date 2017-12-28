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
using System.IO;
using Android.Graphics;

namespace sqlitedbdemo.Adapter
{

    class ListViewAdapter : BaseAdapter<Person>
    {
        Activity context;
        List<Person> list;

        public ListViewAdapter(Activity _context, List<Person> _list) : base()
        {
            this.context = _context;
            list = _list;

        }
        public override Person this[int position]
        {
            get
            {
                return list[position];
            }
        }

        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.userlist, parent, false);

            Person item = this[position];

            view.FindViewById<TextView>(Resource.Id.name).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.email).Text = item.email;
            byte[] newBytes = item.image;
            MemoryStream ms = new MemoryStream();

            Bitmap n1 = BitmapFactory.DecodeByteArray(newBytes, 0, newBytes.Length);
            view.FindViewById<ImageView>(Resource.Id.imguser).SetImageBitmap(n1);
           
            //using (var imageView = view.FindViewById<ImageView>(Resource.Id.Thumbnail))
            //{
            //    string url = Android.Text.Html.FromHtml(item.thumbnail).ToString();

            //    //Download and display image
            //    Koush.UrlImageViewHelper.SetUrlDrawable(imageView,
            //        url, Resource.Drawable.Placeholder);
            //}
            return view;
        }
    }
}