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

using Beats2.Core;
using UnityEngine;

public class SandboxScene : MonoBehaviour {
	private const string TAG = "Sandbox";
	
	public GameObject Arrow1Prefab, Arrow2Prefab, Arrow3Prefab, Arrow4Prefab;
	//public float AddInterval = 0.5f;
	public MusicTimeScript MusicScript;
	public TestTimeDiffScript TimeDiffScript;
	public BetterList<GameObject> ArrowsList;
	public int ArrowIndex;
	
	// Use this for initialization
	void Start () {
		_camera = GameObject.FindGameObjectWithTag("Runtime");
		//_addTimer = AddInterval;
		_arrowCount = 0;
		ArrowsList = new BetterList<GameObject>();
		MusicScript = (MusicTimeScript)GameObject.FindObjectOfType(typeof(MusicTimeScript));
		TimeDiffScript = (TestTimeDiffScript)GameObject.FindObjectOfType(typeof(TestTimeDiffScript));
		ArrowIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.Escape)){
			Application.Quit();
		} else if (Input.GetKey(KeyCode.P) || Input.GetKey(KeyCode.Home)){
			Application.CaptureScreenshot("Screenshot.png");
		}
		while (ArrowIndex < TestNotes.Data.Length - 1 && TestNotes.Data[ArrowIndex] < MusicScript.MusicTime + 3f) {
			float time = TestNotes.Data[ArrowIndex];
			ArrowIndex++;
			int column = (int)TestNotes.Data[ArrowIndex];
			Logger.Log(TAG, "Adding arrow: {0}, {1}", time, column);
			
			GameObject arrowPrefab = Arrow1Prefab;
			switch(column) {
				case 0: arrowPrefab = Arrow1Prefab; break;
				case 1: arrowPrefab = Arrow2Prefab; break;
				case 2: arrowPrefab = Arrow3Prefab; break;
				case 3: arrowPrefab = Arrow4Prefab; break;
			}
			Vector3 position = new Vector3(
				-210f + column * 140f,
				700f,
				0f
			);
			GameObject arrow = NGUITools.AddChild(_camera, arrowPrefab);
			arrow.name = "_arrow" + _arrowCount;
			arrow.transform.localPosition = position;
			UISprite sprite = arrow.GetComponent<UISprite>();
			sprite.depth = _arrowCount;
			TestArrowScript script = arrow.GetComponent<TestArrowScript>();
			script.column = column;
			script.time = time;
			_arrowCount++;
			ArrowsList.Add(arrow);
		}
	}
	
	public void DestroyArrow(GameObject arrow)
	{
		ArrowsList.Remove(arrow);
		GameObject.Destroy(arrow);
	}
	
	public void OnHitboxClick(int column)
	{
		for (int i = 0; i < ArrowsList.size; i++) {
			GameObject arrow = ArrowsList.buffer[i];
			TestArrowScript script = arrow.GetComponent<TestArrowScript>();
			if (script.column == column) {
				float timeDiff = script.time - MusicScript.MusicTime;
				if (Mathf.Abs(timeDiff) < 0.3f) {
					TimeDiffScript.TimeDiff = timeDiff;
					Logger.Log(TAG, "Column {0} diff: {1}", column, timeDiff);
					script.hit = true;
					DestroyArrow(arrow);
					return;
				}
				
				/*
				if (y > -350 && y < -100) {
					Logger.Log(TAG, "Destroying arrow on column: {0}", column);
					DestroyArrow(arrow);
					return;
				}
				*/
				/*
				float diff = (arrow.transform.localPosition.y - TestHitbuttonScript.HITBUTTON_Y);
				if (diff > 0f && diff < 100f)
				{
					Logger.Log(TAG, "Column {0} diff: {1}", column, diff);
					DestroyArrow(arrow);
					return;
				}
				*/
			}
		}
		
	}
	
	private float _addTimer;
	private int _arrowCount;
	private GameObject _camera;
}
