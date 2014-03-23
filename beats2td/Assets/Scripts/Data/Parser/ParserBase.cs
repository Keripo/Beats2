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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Beats2.System;

namespace Beats2.Data {

	/// <summary>
	/// Base parser class, all parsers should extend from this.
	/// </summary>
	/// <exception cref='ParserException'>
	/// Is thrown when a fatal parsing error occurs
	/// </exception>
	/// <exception cref='NotImplementedException'>
	/// Is thrown when the not implemented exception.
	/// </exception>
	public class ParserBase : IParser {
		protected const string TAG = "ParserBase";

		/// <summary>
		/// <see cref="Info"/> file created for loaded simfile
		/// </summary>
		protected Info _info = new Info();

		/// <summary>
		/// Current index of line being parsed in simfile, keep this updated in methods
		/// </summary>
		protected int _index;

		/// <summary>
		/// Current value of line being parsed in simfile, keep this updated in methods
		/// </summary>
		protected string _line;

		/// <summary>
		/// Load a simfile
		/// </summary>
		/// <param name='path'>
		/// Path to simfile
		/// </param>
		/// <exception cref='ParserException'>
		/// Is thrown when the parser is unable to find the simfile
		/// </exception>
		public virtual void Load(string path) {
			if (!SysPath.FileExists(path)) {
				throw new ParserException(TAG, "Unable to find simfile: " + path);
			}
			_info.path = path;

			string parentFolder = SysPath.GetParentFolder(path);
			if (parentFolder == null) {
				throw new ParserException(TAG, "Unable to determine parent folder for path: " + path);
			}
			_info.folder = parentFolder;
		}

		/// <summary>
		/// Return simfile's <see cref="Info"/>
		/// </summary>
		public virtual Info GetInfo() {
			return _info;
		}

		/// <summary>
		/// Returns a list of unprocessed <see cref="Pattern"/>, need to call <see cref="LoadPattern"/>
		/// </summary>
		public virtual List<Pattern> GetPatterns() {
			return _info.patterns;
		}

		/// <summary>
		/// Loads a Pattern's list of <see cref="Note"/>, this needs to be implemented by the inheriter
		/// </summary>
		/// <exception cref='NotImplementedException'>
		/// Is thrown as this method should be implemented by the inheriter
		/// </exception>
		public virtual void LoadPattern(Pattern pattern) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds a Note to a Pattern's notes list, don't forget to call <see cref="SortNotes"/> afterwards
		/// </summary>
		public void AddNote(Pattern pattern, Note note) {
			if (note.column >= pattern.keyCount) {
				this.Error("Trying to add a note with a column greater than the keyCount");
			} else {
				if (note.isNote && note.type != NoteType.MINE) {
					pattern.noteCount++;
				}
				pattern.notes.Add(note);
			}
		}

		/// <summary>
		/// Sorts a list using the faster TimSort
		/// We use TimSort because the list is usually already (mostly) sorted and
		/// TimSort has a best time of O(n) vs the built-in QuickSort's O(n log(n))
		/// Common cases are for inserting events and long notes (Holds, Rolls, etc.)
		/// in their correct locations among the sea of Tap notes
		/// </summary>
		public void SortList<T>(IList<T> list) {
			//list.Sort();
			TimSortExtender.TimSort<T>(list, false);
		}

		/// <summary>
		/// Sorts a Pattern's notes list
		/// </summary>
		public void SortNotes(Pattern pattern) {
			//this.SortList<Note>(pattern.notes);
			this.SortList<Note>(pattern.notes);
		}

		/// <summary>
		/// Parses a key=value pair. Sets outs to null if incorrect formatting
		/// </summary>
		/// <returns>
		/// Whether or not a key-value pair was detected
		/// </returns>
		/// <param name='line'>
		/// Line to parse
		/// </param>
		/// <param name='key'>
		/// Parsed key, null on failure
		/// </param>
		/// <param name='val'>
		/// Parsed value, null on failure
		/// </param>
		public bool ParseKeyValuePair(string line, char separator, out string key, out string val) {
			int indexEquals;
			if ((indexEquals = line.IndexOf(separator)) != -1) {
				key = line.Substring(0, indexEquals);
				val = line.Substring(indexEquals + 1);
				return true;
			} else {
				key = null;
				val = null;
			}
			return false;
		}

