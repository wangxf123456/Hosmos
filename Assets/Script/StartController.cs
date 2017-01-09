using UnityEngine;
using System.Collections;

public class StartController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {

		//guiStyle.fontSize = 20;
		//GUI.color = Color.red;
		GUI.contentColor = Color.white;

		if (GUI.Button(new Rect(Screen.width/2 - 75, Screen.height/2 + 20, 150, 60), "Start Hunt"))
			Application.LoadLevel("_Scene_1");
		
		if (GUI.Button(new Rect(Screen.width/2 - 75, Screen.height/2 + 90, 150, 60), "Help"))
			Application.LoadLevel("_Scene_Help");
		
	}
}
