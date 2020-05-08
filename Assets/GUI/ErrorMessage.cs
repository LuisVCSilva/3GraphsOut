using UnityEngine;
using System.Collections;

public class ErrorMessage : MonoBehaviour {

	Vector2 initialPosition;
	Vector2 finalPosition;
	Vector2 currentPosition;
	GameManager gameManager;
	bool isOk = false;

	void Awake () {
	gameManager = GetComponent<GameManager>();
	initialPosition = new Vector2(-Screen.width*2,Screen.height/2-((Screen.height/3)/2));
	finalPosition = new Vector2(Screen.width/2-((Screen.width/3)/2),Screen.height/2-((Screen.height/3)/2));
	currentPosition = initialPosition;
	}

	// Use this for initialization
	void Start () {
	StartCoroutine(Go());
	}

	// Update is called once per frame
	void Update () {

	}

	IEnumerator Go () {
	yield return new WaitForSeconds(3);
	isOk = true;
	}

	//int choice = -1;
	void OnGUI () {
	GUI.skin = GetComponent<GameManager>().skin;
	if(isOk)
	{
		Vector2 dim = new Vector2(Screen.width/3,Screen.height/3);
		currentPosition = Vector2.Lerp(currentPosition,finalPosition,0.3f);
		GUI.BeginGroup(new Rect(currentPosition,dim),"");
		GUI.Box(new Rect(0,0,dim.x,dim.y),"");
		GUI.Label(new Rect(5,5,dim.x-10,dim.y/2-10),"It seems that you don't have an Internet Connection\nTry again later...");
		Vector2 dimChoiceButton = new Vector2(dim.x-10,dim.y/2-10);
		if(GUI.Button(new Rect(5, dim.y-dimChoiceButton.y-5, dimChoiceButton.x, dimChoiceButton.y),"Quit"))
		{
		Application.Quit();
		}
		GUI.EndGroup();
	}
	else
	{
		Vector2 dim = new Vector2(Screen.width/3,Screen.height/3);

		currentPosition = Vector2.Lerp(currentPosition,finalPosition,0.3f);


		GUI.BeginGroup(new Rect(currentPosition,dim),"");
		GUI.Box(new Rect(0,0,dim.x,dim.y),"");
		GUI.Label(new Rect(5,5,dim.x-10,dim.y/2-10),"Loading some stuff\nHang on a little...");
		Vector2 dimChoiceButton = new Vector2(dim.x-10,dim.y/2-10);
		GUI.EndGroup();
	
	}
	}

	public bool Reset () {
		currentPosition = Vector2.Lerp(currentPosition,initialPosition,0.3f);
		return currentPosition.ToString()==initialPosition.ToString();
	}
}
