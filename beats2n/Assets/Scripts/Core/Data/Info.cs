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

using System.Collections.Generic;

namespace Beats2.Core {

	/// <summary>
	/// Simfile info
	/// </summary>
	public class Info {

		/// <summary>
		/// Simfile path
		/// </summary>
		public string path;

		/// <summary>
		/// Simfile parent folder
		/// </summary>
		public string folder;

		/// <summary>
		/// Simfile song file path
		/// </summary>
		public string song;

		/// <summary>
		/// Song title
		/// </summary>
		public string title;

		/// <summary>
		/// Song title transliteration
		/// </summary>
		public string titleTranslit;

		/// <summary>
		/// Song subtitle
		/// </summary>
		public string subtitle;

		/// <summary>
		/// Song subtitle transliteration
		/// </summary>
		public string subtitleTranslit;

		/// <summary>
		/// Song artist
		/// </summary>
		public string artist;

		/// <summary>
		/// Song artist transliteration
		/// </summary>
		public string artistTranslit;

		/// <summary>
		/// Song album name
		/// </summary>
		public string album;

		/// <summary>
		/// Song album transliteration
		/// </summary>
		public string albumTranslit;

		/// <summary>
		/// Song genre
		/// </summary>
		public string genre;

		/// <summary>
		/// User-defined list of tags
		/// </summary>
		public List<string> tags = new List<string>();

		/// <summary>
		/// Song select sample playback start time
		/// </summary>
		public float sampleStart;

		/// <summary>
		/// Song select sample playback length
		/// </summary>
		public float sampleLength;

		/// <summary>
		/// Song select path to cover art imge
		/// </summary>
		public string cover;

		/// <summary>
		/// Song select path to banner image
		/// </summary>
		public string banner;

		/// <summary>
		/// Gameplay background images
		/// </summary>
		public List<string> backgrounds = new List<string>();

		/// <summary>
		/// Gameplay lyrics
		/// </summary>
		public string lyrics;

		/// <summary>
		/// List of Patterns
		/// </summary>
		public List<Pattern> patterns = new List<Pattern>();
	}
}
