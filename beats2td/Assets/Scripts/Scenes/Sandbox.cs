using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Beats2;
using Beats2.System;
using Beats2.Audio;
using Beats2.Data;
using Beats2.Graphic;
using Beats2.UI;

namespace Beats2.Scenes {
	public class Sandbox : BaseScene {
		private const string TAG = "Sandbox";
		private static KeyCode[] _keyListeners = {
			KeyCode.W,
			KeyCode.A,
			KeyCode.S,
			KeyCode.D,
			KeyCode.Space,
			KeyCode.Return,
			KeyCode.Escape,
			KeyCode.Menu
		};

		private List<TestArrow> _arrows;
		private int _arrowCount;
		private float _addTimer;
		private const float ADD_INTERVAL = 0.3f;
		private const float SCREEN_DURATION = 5f;
		private TestArrow _randomArrow;
		private TestHold _hold1, _hold2;
		private TestText _audioTime, _touchLog, _collisionLog, _sysInfo;
		private FpsCounter _fpsCounter;
		private AudioPlayer _audioPlayer;
		private float _time;

		private TestMine _mine;

		// Use this for initialization
		public override void Start() {
			// Initialize
			InitAll();
			Logger.debug = true;

			// Set up input listeners
			Inputs.SetKeyListeners(_keyListeners);

			// Beats logo
			TestLogo.Init();
			TestLogo logo = TestLogo.Instantiate();
			logo.position = new Vector3(Screens.xmid, Screens.ymid, Screens.zmin);

			// Arrow in each corner
			TestArrow.Init();
			for (int x = 0; x < 3; x++) {
				for (int y = 0; y < 3; y++) {
					float posX = (x == 0) ? Screens.xmin : (x == 1) ? Screens.xmid : Screens.xmax;
					float posY = (y == 0) ? Screens.ymin : (y == 1) ? Screens.ymid : Screens.ymax;
					float posZ = Screens.zmin;
					TestArrow arrow = TestArrow.Instantiate();
					arrow.name = String.Format("_arrow({0}, {1})", x, y);
					arrow.position = new Vector3(posX, posY, posZ);
				}
			}
			_randomArrow = TestArrow.Instantiate();
			_randomArrow.name = "_randomArrow";
			_randomArrow.position = new Vector3(Screens.xmax - 75f, Screens.ymax - 50f, Screens.zmin);

			// Two random holds
			TestHold.Init();
			_hold1 = TestHold.Instantiate(Screens.height - _randomArrow.height);
			_hold1.name = "_hold1";
			_hold1.position = new Vector3(Screens.xmax - (_hold1.width / 2), Screens.ymid, Screens.zmid);

			_hold2 = TestHold.Instantiate(_randomArrow.height * 4);
			_hold2.name = "_hold2";
			_hold2.position = new Vector3(_hold1.x - _hold1.width * 2, Screens.ymid, Screens.zmid);

			// Background image
			TestBackground background = TestBackground.Instantiate();
			background.name = "_background2";
			background.position = new Vector3(Screens.xmid, Screens.ymid, Screens.zmax);
			background.color = new Color(background.color.r, background.color.g, background.color.b, 0.75f);

			// Generated arrows
			_arrows = new List<TestArrow>();
			_addTimer = ADD_INTERVAL;
			_arrowCount = 0;

			// Text label
			FontMeshData squareTextData = new FontMeshData(
				"_SquareFont",
				SysInfo.GetPath("Sandbox/Square.png"),
				SysInfo.GetPath("Sandbox/Square.fnt")
				);
			float textWidth = squareTextData.width * (_randomArrow.height / 2) / squareTextData.height;
			float textHeight = (_randomArrow.height / 2);

			_sysInfo = TestText.Instantiate(
				squareTextData,
				"_SysInfo",
				textWidth * 0.7f, textHeight * 0.7f,
				TextAnchor.UpperLeft
				);
			_sysInfo.position = new Vector3(Screens.xmin, Screens.ymax, Screens.zdebug);
			_sysInfo.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent Gray
			_sysInfo.text = SysInfo.InfoString();

			_audioTime = TestText.Instantiate(
				squareTextData,
				"_AudioTime",
				textWidth, textHeight,
				TextAnchor.LowerLeft
				);
			_audioTime.position = new Vector3(Screens.xmin, Screens.ymin + (_randomArrow.height / 2), Screens.zdebug);

			_touchLog = TestText.Instantiate(
				squareTextData,
				"_Touch Log",
				textWidth, textHeight,
				TextAnchor.LowerLeft
				);
			_touchLog.position = new Vector3(Screens.xmin, Screens.ymin, Screens.zdebug);
			_touchLog.color = new Color(255f / 255f, 204f/255f, 0f); // Tangerine

			_collisionLog = TestText.Instantiate(
				squareTextData,
				"_Collision Log",
				textWidth, textHeight,
				TextAnchor.LowerRight
				);
			_collisionLog.position = new Vector3(Screens.xmax, Screens.ymin, Screens.zdebug);
			_collisionLog.color = Color.red;

			// FPS Counter
			_fpsCounter = FpsCounter.Instantiate(squareTextData,textHeight);
			_fpsCounter.position = new Vector3(Screens.xmax, Screens.ymax, Screens.zdebug);

			// Load music
			_audioPlayer = AudioPlayer.Instantiate();
			_audioPlayer.Set(AudioClips.SANDBOX_SONG);
			_audioPlayer.loop = true;
			_audioPlayer.Play();

			TestMine.Init();
			_mine = TestMine.Instantiate();
			_mine.gameObject.transform.position = new Vector3(Screens.xmid, (Screens.ymin + Screens.ymid) / 2, Screens.zmin - 10);

			SettingsFile testIniFile = new SettingsFile(SysInfo.GetPath("Sandbox/Test.ini"));
			testIniFile.Set("Beats", "Version", "MODIFIED");
			testIniFile.Write(SysInfo.GetPath("Sandbox/Test2.ini"));
		}
		
