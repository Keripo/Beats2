/*
 * Copyright (C) 2015, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Beats2.Audio
{

	public class MusicPlayerTest : MonoBehaviour
	{
		private const string TAG = "MusicPlayerTest";

		public Text togglePlaybackButtonText;
		public Text playbackStateText;
		public AudioSource audioSource;
		public int syncFrameCount;

		public Text logHistoryText;
		public int logHistorySize;

		private MusicPlayer _player;
		
		void Start()
		{
			_player = new MusicPlayer(audioSource, syncFrameCount);
			Logger.SetLogHistory(true);
			Logger.SetLogHistorySize(logHistorySize);
		}
	
		void Update()
		{
			_player.Update();
			playbackStateText.text = string.Format("Time: {0}", _player.GetTime());
			logHistoryText.text = Logger.GetLogHistoryString();
		}

		public void TogglePlayback()
		{
			Logger.Debug(TAG, "TogglePlayback");
			if (_player.IsPlaying()) {
				_player.Pause();
				togglePlaybackButtonText.text = "Paused";
			} else {
				_player.Play();
				togglePlaybackButtonText.text = "Playing";
			}
		}
	}
}
