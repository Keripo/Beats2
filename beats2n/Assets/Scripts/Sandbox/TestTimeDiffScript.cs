using UnityEngine;
using System.Collections;

public class TestTimeDiffScript : MonoBehaviour {

	UILabel _info;
	public float TimeDiff;
	// Use this for initialization
	void Start () {
		_info = this.gameObject.GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		_info.text = string.Format("{0:f3}", TimeDiff);
		float timeDiffAbs = Mathf.Abs(TimeDiff);
		if (timeDiffAbs < 0.1f) {
			_info.color = Color.green;
		} else if (timeDiffAbs < 0.2f) {
			_info.color = Color.yellow;
		} else {
			_info.color = Color.red;
		}
	}
}
