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

namespace Beats2.System {

	/// <summary>
	/// Logger static class, prints to the Unity log
	/// </summary>
	public static class Logger {
		private const string TAG = "Logger";

		/// <summary>
		/// Whether or not debug messages are printed
		/// </summary>
		public static bool debug = true;

		/// <summary>
		/// Latest printed log message
		/// </summary>
		public static string msg;

		/// <summary>
		/// Init this instance.
		/// </summary>
		public static void Init() {
			msg = "";
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		public static void Reset() {
			Logger.Debug(TAG, "Reset...");
		}

		/// <summary>
		/// Print to Unity debug log if <see cref="debug"/> variable set
		/// </summary>
		public static void Debug(string tag, object obj) {
			if (debug) {
				msg = String.Format("D: {0}: {1}", tag, obj);
				UnityEngine.Debug.Log(msg);
			}
		}

		/// <summary>
		/// Print to Unity output log
		/// </summary>
		public static void Log(string tag, object obj) {
			msg = String.Format("L: {0}: {1}", tag, obj);
			UnityEngine.Debug.Log(msg);
		}

		/// <summary>
		/// Print to Unity warning log
		/// </summary>
		public static void Warning(string tag, object obj) {
			msg = String.Format("W: {0}: {1}", tag, obj);
			UnityEngine.Debug.LogWarning(msg);
		}

		/// <summary>
		/// Print to Unity error log, make sure an appropriate exception is thrown by the parent code if necessary
		/// </summary>
		public static void Error(string tag, object obj) {
			msg = String.Format("E: {0}: {1}", tag, obj);
			UnityEngine.Debug.LogError(msg);
		}

		/// <summary>
		/// Log an exception, called by <see cref="BeatsException"/>
		/// </summary>
		public static void Exception(string tag, object obj, Exception e) {
			msg = String.Format("X: {0}: {1}", tag, obj);
			UnityEngine.Debug.LogError(msg);
			UnityEngine.Debug.LogException(e);
		}
	}
}
