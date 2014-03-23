using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Beats2;
using Beats2.System;

namespace Beats2.Graphic {

	/// <summary>
	/// SpriteData. Wraps tk2d's tk2dSpriteCollectionDatas
	/// </summary>
	public class SpriteFlatData {
		public string name;
		public float width, height, regionWidth, regionHeight;
		public tk2dSpriteCollectionData data;

		public SpriteFlatData(string name, Texture2D texture) : this(name, texture, 0f, 0f, ScaleType.NONE) {}
		public SpriteFlatData(string name, Texture2D texture, float width) : this(name, texture, width, 0f, ScaleType.SCALED_WIDTH) {}
		public SpriteFlatData(string name, Texture2D texture, float width, float height) : this(name, texture, width, height, ScaleType.SCALED) {}
		public SpriteFlatData(string name, Texture2D texture, float width, float height, ScaleType scaleType) {
			this.name = name;

			float textureWidth = texture.width;
			float textureHeight = texture.height;
			switch (scaleType) {
				case ScaleType.NONE:
					this.width = textureWidth;
					this.height = textureHeight;
					break;
				case ScaleType.SCALED_WIDTH:
					this.width = width;
					this.height = width * textureHeight / textureWidth;
					break;
				case ScaleType.SCALED_HEIGHT:
					this.width = height * textureWidth / textureHeight;
					this.height = height;
					break;
				case ScaleType.SCALED:
				default:
					this.width = width;
					this.height = height;
					break;
			}
			this.regionWidth = textureWidth;
			this.regionHeight =
				texture.wrapMode == TextureWrapMode.Repeat ?
				this.height * textureWidth / this.width :
				textureHeight
			;

			Rect region = new Rect(0f, 0f, this.regionWidth, this.regionHeight);
			Vector2 anchor = new Vector2(this.regionWidth / 2, this.regionHeight / 2);

			tk2dRuntime.SpriteCollectionSize size = tk2dRuntime.SpriteCollectionSize.ForTk2dCamera();
			this.data = tk2dRuntime.SpriteCollectionGenerator.CreateFromTexture(texture, size, region, anchor);
			this.data.gameObject.name = String.Format("DataSpriteFlat{0}", name);
		}

		/// <summary>
		/// Destroys the spriteCollection parent GameObject
		/// </summary>
		public void Destroy() {
			UnityEngine.Object.Destroy(data.gameObject);
		}
	}
}