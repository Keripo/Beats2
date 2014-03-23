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
using System.Collections;
using Beats2.Core;

namespace Beats2.Common {
	
	/// <summary>
	/// FPS counter UI object
	/// </summary>
	public class FpsCounterScript : UIElement {
	
		// Private variables
		private FpsCounter _counter;
		private UILabel _label;
		
		// Settings
		private float FPS_RED = 30f;
		private float FPS_YELLOW = 45f;
		
		/// <summary>
		/// Static initializer
		/// </summary>
		public static FpsCounterScript Init(UIFont font) {
			GameObject gameObj = new GameObject();
			FpsCounterScript script = gameObj.AddComponent<FpsCounterScript>();
			script.Setup(font);
			return script;
		}
		
		/// <summary>
		/// Setup the script
		/// </summary>
		public void Setup(UIFont font) {
			// Setup base properties
			GameObject parent = Display.topRight;
			string name = "FpsCounter";
			Vector3 position = new Vector3(-15f, -15f, 0f);
			base.Setup(parent, name, position);
			
			// Add label
			_label = this.gameObject.AddComponent<UILabel>();
			_label.font = font;
			_label.text = "XX.X FPS";
			_label.pivot = UIWidget.Pivot.TopRight;
			_label.effectStyle = UILabel.Effect.Outline;
			_label.MakePixelPerfect();
			
			// Setup counter
			_counter = new FpsCounter(0.5f);
			_counter.Reset();
		}
		
		// TODO: Remove me, for Sandbox only
		public override void Start() {
			_counter = new FpsCounter(0.5f);
			_counter.Reset();
			_label = this.gameObject.GetComponent<UILabel>();
		}
		
		// Called upon each frame update
		public override void Update() {
			
			// Update counter
			_counter.Update();
			if (_counter.updated && _counter.fps != 0.0f) {
				float fps = _counter.fps;
				_counter.updated = false;
				_label.text = String.Format("{00:f1} FPS", fps);
				if (fps < FPS_RED) {
					_label.color = Color.red;
				} else if (fps < FPS_YELLOW) {
					_label.color = Color.yellow;
				} else {
					_label.color = Color.white;
				}
			}
		}
	}
}
