using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;

namespace Beats2.Graphic {

	/// <summary>
	/// Flat sprite object. Creates and wraps tk2D's tk2dSprite class
	/// </summary>
	public class SpriteFlat : BaseGraphic {

		protected tk2dSprite _sprite;
		protected SpriteFlatData _data;
		protected Vector3 _dimensions, _dimensionsInit;

		public void Setup(SpriteFlatData data) {
			_data = data;
			_sprite = gameObject.AddComponent<tk2dSprite>();
			_sprite.SwitchCollectionAndSprite(data.data, 0);
			_sprite.Build();

			_dimensionsInit = new Vector3(data.regionWidth, data.regionHeight, 1f);
			dimensions = new Vector3(data.width, data.height, 1f);
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
			set { _sprite.color = value; }
		}
	}
}