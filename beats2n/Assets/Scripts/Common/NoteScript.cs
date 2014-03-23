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

namespace Beats2.Common {
	
	public class NoteScript : MonoBehaviour {
		
		/// <summary>
		/// Parent game script
		/// </summary>
		public GameScript game;
		
		/// <summary>
		/// Note info/state
		/// </summary>
		public Note note;
		
		/// <summary>
		/// Sprite object
		/// </summary>
		public UISprite sprite;
		
		/// <summary>
		/// Whether or not the sprite currently has touch focus
		/// </summary>
		public bool hasFocus;
		
		/// <summary>
		/// Last time the note had focus
		/// </summary>
		public float focusTime;
		
		/// <summary>
		/// Time difference between the current music time and the last time the note had focus
		/// Always positive
		/// </summary>
		public float focusTimeDiff;
		
		/// <summary>
		/// Time difference between the current music time and note's first time
		/// Negative if before target time, positive if after
		/// </summary>
		public float timeDiff;
		
		/// <summary>
		/// Time fetched from the music player
		/// </summary>
		public float musicTime;
		
		/// <summary>
		/// Called on during creation
		/// </summary>
		public virtual void Start() {
			sprite = this.gameObject.GetComponent<UISprite>();
			sprite.MakePixelPerfect();
		}
		
		/// <summary>
		/// Called on every frame
		/// </summary>
		public virtual void Update() {
			// Update variables
			musicTime = game.musicPlayer.time;
			timeDiff = musicTime - note.time;
			focusTimeDiff = musicTime - focusTime;
			
			this.UpdateNoteState();
		}
		
		/// <summary>
		/// Update the note's state - see each note's specific update function
		/// </summary>
		public void UpdateNoteState() {			
			switch (note.state) {
				case NoteState.OFFSCREEN:
					// Wait til on-screen
					if (timeDiff > TimeWindow.TIMEDIFF_OFFSCREEN) {
						note.state = NoteState.ASLEEP;
					}
					break;
				case NoteState.ASLEEP:
					// Wait til active
					if (timeDiff > TimeWindow.TIMEDIFF_ASLEEP) {
						note.state = NoteState.ACTIVE;
					}
					break;
				case NoteState.INACTIVE_HITTED:
				case NoteState.INACTIVE_MISSED:
					// Wait til off-screen
					if (timeDiff > TimeWindow.TIMEDIFF_COMPLETED) {
						note.state = NoteState.COMPLETED;
					}
					break;
				case NoteState.COMPLETED:
					// Do nothing! Final state
					break;
				default:
					// For all the other states, update based on note type
					switch (note.type) {
						case NoteType.MINE:		this.UpdateNoteStateMine();		break;
						case NoteType.TAP:		this.UpdateNoteStateTap();		break;
						case NoteType.HOLD:		this.UpdateNoteStateHold();		break;
						case NoteType.ROLL:		this.UpdateNoteStateRoll();		break;
						case NoteType.REPEAT:	this.UpdateNoteStateRepeat();	break;
						case NoteType.SLIDE:	this.UpdateNoteStateSlide();	break;
					}
					break;
			}
		}
		
		/// <summary>
		/// Update mine note state
		/// 	HIT -> FOCUSED -> INACTIVE_HITTED
		/// 	MISSED -> INACTIVE_MISSED
		/// </summary>
		private void UpdateNoteStateMine() {
			switch (note.state) {
				case NoteState.ACTIVE:
					if (hasFocus) {
						focusTime = musicTime;
						note.state = NoteState.HIT;
					} else if (timeDiff > TimeWindow.TIMEDIFF_MISSED) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.HIT:
					// TODO: trigger mine score event
					note.state = NoteState.FOCUSED;
					break;
				case NoteState.FOCUSED:
					if (focusTimeDiff > TimeWindow.TIMEWINDOW_MINE) {
						note.state = NoteState.INACTIVE_HITTED;
					}
					break;
				case NoteState.MISSED:
					note.state = NoteState.INACTIVE_MISSED;
					break;
			}
		}
		
