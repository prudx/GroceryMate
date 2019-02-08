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

            TextRecognizer textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
            if (!textRecognizer.IsOperational)
                Log.Error("Main Activity", "Detector dependencies are not yet available");
            else
            {
                cameraSource = new CameraSource.Builder(ApplicationContext, textRecognizer)
                    .SetFacing(CameraFacing.Back)
                    .SetRequestedPreviewSize(1280, 1024)
                    .SetRequestedFps(2.0f)
                    .SetAutoFocusEnabled(true)
                    .Build();

                cameraView.Holder.AddCallback(this);
                textRecognizer.SetProcessor(this);
            }

            btn_Capture.Click += (s, e) =>
            {
                capture = CameraText.Text;
                //seperate determination to receipt class by sending to receipt, initiallize type of receipt in constructor? 
                //good code seperation for architecture diagram etc
                if (capture.Contains("TESCO"))
                {
                    Receipt r = new TescoReceipt();
                    SurfaceDestroyed(cameraView.Holder);
                    capture = r.GetItems(capture);
                    CameraText.Text = capture;
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