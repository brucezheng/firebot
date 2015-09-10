using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		particleSystem.renderer.sortingLayerName = "Foreground";
	}
}
