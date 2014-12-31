/*
 * Copyright (C) 2015, Philip Peng (Keripo). All rights reserved.
 * http://beats2.net
 * The software in this package is published under the terms of the BSD-style license
 * a copy of which has been included with this distribution in the LICENSE file.
 */
using System.Collections.Generic;
using UnityEngine;

namespace Beats2
{

	public class SyncedTimer
	{
		private const string TAG = "SyncedTimer";

		private bool _isStarted;
		private bool _isPaused;
		private float _currentTime;
		private float _startTime;
		private float _offsetTime;
		private float _lastPausedTime;
		private float _lastSyncedTime;
		
		public SyncedTimer()
		{
			Stop();
		}
		
		public void Update()
		{
			if (_isStarted && !_isPaused) {
				_currentTime = GetSystemTime() - _startTime + _offsetTime;
			}
		}
		
		public void Start()
		{
			if (!_isStarted) {
				_isStarted = true;
				_startTime = GetSystemTime();
				_currentTime = _startTime;
				Logger.Debug(TAG, "Start at {0}", _startTime);
			}
		}
		
		public void Pause()
		{
			if (_isStarted && !_isPaused) {
				_isPaused = true;
				_lastPausedTime = GetSystemTime();
				Logger.Debug(TAG, "Pause at {0}", _lastPausedTime);
			}
		}
		
		public void Resume()
		{
			if (_isStarted && _isPaused) {
				float resumeOffset = _lastPausedTime - GetSystemTime();
				_offsetTime += resumeOffset;
				_isPaused = false;
				Logger.Debug(TAG, "Resume with offset {0}", resumeOffset);
			}
		}
		
		public void Stop()
		{
			_currentTime = 0f;
			_startTime = 0f;
			_offsetTime = 0f;
			_lastPausedTime = 0f;
			_lastSyncedTime = 0f;
			_isStarted = false;
			_isPaused = false;
			Logger.Debug(TAG, "Stop");
		}
		
		public bool SyncTime(float syncedTime, float weight)
		{
			if (_isStarted && _lastSyncedTime != syncedTime) {
				_lastSyncedTime = syncedTime;
				float syncOffset = (syncedTime - GetTime()) * weight;
				if (syncOffset != 0f) {
					_offsetTime += syncOffset;
					Logger.Debug(TAG, "Synced timer with offset {0}", syncOffset);
					return true;
				}
			}
			return false;
		}
		
		public bool IsStarted()
		{
			return _isStarted;
		}
		
		public bool IsPaused()
		{
			return _isPaused;
		}

		public float GetTime()
		{
			return _currentTime; 
		}
		
		private float GetSystemTime()
		{
			return Time.time;
		}
	}
}