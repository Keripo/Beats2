/*
 * Copyright (C) 2015, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */
using System.Collections.Generic;
using UnityEngine;

namespace Beats2.Audio
{

	public class MusicPlayer
	{
		private const string TAG = "MusicPlayer";

		private AudioSource _player;
		private SyncedTimer _timer;
		private int _syncFramesMax;
		private int _syncFrames;
		
		public MusicPlayer(AudioSource src, int syncFrameCount)
		{
			_player = src;
			_timer = new SyncedTimer();
			_syncFramesMax = syncFrameCount;
			_syncFrames = 0;
		}
		
		public void Update()
		{
			_timer.Update();
			if (_player.isPlaying) {
				if (_syncFrames > 0) {
					if (_timer.SyncTime(_player.time, _syncFrames / _syncFramesMax)) {
						_syncFrames--;
					}
				}
			}
		}
		
		public void Play()
		{
			Logger.Debug(TAG, "Play");
			if (!_timer.IsStarted()) {
				_timer.Start();
			} else {
				_timer.Resume();
			}
			_player.Play();
			_syncFrames = _syncFramesMax;
		}
		
		public void Pause()
		{
			Logger.Debug(TAG, "Pause");
			if (_timer.IsStarted()) {
				_timer.Pause();
				_player.Pause();
			}
		}
		
		public void Stop()
		{
			Logger.Debug(TAG, "Stop");
			if (_timer.IsStarted()) {
				_timer.Stop();
				_player.Stop();
			}
		}
		
		public bool IsPlaying()
		{
			return _timer.IsStarted() && _player.isPlaying;
		}

		public float GetTime()
		{
			return _timer.GetTime();
		}
	}
}