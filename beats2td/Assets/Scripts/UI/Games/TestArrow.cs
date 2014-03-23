using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;
using Beats2.Graphic;

namespace Beats2.UI {
	
	public class TestArrow : BeatsObject<SpriteAnim> {
		private const string NAME = "_TestArrow";

		private static SpriteAnimData _data;

		public static void Init() {
			// Create SpriteData
			float width = Screens.min * 0.10f;
			Texture2D texture = SpriteLoader.GetTexture(Sprites.SANDBOX_ARROW);
			_data = new SpriteAnimData(NAME, texture, width, 4, true, 1f);
		}

		public static TestArrow Instantiate() {
			// Create GameObject
			GameObject obj = new GameObject();
			obj.name = NAME;
			obj.tag = Tags.SANDBOX_TEST_ARROW;

			// Add TestArrow Component
			TestArrow beatsObj = obj.AddComponent<TestArrow>();

			// Add Sprite Component
			SpriteAnim sprite = obj.AddComponent<SpriteAnim>();
			sprite.Setup(_data);
			sprite.Play();
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
