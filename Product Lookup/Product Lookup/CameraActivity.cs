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
using Product_Lookup.Model;
using Android.Content;
using System.Collections.Generic;

namespace Product_Lookup
{
    [Activity(Label = "CameraActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar", MainLauncher = false)]
    public class CameraActivity : AppCompatActivity, ISurfaceHolderCallback, IProcessor
    {
        private SurfaceView cameraView;
        private TextView cameraText;
        private CameraSource cameraSource;
        private Button btn_Capture;
        private string capture;
        private const int RequestCameraPermissionID = 1001;

        //TEMP VAR?
        ListView ReceiptItems;
        public static List<Item> CapturedItems;

        public TextView CameraText { get => cameraText; set => cameraText = value; }

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
            SetContentView(Resource.Layout.activity_camera);

            //Permission issue displaying camera in app
            cameraView = FindViewById<SurfaceView>(Resource.Id.camera_View1); 
            CameraText = FindViewById<TextView>(Resource.Id.camera_TextSense1);
            btn_Capture = FindViewById<Button>(Resource.Id.btn_Capture);

            //TEMP RECEIPT STUFF?
            ReceiptItems = FindViewById<ListView>(Resource.Id.listViewReceipt);

            TextRecognizer textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
            if (!textRecognizer.IsOperational)
                Log.Error("Main Activity", "Detector dependencies are not yet available");
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

            btn_Capture.Click += (s, e) =>
            {
                //capture = CameraText.Text;
                capture = "tesco\noranges\nEUR2.23\nmilk\nEUR1.00\nbread\nEUR1.55\nspices\nEUR3.46\nchocolate\nEUR1.20\nwaffles\nEUR1.80\nbananas\nEUR1.70\ncake\nEUR2.00\nrice\nEUR1.25\nEUR2.44"; //test string
                //capture = "tesco\noranges\nEUR2.23\nmilk\nEUR1.00";

                Receipt r = Sorter.DetermineStore(capture);
                SurfaceDestroyed(cameraView.Holder);

                CapturedItems = r.GetItems();

                //RECEIPT ACTIVITY
                Intent receiptActivity = new Intent(this, typeof(ReceiptActivity));
                StartActivity(receiptActivity);
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