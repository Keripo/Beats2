using System;
using Beats2.System;

/*
 * DONE
 */
namespace Beats2.System {
	
	/// <summary>
	/// Screen info. Wraps Unity's Screen class.
	/// </summary>
	public static class Screens {
		private const string TAG = "Screens";

		public enum ScreenOrientations {
			PORTRAIT,
			LANDSCAPE
		}

		public static float width			{ get; private set; }
		public static float height			{ get; private set; }
		public static float depth			{ get; private set; }
		public static float min				{ get; private set; }
		public static float dpi				{ get; private set; }
		public static float widthPhysical	{ get; private set; }
		public static float heightPhysical	{ get; private set; }
		public static float minPhysical		{ get; private set; }

		public static float xmin			{ get; private set; }
		public static float xmid			{ get; private set; }
		public static float xmax			{ get; private set; }
		public static float ymin			{ get; private set; }
		public static float ymid			{ get; private set; }
		public static float ymax			{ get; private set; }
		public static float zmin			{ get; private set; }
		public static float zmid			{ get; private set; }
		public static float zmax			{ get; private set; }
		public static float zdebug			{ get; private set; }
		public static UnityEngine.Vector3 middle { get; private set; }

		private static ScreenOrientations _orientation;
		public static ScreenOrientations orientation {
			get {
				return _orientation;
			}
			set {
				SetOrientation(value);
				_orientation = value;
			}
		}

		public static void Init() {
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}
		
		public static void Reset() {
			width = (float)UnityEngine.Screen.width;
			height = (float)UnityEngine.Screen.height;
			depth = UnityEngine.Camera.mainCamera.farClipPlane;
			min = (width < height) ? width : height;
			dpi = (UnityEngine.Screen.dpi > 0) ? UnityEngine.Screen.dpi : 1f;
			widthPhysical = width / dpi;
			heightPhysical = height / dpi;
			minPhysical = (widthPhysical < heightPhysical) ? widthPhysical : heightPhysical;
			orientation = GetOrientation();
			SetReferencePoints();
			Logger.Debug(TAG, "Reset...");
		}

		public static void Screenshot() {
			string fileName = String.Format("{0}.png", DateTime.Now.ToString());
			UnityEngine.Application.CaptureScreenshot(fileName);
			Logger.Log("Screenshot", fileName);
		}

		private static void SetReferencePoints() {
			xmin = 0f;
			xmid = width / 2;
			xmax = width;
			ymin = 0f;
			ymid = height / 2;
			ymax = height;
			zmin = 1 * depth / 4;
			zmid = 2 * depth / 4;
			zmax = 3 * depth / 4;
			zdebug = 0f;
			middle = new UnityEngine.Vector3(xmid, ymid, zmid);
		}

		private static ScreenOrientations GetOrientation() {
			switch (UnityEngine.Screen.orientation) {
				case UnityEngine.ScreenOrientation.Portrait:
				case UnityEngine.ScreenOrientation.PortraitUpsideDown:
					return ScreenOrientations.PORTRAIT;
				case UnityEngine.ScreenOrientation.LandscapeLeft:
				case UnityEngine.ScreenOrientation.LandscapeRight:
				default:
					return ScreenOrientations.LANDSCAPE;
			}
		}

		private static void SetOrientation(ScreenOrientations newOrientation) {
			if (newOrientation != orientation) {
				switch (newOrientation) {
					case ScreenOrientations.PORTRAIT:
						UnityEngine.Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
						break;
					case ScreenOrientations.LANDSCAPE:
					default:
						UnityEngine.Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
						break;
				}
			}
		}
	}
}
