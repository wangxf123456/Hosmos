using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {

	public float     mapScale;
	//public float dampTime = 0.05f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public Transform monster;
	public Transform Explodemonster;
	private bool Lose = false;
	private bool Win = false;
	private bool Empty = false;

	// Use this for initialization
	void Start () {

		Lose = false;
		Win = false;
		Empty = false;
		
		for(int i = 0 ; i < 800; i++){
			float x = Random.Range(-95,95);
			float y = Random.Range(-95,95);
			float r = Random.Range(0.01f,4.5f);
			if(x > -25 + r && x < 5 + r && y > -9 + r && y < 10 + r){
				i++;
				continue;
			}
			Transform temp = (Transform)Instantiate(monster, new Vector3(x,y,0), Quaternion.identity);
			temp.GetComponent<Monster>().radius = r;
			temp.collider.isTrigger = false;
			temp.rigidbody.WakeUp();
			
			Vector3 direction = temp.transform.position;
			float Vmag = Random.Range(0.01f,0.06f);
			Vector3	velocity = new Vector3(-direction.y, direction.x, 0);
			print ("velocity:" + velocity);
			temp.rigidbody.velocity = velocity.normalized * Vmag;
		}
		
		for(int i = 0 ; i < 20; i++){
			float x = Random.Range(-30,30);
			float y = Random.Range(-30,30);
			float r = Random.Range(0.01f,4.5f);
			if(x > -18 + r && x < 0 + r && y > -5 + r && y < 8 + r){
				i++;
				continue;
			}
			Transform temp = (Transform)Instantiate(monster, new Vector3(x,y,0), Quaternion.identity);
			temp.GetComponent<Monster>().radius = r;
			temp.collider.isTrigger = false;
			temp.rigidbody.WakeUp();
			
			Vector3 direction = temp.transform.position;
			float Vmag = Random.Range(0.1f,0.12f);
			Vector3	velocity = new Vector3(-direction.y, direction.x, 0);
			print ("velocity:" + velocity);
			temp.rigidbody.velocity = velocity.normalized * Vmag;
		}
		
		for(int i = 0 ; i < 50; i++){
			float x = Random.Range(-95,95);
			float y = Random.Range(-95,95);
			float r = Random.Range(0.01f,2);
			if(x > -14 + r && x < -2 + r && y > -1 + r && y < 5 + r){
				i++;
				continue;
			}
			Transform temp = (Transform)Instantiate(Explodemonster, new Vector3(x,y,0), Quaternion.identity);
			temp.GetComponent<Monster>().radius = r;
			temp.collider.isTrigger = false;
			temp.rigidbody.WakeUp();
			
			Vector3 direction = temp.transform.position;
			float Vmag = Random.Range(0.1f,0.2f);
			Vector3	velocity = new Vector3(-direction.y, direction.x, 0);
			print ("velocity:" + velocity);
			temp.rigidbody.velocity = velocity.normalized * Vmag;
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if (GameObject.Find("character") != null)
			GameObject.Find ("Counter").GetComponent<GUIText> ().text = "Mass = " + GameObject.Find ("character").rigidbody.mass + " Unit";

		if (GameObject.Find("character") == null)
		{
			Lose = true;
			print ("lose");
		}
    	else if (GameObject.Find("character").rigidbody.mass <= 0.1f)
	    {
		    Empty = true;
		}
		else if (GameObject.Find("character").rigidbody.mass >= 350f)
		{
			Win = true;
			print ("win");
		}

		if ((Camera.main.orthographicSize - 0.2f > 1.2f) && (Input.GetAxis("Mouse ScrollWheel") < 0)) // back
		{
			Camera.main.orthographicSize = Camera.main.orthographicSize-0.2f;
			
		}

		if ((GameObject.Find("character") != null) && (Camera.main.orthographicSize + 0.2f < 10 * GameObject.Find("character").transform.localScale.x) && (Input.GetAxis("Mouse ScrollWheel") > 0)) // forward
		{
			Camera.main.orthographicSize = Camera.main.orthographicSize+0.2f;
		}

		if ((Camera.main.orthographicSize + 0.2f < 40f) && (Input.GetAxis("Mouse ScrollWheel") > 0)) // forward
		{
			Camera.main.orthographicSize = Camera.main.orthographicSize+0.2f;
		}

		/*Vector3 tempPos = transform.position;
		float vertExtent = Camera.main.camera.orthographicSize;   
		float horzExtent = vertExtent * Screen.width / Screen.height;


		if ((transform.position.x + horzExtent >= mapScale / 2)
		    || (transform.position.x - horzExtent <= -mapScale / 2)
		    || (transform.position.y + vertExtent >= mapScale / 2)
		    || (transform.position.y - vertExtent <= -mapScale / 2))
		{

			if (transform.position.x + horzExtent >= mapScale / 2)
			{
				tempPos.x = mapScale / 2 - horzExtent + 1;
			}
			if (transform.position.x - horzExtent <= -mapScale / 2)
			{
				tempPos.x = -mapScale / 2 + horzExtent - 1;
			}
			if (transform.position.y + vertExtent >= mapScale / 2)
			{
				tempPos.y = mapScale / 2 - vertExtent + 1;
			}
			if (transform.position.y - vertExtent <= -mapScale / 2)
			{
				tempPos.y = -mapScale / 2 + vertExtent - 1;
			}
			Vector3 point = camera.WorldToViewportPoint(tempPos);
			Vector3 delta = tempPos - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, Time.deltaTime);
			return;
		}

		else */
		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, Time.deltaTime);
		}
	}

	void OnGUI()
	{
		GUI.contentColor = Color.white;

		if(Empty)
		{
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/2-75,200,150),"Too Weak To Hunt");
			
			if(GUI.Button(new Rect(Screen.width/2 - 50,Screen.height/2 - 20,100,50),"Back To Menu"))
			{
				Application.LoadLevel("_Scene_0");
				Empty=false;
			}
			return;
		}

		if(Lose)
		{
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/2-75,200,150),"You Are Hunted");
			
			if(GUI.Button(new Rect(Screen.width/2 - 50,Screen.height/2 - 20,100,50),"Back To Menu"))
			{
				Lose=false;
				Application.LoadLevel("_Scene_0");
			}
		}
		
		if(Win)
		{
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/2-75,200,150),"You Are Great Hunter");
			
			if(GUI.Button(new Rect(Screen.width/2 - 50,Screen.height/2 - 20,100,50),"Back To Menu"))
			{
				Win=false;
				Application.LoadLevel("_Scene_0");
			}
		}
	}
}
