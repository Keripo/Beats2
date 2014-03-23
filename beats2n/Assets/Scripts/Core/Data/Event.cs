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

namespace Beats2.Core {

	/// <summary>
	/// Event type, for categorizing event behaviour
	/// </summary>
	public enum EventType {
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
		/// Either unknown EventType or no event
		/// </summary>
		UNKNOWN = -99
	}

	/// <summary>
	/// Event state, used to determine gameplay behaviour
	/// </summary>
	public enum EventState {
		
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
		/// Hit and done, switches to INACTIVE
		/// </summary>
		DONE,
		
		/// <summary>
		/// Off-screen, no need to draw or update
		/// </summary>
		COMPLETED
	}

	/// <summary>
	/// Event data, the core pieces of a <see cref="Pattern"/>
	/// </summary>
	public class Event : IComparable<Event> {
		
		/// <summary>
		/// Type of event. Defaults as <see cref="EventType.LABEL"/>
		/// </summary>
		public EventType type = EventType.LABEL;

		/// <summary>
		/// State of event. Starts as <see cref="EventState.OFFSCREEN"/>
		/// </summary>
		public EventState state = EventState.OFFSCREEN;

		/// <summary>
		/// Event time
		/// </summary>
		public float time;

		/// <summary>
		/// Event string value, parse this if needed
		/// </summary>
		public string eventValue = "";
		
		/// <summary>
		/// Comparator for sorting, compares <see cref="Event.time"/> values
		/// </summary>
		public int CompareTo(Event other) {
			return time.CompareTo(other.time);
		}
	}
}
