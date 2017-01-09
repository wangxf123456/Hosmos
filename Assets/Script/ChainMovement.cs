using UnityEngine;
using System.Collections;

public class ChainMovement : MonoBehaviour {
	[HideInInspector]
	public Transform origin;
	
	public Transform orphan;
	
	[HideInInspector]
	public GameObject orphanObj = null;
	
	public int segments = 100;
	public float segmentLength;
	
	Particle[] particles;
	
	[HideInInspector]
	public Vector3 velocity;
	
	public BallMovement parent;
	
	// Use this for initialization
	void Start () {
		segmentLength = 1.0f / segments;
		particleEmitter.emit = false;
		
		particleEmitter.Emit (segments);
		particles = particleEmitter.particles;
	}
	
	public float bend = 0.00001f;
	public float maxHookMultiplier = 10f;
	
	// Update is called once per frame
	void Update () {
		if (!origin) {
			//origin.gameObject.GetComponent<character>().hookAbandon();
			return;
		}
		Vector3 surfacePosition = Vector3.Normalize (this.transform.position - origin.position) *
			origin.localScale.x / 2 + origin.position;
		
		Vector3 perpendicular = Vector3.Normalize (Vector3.Cross(surfacePosition - this.transform.position
		                                                         ,Vector3.forward));
		float distance = (surfacePosition - this.transform.position).magnitude;
		if (!abandoned && distance > maxHookMultiplier * origin.localScale.x) {
			//origin.gameObject.GetComponent<character>().hookAbandon();
		}
		for(int i = 0; i < particles.Length; i++) {
			Vector3 pos = Vector3.Lerp (surfacePosition, this.transform.position, segmentLength * i);
			float displacement = (Mathf.PerlinNoise(Time.time, i * segmentLength) - 0.5f) * bend *
				Mathf.Sqrt(distance) * (i * segmentLength) * (1 - i * segmentLength);
			particles[i].position = pos + displacement * perpendicular;
			particles[i].color = Color.magenta;
			particles[i].energy = 1f;
		}
		
		particleEmitter.particles = particles;
	}
	
	void FixedUpdate () {
		this.transform.position += velocity * Time.fixedDeltaTime;
	}
	
	void OnTriggerEnter(Collider col) {
		Debug.Log ("Triggered");
		if (abandoned) {
			if (col.gameObject == orphanObj) {
				Destroy (orphanObj);
				Destroy (this.gameObject);
			}
		} 
		else {
			GameObject GO = col.gameObject;
			if (GO.GetComponent<BallMovement>() != null) {
				if (GO.GetComponent<BallMovement>() == parent) 
					return;
			}
			if (GO.GetComponent<Monster>() != null) {
				Vector3 relDir = this.transform.position - GO.transform.position;
				Quaternion quat = Quaternion.FromToRotation (GO.transform.forward, relDir);
				GameObject.Find ("character").GetComponent<character> ().hookAttach (GO, quat);
				this.transform.parent = GO.transform;
				velocity = Vector3.zero;
			}
			else {
				origin.gameObject.GetComponent<character>().hookAbandon();
			}
		}
	}
	
	private bool abandoned = false;
	public void hookAbandon() {
		
		abandoned = true;
		orphanObj = ((Transform)Instantiate (orphan, Vector3.Normalize 
		                                     (this.transform.position - origin.position) * origin.localScale.x / 2 + origin.position,
		                                     Quaternion.identity)).gameObject;
		Vector3 dir = Vector3.Normalize(orphanObj.transform.position - this.transform.position);
		orphanObj.GetComponent<Basic_move> ().velocity = -5 * dir;
		this.velocity = 5 * dir;
		origin = orphanObj.transform;
	}
	
	public void destroyChain() {
		if (orphanObj != null) Destroy (orphanObj);
		Destroy (this.gameObject);
	}
}