		/// <summary>
		/// Update tap note state
		/// 	HIT -> INACTIVE_HITTED
		/// 	MISSED -> INACTIVE_MISSED
		/// </summary>
		private void UpdateNoteStateTap() {
			switch (note.state) {
				case NoteState.ACTIVE:
					if (hasFocus) {
						note.state = NoteState.HIT;
					} else if (timeDiff > TimeWindow.TIMEDIFF_MISSED) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.HIT:
					// TODO: trigger tap hit score event
					note.state = NoteState.INACTIVE_HITTED;
					break;
				case NoteState.MISSED:
					// TODO: trigger tap miss score event
					note.state = NoteState.INACTIVE_MISSED;
					break;
			}
		}
		
		/// <summary>
		/// Update hold note state
		/// 	HIT -> FOCUSED -> FOCUSED -> FOCUSED -> INACTIVE_HITTED
		/// 	HIT -> FOCUSED -> FOCUSED -> UNFOCUSED -> INACTIVE_HITTED
		/// 	HIT -> FOCUSED -> UNFOCUSED -> UNFOCUSED -> MISSED -> INACTIVE_MISSED
		/// 	MISSED -> INACTIVE_MISSED
		/// </summary>
		private void UpdateNoteStateHold() {
			switch (note.state) {
				case NoteState.ACTIVE:
					if (hasFocus) {
						note.state = NoteState.HIT;
					} else if (timeDiff > TimeWindow.TIMEDIFF_MISSED) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.HIT:
					// TODO: trigger hold hit score event
					focusTime = musicTime;
					note.state = NoteState.FOCUSED;
					break;
				case NoteState.FOCUSED:
					if (musicTime > note.endTime) {
						// TODO: trigger hold finished score event
						note.state = NoteState.INACTIVE_HITTED;
					} else if (hasFocus) {
						focusTime = musicTime;
					} else { //if (!hasFocus){
						note.state = NoteState.UNFOCUSED;
					}
					break;
				case NoteState.UNFOCUSED:
					if (musicTime > note.endTime) {
						// TODO: trigger hold finished score event
						note.state = NoteState.INACTIVE_HITTED;
					} else if (hasFocus) {
						focusTime = musicTime;
						note.state = NoteState.FOCUSED;
					} else if (focusTimeDiff > TimeWindow.TIMEWINDOW_HOLD) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.MISSED:
					// TODO: trigger hold miss score event
					note.state = NoteState.INACTIVE_MISSED;
					break;
			}
		}
		
		/// <summary>
		/// Update roll note state
		/// 	HIT -> FOCUSED -> UNFOCUSED -> FOCUSED -> UNFOCUSED -> INACTIVE_HITTED
		/// 	HIT -> FOCUSED -> FOCUSED -> MISSED -> INACTIVE -> INACTIVE_MISSED
		/// 	HIT -> FOCUSED -> UNFOCUSED -> UNFOCUSED -> MISSED -> INACTIVE_MISSED
		/// 	MISSED -> INACTIVE_MISSED
		/// </summary>
		private void UpdateNoteStateRoll() {
			switch (note.state) {
				case NoteState.ACTIVE:
					if (hasFocus) {
						note.state = NoteState.HIT;
					} else if (timeDiff > TimeWindow.TIMEDIFF_MISSED) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.HIT:
					// TODO: trigger roll hit score event
					focusTime = musicTime;
					note.state = NoteState.FOCUSED;
					break;
				case NoteState.FOCUSED:
					if (musicTime > note.endTime) {
						// TODO: trigger roll finished score event
						note.state = NoteState.INACTIVE_HITTED;
					} else if (hasFocus && focusTimeDiff > TimeWindow.TIMEWINDOW_ROLL) {
						note.state = NoteState.MISSED;
					} else if (!hasFocus) {
						focusTime = musicTime;
						note.state = NoteState.UNFOCUSED;
					}
					break;
				case NoteState.UNFOCUSED:
					if (musicTime > note.endTime) {
						// TODO: trigger roll finished score event
						note.state = NoteState.INACTIVE_HITTED;
					} else if (hasFocus) {
						focusTime = musicTime;
						note.state = NoteState.FOCUSED;
					} else if (focusTimeDiff > TimeWindow.TIMEWINDOW_HOLD) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.MISSED:
					// TODO: trigger roll miss score event
					note.state = NoteState.INACTIVE_MISSED;
					break;
			}
		}
		
