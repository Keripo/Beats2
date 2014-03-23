using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;
using Beats2.System;
using Beats2.Graphic;

namespace Beats2.UI {
	
	public class BeatsObject<T> : MonoBehaviour where T : BaseGraphic {

		protected T _sprite = null;
		protected BoxCollider _collider = null;

		public new string name {
			get { return gameObject.name; }
			set { gameObject.name = value; }
		}

		public Vector3 position {
			get { return _sprite.position; }
			set { _sprite.position = value; }
		}
		public float x {
			get { return _sprite.x; }
			set { _sprite.x = value; }
		}
		public float y {
			get { return _sprite.y; }
			set { _sprite.y = value; }
		}
		public float z {
			get { return _sprite.z; }
			set { _sprite.z = value; }
		}
		public Vector3 dimensions {
			get { return _sprite.dimensions; }
			set {
				_sprite.dimensions = value;
				if (_collider != null) _collider.size = value;
			}
		}
		public float width {
			get { return _sprite.width; }
			set { _sprite.width = value; }
		}
		public float height {
			get { return _sprite.height; }
			set { _sprite.height = value; }
		}
		public Color color {
			get { return _sprite.color; }
			set { _sprite.color = value; }
		}

		/// <summary>
		/// Destroy the GameObject
		/// </summary>
		public virtual void Destroy() {
			UnityEngine.Object.Destroy(gameObject);
		}
	}
}

