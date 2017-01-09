using UnityEngine;
using System.Collections;

public class spacefield : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		Object[] temp = GameObject.FindObjectsOfType(typeof(Monster));
		for(int i = 0 ; i < temp.Length; i++){
			Monster monster = (Monster)temp[i];
			Vector3 pos = monster.rigidbody.velocity;
			Vector3 accPos = new Vector3(pos.y, -pos.x, 0f).normalized * 0.09f;
			monster.rigidbody.velocity += accPos * Time.deltaTime;
			monster.rigidbody.velocity = monster.rigidbody.velocity.normalized * monster.velocity.magnitude;
		}
	}
}
