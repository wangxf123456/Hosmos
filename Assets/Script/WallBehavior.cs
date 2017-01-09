using UnityEngine;
using System.Collections;

public class WallBehavior : MonoBehaviour {

	float time = 0;

	// Use this for initialization
	void Start () {
		time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - time > 2){
			Destroy(this.gameObject);
		}
	}
}
