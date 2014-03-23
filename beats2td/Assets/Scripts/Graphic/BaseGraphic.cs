using UnityEngine;
using System;
using System.Collections.Generic;
using Beats2;

namespace Beats2.Graphic {

	public enum ScaleType {
		NONE,
		SCALED,
		SCALED_WIDTH,
		SCALED_HEIGHT
	}

	public abstract class BaseGraphic : MonoBehaviour {

		public abstract Vector3 position { get; set; }
		public abstract float x { get; set; }
		public abstract float y { get; set; }
		public abstract float z { get; set; }
		public abstract Vector3 dimensions { get; set; }
		public abstract float width { get; set; }
		public abstract float height { get; set; }
		public abstract Color color { get; set; }
	}
}