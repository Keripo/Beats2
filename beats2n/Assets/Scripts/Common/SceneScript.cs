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
using System.Collections.Generic;
using Beats2.Core;

namespace Beats2.Common {
	
	/// <summary>
	/// UI anchors
	/// </summary>
	public static class Display {
		// Root
		public static UIRoot root;
		public static int width {
			get { return root.activeHeight * Screen.width / Screen.height; }
			set {} // Readonly
		}
		public static int height {
			get { return root.activeHeight; }
			set {} // Readonly
		}
		
		// Anchors
		public static GameObject topLeft, top, topRight;
		public static GameObject left, centre, right;
		public static GameObject bottomLeft, bottom, bottomRight;
		
		// Retrieve references from current scene
		public static void SetupDisplay() {
			root = GameObject.Find("Root").GetComponent<UIRoot>();
			topLeft = GameObject.Find("TopLeft");
			top = GameObject.Find("Top");
			topRight = GameObject.Find("TopRight");
			left = GameObject.Find("Left");
			centre = GameObject.Find("Centre");
			right = GameObject.Find("Right");
			bottomLeft = GameObject.Find("BottomLeft");
			bottom = GameObject.Find("Bottom");
			bottomRight = GameObject.Find("BottomRight");
		}
	}
	
	/// <summary>
	/// Base script for setting up the UI
	/// </summary>
	public class SceneScript : MonoBehaviour {
		
		// Added objects
		public BackgroundScript background;
		public FpsCounterScript fpsCounter;
		
		// Temp
		public Font testFont;
		
		/// <summary>
		/// Called on initialization
		/// </summary>
		public virtual void Awake() {
			Display.SetupDisplay();
		}
		
		/// <summary>
		/// Called on during creation
		/// </summary>
		public virtual void Start() {
			// Add a background image
			Texture2D testBackground = Loader.LoadTexture(@"Z:\SkyDrive\Development\Workspace\trunk\beats2n\Assets\Resources\Common\Game\Background.jpg", false);
			background = BackgroundScript.Init(Display.centre, testBackground, Display.height);
			
			// Add an FPS counter
			UIFont uiFont = UITools.CreateFont(testFont, 30);
			fpsCounter = FpsCounterScript.Init(uiFont);
		}		
		/// <summary>
		/// Called on every frame
		/// </summary>
		public virtual void Update() {
			
		}
		
	}
	
}