		/// <summary>
		/// Update repeat note state
		/// Independent points, final state dependent on last hit
		/// 	HIT -> HIT -> HIT -> HIT -> INACTIVE_HITTED
		/// 	HIT -> HIT -> HIT -> MISSED -> INACTIVE_MISSED
		/// 	HIT -> MISSED -> MISSED -> HIT -> INACTIVE_HITTED
		/// 	MISSED -> MISSED -> HIT -> HIT -> INACTIVE_HITTED
		/// 	MISSED -> MISSED -> MISSED -> MISSED -> INACTIVE_MISSED
		/// </summary>
		private void UpdateNoteStateRepeat() {
			switch (note.state) {
				case NoteState.ACTIVE:
					if (hasFocus) {
						note.state = NoteState.HIT;
					} else if (timeDiff > TimeWindow.TIMEDIFF_MISSED) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.HIT:
					// TODO: trigger repeat hit score event
					if (note.pointIndex >= note.points.Count - 1) {
						note.state = NoteState.INACTIVE_HITTED;
					} else {
						note.pointIndex++;
						note.state = NoteState.ACTIVE;
					}
					break;
				case NoteState.MISSED:
					// TODO: trigger roll miss score event
					if (note.pointIndex >= note.points.Count - 1) {
						note.state = NoteState.INACTIVE_MISSED;
					} else {
						note.pointIndex++;
						note.state = NoteState.ACTIVE;
					}
					break;
			}
		}
		
		/// <summary>
		/// Update slide note state
		/// All points need to be hit
		/// 	HIT -> HIT -> HIT -> INACTIVE_HITTED
		/// 	HIT -> HIT -> MISSED -> INACTIVE_MISSED
		/// 	HIT -> MISSED -> INACTIVE_MISSED
		/// 	MISSED -> INACTIVE_MISSED
		/// </summary>
		private void UpdateNoteStateSlide() {
			switch (note.state) {
				case NoteState.ACTIVE:
					if (hasFocus) {
						note.state = NoteState.HIT;
					} else if (timeDiff > TimeWindow.TIMEDIFF_MISSED) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.HIT:
					// TODO: trigger slide hit score event
					focusTime = musicTime;
					if (note.pointIndex >= note.points.Count - 1) {
						note.state = NoteState.INACTIVE_HITTED;
					} else {
						note.pointIndex++;
						note.state = NoteState.FOCUSED;
					}
					break;
				case NoteState.FOCUSED:
					if (musicTime > note.time) {
						// TODO: trigger slide hit score event
						if (note.pointIndex >= note.points.Count - 1) {
							note.state = NoteState.INACTIVE_HITTED;
						} else {
							note.pointIndex++;
						}
					}
					if (hasFocus) {
						focusTime = musicTime;
					} else { //if (!hasFocus){
						note.state = NoteState.UNFOCUSED;
					}
					break;
				case NoteState.UNFOCUSED:
					if (musicTime > note.time) {
						// TODO: trigger slide hit score event
						if (note.pointIndex >= note.points.Count - 1) {
							note.state = NoteState.INACTIVE_HITTED;
						} else {
							note.pointIndex++;
						}
					}
					if (hasFocus) {
						focusTime = musicTime;
						note.state = NoteState.FOCUSED;
					} else if (focusTimeDiff > TimeWindow.TIMEWINDOW_HOLD) {
						note.state = NoteState.MISSED;
					}
					break;
				case NoteState.MISSED:
					// TODO: trigger slide miss score event
					note.state = NoteState.INACTIVE_MISSED;
					break;
			}
		}
		
	}
	
}