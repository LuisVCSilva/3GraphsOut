using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

	GameManager gameManager;
	void Awake () {
	gameManager = GetComponent<GameManager>();
	}
	
	void OnGUI () {
	Rect dimPauseGUI = new Rect(0,0,Screen.width/2,Screen.height);
	Vector2 gridPosition = new Vector2(5,5);
	if(gameManager.getIsPaused())
	{
	Rect dimensions = new Rect(Screen.width/2,0,Screen.width/2,Screen.height);
	GUI.BeginGroup(dimensions);
	GUI.Box(new Rect(0,0,dimensions.width,Screen.height),"Options");
	GUI.Box(new Rect(gridPosition.x,gridPosition.y+Screen.height/8,dimPauseGUI.width-gridPosition.x*2,dimPauseGUI.height-Screen.height/8-gridPosition.y*2),"");
	GUI.EndGroup();
	}
	}
}
