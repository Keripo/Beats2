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

namespace Beats2.Audio {

	/// <summary>
	/// Audio player interface, to be implemented by a GameObject
	/// </summary>
	public interface IAudioPlayer {

		/// <summary>
		/// Load the specified AudioInfo
		/// </summary>
		void Load(AudioInfo audioInfo);

		/// <summary>
		/// Load a variable audio file
		/// </summary>
		void Load(string path);

		/// <summary>
		/// Start or resume audio playback
		/// </summary>
		void Play();

		/// <summary>
		/// Pause audio playback
		/// </summary>
		void Pause();

		/// <summary>
		/// Stop audio playback, time will be reset
		/// </summary>
		void Stop();

		/// <summary>
		/// Determines whether the audio is playing
		/// </summary>
		bool IsPlaying();

		/// <summary>
		/// Gets the current audio time in seconds
		/// </summary>
		float GetTime();

		/// <summary>
		/// Gets the player's volume
		/// </summary>
		float GetVolume();

		/// <summary>
		/// Sets the player's volume
		/// </summary>
		void SetVolume(float volume);
	}
}

