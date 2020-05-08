using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	int optionsMainMenu = -1;

	public int getOptionsMainMenu () {
	return optionsMainMenu;
	}

	public GameObject vertex;
	public string[] mainMenuOptions;
	Credits credits;
	Password password;
	GameManager gameManager;
	//Highscores highscores;

	void Start () {
	gameManager = GetComponent<GameManager>();
	credits = gameManager.getCredits();
	password = gameManager.getPassword();
	//highscores = gameManager.getHighscores();
	}

	void OnGUI () {
	float progress = gameManager.level.Length==0 ? 0.0f : (GetComponent<LevelLoader>().k/gameManager.level.Length)*100.0f;
	GUI.skin = GetComponent<GameManager>().skin;
	GUI.enabled = gameManager.levelSequence.Length>0 && gameManager.levelSequence[0]!="error";
	//GUI.Box(new Rect(0,0,Screen.width,Screen.height),"");
	Rect dimPauseGUI = new Rect(0,0,Screen.width/2,Screen.height);
	GUI.BeginGroup(dimPauseGUI);
	GUI.Box(new Rect(0,0,dimPauseGUI.width,dimPauseGUI.height),"Ko");
	Vector2 gridPosition = new Vector2(5,5);
	optionsMainMenu = GUI.SelectionGrid(new Rect(gridPosition.x,gridPosition.y+Screen.height/8,dimPauseGUI.width-gridPosition.x*2,dimPauseGUI.height-Screen.height/8-gridPosition.y*2),optionsMainMenu, mainMenuOptions, 1);
	GUI.EndGroup();
	switch(optionsMainMenu) {
	case 0://Play
	password.Reset();
	gameManager.gameState = GameManager.GameState.PLAYING;
	credits.enabled = false;
	password.enabled = false;
	gameManager.LoadLevel(0);
	//StartCoroutine(gameManager.LoadLevel(gameManager.getCurrentLevel()+1));
	break;

	case 1://Password
	password.Reset();
	password.enabled = !password.enabled;
	credits.enabled = false;
	break;

	/*case 2://Highscores
	highscores.Reset();
	highscores.enabled = !highscores.enabled;
	highscores.enabled = false;
	break;*/

	case 2://Credits
	credits.Reset();
	credits.enabled = !credits.enabled;
	password.enabled = false;
	break;

	case 3://Quit
	Application.Quit();
	break;
	}
	optionsMainMenu = -1;
	}
}
