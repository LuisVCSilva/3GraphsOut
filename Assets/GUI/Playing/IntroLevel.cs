using UnityEngine;
using System.Collections;

public class IntroLevel : MonoBehaviour {

	Vector2 initialPosition;
	Vector2 finalPosition;
	Vector2 currentPosition;
	GameManager gameManager;

	void Awake () {
	gameManager = GetComponent<GameManager>();
	initialPosition = new Vector2(-Screen.width*2,Screen.height/2-((Screen.height/3)/2));
	finalPosition = new Vector2(Screen.width/2-((Screen.width/3)/2),Screen.height/2-((Screen.height/3)/2));
	currentPosition = initialPosition;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	//int choice = -1;
	void OnGUI () {
		GUI.skin = GetComponent<GameManager>().skin;
		Vector2 dim = new Vector2(Screen.width/3,Screen.height/3);

		currentPosition = Vector2.Lerp(currentPosition,finalPosition,0.3f);


		GUI.BeginGroup(new Rect(currentPosition,dim),"");
		GUI.Box(new Rect(0,0,dim.x,dim.y),"");
		GUI.Label(new Rect(5,5,dim.x-10,dim.y/2-10),"Level " + (gameManager.getCurrentLevel()+1).ToString() + "/" + gameManager.getCountLevels() + "\nPassword: " + gameManager.levelSequence[gameManager.getCurrentLevel()]);
		Vector2 dimChoiceButton = new Vector2(dim.x-10,dim.y/2-10);
		if(GUI.Button(new Rect(5, dim.y-dimChoiceButton.y-5, dimChoiceButton.x, dimChoiceButton.y),"Play"))
		{
		//gameManager.LoadGraph();
		this.enabled = false;
		}

		GUI.EndGroup();

	}

	public bool Reset () {
		currentPosition = Vector2.Lerp(currentPosition,initialPosition,0.3f);
		return currentPosition.ToString()==initialPosition.ToString();
	}
}
