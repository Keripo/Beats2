using System;
using System.Collections.Generic;

/*
 * TODO:
 * - add Tracker (Playtonic?)
 */
namespace Beats2.System {
	
	/// <summary>
	/// Tracker. TODO
	/// </summary>
	public static class Tracker {
		private const string TAG = "Tracker";
		
		public enum TrackEvent {
			// TODO
		}
		
		public static void Init() {
			// TODO
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}

		public static void Reset() {
			// TODO
			Logger.Debug(TAG, "Reset...");
		}
		
		public static void Track(TrackEvent ev) {
			// TODO
		}
		
		public static void Track(TrackEvent ev, string attribute) {
			// TODO
		}
		
		public static void Track(TrackEvent ev, Dictionary<String, String> attributes) {
			// TODO
		}
		
		public static void Track(TrackEvent ev, string attribute, Exception e) {
			// TODO
		}
	}
}