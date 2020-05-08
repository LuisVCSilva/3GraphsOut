using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {

	Vector2 initialPosition;
	Vector2 finalPosition;
	Vector2 currentPosition;
	Vector2 scrollPosition = Vector2.zero;


	void Awake () {
	initialPosition = new Vector2(-Screen.width*2,0);
	finalPosition = new Vector2(Screen.width/2,0);
	currentPosition = initialPosition;
	}

	void OnGUI () {
	GUI.skin = GetComponent<GameManager>().skin;
	currentPosition = Vector2.Lerp(currentPosition,finalPosition,0.3f);
	GUI.BeginGroup(new Rect(currentPosition.x,currentPosition.y,Screen.width/2,Screen.height));

	Vector2 gridPosition = new Vector2(5,5);
	float width = Screen.width/2;
	float height = (Screen.height-Screen.height/8-gridPosition.y*2)/2;

	GUI.Box(new Rect(0,0,width,Screen.height),"About");
		GUI.BeginGroup(new Rect(5,Screen.height/8,width,height+10));
		GUI.Box(new Rect(0,0,width-10,height+5),"How to play");
		GUI.enabled = false;
		GUI.SelectionGrid(new Rect(5,50,(width-20),height/1.25f),-1, new string[] {"Turn off all vertices in each level","Each vertex can be turned on/off when clicked","When a vertex is switched,\ntheir neighbours are also switched"}, 1);
		GUI.enabled = true;
		GUI.EndGroup();



		scrollPosition = GUI.BeginScrollView(new Rect(5,(Screen.height/8+5)+height/1.0f, width-10,height-1), scrollPosition, new Rect(0, 0, width-10,(height-1)*2));
		GUI.Box(new Rect(0,0,width-10,(height-1)*2),"Credits");
		string textCredits = "Game Design & Programming: Luis Vinicius Costa Silva\n\n" +

													"GUI Skin: Unity Extra Skins -- Unity Technologies -- Adapted\n\n" +

													"Audio: Freesound.org\n" +
													"[1] Click Sound -- gunbladez -- freesound.org/people/gunnbladez\n" +
													"[2] Theme Song -- Flick3r -- freesound.org/people/Flick3r\n" +
													"[3] Victory Sound -- woowah -- freesound.org/people/woowah\n\n" +

													"[1,2] -- Licensed under the Attribution License (CC BY 3.0) -- https://creativecommons.org/licenses/by/3.0 \n" +
													"[3]   -- Licensed under the Creative Commons 0 License (CC0)\n\n" +

													"Ko is a game based on the concept of KO-reducible graphs.";
		GUI.Label(new Rect(gridPosition.x,gridPosition.y+40,width-10,(height-1)*2),textCredits);
		GUI.EndScrollView();

	GUI.EndGroup();
	}

	public bool Reset () {
		currentPosition = Vector2.Lerp(currentPosition,initialPosition,0.3f);
		return currentPosition.ToString()==initialPosition.ToString();
	}
}
