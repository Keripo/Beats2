using Beats2.System;

/*
 * DONE
 */
namespace Beats2.System {
	
	/// <summary>
	/// Random value generator. Wraps Unity's Random class.
	/// </summary>
	public static class Rand {
		private const string TAG = "Random";
		
		public static void Init() {
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}
		
		public static void Reset() {
			SetSeed(SettingsManager.GetValueInt(Settings.MISC_RANDOM_SEED));
			Logger.Debug(TAG, "Reset...");
		}
		
		public static void SetSeed(int seed) {
			UnityEngine.Random.seed = seed;
		}
		
		public static int NextInt(int min, int max) {
			return UnityEngine.Random.Range(min, max);
		}
		
		public static float NextFloat(float min, float max) {
			return UnityEngine.Random.Range(min, max);
		}
		
	}
}