		// Update is called once per frame
		public override void Update() {
			_audioTime.text = String.Format("Time: {0:f3}", _audioPlayer.time);

			//Logger.Log(TAG, _audioPlayer.time);

			// Get key input
			foreach (Inputs.KeyEvent e in Inputs.GetKeyEvents()) {
				_touchLog.text = e.ToString();
				if (e.key == KeyCode.Escape) {
					Application.Quit();
				}
			}

			// Get touch input
			foreach (Inputs.TouchEvent e in Inputs.GetTouchEvents()) {
				_touchLog.text = e.ToString();
				GameObject obj = Inputs.CollisionCheck(e.position);
				if (obj != null) {
					_collisionLog.text = obj.name;
					if (e.state == Inputs.TouchState.DOWN) {
						if (obj.tag == Tags.SANDBOX_TEST_MINE) {
							TestMine mine = obj.GetComponent<TestMine>();
							if (mine != null) {
								mine.Play();
							}
							if (_audioPlayer.isPlaying) {
								_audioPlayer.Pause();
							} else {
								_audioPlayer.Play();
							}
						} else if (obj.tag == Tags.SANDBOX_TEST_ARROW) {
							Logger.Warning(TAG, "Destroying arrow: " + obj.name);
							TestArrow arrow = obj.GetComponent<TestArrow>();
							if (arrow != null) {
								_arrows.Remove(arrow);
								arrow.Destroy();
							}
						}

						// DESTROY EVERYTHING!
						/*
						BeatsObject beatsObj = obj.GetComponent<BeatsObject>();
						if (beatsObj != null) {
							Logger.Warning(TAG, "Destroying " + obj.tag + ": " + obj.name);
							beatsObj.Destroy();
						}
						*/
					}
				}
			}

			for (int i = 0; i < _arrows.Count; i++) {
				TestArrow arrow = _arrows[i];
				if (arrow != null) {
					arrow.position = new Vector3(arrow.x, arrow.y - Screens.height * Time.deltaTime / SCREEN_DURATION, Screens.zmid + i);
					if (arrow.y < -arrow.height) {
						_arrows.Remove(arrow);
						arrow.Destroy();
					}
				}
			}

			_addTimer -= Time.deltaTime;
			if (_addTimer <= 0) {
				for (int i = 0; i < 4; i++) {
					TestArrow arrow = TestArrow.Instantiate();
					arrow.name = "_arrow" + _arrowCount;
					arrow.position = new Vector3(Screens.width / 4 + arrow.width * i, Screens.ymax + arrow.height, Screens.zmid);
					arrow.tag = Tags.SANDBOX_TEST_ARROW;
					_arrowCount++;
					_arrows.Add(arrow);
				}
				_addTimer = ADD_INTERVAL;
			}

			_fpsCounter.OnUpdate();
		}
	}
}