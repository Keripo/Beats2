using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;
using Beats2.Graphic;

namespace Beats2.UI {
	
	public class TestText : BeatsObject<FontMesh> {
		private const string NAME = "_TestText";

		public static TestText Instantiate(FontMeshData data, string text, float fontWidth, float fontHeight, TextAnchor anchor) {
			// Create GameObject
			GameObject obj = new GameObject();
			obj.name = text;
			obj.tag = Tags.UNTAGGED;

			// Add TestText Component
			TestText beatsObj = obj.AddComponent<TestText>();

			// Add Sprite Component
			FontMesh sprite = obj.AddComponent<FontMesh>();
			sprite.Setup(data, fontWidth, fontHeight, anchor);
			sprite.text = text;
			beatsObj._sprite = sprite;

			// Return instantiated BeatsObject
			return beatsObj;
		}

		public string text {
			get { return _sprite.text; }
			set { _sprite.text = value; }
		}
	}
}

