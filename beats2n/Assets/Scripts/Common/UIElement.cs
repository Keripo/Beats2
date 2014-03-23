/*
	Copyright (c) 2013, Keripo
	All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
	    * Redistributions of source code must retain the above copyright
	      notice, this list of conditions and the following disclaimer.
	    * Redistributions in binary form must reproduce the above copyright
	      notice, this list of conditions and the following disclaimer in the
	      documentation and/or other materials provided with the distribution.
	    * Neither the name of the <organization> nor the
	      names of its contributors may be used to endorse or promote products
	      derived from this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using UnityEngine;
using System;
using System.Collections;
using Beats2.Core;

namespace Beats2.Common {
	
	/// <summary>
	/// Base UI element
	/// </summary>
	public class UIElement : MonoBehaviour {
		
		/// <summary>
		/// Setup the element's parent, name and position.
		/// </summary>
		public virtual void Setup(GameObject parent, string name, Vector3 position) {
			// Assign parent (NGUITools.AddChild)
			Transform t = this.gameObject.transform;
			t.parent = parent.transform;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			this.gameObject.layer = parent.layer;
			
			// Set initial position
			t.localPosition = position;
			
			// Set name;
			this.gameObject.name = name;
		}
		
		/// <summary>
		/// Called upon scene start
		/// </summary>
		public virtual void Start() {}
		
		/// <summary>
		/// Called upon each frame update
		/// </summary>
		public virtual void Update() {}
	}
	
}
