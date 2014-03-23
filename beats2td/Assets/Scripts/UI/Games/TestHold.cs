using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;
using Beats2.Graphic;

namespace Beats2.UI {
	
	public class TestHold : BeatsObject<SpriteFlat> {
		private const string NAME = "_TestHold";

		private static Texture2D _texture;
		private static float _width;
		private SpriteFlatData _data;

		public static void Init() {
			// Create SpriteData
			_width = Screens.min * 0.10f;
			_texture = SpriteLoader.GetTexture(Sprites.SANDBOX_HOLD);
		}

		public static TestHold Instantiate(float height) {
			// Create GameObject
			GameObject obj = new GameObject();
			obj.name = NAME;
			obj.tag = Tags.SANDBOX_TEST_HOLD;

			// Add TestHold Component
			TestHold beatsObj = obj.AddComponent<TestHold>();

			// Add Sprite Component
			SpriteFlat sprite = obj.AddComponent<SpriteFlat>();
			SpriteFlatData data = new SpriteFlatData(NAME, _texture, _width, height);
			sprite.Setup(data);
			beatsObj._sprite = sprite;
			beatsObj._data = data;

			// Add BoxCollider
			BoxCollider collider = obj.AddComponent<BoxCollider>();
			collider.size = sprite.dimensions;
			beatsObj._collider = collider;

			// Return instantiated BeatsObject
			return beatsObj;
		}

		public override void Destroy() {
			base.Destroy();
			_data.Destroy();
		}
	}
}
