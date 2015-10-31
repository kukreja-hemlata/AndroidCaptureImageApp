using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using System.IO;
using Java.IO;
using Android.Graphics;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Provider;
using Android.Content.PM;
using System.Net;
using CSharpIO = System.IO;
using System.Json;

namespace SnapWound
{
	[Activity(Label = "SnapWound", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		private ImageView _imageView;
		private String _stringImageBase64;
		private TextView _txtview;

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			// Make it available in the gallery

			Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
			Uri contentUri = Uri.FromFile(App._file);
			mediaScanIntent.SetData(contentUri);
			SendBroadcast(mediaScanIntent);

			// Display in ImageView. We will resize the bitmap to fit the display.
			// Loading the full sized image will consume to much memory
			// and cause the application to crash.

			int height = Resources.DisplayMetrics.HeightPixels;
			int width = _imageView.Height;

			if (App._file != null)
			{
				App.bitmap = BitmapFactory.DecodeFile(App._file.AbsolutePath);
				_stringImageBase64 = BitmapToBase64(App.bitmap);
            }


			App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
			if (App.bitmap != null)
			{
				_imageView.SetImageBitmap(App.bitmap);
				App.bitmap = null;
			}



			#region //Call a web api to get data commented code for later use
			//string url = "http://10.0.2.2/api/values/2"; //TODO Add here URL of your WebAPI   
			//var request = WebRequest.Create(url) as WebRequest;
			//request.Credentials = CredentialCache.DefaultNetworkCredentials;
			//request.Method = "GET";
			//HttpWebResponse responseTemp = (HttpWebResponse)request.GetResponse();
			//string returnString = responseTemp.StatusCode.ToString();
			//CSharpIO.Stream responseStream = responseTemp.GetResponseStream();
			//string returnData = new CSharpIO.StreamReader(responseStream).ReadToEnd();
			#endregion


			#region//Call a web api to send image data/test data for a string
			//string url = "http://10.0.2.2/api/values?value=Mita Param 1"; //TODO Add here URL of your WebAPI   
			//var request = WebRequest.Create(url) as WebRequest;
			//request.Credentials = CredentialCache.DefaultNetworkCredentials;
			//request.Method = "POST";

			//System.Json.JsonObject jsonNotif = new System.Json.JsonObject() { { "value", "Mita Post Test" } };

			//string body = jsonNotif.ToString();
			//request.ContentLength = body.Length;
			//request.ContentType = "application / json; charset = utf - 8";

			//CSharpIO.StreamWriter stOut = new CSharpIO.StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
			//stOut.Write(body);
			//stOut.Flush();
			//stOut.Close();

			//HttpWebResponse responseTemp = (HttpWebResponse)request.GetResponse();
			//string returnString = responseTemp.StatusCode.ToString();
			#endregion

			#region//Call a web api to send image data/test data in a json format
			//string url = "http://10.0.2.2/api/values/"; //TODO Add here URL of your WebAPI   
			//var request = WebRequest.Create(url) as WebRequest;
			//request.Credentials = CredentialCache.DefaultNetworkCredentials;
			//request.Credentials = new NetworkCredential();
			//request.Method = "POST";

			////String x = "{"AssessmentID": "1111","img":"ghghjghjgh"}"
			////JsonObject jsonSnapWound = new JsonObject() { { "SomeData", "SnapWound Post Test JSON 3" } }; 
			//JsonObject jsonSnapWound = new JsonObject() { { "imageBase64String", string.Format("{0}", _stringImageBase64) } };
			//string body = jsonSnapWound.ToString();

			//request.ContentLength = body.Length;
			//request.ContentType = "application/json; charset=utf-8";

			//CSharpIO.StreamWriter stOut = new CSharpIO.StreamWriter(request.GetRequestStream());
			//stOut.Write(body);
			//stOut.Flush();
			//stOut.Close();

			//try
			//{
			//	HttpWebResponse responseTemp = (HttpWebResponse)request.GetResponse();
			//	string returnString = responseTemp.StatusCode.ToString();

			//	CSharpIO.Stream responseStream = responseTemp.GetResponseStream();
			//	string returnData = "";
			//	if (responseStream != null)
			//		returnData = new CSharpIO.StreamReader(responseStream).ReadToEnd();
			//}
			//catch (Exception e)
			//{

			//}

			//// Dispose of the Java side bitmap.
			//GC.Collect();
			#endregion

			#region//Call a web api to send image data/test data in a json format
			string url = "http://10.0.2.2/api/WoundAssessment/Post"; //TODO Add here URL of your WebAPI   
			var request = WebRequest.Create(url) as WebRequest;
			//request.Credentials = CredentialCache.DefaultNetworkCredentials;
			request.Credentials = new NetworkCredential();
			request.Method = "POST";


			woundAssessmentImage tempImg = new woundAssessmentImage();
			tempImg.assessmentId = 141796;
			tempImg.woundId = 172138;
			tempImg.woundNumber = 1;
			tempImg.patientName = "Abbott, Bryan";
			tempImg.dos = "9/23/2015";
			tempImg.photoKey = "141796";
			tempImg.images = new List<image>();
				image img = new image();
				img.number = 1;
				img.data = "Hi";
				tempImg.images.Add(img);

				img = new image();
				img.number = 2;
				img.data = "12212";
				tempImg.images.Add(img);

			JsonObject j = new JsonObject();
			j.Add("assessmentId",141796);
            j.Add("woundId",172138);
            j.Add("woundNumber",1);
            j.Add("patientName","Abbott, Bryan");
            j.Add("dos","9 /23/2015");
            j.Add("photoKey","141796");
			JsonArray arr = new JsonArray();
			JsonObject jimg = new JsonObject();
			jimg.Add("number", 1);
			jimg.Add("data", _stringImageBase64);
			arr.Add(jimg);
			//j.Add("images",arr);
			j.Add("imageNumber", "5");
			j.Add("imageData", _stringImageBase64);

			string body = j.ToString();

			//JsonObject 

			request.ContentLength = body.Length;
			request.ContentType = "application/json; charset=utf-8";
			

			CSharpIO.StreamWriter stOut = new CSharpIO.StreamWriter(request.GetRequestStream());
			stOut.Write(body);
			stOut.Flush();
			stOut.Close();

			HttpWebResponse responseTemp = (HttpWebResponse)request.GetResponse();
			string returnString = responseTemp.StatusCode.ToString();

			CSharpIO.Stream responseStream = responseTemp.GetResponseStream();
			string returnData = "";
			if (responseStream != null)
				returnData = new CSharpIO.StreamReader(responseStream).ReadToEnd();

			if (returnString.Equals("OK"))
			{
				_txtview.Visibility = ViewStates.Visible;
			}

			// Dispose of the Java side bitmap.
			GC.Collect();
			#endregion
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);


			if (IsThereAnAppToTakePictures())
			{
				CreateDirectoryForPictures();

				//// Get our button from the layout resource,
				//// and attach an event to it
				_txtview = FindViewById<TextView>(Resource.Id.txtView);
				_txtview.Visibility = ViewStates.Invisible;
				Button button = FindViewById<Button>(Resource.Id.myButton);
				_imageView = FindViewById<ImageView>(Resource.Id.imageView1);
				button.Click += TakeAPicture;
			}
		}

