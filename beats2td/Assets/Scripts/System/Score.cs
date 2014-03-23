using System;
using Beats2.System;
using Beats2.Data;

/*
 * TODO:
 * - keep updated
 */
namespace Beats2.System {
	
	/// <summary>
	/// Scoring system. Based on DDR Extreme (8th Mix)'s scoring system
	/// </summary>
	public static class Score {
		private const string TAG = "Score";
		
		public enum AccuracyType {
			FLAWLESS, // MARVELOUS
			PERFECT,
			GREAT,
			GOOD,
			BAD, // BOO
			MISS,
			OK,
			NG,
			IGNORE
		}
		
		public enum GameState {
			PLAYING,
			PAUSED,
			FINISHED,
			FAILED
		}

		public enum Rankings {
			F,
			D,
			C,
			B,
			A,
			AA,
			AAA
		}
		
		private static GameState _state;
		private static float SCORE_PERCENT_AAA, SCORE_PERCENT_AA, SCORE_PERCENT_A, SCORE_PERCENT_B, SCORE_PERCENT_C;
		private static float SCORE_TIMING_FLAWLESS, SCORE_TIMING_PERFECT, SCORE_TIMING_GREAT, SCORE_TIMING_GOOD, SCORE_TIMING_BAD, SCORE_TIMING_OK;
		
		public static void Init() {
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}
		
		public static void Reset() {
			// Game state
			_state = GameState.PLAYING;
			
			// Load scoring settings
			SCORE_PERCENT_AAA = SettingsManager.GetValueFloat(Settings.SCORE_PERCENT_AAA);
			SCORE_PERCENT_AA = SettingsManager.GetValueFloat(Settings.SCORE_PERCENT_AA);
			SCORE_PERCENT_A = SettingsManager.GetValueFloat(Settings.SCORE_PERCENT_A);
			SCORE_PERCENT_B = SettingsManager.GetValueFloat(Settings.SCORE_PERCENT_B);
			SCORE_PERCENT_C = SettingsManager.GetValueFloat(Settings.SCORE_PERCENT_C);
			SCORE_TIMING_FLAWLESS = SettingsManager.GetValueFloat(Settings.SCORE_TIMING_FLAWLESS);
			SCORE_TIMING_PERFECT = SettingsManager.GetValueFloat(Settings.SCORE_TIMING_PERFECT);
			SCORE_TIMING_GREAT = SettingsManager.GetValueFloat(Settings.SCORE_TIMING_GREAT);
			SCORE_TIMING_GOOD = SettingsManager.GetValueFloat(Settings.SCORE_TIMING_GOOD);
			SCORE_TIMING_BAD = SettingsManager.GetValueFloat(Settings.SCORE_TIMING_BAD);
			SCORE_TIMING_OK = SettingsManager.GetValueFloat(Settings.SCORE_TIMING_OK);
			Logger.Debug(TAG, "Reset...");
		}
		
		public static GameState GetGameState() {
			return _state;
		}
		
		public static int GetPointValue(AccuracyType accuracy) {
			switch (accuracy) {
				case AccuracyType.FLAWLESS:	return 2;
				case AccuracyType.PERFECT:	return 2;
				case AccuracyType.GREAT:	return 1;
				case AccuracyType.GOOD:		return 0;
				case AccuracyType.BAD:		return -4;
				case AccuracyType.MISS:		return -8;
				case AccuracyType.OK:		return 6;
				case AccuracyType.NG:		return 0;
				default:
					Logger.Error("Scores.GetPointValue", String.Format("Unknown AccuracyType \"{0}\"", accuracy));
					return -999;
			}
		}
		
		public static Rankings GetRanking(float percent) {
			if (_state == GameState.FAILED) {
				return Rankings.F;
			} else if (percent >= SCORE_PERCENT_AAA) {
				return Rankings.AAA;
			} else if (percent >= SCORE_PERCENT_AA) {
				return Rankings.AA;
			} else if (percent >= SCORE_PERCENT_A) {
				return Rankings.A;
			} else if (percent >= SCORE_PERCENT_B) {
				return Rankings.B;
			} else if (percent >= SCORE_PERCENT_C) {
				return Rankings.C;
			} else {
				return Rankings.D;
			}
		}
		
		public static AccuracyType GetAccuracyValue(float timeDiff, NoteType type) {
			float timeDiffAbs = (timeDiff > 0) ? (timeDiff) : (-timeDiff);
			switch (type) {
				case NoteType.TAP:
				case NoteType.HOLD:
				case NoteType.ROLL:
				case NoteType.SLIDE:
				case NoteType.REPEAT:
					if (timeDiffAbs <= SCORE_TIMING_FLAWLESS) {
						return AccuracyType.FLAWLESS;
					} else if (timeDiffAbs <= SCORE_TIMING_PERFECT) {
						return AccuracyType.PERFECT;
					} else if (timeDiffAbs <= SCORE_TIMING_GREAT) {
						return AccuracyType.GREAT;
					} else if (timeDiffAbs <= SCORE_TIMING_GOOD) {
						return AccuracyType.GOOD;
					} else if (timeDiffAbs <= SCORE_TIMING_BAD) {
						return AccuracyType.BAD;
					} else {
						if (timeDiff > SCORE_TIMING_BAD) {
							return AccuracyType.MISS;
						} else {
							return AccuracyType.IGNORE;	
						}
					}
				default:
					Logger.Error("Scores.GetAccuracyValue", String.Format("Incorrect noteEvent \"{0}\"", type));
					return AccuracyType.IGNORE;	
			}
		}
		
	}
}
