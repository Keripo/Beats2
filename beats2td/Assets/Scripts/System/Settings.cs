using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Beats2.System;

namespace Beats2.System {

	public class SettingsInfo : Attribute {
		public string name, defaultValue;
		public SettingsInfo(string name, string defaultValue) {
			this.name = name;
			this.defaultValue = defaultValue;
		}
	}

	public enum Settings {
		// Try to keep these in alphabetic order
		[SettingsInfo("DebugMode",				"True")]		DEBUG,
		[SettingsInfo("InputSwipeTimeMax",		"0.250000")]	INPUT_SWIPE_TIME_MAX,
		[SettingsInfo("InputSwipeDistMin",		"0.20")]		INPUT_SWIPE_DIST_MIN,
		[SettingsInfo("MiscRandomSeed",			"0")]			MISC_RANDOM_SEED,
		[SettingsInfo("MiscFpsUpdateInterval",	"0.5")]			MISC_FPS_UPDATE_INTERVAL,
		[SettingsInfo("ScorePercentAAA",		"1.00")]		SCORE_PERCENT_AAA,
		[SettingsInfo("ScorePercentAA",			"0.93")]		SCORE_PERCENT_AA,
		[SettingsInfo("ScorePercentA",			"0.80")]		SCORE_PERCENT_A,
		[SettingsInfo("ScorePercentB",			"0.65")]		SCORE_PERCENT_B,
		[SettingsInfo("ScorePercentC",			"0.45")]		SCORE_PERCENT_C,
		[SettingsInfo("ScoreTimingFlawless",	"0.016667")]	SCORE_TIMING_FLAWLESS,
		[SettingsInfo("ScoreTimingPerfect",		"0.033333")]	SCORE_TIMING_PERFECT,
		[SettingsInfo("ScoreTimingGreat",		"0.066667")]	SCORE_TIMING_GREAT,
		[SettingsInfo("ScoreTimingGood",		"0.083333")]	SCORE_TIMING_GOOD,
		[SettingsInfo("ScoreTimingBad",			"0.133333")]	SCORE_TIMING_BAD,
		[SettingsInfo("ScoreTimingOk",			"0.250000")]	SCORE_TIMING_OK,
		[SettingsInfo("SystemPhoneScreenWidth",	"3")]			SYSTEM_PHONE_SCREEN_WIDTH,
		[SettingsInfo("SystemEnableVibrations",	"True")]		SYSTEM_ENABLE_VIBRATIONS
	}

	public static class SettingsManager {
		private const string TAG = "SettingsManager";

		private static Dictionary<Settings, SettingsInfo> _settingsMap;
		private static Dictionary<string, Settings> _settingsNames;
		private static Dictionary<Settings, string> _settingsValues;

		public static void Init() {
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}

		public static void Reset() {
			InitDefaults();
			LoadSettings();
			Logger.Debug(TAG, "Reset...");
		}

		public static void InitDefaults() {
			int numSettings = Enum.GetNames(typeof(Settings)).Length;
			_settingsMap = new Dictionary<Settings, SettingsInfo>(numSettings);
			_settingsNames = new Dictionary<string, Settings>(numSettings);
			_settingsValues = new Dictionary<Settings, string>(numSettings);

			foreach (Settings setting in Enum.GetValues(typeof(Settings))) {
				// Reflection magic!
				MemberInfo memberInfo = typeof(Settings).GetMember(setting.ToString()).FirstOrDefault();
				SettingsInfo settingInfo = (SettingsInfo)Attribute.GetCustomAttribute(memberInfo, typeof(SettingsInfo));

				// Build reference dictionaries
				_settingsMap.Add(setting, settingInfo);
				_settingsNames.Add(settingInfo.name, setting);
				_settingsValues.Add(setting, settingInfo.defaultValue);
			}
		}

		public static void LoadSettings() {
			// TODO - Use IniParser to read from settings file
		}

		public static void SaveSettings() {
			// Use LINQ to sort dictionary alphabetically
			//foreach (KeyValuePair<Settings, string> pair in _settingsValues.OrderBy(i => i.Key)) {
				// TODO - Use IniParser to write from settings file
			//}
		}

		public static string GetName(Settings setting) {
			SettingsInfo settingInfo;
			if (_settingsMap.TryGetValue(setting, out settingInfo)) {
				return settingInfo.name;
			} else {
				Logger.Error(TAG, String.Format("Unable to get name for setting \"{0}\"", setting.ToString()));
				return null;
			}
		}

		public static string GetDefaultValue(Settings setting) {
			SettingsInfo settingInfo;
			if (_settingsMap.TryGetValue(setting, out settingInfo)) {
				return settingInfo.defaultValue;
			} else {
				Logger.Error(TAG, String.Format("Unable to get name for setting \"{0}\"", setting.ToString()));
				return null;
			}
		}

		public static Settings GetSetting(string name) {
			object setting = Enum.Parse(typeof(Settings), name, true);
			if (setting != null) {
				return (Settings)setting;
			} else {
				Logger.Error(TAG, String.Format("Unable to find setting with name \"{0}\"", name));
				return Settings.DEBUG; // Can't return null
			}
		}

		public static string GetValue(Settings setting) {
			return _settingsValues[setting];
		}
		
		public static bool GetValueBool(Settings setting) {
			bool val;
			if (bool.TryParse(_settingsValues[setting], out val)) {
				return val;
			} else {
				Logger.Error(TAG, String.Format("Unable to parse bool \"{0}\" for setting \"{1}\"", _settingsValues[setting], setting));
				return false;
			}
		}
		public static int GetValueInt(Settings setting) {
			int val;
			if (int.TryParse(_settingsValues[setting], out val)) {
				return val;
			} else {
				Logger.Error(TAG, String.Format("Unable to parse int \"{0}\" for setting \"{1}\"", _settingsValues[setting], setting));
				return -1;
			}
		}
		
		public static float GetValueFloat(Settings setting) {
			float val;
			if (float.TryParse(_settingsValues[setting], out val)) {
				return val;
			} else {
				Logger.Error(TAG, String.Format("Unable to parse float \"{0}\" for setting \"{1}\"", _settingsValues[setting], setting));
				return -1f;
			}
		}
		
		private static void SetValue(string settingName, string newValue) {
			Settings setting = (Settings)Enum.Parse(typeof(Settings), settingName, true);
			_settingsValues[setting] = newValue;
		}
		
		public static void SetValue(Settings setting, string newValue) {
			_settingsValues[setting] = newValue;
		}
		
	}
}
