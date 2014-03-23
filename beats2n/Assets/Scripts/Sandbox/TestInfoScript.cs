using Beats2.Core;
using UnityEngine;
using System.Collections;

public class TestInfoScript : MonoBehaviour {
	
	UILabel _info;
	// Use this for initialization
	void Start () {
		DeviceInfo.LoadInfo();
		_info = this.gameObject.GetComponent<UILabel>();
		_info.text = DeviceInfo.GetInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
