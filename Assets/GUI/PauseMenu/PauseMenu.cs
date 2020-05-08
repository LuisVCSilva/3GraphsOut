using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	int optionPauseMenu = -1;
	public string[] pauseMenuOptions;
	Options options;
	
	GameManager gameManager;
	void Start () {
	gameManager = GetComponent<GameManager>();
	options = gameManager.getOptions();
	}

	void OnGUI () {
	Rect dimPauseGUI = new Rect(0,0,Screen.width/2,Screen.height);
	GUI.BeginGroup(dimPauseGUI);
	GUI.Box(new Rect(0,0,dimPauseGUI.width,dimPauseGUI.height),"Game Paused");
	Vector2 gridPosition = new Vector2(5,5);
	optionPauseMenu = GUI.SelectionGrid(new Rect(gridPosition.x,gridPosition.y+Screen.height/8,dimPauseGUI.width-gridPosition.x*2,dimPauseGUI.height-Screen.height/8-gridPosition.y*2),optionPauseMenu, pauseMenuOptions, 1);
	GUI.EndGroup();

	switch(optionPauseMenu) {
	case 0://Keep Playing
	gameManager.gameState = GameManager.GameState.PLAYING;
	options.enabled = false;
	gameManager.setIsPaused(false);
	break;
	case 1://Load/Unload Options
	options.enabled = !options.enabled;
	break;
	case 2://Give tip
	options.enabled = false;
	break;
	case 3://Main Menu
	gameManager.LoadMainMenu();
	break;
	}
	optionPauseMenu = -1;
	}
}
