using UnityEngine;
using System.Collections;

public class TestArrowScript : MonoBehaviour {
	
	public int column;
	public float time;
	public bool hit;
	public SandboxScene scene;
	public MusicTimeScript music;
	
	// Use this for initialization
	void Start () {
		UISprite sprite = this.gameObject.GetComponent<UISprite>();
		sprite.MakePixelPerfect();
		scene = (SandboxScene)GameObject.FindObjectOfType(typeof(SandboxScene));
	}
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject == null) return;
		Vector3 moveDist = new Vector3(0, Time.deltaTime * -500, 0);
		this.gameObject.transform.localPosition += moveDist;
		
		float timeDiff = time - scene.MusicScript.MusicTime;
		float updatedY = TestHitbuttonScript.HITBUTTON_Y + timeDiff * 500f;
		this.gameObject.transform.localPosition = new Vector3(
			this.gameObject.transform.localPosition.x,
			updatedY,
			this.gameObject.transform.localPosition.z
		);
		
		float y = this.gameObject.transform.localPosition.y;
		if (y < -400f && y > -700f) {
			if (!hit) {
				scene.TimeDiffScript.TimeDiff = -999f;
			}
			hit = true;
		} else if (y < -700f) {
			scene.DestroyArrow(this.gameObject);
		}
	}
}
