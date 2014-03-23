using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Beats2;
using Beats2.System;

namespace Beats2.Graphic {

	/// <summary>
	/// FontMeshData. Wraps tk2d's tk2dFontData
	/// </summary>
	public class FontMeshData {
		public tk2dFontData data;
		public float width, height;

		public FontMeshData(string name, string fontBitmapPath, string fontInfoPath) {
			GameObject obj = new GameObject();
			obj.name = String.Format("DataFontMesh{0}", name);
			this.data = obj.AddComponent<tk2dFontData>();

			FontInfo fontInfo = FontBuilder.ParseBMFont(fontInfoPath);
			FontBuilder.BuildFont(fontInfo, data, 1, 0, false, false, null, 0);

			Material fontMaterial = new Material(Shader.Find("tk2d/BlendVertexColor"));
			Texture2D texture = SpriteLoader.LoadTexture(fontBitmapPath, false);

			fontMaterial.mainTexture = texture;
			this.data.material = fontMaterial;
			this.width = this.data.largestWidth;
			this.height = this.data.lineHeight;
		}

		public void Destroy() {
			UnityEngine.Object.Destroy(data.gameObject);
		}
	}
}
