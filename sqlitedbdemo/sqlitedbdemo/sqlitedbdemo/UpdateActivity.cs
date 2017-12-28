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
using sqlitedbdemo.Adapter;
using Android.Database;
using System.IO;
using Android.Graphics;
using static Android.App.DatePickerDialog;
using Android.Provider;
using Android.Support.V7.App;

namespace sqlitedbdemo
{
    [Activity(Label = "UpdateActivity",Theme = "@style/MyCustomTheme")]
    public class UpdateActivity : Activity, IOnDateSetListener
    {
        int sid;
        ImageView img, imguser;
        EditText ename, eemail, epwd, edatee;
        Spinner scity;
        RadioButton rm, rf;
        RadioGroup rgender;
        Button btnsubmit, btnshow;
        private const int DATE_DIALOG = 1;
        private int year = 1996, month, day;
        Android.Net.Uri imageUri;
        string gender;
        string name, email, pwd, date, city, gen;
        byte[] simg,uimg;
        Bitmap bitmap;
        int set = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Update);
            sid = Intent.GetIntExtra("id", 0);

            img = FindViewById<ImageView>(Resource.Id.imgcal);
            imguser = FindViewById<ImageView>(Resource.Id.image);
            ename = FindViewById<EditText>(Resource.Id.ename);
            epwd = FindViewById<EditText>(Resource.Id.epwd);
            eemail = FindViewById<EditText>(Resource.Id.eemail);
            scity = FindViewById<Spinner>(Resource.Id.ecity);
            btnsubmit = FindViewById<Button>(Resource.Id.btnsubmit);
            btnshow = FindViewById<Button>(Resource.Id.btnshow);
            edatee = FindViewById<EditText>(Resource.Id.datee);
            img.Click += Img_Click;
            imguser.Click += Imguser_Click;
            btnsubmit.Click += Btnsubmit_Click;
            rgender = FindViewById<RadioGroup>(Resource.Id.gender);
            rm = FindViewById<RadioButton>(Resource.Id.male);
            rf = FindViewById<RadioButton>(Resource.Id.female);

            DBAdapter db = new DBAdapter(this);
            db.openDb();
            ICursor c= db.showbyId(sid);
          
            while(c.MoveToNext())
            {
          
                name= c.GetString(1);
                email = c.GetString(2);
                pwd = c.GetString(3);
                date= c.GetString(4);
             //   Toast.MakeText(this, date, ToastLength.Short).Show();
                city = c.GetString(5);
                gender = c.GetString(6);
                simg = c.GetBlob(7);
                
            }
            ename.Text = name.ToString();
            epwd.Text = pwd.ToString();
            eemail.Text = email.ToString();
            edatee.Text = date;
          
            string[] itemList = Resources.GetStringArray(Resource.Array.city);
            ArrayAdapter<string> arr = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, itemList);
            arr.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            scity.Adapter = arr;
            int itemPosition = arr.GetPosition(city);
            scity.SetSelection(itemPosition);

            // byte[] newBytes = Convert.FromBase64String(simg);
            byte[] newBytes = simg;
            MemoryStream ms = new MemoryStream();
            Bitmap n1 = BitmapFactory.DecodeByteArray(newBytes, 0, newBytes.Length);
            imguser.SetImageBitmap(n1);


            if(gender=="male")
            {
                rm.Checked = true;
            }
            else
            {
                rf.Checked = true;
            }
          
        }

        private void Btnsubmit_Click(object sender, EventArgs e)
        {
            DBAdapter db = new DBAdapter(this);
            db.openDb();
            string name = ename.Text;
            string email = eemail.Text;
            string pwd = epwd.Text;
            string date = edatee.Text;
            string city = scity.SelectedItem.ToString();
            string g = gender;

           // Bitmap bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, imageUri);

           //   Bitmap bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.image4);
           if(set==0)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                byte[] ba = stream.ToArray();
                uimg = ba;
            }
           else
            {
                uimg = simg;
            }
          
          
            long i = db.update(sid,name, email, pwd, date, city, g, uimg);
            if (i > 0)
            {
                Toast.MakeText(this, "Updated Successfully", ToastLength.Short).Show();
                Intent intent = new Intent(this, typeof(ShowActivity));
                StartActivity(intent);

            }
            else
            {
                Toast.MakeText(this, "Error", ToastLength.Short).Show();
            }
            db.closeDb();
        }

        private void Imguser_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            //imageIntent.SetType("image/*");
            //imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 2);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0 && resultCode == Result.Ok)
                CropImage();
            else if (requestCode == 2)
            {
                if (data != null)
                {
                    imageUri = data.Data;
                    //imguser.SetImageURI(imageUri);
                    CropImage();
                }
            }
            else if (requestCode == 1)
            {
                if (data != null)
                {
                    Bundle bundle = data.Extras;
                    bitmap = (Bitmap)bundle.GetParcelable("data");
                    imguser.SetImageBitmap(bitmap);
                    set = 0;

                    // imguser.SetImageURI(imageUri);
                }
            }
        }

        private void CropImage()
        {
            try
            {
                Intent CropIntent;
                CropIntent = new Intent("com.android.camera.action.CROP");
                CropIntent.SetDataAndType(imageUri, "image/*");

                CropIntent.PutExtra("crop", "true");
                CropIntent.PutExtra("outputX", 180);
                CropIntent.PutExtra("outputY", 180);
                CropIntent.PutExtra("aspectX", 3);
                CropIntent.PutExtra("aspectY", 4);
                CropIntent.PutExtra("scaleUpIfNeeded", true);
                CropIntent.PutExtra("return-data", true);

                StartActivityForResult(CropIntent, 1);
            }
            catch (ActivityNotFoundException ex)
            {

            }
        }

        private void Img_Click(object sender, EventArgs e)
        {
            ShowDialog(DATE_DIALOG);
        }
        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG:
                    return new DatePickerDialog(this, this, year, month, day);
                default:
                    break;
            }
            return null;
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            this.year = year;
            this.month = month;
            this.day = dayOfMonth;
            edatee.Text = year + "-" + (month + 1) + "-" + day;
            // Toast.MakeText(this, "You have selected : " + (month+1)  + day + year, ToastLength.Short).Show();
        }
    }
}