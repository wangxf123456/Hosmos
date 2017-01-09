using UnityEngine;
using System.Collections;

public class header : MonoBehaviour {

	private character main;

	// Use this for initialization
	void Start () {
		main = GameObject.Find ("Sphere").GetComponent<character> ();
	}
	
	// Update is called once per frame
	void Update () {

		//rotate the header
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		this.transform.LookAt (new Vector3(mousePos.x, mousePos.y , 0));

		//update the position
		this.transform.position = main.transform.position;
	}
}
