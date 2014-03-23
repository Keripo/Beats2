using UnityEngine;
using System.Collections;

public class TestHitbuttonScript : MonoBehaviour {
		
	public KeyCode key1, key2;
	public int column = 0;
	public SandboxScene scene;
	public static float HITBUTTON_Y = -253.2f;
	
	// Use this for initialization
	void Start () {
		scene = (SandboxScene)GameObject.FindObjectOfType(typeof(SandboxScene));
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(key1) || Input.GetKeyDown(key2)) {
			SendMessage("OnPress", true);
		} else if (Input.GetKeyUp(key1) || Input.GetKeyUp(key2)) {
			SendMessage("OnPress", false);
		}
	}
	
	public void OnPress(bool isDown)
	{
		if (isDown) {
			scene.OnHitboxClick(column);
		}
	}
}