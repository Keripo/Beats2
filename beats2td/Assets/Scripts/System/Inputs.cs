using UnityEngine; // Keep for KeyCode, may try to abstract away later
using System;
using System.Collections.Generic;
using Beats2.System;

namespace Beats2.System {

	/// <summary>
	/// Input manager. Wraps Unity's Input class.
	/// </summary>
	public static class Inputs {
		private const string TAG = "Inputs";

		public enum KeyState {
			DOWN,
			HOLD,
			UP
		}

		public enum TouchState {
			DOWN,
			HOLD,
			UP,
			SWIPE
		}

		public struct KeyEvent {
			public KeyCode key { get; private set; }
			public KeyState state { get; private set; }
			public KeyEvent(KeyCode _key, KeyState _state) : this() {
				key = _key;
				state = _state;
			}
			public override string ToString() {
				return String.Format("Key {0} - {1}", key, state);
			}
		}

		public struct TouchEvent {
			public int touchId { get; private set; }
			public Vector2 position { get; private set; }
			public Vector2 velocity { get; private set; }
			public TouchState state { get; private set; }
			public TouchEvent(int _touchId, Vector2 _position, Vector2 _velocity, TouchState _state) : this() {
				touchId = _touchId;
				position = _position;
				velocity = _velocity;
				state = _state;
			}
			public override string ToString() {
				return String.Format("Touch {0} - {1}, {2}, {3}", touchId, state, position, velocity);
			}
		}
		
		private static KeyCode[] _keyListeners;
		private static Dictionary<int, Vector2> _touchStartPositions;
		private static Dictionary<int, float> _touchStartTimes;
		private static float INPUT_SWIPE_TIME_MAX;
		private static float INPUT_SWIPE_DIST_MIN;
		public const int MOUSE_ID = -1;

		public static void Init() {
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}

		public static void Reset() {
			_keyListeners = new KeyCode[] {};
			_touchStartPositions = new Dictionary<int, Vector2>();
			_touchStartTimes = new Dictionary<int, float>();
			INPUT_SWIPE_TIME_MAX = SettingsManager.GetValueFloat(Settings.INPUT_SWIPE_TIME_MAX);
			INPUT_SWIPE_DIST_MIN = SettingsManager.GetValueFloat(Settings.INPUT_SWIPE_DIST_MIN) * Screens.minPhysical;
			Logger.Debug(TAG, "Reset...");
		}

		public static void SetKeyListeners(KeyCode[] keys) {
			_keyListeners = keys;
		}
		
		public static List<KeyEvent> GetKeyEvents() {
			List<KeyEvent> keyEvents = new List<KeyEvent>();
			foreach (KeyCode k in _keyListeners) {
				if (UnityEngine.Input.GetKeyDown(k)) {
					keyEvents.Add(new KeyEvent(k, KeyState.DOWN));
				} else if (UnityEngine.Input.GetKeyUp(k)) {
					keyEvents.Add(new KeyEvent(k, KeyState.UP));
				} else if (UnityEngine.Input.GetKey(k)) {
					keyEvents.Add(new KeyEvent(k, KeyState.HOLD));
				}
			}
			if (Logger.debug) {
				foreach (KeyEvent e in keyEvents) {
					if (e.state != KeyState.HOLD)
					Logger.Debug(TAG, e);
				}
			}
			return keyEvents;
		}

		public static List<TouchEvent> GetTouchEvents() {
			List<TouchEvent> touchEvents = new List<TouchEvent>();

			if (SysInfo.touchSupport) { // Check for touch input
				foreach (Touch touch in UnityEngine.Input.touches) {
					if (touch.phase == TouchPhase.Began) {
						int touchId = touch.fingerId;
						Vector2 position = touch.position;
						if (_touchStartPositions.ContainsKey(touchId)) {
							_touchStartPositions[touchId] = position;
						} else {
							_touchStartPositions.Add(touchId, position);
						}
						if (_touchStartTimes.ContainsKey(touchId)) {
							_touchStartTimes[touchId] = Time.time;
						} else {
							_touchStartTimes.Add(touchId, Time.time);
						}
						touchEvents.Add(new TouchEvent(
							touchId,
							position,
							Vector2.zero,
							TouchState.DOWN
						));
					} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
						int touchId = touch.fingerId;
						Vector2 position = touch.position;
						Vector2 posDiff = position - _touchStartPositions[touchId];
						float timeDiff = Time.time - _touchStartTimes[touchId];
						if (posDiff.magnitude > INPUT_SWIPE_DIST_MIN && timeDiff < INPUT_SWIPE_TIME_MAX) {
							Vector2 velocity = (posDiff / Screens.minPhysical) / timeDiff;
							touchEvents.Add(new TouchEvent(
								touchId,
								position,
								velocity,
								TouchState.SWIPE
							));
						} else {
							touchEvents.Add(new TouchEvent(
								touchId,
								position,
								Vector2.zero,
								TouchState.UP
							));
						}
						_touchStartPositions[touchId] = Vector2.zero;
						_touchStartTimes[touchId] = 0f;
					} else if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
						int touchId = touch.fingerId;
						Vector2 position = touch.position;
						touchEvents.Add(new TouchEvent(
							touchId,
							position,
							Vector2.zero,
							TouchState.HOLD
						));
					}
				}
			} else { // Check for mouse input
				if (UnityEngine.Input.GetMouseButtonDown(0)) {
					int touchId = MOUSE_ID;
					Vector2 position = UnityEngine.Input.mousePosition;
					if (_touchStartPositions.ContainsKey(touchId)) {
						_touchStartPositions[touchId] = position;
					} else {
						_touchStartPositions.Add(touchId, position);
					}
					if (_touchStartTimes.ContainsKey(touchId)) {
						_touchStartTimes[touchId] = Time.time;
					} else {
						_touchStartTimes.Add(touchId, Time.time);
					}
					touchEvents.Add(new TouchEvent(
						touchId,
						position,
						Vector2.zero,
						TouchState.DOWN
					));
				} else if (UnityEngine.Input.GetMouseButtonUp(0)) {
					int touchId = MOUSE_ID;
					Vector2 position = UnityEngine.Input.mousePosition;
					Vector2 posDiff = position - _touchStartPositions[touchId];
					float timeDiff = Time.time - _touchStartTimes[touchId];
					if (posDiff.magnitude > INPUT_SWIPE_DIST_MIN && timeDiff < INPUT_SWIPE_TIME_MAX) {
						Vector2 velocity = (posDiff / Screens.minPhysical) / timeDiff;
						touchEvents.Add(new TouchEvent(
							touchId,
							position,
							velocity,
							TouchState.SWIPE
						));
					} else {
						touchEvents.Add(new TouchEvent(
							touchId,
							position,
							Vector2.zero,
							TouchState.UP
						));
					}
					_touchStartPositions[touchId] = Vector2.zero;
					_touchStartTimes[touchId] = 0f;
				} else if (UnityEngine.Input.GetMouseButton(0)) {
					int touchId = MOUSE_ID;
					Vector2 position = UnityEngine.Input.mousePosition;
					touchEvents.Add(new TouchEvent(
						touchId,
						position,
						Vector2.zero,
						TouchState.HOLD
					));
				}
			}

			if (Logger.debug) {
				foreach (TouchEvent e in touchEvents) {
					if (e.state != TouchState.HOLD)
					Logger.Debug(TAG, e);
				}
			}
			return touchEvents;
		}

		public static GameObject CollisionCheck(Vector2 position) {
			Ray ray = Camera.mainCamera.ScreenPointToRay(position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				return hit.collider.gameObject;
			} else {
				return null;
			}
		}
	}
}
