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
using System;
using System.Collections.Generic;
using Beats2.Core;
using Beats2.Common;

namespace Beats2.Beats {
	
	/// <summary>
	/// Beats tap note
	/// </summary>
	public class TapNoteScript : NoteScript {
		
		#region Static variables and functions
		
		/// <summary>
		/// Starting coordinates of note, should be off-screen
		/// </summary>
		public static Vector3[] COORD_START;
		
		/// <summary>
		/// Ending coordinates of note, should match hitbox coords
		/// </summary>
		public static Vector3[] COORD_END;
		
		/// <summary>
		/// Static startCoords - endCoords so we don't have to recalculate
		/// </summary>
		public static Vector3[] COORD_DIFFS;
		
		#endregion
		
		#region Script variables and functions
		
		/// <summary>
		/// Called on during creation
		/// </summary>
		public override void Start() {
			base.Start();
		}
		
		/// <summary>
		/// Called on every frame
		/// </summary>
		public override void Update() {
			base.Update();
			UpdateCoords();
			UpdateGraphics();
		}
		
		/// <summary>
		/// Updates the game object's coords
		/// </summary>
		private void UpdateCoords() {
			// Summarize the following logic to avoid roundoff error
			// float timePerBeat = SECONDS_IN_MINUTES / game.currentBpm;
			// float beatsAway = timeDiff / timePerBeat;
			// float beatsFactor = beatsAway / TARGET_BEATS_IN_WINDOW;
			// float bpmFactor = game.currentBpm / TARGET_BPM_VALUE;
			// float speedFactor = speedMultiplier;
			// float distanceMultiplier = beatsFactor * bpmFactor * speedFactor;
			// float distanceDiff = (COORD_START[note.column] - COORD_END[note.column]) * distanceMultiplier;
			float distanceMultiplier = timeDiff * game.currentBpm * game.currentBpm / TimeWindow.NOTE_DISTANCE_FACTOR;
			Vector3 distanceDiff = COORD_DIFFS[note.column] * distanceMultiplier;
			this.gameObject.transform.localPosition = COORD_END[note.column] + distanceDiff;
		}
		
		/// <summary>
		/// Updates the UI sprite
		/// </summary>
		private void UpdateGraphics() {
			// TODO
		}
		#endregion
	}
	
}