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

using System;
using System.Collections.Generic;

namespace Beats2.Core {

	/// <summary>
	/// Note type, for categorizing note behaviour
	/// </summary>
	public enum NoteType {

		/// <summary>
		/// Mine note: time[0], column[0]
		/// </summary>
		MINE = 0,

		/// <summary>
		/// Tap note: time[0], column[0]
		/// </summary>
		TAP = 1,

		/// <summary>
		/// Hold note: time[0] = start time, time[1] = end time, column[0]
		/// </summary>
		HOLD = 2,

		/// <summary>
		/// Roll note: time[0] = start time, time[1] = end time, column[0]
		/// </summary>
		ROLL = 3,

		/// <summary>
		/// Repeat note: time[X], column[0]
		/// </summary>
		REPEAT = 4,

		/// <summary>
		/// Slide note: time[X], column[X]
		/// </summary>
		SLIDE = 5,

		/// <summary>
		/// Either unknown NoteType or no note
		/// </summary>
		UNKNOWN = -99
	}

	/// <summary>
	/// Note state, used to determine gameplay behaviour
	/// See the comments of <see cref="NoteScript.UpdateNoteState"/> for flow
	/// </summary>
	public enum NoteState {
		
		/// <summary>
		/// Not yet on-screen, no need to draw
		/// </summary>
		OFFSCREEN,
		
		/// <summary>
		/// Not yet in hit window
		/// </summary>
		ASLEEP,

		/// <summary>
		/// In hit window
		/// </summary>
		ACTIVE,

		/// <summary>
		/// Initial contact, switches to FOCUSED or INACTIVE_HITTED
		/// </summary>
		HIT,

		/// <summary>
		/// Hit and currently focused, switches to UNFOCUSED or INACTIVE_HITTED
		/// </summary>
		FOCUSED,

		/// <summary>
		/// Hit and temporarily unfocused, switches to FOCUSED or MISSED
		/// </summary>
		UNFOCUSED,
		
		/// <summary>
		/// Missed or unfocused too long, switches to INACTIVE_MISSED
		/// </summary>
		MISSED,
		
		/// <summary>
		/// Hit and no longer actionable, switches to COMPLETED
		/// </summary>
		INACTIVE_HITTED,
		
		/// <summary>
		/// Missed and no longer actionable, switches to COMPLETED
		/// </summary>
		INACTIVE_MISSED,
		
		/// <summary>
		/// Off-screen, no need to draw or update
		/// </summary>
		COMPLETED
	}

	/// <summary>
	/// Data point for a note, contains timing and column info
	/// </summary>
	public class NotePoint : IComparable<NotePoint> {
		
		/// <summary>
		/// Time into the song that the note should be hit
		/// </summary>
		public float time;

		/// <summary>
		/// Column in which the note should be in
		/// </summary>
		public int column;

		/// <summary>
		/// Constructor for notes
		/// </summary>
		public NotePoint(float time, int column) {
			this.time = time;
			this.column = column;
		}

		/// <summary>
		/// Comparator for sorting, compares <see cref="Note.time"/> values
		/// </summary>
		public int CompareTo(NotePoint other) {
			return time.CompareTo(other.time);
		}

	}

	/// <summary>
	/// Note data, the core pieces of a <see cref="Pattern"/>
	/// </summary>
	public class Note : IComparable<Note> {
		
		/// <summary>
		/// Type of note. Defaults as <see cref="NoteType.TAP"/>
		/// </summary>
		public NoteType type = NoteType.TAP;

		/// <summary>
		/// State of note. Starts as <see cref="NoteState.OFFSCREEN"/>
		/// </summary>
		public NoteState state = NoteState.OFFSCREEN;

		/// <summary>
		/// Data points of the note, contains timing and column info
		/// </summary>
		public List<NotePoint> points = new List<NotePoint>();
		
		/// <summary>
		/// Currently active point
		/// </summary>
		public int pointIndex = 0;
		
		/// <summary>
		/// Next note time determined by the pointIndex
		/// </summary>
		public float time {
			get {
				// Note: No boundary check on purpose to catch bugs
				return points[pointIndex].time;
			}
		}
		
		/// <summary>
		/// Last note time
		/// </summary>
		public float endTime {
			get {
				return points[points.Count - 1].time;
			}
		}

		/// <summary>
		/// First note column
		/// </summary>
		public int column {
			get {
				return points[0].column;
			}
		}

		/// <summary>
		/// Add a note point.
		/// </summary>
		public void AddPoint(float time, int column) {
			points.Add(new NotePoint(time, column));
		}

		/// <summary>
		/// Comparator for sorting, compares <see cref="Note.time"/> values
		/// </summary>
		public int CompareTo(Note other) {
			return time.CompareTo(other.time);
		}
	}
}
