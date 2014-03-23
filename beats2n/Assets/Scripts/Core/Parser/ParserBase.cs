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
	
	public class ParserBase {
		
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
			if (!Loader.FileExists(path)) {
				throw new ParserException(TAG, "Unable to find simfile: " + path);
			}
			_info.path = path;

			string parentFolder = Loader.GetParentPath(path);
			if (parentFolder == null) {
				throw new ParserException(TAG, "Unable to determine parent folder for path: " + path);
			}
			_info.folder = parentFolder;
		}
		
	}
}