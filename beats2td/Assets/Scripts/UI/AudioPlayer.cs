using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;
using Beats2.Audio;

namespace Beats2.UI {
	
	public class AudioPlayer : MonoBehaviour {

		private AudioSource _audioSrc;

		public static AudioPlayer Instantiate() {
			// Create GameObject
			GameObject obj = new GameObject();
			obj.name = "_AudioPlayer";
			obj.tag = Tags.MENU_MUSIC_PLAYER;

			// Add AudioPlayer Component
			AudioPlayer audioPlayer = obj.AddComponent<AudioPlayer>();

			// Add AudioSource Component
			AudioSource audioSrc = obj.AddComponent<AudioSource>();
			audioPlayer._audioSrc = audioSrc;

			return audioPlayer;
		}

		public void Set(AudioClips audio) {
			_audioSrc.clip = AudioLoader.GetAudio(audio).audio;
		}

		public void Set(AudioClip clip) {
			_audioSrc.clip = clip;
		}

		public void Play() {
			_audioSrc.Play();
		}

		public void Pause() {
			_audioSrc.Pause();
		}

		public void Stop() {
			_audioSrc.Stop();
		}

		public bool isPlaying {
			get { return _audioSrc.isPlaying; }
		}

		public float volume {
			get { return _audioSrc.volume; }
			set { _audioSrc.volume = value; }
		}

		public bool loop {
			get { return _audioSrc.loop; }
			set { _audioSrc.loop = value; }
		}

		public float time {
			get {
				// AudioSource.time is inaccurate for compressed audio data
				//return audioSrc.time;
				return (float)_audioSrc.timeSamples / (float)_audioSrc.clip.frequency;
			}
			set { _audioSrc.time = value; }
		}

		public void Destroy() {
			UnityEngine.Object.Destroy(gameObject);
		}

	}
}
