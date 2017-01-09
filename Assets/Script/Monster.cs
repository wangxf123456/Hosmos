using UnityEngine;
using System.Collections;
using System.Reflection;

public class Monster : MonoBehaviour {
	static float border = 25;
	static float shrink_rate = 1f;
	public float radius = 0.5f;
	public Vector3 velocity = new Vector3(1, 1, 0);
	public Light point_light;
	private int shrink_timer = 0;
	private float explode_bound = 0;
	private bool if_collided = false;
	private bool if_shrink = false;
	private float mass_bound = 0;
	private float max_velocity;
	
	// Use this for initialization
	void Start () {
		if(!GetComponent<character>()){
			//rigidbody.velocity = velocity;
			rigidbody.mass = radius * radius;
			updateScale ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		max_velocity = 100 / rigidbody.mass;
		if(this.rigidbody.velocity.magnitude > max_velocity){
			this.rigidbody.velocity *= max_velocity / this.rigidbody.velocity.magnitude;
		}

		if(!GetComponent<character>()){
			Vector3 velocity = rigidbody.velocity;
			this.rigidbody.AddForce(rigidbody.mass * new Vector3(-velocity.y, velocity.x, 0).normalized * velocity.magnitude * velocity.magnitude / transform.position.magnitude);
			this.rigidbody.AddForce(new Vector3(velocity.x, velocity.y, 0).normalized / ((rigidbody.mass+ 0.1f) * (rigidbody.mass+ 0.1f) * (transform.position.magnitude + 0.1f)));
		}
		if (if_collided) {
			if (if_shrink) {
				shrink();
			} else {
				explode();
			}
		}
		if (!GetComponent<character> ()) {
			this.transform.localScale = Mathf.Sqrt(this.rigidbody.mass) * new Vector3 (1, 1, 1);
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		combine (collision);
	}
	
	void OnTriggerExit(Collider col){
		if(!GetComponent<character>() && col.GetType() == typeof(BoxCollider)){
			Destroy(this.gameObject);
		}
	}
	
	void updateScale() {
		Vector3 scale = transform.localScale;
		scale.x = radius;
		scale.y = radius;
		transform.localScale = scale;		
	}
	
	void combine(Collision collision) {
		//print ("in combine");
		GameObject other = collision.collider.gameObject;
		if (other.GetComponent<Monster> () || other.GetComponent<character>()) {
			if_collided = true;	
			//print (other.rigidbody.mass +" " + other.GetComponent<character>() + Time.time);
			if (other.rigidbody.mass < rigidbody.mass || (other.rigidbody.mass == rigidbody.mass && GetComponent<character>())) {
				rigidbody.velocity = (rigidbody.mass * rigidbody.velocity 
				                      + other.rigidbody.mass * other.rigidbody.velocity) 
					/ (rigidbody.mass + other.rigidbody.mass);
				mass_bound = rigidbody.mass + other.rigidbody.mass;	
				radius = Mathf.Sqrt(rigidbody.mass);
				if_shrink = false;
				foreach (ContactPoint contact in collision.contacts) {
					GameObject lightGameObject = new GameObject("The Light");
					lightGameObject.AddComponent<Light>();
					lightGameObject.AddComponent<Basic_move>();
					Basic_move basic_move = lightGameObject.GetComponent<Basic_move>();
					basic_move.velocity = rigidbody.velocity;
					lightGameObject.light.intensity = 1;
					Vector3 pos = contact.point;
					pos.z -= 1;
					lightGameObject.transform.position = pos;
					Destroy(lightGameObject, 1);
				}
			} else if (other.rigidbody.mass == rigidbody.mass && !GetComponent<character>() && !other.GetComponent<character>()) {
				if (transform.position.x < other.transform.position.x) {
					rigidbody.velocity = (rigidbody.mass * rigidbody.velocity 
					                      + other.rigidbody.mass * other.rigidbody.velocity) 
						/ (rigidbody.mass + other.rigidbody.mass);
					mass_bound = rigidbody.mass + other.rigidbody.mass;	
					radius = Mathf.Sqrt(rigidbody.mass);
					if_shrink = false;
					foreach (ContactPoint contact in collision.contacts) {
						GameObject lightGameObject = new GameObject("The Light");
						lightGameObject.AddComponent<Light>();
						lightGameObject.AddComponent<Basic_move>();
						Basic_move basic_move = lightGameObject.GetComponent<Basic_move>();
						basic_move.velocity = rigidbody.velocity;
						lightGameObject.light.intensity = 1;
						Vector3 pos = contact.point;
						pos.z -= 1;
						lightGameObject.transform.position = pos;
						Destroy(lightGameObject, 1);
					}				
				} else if (transform.position.x == other.transform.position.x) {
					if (transform.position.y < other.transform.position.y) {
						rigidbody.velocity = (rigidbody.mass * rigidbody.velocity 
						                      + other.rigidbody.mass * other.rigidbody.velocity) 
							/ (rigidbody.mass + other.rigidbody.mass);
						mass_bound = rigidbody.mass + other.rigidbody.mass;	
						radius = Mathf.Sqrt(rigidbody.mass);
						if_shrink = false;
						foreach (ContactPoint contact in collision.contacts) {
							GameObject lightGameObject = new GameObject("The Light");
							lightGameObject.AddComponent<Light>();
							lightGameObject.AddComponent<Basic_move>();
							Basic_move basic_move = lightGameObject.GetComponent<Basic_move>();
							basic_move.velocity = rigidbody.velocity;
							lightGameObject.light.intensity = 1;
							Vector3 pos = contact.point;
							pos.z -= 1;
							lightGameObject.transform.position = pos;
							Destroy(lightGameObject, 1f);
						}					
					} else {
						if_shrink = true;						
					}
				} else {
					if_shrink = true;
				}
			} else {
				if_shrink = true;
				//other.collider.enabled = false;
				//print (other.GetComponent<character>());
				if (other.GetComponent<character> ()) {
					print ("call it");
					GameObject.Find("character").GetComponent<character>().collideOnCharacter(this.gameObject);
				}
			}
		}
	}
	
	void shrink() {
		if (rigidbody.mass <= 1) {
			Destroy(gameObject);
			//print ("destroyt this one " + Time.time);
		} else {
			if (shrink_timer > 0) {
				shrink_timer--;
			}
			if (shrink_timer == 0) {
				rigidbody.mass -= shrink_rate;
				shrink_timer = 1;
			}		
		}
	}
	
	void explode() {
		if (rigidbody.mass < mass_bound) {
			
			if (shrink_timer > 0) {
				shrink_timer--;
			}
			
			if (shrink_timer <= 0) {
				rigidbody.mass += shrink_rate;
				shrink_timer = 1;
			}	
		} else {
			if_collided = false;
			mass_bound = 0;
		}
	}
}

