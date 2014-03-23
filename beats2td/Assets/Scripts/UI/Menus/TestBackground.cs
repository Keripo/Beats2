using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;
using Beats2.Graphic;

namespace Beats2.UI {
	
	public class TestBackground : BeatsObject<SpriteFlat> {
		private const string NAME = "_TestBackground";

		private SpriteFlatData _data;

		public static TestBackground Instantiate() {
			// Create GameObject
			GameObject obj = new GameObject();
			obj.name = NAME;
			obj.tag = Tags.UNTAGGED;

			// Add TestBackground Component
			TestBackground beatsObj = obj.AddComponent<TestBackground>();

			// Create SpriteData
			Texture2D texture = SpriteLoader.GetTexture(Sprites.SANDBOX_BACKGROUND);
			if (Screens.width / texture.width > Screens.height / texture.height) {
				beatsObj._data = new SpriteFlatData(NAME, texture, Screens.width, 0f, ScaleType.SCALED_WIDTH);
			} else {
				beatsObj._data = new SpriteFlatData(NAME, texture, 0f, Screens.height, ScaleType.SCALED_HEIGHT);
			}

			// Add Sprite Component
			SpriteFlat sprite = obj.AddComponent<SpriteFlat>();
			sprite.Setup(beatsObj._data);
			beatsObj._sprite = sprite;

			// Return instantiated BeatsObject
			return beatsObj;
		}

		public override void Destroy() {
			base.Destroy();
			_data.Destroy();
		}
	}
}