		/// <summary>
		/// Parse a value as a float.
		/// </summary>
		/// <returns>
		/// The parsed float, -1f on failure
		/// </returns>
		/// <param name='name'>
		/// Name of key being parsed for
		/// </param>
		/// <param name='val'>
		/// Value string being parsed
		/// </param>
		/// <param name='fatal'>
		/// Whether or not a <see cref="ParserException"/> should be thrown upon parsing error
		/// </param>
		public float ParseFloat(string name, string val, bool fatal) {
			float parsed = -1f;
			if (!float.TryParse(val, out parsed)) {
				string msg = String.Format("Unable to parse {0} float value: {1}", name, val);
				if (fatal) {
					Error(msg);
				} else {
					Warning(msg);
				}
			}
			return parsed;
		}

		/// <summary>
		/// Parse a value as an int.
		/// </summary>
		/// <returns>
		/// The parsed int, -1 on failure
		/// </returns>
		/// <param name='name'>
		/// Name of key being parsed for
		/// </param>
		/// <param name='val'>
		/// Value string being parsed
		/// </param>
		/// <param name='fatal'>
		/// Whether or not a <see cref="ParserException"/> should be thrown upon parsing error
		/// </param>
		public int ParseInt(string name, string val, bool fatal) {
			int parsed = -1;
			if (!int.TryParse(val, out parsed)) {
				string msg = String.Format("Unable to parse {0} int value: {1}", name, val);
				if (fatal) {
					Error(msg);
				} else {
					Warning(msg);
				}
			}
			return parsed;
		}

		/// <summary>
		/// Finds a file. Uses <see cref="SysPath.FindFile"/>
		/// </summary>
		/// <returns>
		/// The path to the file, null if not found
		/// </returns>
		/// <param name='name'>
		/// Name of key being parsed for
		/// </param>
		/// <param name='val'>
		/// Value string being parsed
		/// </param>
		/// <param name='fatal'>
		/// Whether or not a <see cref="ParserException"/> should be thrown upon parsing error
		/// </param>
		/// <param name='type'>
		/// Type of file
		/// </param>
		/// <param name='extensions'>
		/// List of file extensions to check with
		/// </param>
		public string FindFile(string name, string val, bool fatal, string type, string[] extensions) {
			string path = SysPath.FindFile(SysPath.GetPath(_info.folder, val), extensions);
			if (path == null) {
				string msg = String.Format("Unable to find {0} {1} file: {2}", name, type, val);
				if (fatal) {
					Error(msg);
				} else {
					Warning(msg);
				}
			}
			return path;
		}

		/// <summary>
		/// Finds an audio file. Uses <see cref="FindFile"/>
		/// </summary>
		public string FindAudioFile(string name, string val, bool fatal) {
			return FindFile(name, val, fatal, "audio", SysPath.AudioExtensions);
		}

		/// <summary>
		/// Finds an image file. Uses <see cref="FindFile"/>
		/// </summary>
		public string FindImageFile(string name, string val, bool fatal) {
			return FindFile(name, val, fatal, "image", SysPath.ImageExtensions);
		}

		/// <summary>
		/// Finds a lyrics file. Uses <see cref="FindFile"/>
		/// </summary>
		public string FindLyricsFile(string name, string val, bool fatal) {
			return FindFile(name, val, fatal, "lyrics", SysPath.LyricsExtensions);
		}

		/// <summary>
		/// Throws a <see cref="ParserException"/>
		/// </summary>
		public void Error(string msg) {
			throw new ParserException(TAG,
				String.Format("Line {0}: {1}\r\n{2}", _index, _line, msg));
		}

		/// <summary>
		/// Calls <see cref="Logger.Warning"/>
		/// </summary>
		public void Warning(string msg) {
			Logger.Warning(TAG,
				String.Format("Line {0}: {1}\r\n{2}", _index, _line, msg));
		}
	}
}

