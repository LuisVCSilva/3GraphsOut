using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Password : MonoBehaviour {

	GameManager gameManager;
	public string[] passwords;
	int isOk = -2;

	string result = "";
	string password = "";

	Vector2 initialPosition;
	Vector2 finalPosition;
	Vector2 currentPosition;

	void Awake () {
	initialPosition = new Vector2(-Screen.width,0);
	finalPosition = new Vector2(Screen.width/2,0);
	currentPosition = initialPosition;
	gameManager = GetComponent<GameManager>();
	}

	void Start () {
	passwords = gameManager.levelSequence;
	}

	void Update () {
	}

	string fileCache = "";
	void OnGUI () {
	GUI.skin = GetComponent<GameManager>().skin;
	currentPosition = Vector2.Lerp(currentPosition,finalPosition,0.3f);
	GUI.BeginGroup(new Rect(currentPosition.x,currentPosition.y,Screen.width/2,Screen.height));
	Vector2 gridPosition = new Vector2(5,5);
	float width = Screen.width/2;
	float height = (Screen.height-Screen.height/8-gridPosition.y*2)/2;

	GUI.Box(new Rect(0,0,width,Screen.height),"Password");
		GUI.BeginGroup(new Rect(5,(Screen.height/8+5)+height/2.0f,width-10,height*2));
		GUI.Box(new Rect(0,0,width-10,(height-gridPosition.y)/2),"");
		password = GUI.TextField(new Rect(gridPosition.x,gridPosition.y,width*3/4,(height-gridPosition.y)/2-10),password);
		if(GUI.Button(new Rect(width*3/4+10,gridPosition.y,width-(width*3/4)-25,(height-gridPosition.y)/2-10),"OK"))
		{
			for(int i=0;i<passwords.Length;i++)
			{
				if(passwords[i]==password)
				{
				isOk = i;
				}
			}
		}
		if(isOk>=0)
		{
		gameManager.LoadLevel(isOk);
		}
		else
		{
		GUI.BeginGroup(new Rect(0,((height-gridPosition.y)/2-10)+15,width-10,((height-gridPosition.y)/2)));
		GUI.Box(new Rect(0,0,width-10,((height-gridPosition.y)/2)),"");
		string stringMsg = (gameManager.level[gameManager.level.Length-1]==null ? "Still loading levels, wait a little..." : (password=="" ? "Enter the password of the level" : "Incorrect password"));
		GUI.Label(new Rect(5,((height-gridPosition.y)/4)-15,width-10,30),stringMsg);
		GUI.EndGroup();

		}
		/*if(isOk!=-2)
		{
		GUI.BeginGroup(new Rect(0,((height-gridPosition.y)/2-10)+15,width-10,((height-gridPosition.y)/2)));
		GUI.Box(new Rect(0,0,width-10,((height-gridPosition.y)/2)),"");
		string stringMsg = (result==null || result.Contains("404 Not Found") ? "Incorrect password" : (result.Contains("Couldn't resolve host") ? "No internet connection" : (result.Contains("vertices") ? "Loading Level" : "Requesting Level")));
		GUI.Label(new Rect(5,((height-gridPosition.y)/4)-15,width-10,30),stringMsg);
			if(isOk>=0)
			{
				gameManager.LoadLevel(isOk);
			}
			else if(stringMsg=="Loading Level")
			{
				Debug.Log(password);
				gameManager.ParseLevel(password,result);
				stringMsg = "";
				isOk = -2;
			}
		GUI.EndGroup();
		}*/
		GUI.EndGroup();
		GUI.EndGroup();
	}

	public bool Reset () {
	isOk = -2;
	password = "";
	result  = "";
	currentPosition = Vector2.Lerp(currentPosition,initialPosition,0.3f);
	return currentPosition.ToString()==initialPosition.ToString();
	}
}
