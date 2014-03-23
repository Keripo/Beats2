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

using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Beats2.System;

namespace Beats2.Audio {

	/// <summary>
	/// Loader class for audio files
	/// </summary>
	public static class AudioLoader {
		private const string TAG = "AudioLoader";

		private static Dictionary<string, AudioClips> _audioMap;
		private static Dictionary<AudioClips, AudioInfo> _audioCache;

		/// <summary>
		/// Init this instance.
		/// </summary>
		public static void Init() {
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		public static void Reset() {
			_audioMap = new Dictionary<string, AudioClips>();
			_audioCache = new Dictionary<AudioClips, AudioInfo>();
			LoadAudioClips();
			Logger.Debug(TAG, "Reset...");
		}

		/// <summary>
		/// Preload all audio from <see cref="AudioClips"/>
		/// </summary>
		private static void LoadAudioClips() {
			// Load default paths first
			foreach (AudioClips audio in Enum.GetValues(typeof(AudioClips))) {
				// Reflection magic!
				MemberInfo memberInfo = typeof(AudioClips).GetMember(audio.ToString()).FirstOrDefault();
				AudioClipAttr audioClipAttr = (AudioClipAttr)Attribute.GetCustomAttribute(memberInfo, typeof(AudioClipAttr));

				AudioInfo audioInfo = new AudioInfo();
				audioInfo.path = audioClipAttr.path;
				audioInfo.loop = audioClipAttr.loop;
				audioInfo.stream = false;
				audioInfo.audio = null;

				_audioMap.Add(audioClipAttr.key, audio);
				_audioCache.Add(audio, audioInfo);
			}

			// Override default paths via settings
			// TODO: Implement

			// Load AudioClip objects
			// Note: No file existence check up until this step, aka no fallbacks
			foreach (AudioInfo audioInfo in _audioCache.Values.ToList()) {
				audioInfo.audio = LoadAudioClip(SysPath.GetDataPath(audioInfo.path), audioInfo.stream);
			}
		}

		/// <summary>
		/// Loads a base <see cref="AudioClip"/> from disk
		/// </summary>
		/// <returns>
		/// A base <see cref="AudioClip"/>
		/// </returns>
		/// <param name='path'>
		/// Path to audio file
		/// </param>
		/// <param name='stream'>
		/// Whether or not the <see cref="AudioClip"/> should be fully loaded into memory or streamed from disk
		/// </param>
		private static AudioClip LoadAudioClip(string path, bool stream) {
			string filePath = SysPath.FindFile(path, SysPath.AudioExtensions);
			if (filePath == null) {
				Logger.Error(TAG, String.Format("Unable to find audio file: \"{0}\"", path));
				return null;
			}

			string url = SysPath.GetWwwPath(filePath);
			WWW www = new WWW(url);
			//while (!www.isDone); // FIXME: Is blocking here necessary?

			AudioClip clip;
			clip = www.GetAudioClip(false, stream);
			while(!clip.isReadyToPlay); // Wait for buffer
			www.Dispose(); // FIXME: What does this even do? Documentation is blank...

			if (clip == null) {
				Logger.Error(TAG, String.Format("Unable to load audio file: \"{0}\"", url));
			}
			return clip;
		}

		/// <summary>
		/// Loads an <see cref="AudioInfo"/> ready to be used by a <see cref="IAudioPlayer"/>
		/// </summary>
		/// <returns>
		/// A prepared <see cref="AudioInfo"/>
		/// </returns>
		/// <param name='path'>
		/// Path to audio file
		/// </param>
		/// <param name='loop'>
		/// Whether or not the <see cref="IAudioPlayer"/> should loop the audio
		/// </param>
		/// <param name='stream'>
		/// Whether or not the <see cref="AudioClip"/> should be fully loaded into memory or streamed from disk
		/// </param>
		public static AudioInfo LoadAudio(string path, bool loop, bool stream) {
			AudioInfo audioInfo = new AudioInfo();
			audioInfo.path = path;
			audioInfo.loop = loop;
			audioInfo.stream = stream;
			audioInfo.audio = LoadAudioClip(path, stream);
			return audioInfo;
		}

		/// <summary>
		/// Gets a preloaded <see cref="AudioInfo"/>
		/// </summary>
		/// <returns>
		/// A prepared <see cref="AudioInfo"/> ready to be used by a <see cref="IAudioPlayer"/>
		/// </returns>
		/// <param name='audio'>
		/// <see cref="AudioInfo"/> enum
		/// </param>
		public static AudioInfo GetAudio(AudioClips audio) {
			AudioInfo audioInfo;
			if (!_audioCache.TryGetValue(audio, out audioInfo)) {
				Logger.Error(TAG, String.Format("Unable to fetch audio clip \"{0}\"", audio));
			}
			return audioInfo;
		}
	}
}
