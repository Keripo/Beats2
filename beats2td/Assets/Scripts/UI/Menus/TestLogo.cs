using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;
using Beats2.Graphic;

namespace Beats2.UI {
	
	public class TestLogo : BeatsObject<SpriteFlat> {
		private const string NAME = "_TestLogo";

		private static SpriteFlatData _data;

		public static void Init() {
			// Create SpriteData
			float width = Screens.width / 2;
			Texture2D texture = SpriteLoader.GetTexture(Sprites.SANDBOX_LOGO);
			_data = new SpriteFlatData(NAME, texture, width);
		}

		public static TestLogo Instantiate() {
			// Create GameObject
			GameObject obj = new GameObject();
			obj.name = NAME;
			obj.tag = Tags.MENU_LOGO;

			// Add Logo Component
			TestLogo beatsObj = obj.AddComponent<TestLogo>();

			// Add Sprite Component
			SpriteFlat sprite = obj.AddComponent<SpriteFlat>();
			sprite.Setup(_data);
			beatsObj._sprite = sprite;

			// Add BoxCollider
			BoxCollider collider = obj.AddComponent<BoxCollider>();
			collider.size = sprite.dimensions;
			beatsObj._collider = collider;

			// Return instantiated BeatsObject
			return beatsObj;
		}

		public static void Cleanup() {
			_data.Destroy();
		}
	}
}
