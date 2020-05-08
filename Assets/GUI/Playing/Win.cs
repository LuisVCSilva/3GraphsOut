using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour {

	GameManager gameManager;

	Vector2 initialPosition;
	Vector2 finalPosition;
	Vector2 currentPosition;
	//string finalTime = "";

	/*public void setFinalTime (string s) {
		finalTime = s;
	}*/

	void Awake () {
	gameManager = GetComponent<GameManager>();

	initialPosition = new Vector2(-Screen.width*2,Screen.height/2-((Screen.height/3)/2));
	finalPosition = new Vector2(Screen.width/2-((Screen.width/3)/2),Screen.height/2-((Screen.height/3)/2));
	currentPosition = initialPosition;
	}

	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	public int choice = -1;
	void OnGUI () {
	GUI.skin = GetComponent<GameManager>().skin;
	Vector2 dim = new Vector2(Screen.width/2.5f,Screen.height/2.5f);

	currentPosition = Vector2.Lerp(currentPosition,finalPosition,0.3f);
	GUI.BeginGroup(new Rect(currentPosition,dim),"");
	GUI.Box(new Rect(0,0,dim.x,dim.y),"");

	GUI.Label(new Rect(5,5,dim.x-10,dim.y/2-10),"Steps: " + gameManager.getStepsMade() + "\nTime: " + gameManager.getTimer() + (gameManager.getCountLevels()-1==gameManager.getCurrentLevel() ? "\nYou played all levels\n\nCongrats!!!" : ""));
	//GUI.SelectionGrid(new Rect(5,dim.y/4,dim.x-10,dim.y/4), -1, new Texture2D [] {gameManager.starIconOn,gameManager.starIconOff,gameManager.starIconOff},3);
	Vector2 dimChoiceButton = new Vector2(dim.x-10,dim.y/2-10);
	choice = GUI.SelectionGrid(new Rect(5, dim.y-dimChoiceButton.y-5, dimChoiceButton.x, dimChoiceButton.y), choice, gameManager.getCountLevels()-1==gameManager.getCurrentLevel() ? new string [] {"Main Menu"} : new string[] {"Main\nMenu","Next\nLevel"}, gameManager.getCountLevels()-1==gameManager.getCurrentLevel() ? 1 : 2);
	GUI.EndGroup();
	switch(choice)
	{
		case 0:
		gameManager.LoadMainMenu();
		break;

		case 1:
		gameManager.LoadLevel(gameManager.getCurrentLevel()+1);
		break;

		default:
		break;
	}
	choice = -1;
	}

	public bool Reset () {
		currentPosition = Vector2.Lerp(currentPosition,initialPosition,0.3f);
		return currentPosition.ToString()==initialPosition.ToString();
	}
}
