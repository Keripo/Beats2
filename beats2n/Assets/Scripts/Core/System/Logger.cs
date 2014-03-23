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
	/// Logger static class, prints to the Unity log
	/// </summary>
	public static class Logger {

		/// <summary>
		/// Whether or not debug messages are printed
		/// </summary>
		public static bool PrintDebug = true;

		/// <summary>
		/// Print to Unity debug log if <see cref="PrintDebug"/> variable set
		/// </summary>
		public static void Debug(string tag, string format, params object[] args) {
			if (PrintDebug) {
				string msg = FormatLogString("D", tag, format, args);
				UnityEngine.Debug.Log(msg);
			}
		}

		/// <summary>
		/// Print to Unity output log
		/// </summary>
		public static void Log(string tag, string format, params object[] args) {
			string msg = FormatLogString("L", tag, format, args);
			UnityEngine.Debug.Log(msg);
		}

		/// <summary>
		/// Print to Unity warning log
		/// </summary>
		public static void Warning(string tag, string format, params object[] args) {
			string msg = FormatLogString("W", tag, format, args);
			UnityEngine.Debug.LogWarning(msg);
		}

		/// <summary>
		/// Print to Unity error log, make sure an appropriate exception is thrown by the parent code if necessary
		/// </summary>
		public static void Error(string tag, string format, params object[] args) {
			string msg = FormatLogString("E", tag, format, args);
			UnityEngine.Debug.LogError(msg);
		}

		/// <summary>
		/// Print to Unity error log and also logs the exception
		/// </summary>
		public static void Exception(string tag, Exception e, string format, params object[] args) {
			string msg = FormatLogString("X", tag, format, args);
			UnityEngine.Debug.LogError(msg);
			UnityEngine.Debug.LogException(e);
		}
		
		/// <summary>
		/// Formats a log string for printing
		/// </summary>
		private static string FormatLogString(string type, string tag, string format, params object[] args) {
			return String.Format("{0}|{1}: {2}", type, tag, String.Format(format, args));
		}
	}
}
