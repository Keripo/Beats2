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
	/// Pattern type, not strict
	/// </summary>
	public enum PatternType {
		/// <summary>
		/// Beatmania IIDX style
		/// </summary>
		BEATS,

		/// <summary>
		/// DJ Max Technika style
		/// </summary>
		TECHNIKA,

		/// <summary>
		/// Jubeats style
		/// </summary>
		SQUARE,

		/// <summary>
		/// Taiko no Tatsujin style
		/// </summary>
		TAIKO,

		/// <summary>
		/// MaiMai style
		/// </summary>
		MAI
	}

	/// <summary>
	/// Pattern info
	/// </summary>
	public class Pattern : IComparable<Pattern> {

		/// <summary>
		/// Link back to parent Info
		/// </summary>
		public Info info;

		/// <summary>
		/// Original pattern type
		/// </summary>
		public PatternType type = PatternType.BEATS;

		/// <summary>
		/// Number of keys (i.e. different note columns) in this pattern
		/// </summary>
		public int keyCount = -1;

		/// <summary>
		/// Difficulty value, user-defined
		/// Please keep this from a scale of 1-12
		/// 1-3 = Beginner
		/// 4-6 = Normal
		/// 7-9 = Hard
		/// 10-12 = Challenge
		/// 13+ = Edit
		/// See http://www.stepmania.com/forums/showthread.php?5817-Can-someone-explain-difficulty&p=66670&viewfull=1#post66670
		/// </summary>
		public int difficulty = -1;

		/// <summary>
		/// Pattern-specific credits, should use parent Info's credits if not specified
		/// </summary>
		public string credits = "";

		/// <summary>
		/// Pattern-specific description, should use parent Info's description if not specified
		/// </summary>
		public string description = "";

		/// <summary>
		/// Total non-event note count
		/// </summary>
		public int noteCount = 0;

		/// <summary>
		/// Starting line index in simfile of pattern data, to be used by parent Parser
		/// </summary>
		public int lineIndex = 0;

		/// <summary>
		/// Whether or not the pattern data has been loaded or not
		/// </summary>
		public bool loaded = false;

		/// <summary>
		/// List of Notes, the simfile's base data
		/// </summary>

		public List<Note> notes = new List<Note>();

		/// <summary>
		/// Comparator for sorting, compares <see cref="Pattern.difficulty"/> values
		/// </summary>
		public int CompareTo(Pattern other) {
			return difficulty.CompareTo(other.difficulty);
		}
	}
}

