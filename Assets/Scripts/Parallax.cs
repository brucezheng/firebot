using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {
	private float X;
	public int offset;
	public bool FollowCamera;
	
	void Start() {
		//renderer.sortingLayerName = "Background";
		X = transform.position.x;
	}
	
	void Update() {
		if(FollowCamera) {
			transform.position = Camera.main.transform.position;
			//transform.position = new Vector2(Camera.main.transform.position.x - (Camera.main.transform.position.x - X) / offset,transform.position.y);
		} else {
			transform.position = new Vector2(X + (Camera.main.transform.position.x) / offset,transform.position.y);
		}
	}
}
