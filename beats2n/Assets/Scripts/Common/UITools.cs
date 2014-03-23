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
	/// NGUI helper functions
	/// </summary>
	public static class UITools {
	
		private const string TAG = "UITools"; 
		
		/// <summary>
		/// Adds an anchor to an object and set it's initial local position
		/// </summary>
		public static UIAnchor AddAnchor(GameObject gameObj, UIAnchor.Side side) {
			UIAnchor anchor = gameObj.AddComponent<UIAnchor>();
			anchor.side = side;
			return anchor;
		}
		
		/// <summary>
		/// Load a list of image files as non-repeating Texture2Ds
		/// </summary>
		public static List<Texture2D> LoadTextures(List<string> paths) {
			Logger.Debug(TAG, "Loading {0} image files", paths.Count);
			List<Texture2D> textures = new List<Texture2D>(paths.Count);
			foreach (string path in paths) {
				Texture2D texture = Loader.LoadTexture(path, false);
				textures.Add(texture);
			}
			return textures;
		}
		
		/// <summary>
		/// Create a UIAtlas on runtime from a list of Texture2Ds
		/// </summary>
		public static UIAtlas CreateAtlas(string atlasName, GameObject parent, List<Texture2D> textures, List<string> names) {
			Logger.Debug(TAG, "Generating UIAtlas: {0}", atlasName);
			
			// Pack textures
			int maxSize = SystemInfo.maxTextureSize;
			Texture2D atlasTexture = new Texture2D(maxSize, maxSize);
			Rect[] rects = atlasTexture.PackTextures(textures.ToArray(), 0, maxSize);
			
			// Create new empty GameObject with UIAtlas component
			UIAtlas atlas = NGUITools.AddChild<UIAtlas>(parent);
			atlas.name = atlasName;
			
			// Set material
			atlas.coordinates = UIAtlas.Coordinates.TexCoords;
			atlas.spriteMaterial = new Material(Shader.Find("Unlit/Transparent Colored"));
			atlas.spriteMaterial.mainTexture = atlasTexture;
			
			// Add sprites
			for (int i = 0; i < rects.Length; i++) {
				UIAtlas.Sprite sprite = new UIAtlas.Sprite();
				sprite.inner = rects[i];
				sprite.outer = rects[i];
				sprite.name = names[i];
				atlas.spriteList.Add(sprite);
			}
			
			// Return reference to the UIAtlas script
			return atlas;
		}
		
		public static UIFont CreateFont(Font font, int size) {
			GameObject gameObj = new GameObject();
			UIFont uiFont = gameObj.AddComponent<UIFont>();
			uiFont.dynamicFont = font;
			uiFont.dynamicFontSize = size;
			return uiFont;
		}
		
	}
	
}