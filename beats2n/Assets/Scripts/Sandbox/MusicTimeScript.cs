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

using Beats2.Core;
using System;
using UnityEngine;


public class MusicTimeScript : MonoBehaviour {
	
	public float MusicTime;
	
	// Use this for initialization
	void Start () {
		_label = this.gameObject.GetComponent<UILabel>();
		_musicPlayer = new MusicPlayer(this.gameObject.audio);
		_musicPlayer.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(_musicPlayer.isPlaying) {
			MusicTime = _musicPlayer.time;
			float timeDiff = Time.time - MusicTime;
			_label.text = String.Format("Time: {0:f2}", MusicTime);
			Logger.Log("MUSIC", "Time: {0}\t{1}", MusicTime, timeDiff);
		}
	}
	
	// Toggle pause play
	public void OnClick() {
		if (_musicPlayer.isPlaying) {
			_musicPlayer.Pause();
		} else {
			_musicPlayer.Play();
		}
	}
	
	private UILabel _label;
	private MusicPlayer _musicPlayer;
}
