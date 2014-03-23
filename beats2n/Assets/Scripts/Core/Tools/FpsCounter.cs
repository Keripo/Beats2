/*
	Copyright (c) 2013, Keripo
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
	    * Redistributions of source code must retain the above copyright
	      notice, this list of conditions and the following disclaimer.
	    * Redistributions in binary form must reproduce the above copyright
	      notice, this list of conditions and the following disclaimer in the
	      documentation and/or other materials provided with the distribution.
	    * Neither the name of the <organization> nor the
	      names of its contributors may be used to endorse or promote products
	      derived from this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using UnityEngine;

namespace Beats2.Core {
	
	/// <summary>
	/// Fps counter
	/// </summary>
	public class FpsCounter {
	
		/// <summary>
		/// Number of frames between updates
		/// </summary>
		public float updateInterval;
		
		/// <summary>
		/// Last calculated FPS value
		/// </summary>
		public float fps;
		
		/// <summary>
		/// Check this value to see if the FPS value has been updated; set to false after reading
		/// </summary>
		public bool updated;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public FpsCounter(float updateInterval) {
			this.updateInterval = updateInterval;
			fps = 0.0f;
			Reset();
		}
		
		/// <summary>
		/// Reset internal counters
		/// </summary>
		public void Reset() {
			_updateDiff = updateInterval;
			_timeDiff = 0f;
			_frameDiff = 0;
			updated = true;
		}
		
		/// <summary>
		/// Update internal counters and the Fps value
		/// </summary>
		public void Update() {
			float deltaTime = Time.deltaTime / Time.timeScale; // More accurate than just Time.deltaTime
			_updateDiff -= deltaTime;
			_timeDiff += deltaTime;
			_frameDiff++;
			if (_updateDiff <= 0f) {
				fps = _frameDiff / _timeDiff;
				Reset();
			}
		}
		
		// Private counters
		private float _updateDiff;
		private float _timeDiff;
		private int _frameDiff;
	}
}
