using UnityEngine;
using System.Collections;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using UnityEngine.UI;
namespace UIFrameWork
{
	public class ScanView : AlphaView
	{
		private ScanContext _context;

        private IScanner BarcodeScanner;
        public Text TextHeader;
        public RawImage Image;
        public AudioSource Audio;
        private float RestartTime;

        public override void Init ()
		{
			base.Init ();
		}
		public override void OnEnter(BaseContext context)
		{
			base.OnEnter(context);
			_context = context as ScanContext;

            // Create a basic scanner
            BarcodeScanner = new Scanner();
            BarcodeScanner.Camera.Play();

            // Display the camera texture through a RawImage
            BarcodeScanner.OnReady += (sender, arg) => {
                // Set Orientation & Texture
                Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
                Image.transform.localScale = BarcodeScanner.Camera.GetScale();
				RectTransform rect = Image.GetComponent<RectTransform>();
				rect.sizeDelta = new Vector2(rect.sizeDelta.x,rect.sizeDelta.x*BarcodeScanner.Camera.Texture.texelSize.y/BarcodeScanner.Camera.Texture.texelSize.x);
                Image.texture = BarcodeScanner.Camera.Texture;

                // Keep Image Aspect Ratio
                var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

                RestartTime = Time.realtimeSinceStartup;
            };
        }
        private void StartScanner()
        {
            BarcodeScanner.Scan((barCodeType, barCodeValue) => {
                BarcodeScanner.Stop();

                RestartTime += Time.realtimeSinceStartup + 1f;

                // Feedback
                //Audio.Play();

#if UNITY_ANDROID || UNITY_IOS
			Handheld.Vibrate();
#endif
            });
        }
        public IEnumerator StopCamera(Action callback)
        {
            // Stop Scanning
            BarcodeScanner.Destroy();
            BarcodeScanner = null;

            // Wait a bit
            yield return new WaitForSeconds(0.02f);

            callback.Invoke();
        }
        public void ClickBack()
        {
            // Try to stop the camera before loading another scene
            StartCoroutine(StopCamera(() => {
                PopCallBack();
            }));
        }
        public override void OnExit(BaseContext context)
		{
			base.OnExit(context);
		}

		public override void OnPause(BaseContext context)
		{
			base.OnPause(context);
            ClickBack();
		}

		public override void OnResume(BaseContext context)
		{
			base.OnResume(context);
            // Create a basic scanner
            BarcodeScanner = new Scanner();
            BarcodeScanner.Camera.Play();

            // Display the camera texture through a RawImage
            BarcodeScanner.OnReady += (sender, arg) => {
                // Set Orientation & Texture
                Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
                Image.transform.localScale = BarcodeScanner.Camera.GetScale();
                Image.texture = BarcodeScanner.Camera.Texture;

                // Keep Image Aspect Ratio
                var rect = Image.GetComponent<RectTransform>();
                var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

                RestartTime = Time.realtimeSinceStartup;
            };
        }
		public override void Excute ()
		{
			base.Excute ();
            if (BarcodeScanner != null)
            {
                BarcodeScanner.Update();
            }

            // Check if the Scanner need to be started or restarted
            if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
            {
                StartScanner();
                RestartTime = 0;
            }
        }
	}
	public class ScanContext : BaseContext
	{
		public ScanContext() : base(UIType.Scan)
		{
		}
	}
}
