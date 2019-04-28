using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Util;
using Android.Graphics;
using Android;
using Android.Support.V4.App;
using Android.Content.PM;
using static Android.Gms.Vision.Detector;
using System.Text;
using GroceryMate.Model;
using Android.Content;
using System.Collections.Generic;
using GroceryMate.Services;
using System;
using GroceryMate.Helpers;
using System.Linq;
using System.Collections.ObjectModel;
using static GroceryMate.Helpers.Helper;

namespace GroceryMate
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false)]
    public class CameraActivity : AppCompatActivity, ISurfaceHolderCallback, IProcessor
    {
        SurfaceView cameraView;
        TextView cameraText;
        CameraSource cameraSource;
        Button btn_Capture;
        string capture;
        const int RequestCameraPermissionID = 1001;

        AzureService azureService = new AzureService();

        ListView ReceiptItems;
        static ICollection<Item> capturedItems; //was static

        TextView CameraText { get => cameraText; set => cameraText = value; }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestCameraPermissionID:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            cameraSource.Start(cameraView.Holder);
                        }
                    }
                    break;

            }
        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(GroceryMate.Resource.Layout.activity_camera);

            //Permission issue displaying camera in app
            cameraView = FindViewById<SurfaceView>(GroceryMate.Resource.Id.camera_View1); 
            CameraText = FindViewById<TextView>(GroceryMate.Resource.Id.camera_TextSense1);
            btn_Capture = FindViewById<Button>(GroceryMate.Resource.Id.btn_Capture);

            ReceiptItems = FindViewById<ListView>(GroceryMate.Resource.Id.listViewReceipt);

            TextRecognizer textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
            if (!textRecognizer.IsOperational)
                Log.Error("Camera Error", "Vision Detector dependencies are not available");
            else
            {
                cameraSource = new CameraSource.Builder(ApplicationContext, textRecognizer)
                    .SetFacing(CameraFacing.Back)
                    .SetRequestedPreviewSize(1920, 1080)
                    .SetRequestedFps(30.0f)
                    .SetAutoFocusEnabled(true)
                    .Build();

                cameraView.Holder.AddCallback(this);
                textRecognizer.SetProcessor(this);
            }

            btn_Capture.Click += async (s, e) =>
            {
                //if signed in & receipt detected, you can push receipt data
                if (Settings.IsLoggedIn == false)
                    CreateAlert(AlertType.Error, GetString(Resource.String.Error_LoggedOut), GetString(Resource.String.Error_LoggedOutTitle));
                else if (CameraText.Text == string.Empty)
                    CreateAlert(AlertType.Error, GetString(Resource.String.Error_NoDetections), GetString(Resource.String.Error_NoDetectionsTitle));
                else
                {
                    CreateAlert(AlertType.Load, GetString(Resource.String.Load_ReceiptScan), null);

                    //below captures used for testing
                    capture = CameraText.Text;
                    capture = "TESCO/nOranges/n2.40";

                    Receipt r = Sorter.DetermineStore(capture);
                    SurfaceDestroyed(cameraView.Holder);

                    capturedItems = r.Items; //sorted receipt items

                    await azureService.AddReceipt(r.StoreName, r.Items);    //add item is called from add receipt                    

                    //RECEIPT ACTIVITY
                    Intent receiptActivity = new Intent(this, typeof(ReceiptActivity));
                    StartActivity(receiptActivity);

                    if (dialog.IsShowing)
                        dialog.Dismiss();
                }
            };
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (ActivityCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[]
                {
                    Android.Manifest.Permission.Camera
                }, RequestCameraPermissionID);
                return;
            }
            cameraSource.Start(cameraView.Holder);
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            cameraSource.Stop();
        }

        public void ReceiveDetections(Detections detections)
        {
            SparseArray items = detections.DetectedItems;
            if (items.Size() != 0)
            {
                CameraText.Post(() =>
                {
                    StringBuilder strBuilder = new StringBuilder();
                    for (int i = 0; i < items.Size(); i++)
                    {
                        strBuilder.Append(((TextBlock)items.ValueAt(i)).Value);
                        strBuilder.Append("\n");

                    }
                    CameraText.Text = strBuilder.ToString();
                });
            }
        }

        public void Release()
        {

        }
    }
}