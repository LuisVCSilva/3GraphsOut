using UnityEngine;
using System.Collections;

public class Edge : MonoBehaviour {

	public Color finalColor;
	public Color currentColor;

	public Vector3 p1;
	public Vector3 p2;
	// Use this for initialization
	void Start () {
		currentColor = GameObject.Find("GameManager").GetComponent<GameManager>()._onMouseDown;
	}

	// Update is called once per frame
	void Update () {
		currentColor = Color.Lerp(currentColor,finalColor,Time.smoothDeltaTime*5);
		GetComponent<LineRenderer>().SetColors(currentColor,currentColor);
	}
}
