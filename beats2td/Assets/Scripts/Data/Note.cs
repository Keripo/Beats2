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

namespace Beats2.Data {

	/// <summary>
	/// Note type, use <see cref="Note.IsNote"/> to check if the type is a note or event
	/// </summary>
	public enum NoteType {

		/// <summary>
		/// Context text event: time[0], eventValue = display text
		/// </summary>
		LABEL = -4,

		/// <summary>
		/// Background image change event: time[0], eventValue = <see cref="Info.backgrounds"/> index
		/// </summary>
		BG = -3,

		/// <summary>
		/// BPM change event: time[0], eventValue = new BPM value
		/// </summary>
		BPM = -2,

		/// <summary>
		/// Stop event: time[0], eventValue = stop duration in seconds
		/// </summary>
		STOP = -1,

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
	/// Note state, used to reflect what the gameplay graphics should display
	/// </summary>
	public enum NoteState {
		/// <summary>
		/// Not yet in hit window
		/// </summary>
		ASLEEP,

		/// <summary>
		/// In hit window
		/// </summary>
		ACTIVE,

		/// <summary>
		/// Initial contact, switches to FOCUSED or DONE
		/// </summary>
		HIT,

		/// <summary>
		/// Hit and currently focused
		/// </summary>
		FOCUSED,

		/// <summary>
		/// Hit and temporarily unfocused
		/// </summary>
		UNFOCUSED,

		/// <summary>
		/// Hit and done, switches to <see cref="INACTIVE"/>
		/// </summary>
		DONE,

		/// <summary>
		/// Missed or unfocused too long, switches to INACTIVE
		/// </summary>
		MISSED,
		
		/// <summary>
		/// Past hit window
		/// </summary>
		INACTIVE
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
		/// Constructor for events, note that <see cref="NotePoint.column"/> is set to -1
		/// </summary>
		public NotePoint(float time) {
			this.time = time;
			this.column = -1;
		}

		/// <summary>
		/// Comparator for sorting, compares <see cref="Note.times[0]"/> values
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
		/// State of note. Starts as <see cref="NoteState.ASLEEP"/>
		/// </summary>
		public NoteState state = NoteState.ASLEEP;

		/// <summary>
		/// Data points of the note, contains timing and column info
		/// </summary>
		public List<NotePoint> points = new List<NotePoint>();

		/// <summary>
		/// Event string value
		/// </summary>
		public string eventStringVal = "";

		/// <summary>
		/// Event float value
		/// </summary>
		public float eventFloatVal = 0f;

		/// <summary>
		/// Event int value
		/// </summary>
		public int eventIntVal = 0;

		/// <summary>
		/// Event time
		/// </summary>
		public float eventTime = -1f;

		/// <summary>
		/// First note/event time
		/// </summary>
		public float time {
			get {
				if (isNote && points.Count > 0) {
					return points[0].time;
				} else {
					return eventTime;
				}
			}
		}

		/// <summary>
		/// First note column
		/// </summary>
		public int column {
			get {
				if (isNote && points.Count > 0) {
					return points[0].column;
				} else {
					return -1;
				}
			}
		}

		/// <summary>
		/// Whether or not this is a note or an event
		/// </summary>
		public bool isNote {
			get {
				return (int)type >= (int)NoteType.MINE;
			}
		}

		/// <summary>
		/// Add a note point.
		/// </summary>
		public void AddPoint(float time, int column) {
			points.Add(new NotePoint(time, column));
		}

		/// <summary>
		/// Comparator for sorting, compares <see cref="Note.times[0]"/> values
		/// </summary>
		public int CompareTo(Note other) {
			return time.CompareTo(other.time);
		}
	}
}

