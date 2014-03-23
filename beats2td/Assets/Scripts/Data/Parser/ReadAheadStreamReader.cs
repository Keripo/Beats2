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
using System.Collections.Generic;
using Beats2.System;

namespace Beats2.Data {

	/// <summary>
	/// Wrapper of StreamReader reader implementing a PeakLine function
	/// </summary>
	public class ReadAheadStreamReader {
		private StreamReader _reader;
		private Queue<string> _buffer;

		/// <summary>
		/// Initializes an internal StreamReader on given path
		/// </summary>
		public ReadAheadStreamReader(string path) {
			_reader = new StreamReader(path);
			_buffer = new Queue<string>();
		}

		/// <summary>
		/// Returns the next line without advancing the reader
		/// </summary>
		public string PeekLine() {
			string line = _reader.ReadLine();
			if (line == null) {
				return null;
			} else {
				_buffer.Enqueue(line);
				return line;
			}
		}

		/// <summary>
		/// Returns the next line and advances the reader
		/// </summary>
		public string ReadLine() {
			if (_buffer.Count > 0) {
				return _buffer.Dequeue();
			} else {
				return _reader.ReadLine();
			}
		}

		/// <summary>
		/// Reads a line without saving it anywhere and advances the reader.
		/// Note that this will silently fail if you've reached the end of the file, so use carefully
		/// </summary>
		public void SkipLine() {
			_reader.ReadLine();
		}

		/// <summary>
		/// Closes the internal StreamReader
		/// </summary>
		public void Close() {
			_reader.Close();
		}
	}
}