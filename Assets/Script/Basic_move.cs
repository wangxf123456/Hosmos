using UnityEngine;
using System.Collections;

public class Basic_move : MonoBehaviour {
	public Vector3 velocity = new Vector3(0, 0, 0);
	public float time = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position += velocity * Time.deltaTime;
	}
}
