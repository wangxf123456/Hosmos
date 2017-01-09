using UnityEngine;
using System.Collections;

public class HelpController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {

		GUI.contentColor = Color.white;
		
		if (GUI.Button(new Rect(Screen.width/2 - 250, Screen.height - 65, 150, 40), "Start Hunt"))
			Application.LoadLevel("_Scene_1");
		
		if (GUI.Button(new Rect(Screen.width/2 + 50, Screen.height- 65, 150, 40), "Back to Menu"))
			Application.LoadLevel("_Scene_0");
		
	}
}
