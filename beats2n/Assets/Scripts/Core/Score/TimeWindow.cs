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


namespace Beats2.Core {
	
	/// <summary>
	/// Timing windows, etc.
	/// </summary>
	public static class TimeWindow {
		
		#region NoteState timediffs
		
		// <summary>
		/// Timediff threshold before switching to <see cref="NoteState.OFFSCREEN"/> state
		/// </summary>
		public static float TIMEDIFF_OFFSCREEN;
		
		/// <summary>
		/// Timediff threshold before switching to <see cref="NoteState.ASLEEP"/> state
		/// </summary>
		public static float TIMEDIFF_ASLEEP;
		
		/// <summary>
		/// Timediff threshold before switching to <see cref="NoteState.ACTIVE"/> state
		/// </summary>
		public static float TIMEDIFF_ACTIVE;
		
		/// <summary>
		/// Timediff threshold before switching to <see cref="NoteState.MISSED"/> state
		/// </summary>
		public static float TIMEDIFF_MISSED;
		
		/// <summary>
		/// Timediff threshold before switching to <see cref="NoteState.COMPLETED"/> state
		/// </summary>
		public static float TIMEDIFF_COMPLETED;
		
		#endregion
		
		#region Notetype-specific timing windows
		
		/// <summary>
		/// Time window allowed for a <see cref="NoteType.MINE"/> to stay as <see cref="NoteState.FOCUSED"/>
		/// </summary>
		public static float TIMEWINDOW_MINE;
		
		/// <summary>
		/// Maximum time window allowed for a <see cref="NoteType.HOLD"/> to stay as <see cref="NoteState.FOCUSED"/>
		/// </summary>
		public static float TIMEWINDOW_HOLD;
		
		/// <summary>
		/// Maximum time window allowed for a <see cref="NoteType.ROLL"/> to stay as <see cref="NoteState.FOCUSED"/>
		/// </summary>
		public static float TIMEWINDOW_ROLL;
		
		/// <summary>
		/// Maximum time window allowed for a <see cref="NoteType.SLIDE"/> to stay as <see cref="NoteState.FOCUSED"/>
		/// </summary>
		public static float TIMEWINDOW_SLIDE;
		
		#endregion
		
		#region Note distance calculating constants
		
		/// <summary>
		/// 128 BPM is the standard BPM of most dance songs
		/// </summary>
		private static readonly float TARGET_BPM_VALUE = 128f;
		
		/// <summary>
		/// One minute has 60 seconds
		/// </summary>
		private static readonly float SECONDS_IN_MINUTES = 60f;
		
		/// <summary>
		/// At 128 BPM at 1x speed, there should be 8 beats on-screenz in Beats mode
		/// </summary>
		private static readonly float TARGET_BEATS_IN_WINDOW = 8f;
		
		/// <summary>
		/// Constant used for ease of calculating a note's distance from the hitbox
		/// </summary>
		public static readonly float NOTE_DISTANCE_FACTOR = TARGET_BPM_VALUE * SECONDS_IN_MINUTES * TARGET_BEATS_IN_WINDOW;
		
		#endregion
	}
	
}