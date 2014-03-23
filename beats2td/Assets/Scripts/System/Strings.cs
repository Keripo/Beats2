using System.Collections.Generic;

/*
 * TODO:
 * - keep updated
 */
namespace Beats2.System {
	
	/// <summary>
	/// Strings manager for localization
	/// </summary>
	public enum Strings {
		// TODO
	}

	public static class StringsManager {
		private const string TAG = "StringsManager";

		private static Dictionary<Strings, string> _strings;

		public static void Init() {
			_strings = new Dictionary<Strings, string>();
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}

		public static void Reset() {
			ReloadStrings();
			Logger.Debug(TAG, "Reset...");
		}

		public static void ReloadStrings() {
			// TODO	
		}

		public static string GetString(Strings stringId) {
			return _strings[stringId];
		}

	}
	
}