		private void CreateDirectoryForPictures()
		{
			App._dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "SnapWound");
			if (!App._dir.Exists())
			{
				App._dir.Mkdirs();
			}
		}

		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities =
				PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		private void TakeAPicture(object sender, EventArgs eventArgs)
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
			StartActivityForResult(intent, 0);
		}

		//private static string ImageToBase64(string imageFileName, System.Drawing.Imaging.ImageFormat format)
		//{
		//	var image = new Bitmap(imageFileName);
		//	using (CSharpIO.MemoryStream ms = new CSharpIO.MemoryStream())
		//	{
		//		// Convert Image to byte[]
		//		image.Save(ms, format);
		//		byte[] imageBytes = ms.ToArray();

		//		// Convert byte[] to Base64 String
		//		string base64String = Convert.ToBase64String(imageBytes);
		//		return base64String;
		//	}
		//}

		private String BitmapToBase64(Bitmap bitmap)
		{
			using (CSharpIO.MemoryStream byteArrayOutputStream = new CSharpIO.MemoryStream())
			{
				bitmap.Compress(Bitmap.CompressFormat.Png, 100, byteArrayOutputStream);
				byte[] byteArray = byteArrayOutputStream.ToArray();
				return Convert.ToBase64String(byteArray);
			}
        }

	}

	public static class App
	{
		public static File _file;
		public static File _dir;
		public static Bitmap bitmap;
	}
}
