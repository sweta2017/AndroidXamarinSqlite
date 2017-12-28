using Android.App;
using Android.Widget;
using Android.OS;
using static Android.App.DatePickerDialog;
using Android.Content;
using Android.Graphics;
using sqlitedbdemo.Adapter;
using Android.Provider;
using System.IO;
using System;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V4.Content;
using Android;
using Android.Support.V7.App;
using Android.Support.V4.App;

namespace sqlitedbdemo
{
    [Activity(Label = "sqlitedbdemo", MainLauncher = true,Theme = "@style/MyCustomTheme")]
    public class MainActivity : Activity,IOnDateSetListener
    {
        ImageView img,imguser;
        EditText ename, eemail, epwd, edate;
        Spinner scity;
        RadioGroup rgender;
        Button btnsubmit,btnshow;
        private const int DATE_DIALOG = 1;
        private int year=1990, month, day;
        Android.Net.Uri imageUri;
        string gender;
        const int RequestPermissionCode = 1;
        Bitmap bitmap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            img = FindViewById<ImageView>(Resource.Id.imgcal);
            imguser = FindViewById<ImageView>(Resource.Id.image);
            ename = FindViewById<EditText>(Resource.Id.ename);
            epwd = FindViewById<EditText>(Resource.Id.epwd);
            eemail = FindViewById<EditText>(Resource.Id.eemail);
            scity = FindViewById<Spinner>(Resource.Id.ecity);
            btnsubmit = FindViewById<Button>(Resource.Id.btnsubmit);
            btnshow = FindViewById<Button>(Resource.Id.btnshow);
            edate = FindViewById<EditText>(Resource.Id.edate);
            rgender = FindViewById<RadioGroup>(Resource.Id.gender);
            // RadioButton radioButton = FindViewById<RadioButton>(rgender.CheckedRadioButtonId);
           
            rgender.CheckedChange += Rgender_CheckedChange;
            img.Click += Img_Click;
            imguser.Click += Imguser_Click;
            btnsubmit.Click += Btnsubmit_Click;
            btnshow.Click += Btnshow_Click;
        

        }
        
        private void Btnshow_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(ShowActivity));
            StartActivity(i);
        }

        private void Btnsubmit_Click(object sender, System.EventArgs e)
        {

            DBAdapter db = new DBAdapter(this);
            db.openDb();
            string name = ename.Text;
            string email = eemail.Text;
            string pwd = epwd.Text;
            string date = edate.Text;
            string city = scity.SelectedItem.ToString();
            string g = gender;


          
            // Bitmap bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, imageUri);

            //Bitmap bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.password);
            MemoryStream stream = new MemoryStream();
               bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
               byte[] ba = stream.ToArray();
              //  string img = Convert.ToBase64String(ba);




            if (ename.Text == "")
            {
                
                Toast.MakeText(this, "Name should not be blank", ToastLength.Short).Show();
            }
            else if (email == "")
            {
                Toast.MakeText(this, "Email should not be blank", ToastLength.Short).Show();
            }
            else if (pwd == "")
            {
                Toast.MakeText(this, "Pwd should not be blank", ToastLength.Short).Show();
            }
            else if (imageUri == null)
            {
                Toast.MakeText(this, "Please select image", ToastLength.Short).Show();
            }
            else
            {
                long i = db.insert(name, email, pwd, date, city, g,ba);
                if (i > 0)
                {
                    Toast.MakeText(this, "Inserted", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Error", ToastLength.Short).Show();
                }
            }

            db.closeDb();

        }

        private void Imguser_Click(object sender, System.EventArgs e)
        {
            var imageIntent = new Intent(Intent.ActionPick,MediaStore.Images.Media.ExternalContentUri);

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
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
           switch(requestCode)
            {
                case RequestPermissionCode:
                    {
                        if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                            Toast.MakeText(this, "Permission Granted", ToastLength.Short).Show();
                        else
                            Toast.MakeText(this, "Permission Canceled", ToastLength.Short).Show();
                    }
                    break;

            }
        }

        private void Rgender_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            switch(rgender.CheckedRadioButtonId)
            {
                case Resource.Id.male:
                    //Toast.MakeText(this, "male", ToastLength.Short).Show();
                    gender = "male";
                    break;
                case Resource.Id.female:
                    //Toast.MakeText(this, "female", ToastLength.Short).Show();
                    gender = "female";
                    break;
            }
          
        }

        private void Img_Click(object sender, System.EventArgs e)
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
            edate.Text = year + "-" + (month+1) + "-" + day;
           // Toast.MakeText(this, "You have selected : " + (month+1)  + day + year, ToastLength.Short).Show();
        }
    }

}

