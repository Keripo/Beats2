using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Beats2;
using Beats2.System;

namespace Beats2.Graphic {

	public class SpriteInfo : Attribute {
		public string name, path;
		public bool repeat;
		public SpriteInfo(string name, string path, bool repeat) {
			this.name = name;
			this.path = path;
			this.repeat = repeat;
		}
	}

	public enum Sprites {
		[SpriteInfo("Sandbox_Background",		"Sandbox/Background.jpg", false)] 				SANDBOX_BACKGROUND,
		[SpriteInfo("Sandbox_Logo",				"Sandbox/Logo.png", false)] 					SANDBOX_LOGO,
		[SpriteInfo("Sandbox_Arrow",			"Sandbox/Arrow.png", false)] 					SANDBOX_ARROW,
		[SpriteInfo("Sandbox_Mine",				"Sandbox/Mine.png", false)] 					SANDBOX_MINE,
		[SpriteInfo("Sandbox_Hold",				"Sandbox/Hold.png", true)] 						SANDBOX_HOLD,
	}

	/// <summary>
	/// Resource loader. Wraps Unity's WWW class.
	/// </summary>
	public static class SpriteLoader {
		private const string TAG = "SpriteLoader";
		private static Dictionary<Sprites, Texture2D> _textureCache;

		public static void Init() {
			Reset();
			Logger.Debug(TAG, "Initialized...");
		}

		public static void Reset() {
			PreloadSprites();
			Logger.Debug(TAG, "Reset...");
		}

		private static void PreloadSprites() {
			int numSprites = Enum.GetNames(typeof(Sprites)).Length;
			_textureCache = new Dictionary<Sprites, Texture2D>(numSprites);

			foreach (Sprites sprite in Enum.GetValues(typeof(Sprites))) {
				// Reflection magic!
				MemberInfo memberInfo = typeof(Sprites).GetMember(sprite.ToString()).FirstOrDefault();
				SpriteInfo spriteInfo = (SpriteInfo)Attribute.GetCustomAttribute(memberInfo, typeof(SpriteInfo));

				string path = SysPath.GetDataPath(spriteInfo.path);
				Texture2D texture = LoadTexture(path, spriteInfo.repeat);
				_textureCache.Add(sprite, texture);
			}
		}

		public static Texture2D LoadTexture(string path, bool repeat) {
			Texture2D texture;
			WWW www = new WWW(SysPath.GetWwwPath(path));
			while (!www.isDone); // FIXME: Blocks, thread this?
			texture = www.texture; // Compare with www.LoadImageIntoTexture(texture)?
			texture.wrapMode = (repeat) ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
			texture.Compress(true);
			www.Dispose();
			return texture;
		}

		public static Texture2D GetTexture(Sprites sprite) {
			Texture2D texture;
			if (!_textureCache.TryGetValue(sprite, out texture)) {
				Logger.Error(TAG, String.Format("Unable to fetch sprite \"{0}\"", sprite));
			}
			return texture;
		}
	}
}
