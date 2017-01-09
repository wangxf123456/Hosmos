using UnityEngine;
using System.Collections;

public class character : MonoBehaviour {
	

	private Vector3 acc = Vector3.zero;
	private Vector3 accChange = Vector3.zero;
	private Vector3 velocity;


	private bool isHooked;
	private bool isAttached;
	private GameObject attachedMonster;
	private Quaternion monsterQua;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//update size
		updateSize ();

		//get the hook behavior
		if(isHooked  == true){
			this.rigidbody.mass -= 0.0005f;
		}
		
		if(isAttached){
			//print ("ahaha");
			this.rigidbody.mass += 0.0005f;
			goForward();
		}

		//Get the behavior of the mouse click
		if(Input.GetMouseButton(0)){
			spray();
			accChange += updateHeadVector().normalized;
		}else if(Input.GetMouseButtonDown(1) && isHooked == false){
			hook();
		}else if(Input.GetMouseButtonDown(1) && isHooked){
			hookAbandon();
		}
	
	}

	void updateSize (){
		this.transform.localScale = Mathf.Sqrt(this.rigidbody.mass) * new Vector3 (1, 1, 1);
	}
	
	void spray() {
		print ("begin to spray");
		float angle = updateHeadPosition ();
		print (angle);
		GetComponent<BallMovement> ().accelerateAt (angle, 4);
		GetComponent<Rigidbody> ().AddForce (updateHeadVector() * -4f);
		if(GetComponent<Rigidbody>().mass <= 0.02f){
			Destroy(this);
		}
		GetComponent<Rigidbody> ().mass -= 0.002f;
	}

	void hook(){
		print ("begin to hook");
		float angle = updateHeadPosition ();
		GetComponent<BallMovement> ().hookOut (angle);
		isHooked = true;
	}

	Vector3 updateHeadVector(){

		Vector3 direction =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		direction.z = 0;
		
		Vector3 pos = transform.position;
		pos.z = 0;
		
		Vector3 final = direction - pos;

		return final;
	}

	void goForward(){
		//print ("combine together");
		if(attachedMonster){
			Vector3 attachpoint = monsterQua * attachedMonster.GetComponent<Monster> ().transform.forward;
			this.rigidbody.AddForce((attachedMonster.transform.position + attachpoint - transform.position) * 2f);
			attachedMonster.rigidbody.AddForceAtPosition((-attachedMonster.transform.position - attachpoint + transform.position) * 2,monsterQua * attachedMonster.GetComponent<Monster>().transform.forward);
		}
	}
	public void collideOnCharacter(GameObject attachObject){
		if(isAttached && attachObject == attachedMonster){
			isAttached = false;
			//GetComponent<BallMovement>.destroyChain();
		}
	}
	
	
	public void hookAttach(GameObject attachedMonsterObj, Quaternion monsterQuaObj){
		
		isAttached = true;
		isHooked = false;
		
		this.attachedMonster = attachedMonsterObj;
		this.monsterQua = monsterQuaObj;
	}

	public void hookAbandon(){
		isHooked = false;
		isAttached = false;
		print ("abandoned");
		GetComponent<BallMovement>().hookAbandon();

	}


	public float updateHeadPosition(){

		Vector3 direction =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		direction.z = 0;
		Vector3 pos = transform.position;
		pos.z = 0;

		Vector3 final = direction - pos;

		if(final.y > 0){
			return Mathf.Acos(final.x / final.magnitude);
		}else{
			return 2 * Mathf.PI - Mathf.Acos(final.x / final.magnitude);
		}
	}
}
