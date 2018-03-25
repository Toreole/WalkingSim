using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVersionNr : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }

	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {

		var LowerRightStyle = GUI.skin.GetStyle("Label");
    	LowerRightStyle.alignment = TextAnchor.LowerRight;

		GUI.Label(new Rect (Screen.width - 55, Screen.height - 20, 50, 20), "v" + Application.version);
	}
}
