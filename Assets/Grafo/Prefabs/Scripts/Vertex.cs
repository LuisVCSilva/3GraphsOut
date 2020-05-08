using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour {

	List<GameObject> neighbours = new List<GameObject>();
	public List<GameObject> getNeighbours () {
			return neighbours;
	}

	public void AddNeighbour (GameObject new_neighbour) {
		neighbours.Add(new_neighbour);
	}

	Color new_color;
	public GameObject edgePrefab;
	GameManager gameManager;
	public bool enableEvents = true;

	bool isClicked = false;
	public bool getIsClicked () {
	return isClicked;
	}

	void Awake () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		enableEvents = true;
	}

	void Start () {
	new_color = gameManager._onMouseExit;
	if(neighbours.Count>0)
	{
		foreach (GameObject neighbour in neighbours)
		{
			if(neighbour!=null && GameObject.Find(neighbour.name+"x"+name)==null)
			{
			GameObject edge = GameObject.Instantiate(edgePrefab,transform.position,Quaternion.identity) as GameObject;
			edge.transform.parent = gameObject.transform;
			edge.name = name+"x"+neighbour.name;
			edge.GetComponent<LineRenderer>().SetPosition(0, transform.position);
			edge.GetComponent<LineRenderer>().SetPosition(1, neighbour.transform.position);
			}
		}
	}
	Switch(false);
	}

	void Update () {
	GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color,new_color,0.3f);
  GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity,isClicked ? 3.0f : 0.0f,0.3f);
	foreach (GameObject neighbour in neighbours)
	{
		if(neighbour!=null)
		{
		GameObject edge = GameObject.Find(neighbour.name+"x"+name);
			if(edge!=null)
			{
			edge.GetComponent<LineRenderer>().SetPosition(0, transform.position);
			edge.GetComponent<LineRenderer>().SetPosition(1, neighbour.transform.position);
			Color new_line_color = isClicked || neighbour.GetComponent<Vertex>().isClicked ? gameManager._onMouseDown : gameManager._onMouseExit;
			edge.GetComponent<Edge>().finalColor = new_line_color;
			}
		}
	}
	}

	void OnMouseEnter () {
	if(enableEvents==true)
	{
	new_color = isClicked ? gameManager._onMouseDown : gameManager._onMouseEnter;
	}
	}

	void OnMouseDown () {
	if(enableEvents==true)
	{
	GetComponent<AudioSource>().PlayOneShot(gameManager.getCurrentBeep());
	gameManager.GetComponent<GameManager>().addStep();
	Switch(true);
	}
	}

	void OnMouseExit () {
	if(enableEvents==true)
	{
	new_color = isClicked ? gameManager._onMouseDown : gameManager._onMouseExit;
	}
	}

	public void Switch (bool isCascade) {
	isClicked = !isClicked;
	new_color = (isClicked ? gameManager._onMouseDown : gameManager._onMouseExit);
	if(isCascade) {
	   foreach (GameObject neighbour in neighbours)
	   {
	   neighbour.GetComponent<Vertex>().Switch(false);
	   }
	}
	}
}
