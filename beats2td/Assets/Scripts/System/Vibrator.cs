/*
 * TODO:
 * - integrate UHL plugin
 */
namespace Beats2.System {
	
	/// <summary>
	/// Vibration Manager.
	/// </summary>
	public static class Vibrator {
		private const string TAG = "Vibrator";
	
		private static bool _vibrate;
		
		public static void Init() {
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}
		
		public static void Reset() {
			_vibrate = SysInfo.vibrationSupport && SettingsManager.GetValueBool(Settings.SYSTEM_ENABLE_VIBRATIONS);
			Vibrate();
			Logger.Debug(TAG, "Reset...");
		}
		
		public static void Vibrate() {
			if (_vibrate) {
				// FIXME does not work...
				//UnityEngine.Handheld.Vibrate();
			}
		}
		
	}
}
