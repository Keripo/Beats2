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
	public class SpriteAnimData {
		public string name;
		public int frames;
		public float width, height, regionWidth, regionHeight;
		public tk2dSpriteCollectionData data;
		public tk2dSpriteAnimation anim;

		public SpriteAnimData(string name, Texture2D texture, float width, int frames) : this(name, texture, width, frames, false) {}
		public SpriteAnimData(string name, Texture2D texture, float width, int frames, bool loop) : this(name, texture, width, frames, loop, 0.25f) {}
		public SpriteAnimData(string name, Texture2D texture, float width, int frames, bool loop, float duration) : this(name, texture, width, 0f, ScaleType.SCALED_WIDTH, frames, loop, duration) {}
		public SpriteAnimData(string name, Texture2D texture, float width, float height, ScaleType scaleType, int frames, bool loop, float duration) {
			this.name = name;
			this.frames = frames;

			float textureWidth = texture.width / frames;
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

			string[] names = new string[frames];
			Rect[] regions = new Rect[frames];
			Vector2[] anchors = new Vector2[frames];
			for (int i = 0; i < frames; i++) {
				names[i] = String.Format("{0}_frame{1}", name, i);
				regions[i] = new Rect(this.regionWidth * i, 0f, this.regionWidth, this.regionHeight);
				anchors[i] = new Vector2(this.regionWidth / 2, this.regionHeight / 2);
			}

			tk2dRuntime.SpriteCollectionSize size = tk2dRuntime.SpriteCollectionSize.ForTk2dCamera();
			this.data = tk2dRuntime.SpriteCollectionGenerator.CreateFromTexture(texture, size, names, regions, anchors);
			this.data.gameObject.name = String.Format("DataSpriteAnim{0}", name);

			tk2dSpriteAnimationFrame[] animationFrames = new tk2dSpriteAnimationFrame[frames];
			for (int i = 0; i < frames; i++) {
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteCollection = this.data;
				frame.spriteId = i;
				animationFrames[i] = frame;
			}
			tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip();
			clip.fps = frames / duration;
			clip.name = name;
			clip.wrapMode = loop ? tk2dSpriteAnimationClip.WrapMode.Loop : tk2dSpriteAnimationClip.WrapMode.Once;
			clip.frames = animationFrames;

			this.anim = this.data.gameObject.AddComponent<tk2dSpriteAnimation>();
			this.anim.clips = new tk2dSpriteAnimationClip[] { clip };
		}

		/// <summary>
		/// Destroys the spriteCollection parent GameObject
		/// </summary>
		public void Destroy() {
			UnityEngine.Object.Destroy(data.gameObject);
		}
	}
}
