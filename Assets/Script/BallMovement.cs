using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {
	
	public Transform tailPref;
	
	public Transform chainPref;
	
	[HideInInspector]
	public Vector3 velocity = Vector3.zero;
	
	GameObject tail;
	GameObject chain;
	// Use this for initialization
	void Start () {
		tail = ((Transform) Instantiate (tailPref, this.transform.position, Quaternion.identity)).gameObject;
	}
	
	public float acc = 2f;
	
	/*void FixedUpdate () {
		float xVel = Input.GetAxis("Horizontal");
		float yVel = Input.GetAxis ("Vertical");
		velocity.x += xVel * Time.deltaTime;
		float angle;
		velocity.y += yVel * Time.deltaTime;
		if(xVel == 0.0f)
			angle = yVel > 0.0f ? Mathf.PI / 2: -Mathf.PI / 2;
		else if(xVel > 0.0f) {
			angle = Mathf.Atan(yVel / xVel);
		}
		else {
			angle = Mathf.PI + Mathf.Atan(yVel / xVel);
		}
		xVel = Mathf.Abs (xVel);
		yVel = Mathf.Abs (yVel);
		this.transform.position += velocity * Time.deltaTime;
		accelerateAt (Mathf.PI + angle, Mathf.Sqrt(xVel * xVel + yVel * yVel));
		
		if(Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log("Space pressed");
			hookOut(Mathf.PI / 4);
			Invoke ("hookAbandon", 2f);
		}
	}*/
	
	public void accelerateAt (float angle, float speed) {
		tail.transform.position = this.transform.position - new Vector3 (this.transform.localScale.x / 2
		                                                                 * Mathf.Cos (angle), this.transform.localScale.x / 2 * Mathf.Sin (angle), 0f);
		tail.transform.LookAt (this.transform.position);
		tail.transform.position = 2 * this.transform.position - tail.transform.position;
		tail.particleSystem.Emit ((int)(speed * 3));
	}
	
	public void hookOut(float angle) {
		Vector3 dir = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0f);
		Vector3 pos = this.transform.position + this.transform.localScale.x / 1.6f * dir;
		chain = ((Transform)Instantiate (chainPref, pos, Quaternion.identity)).gameObject;
		chain.GetComponent<ChainMovement>().origin = this.transform;
		chain.GetComponent<ChainMovement> ().velocity = dir * 2;
		chain.GetComponent<ChainMovement> ().parent = this;
	}
	
	public void hookAbandon() {
		chain.GetComponent<ChainMovement> ().hookAbandon ();
	}
	
	void OnDrawGizmos() {
		if (tail) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine (this.transform.position, this.transform.position + 2 * tail.transform.forward);
		}
	}
}
