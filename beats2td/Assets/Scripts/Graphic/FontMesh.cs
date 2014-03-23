using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;

namespace Beats2.Graphic {

	/// <summary>
	/// Font mesh object. Creates and wraps tk2D's tk2dTextMesh class
	/// </summary>
	public class FontMesh : BaseGraphic {

		protected tk2dTextMesh _sprite;
		protected FontMeshData _data;
		protected Vector3 _dimensions, _dimensionsInit;
		private bool _commit;

		public void Setup(FontMeshData data, float fontWidth, float fontHeight, TextAnchor anchor) {
			_data = data;
			_sprite = gameObject.AddComponent<tk2dTextMesh>();
			_sprite.font = _data.data;
			_sprite.text = "AAAAAAAAAAAAAAA"; // starting size of 16 chars
			_sprite.anchor = anchor;

			_dimensionsInit = new Vector3(data.width, data.height, 1f);
			dimensions = new Vector3(fontWidth, fontHeight, 1f);
			_commit = true;
		}

		public UnityEngine.TextAnchor anchor {
			get { return _sprite.anchor; }
			set { _sprite.anchor = value; _commit = true;; }
		}

		public string text {
			get { return _sprite.text; }
			set {
				_sprite.text = value;
				if (value.Length > _sprite.maxChars) {
					_sprite.maxChars = value.Length * 2;
				}
				_commit = true;;
			}
		}

		public override Vector3 position {
			get { return gameObject.transform.position; }
			set { gameObject.transform.position = value; }
		}
		public override float x {
			get { return position.x; }
			set { position = new Vector3(value, position.y, position.z); }
		}
		public override float y {
			get { return position.y; }
			set { position = new Vector3(position.x, value, position.z); }
		}
		public override float z {
			get { return position.y; }
			set { position = new Vector3(position.x, position.y, value); }
		}
		public override Vector3 dimensions {
			get { return _dimensions; }
			set {
				_dimensions = value;
				_sprite.scale = new Vector3(_dimensions.x / _dimensionsInit.x, _dimensions.y / _dimensionsInit.y, 1f);
				_commit = true;;
			}
		}
		public override float width {
			get { return _dimensions.x; }
			set { dimensions = new Vector2(value, dimensions.y); }
		}
		public override float height {
			get { return _dimensions.y; }
			set { dimensions = new Vector2(dimensions.x, value); }
		}
		public override Color color {
			get { return _sprite.color; }
			set { _sprite.color = value; _commit = true;; }
		}

		public void Update() {
			if (_commit) {
				_sprite.Commit();
				_commit = false;
			}
		}
	}
}
