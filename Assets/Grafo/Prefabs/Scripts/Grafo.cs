using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Grafo : MonoBehaviour {
	public bool isMainMenu = false;
	public bool debugRawData = false;
	public GameObject[] vertices;
	//GameObject[] edges;
 	Vector3[] originalPositionVertices;
	GameManager gameManager;
	GameObject centroid;


	public Dictionary<string, Vector3> verticesList = null;
	public string[] adjacencyList;



	public GameObject getCentroid () {
		return centroid;
	}

	float area = 1.0f;

	public float getArea () {
		return area;
	}

	public void Start () {
	gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	if(transform.childCount==0)
	{
		vertices = new GameObject[verticesList.Count];
		int k = 0;
		foreach(KeyValuePair<string, Vector3> entry in verticesList)
		{
		vertices[k] = GameObject.Instantiate(gameManager.vertexPrefab,(Vector3) entry.Value,Quaternion.identity) as GameObject;
		vertices[k].name = (string) entry.Key;
		vertices[k].tag = "Vertex";
		vertices[k].transform.parent = transform;
		k++;
		}
		k = 0;
	}
	else
	{
		vertices = GameObject.FindGameObjectsWithTag("Vertex").ToList().OrderBy(go=>go.name).ToList().ToArray();
	}

	if(debugRawData==true)
	{
		string s = "";
		s += "vertices: \n" + vertices.Select(i => i.name + " -> " + i.transform.position.ToString()+"\n").Aggregate((i,j) => i + "" + j);
		if(adjacencyList.Length!=0)
		{
		s += "adjacency: \n" + adjacencyList.Select(i => i.ToString()+"\n").Aggregate((i, j) => i + "" + j);
		}
	Debug.Log(s);
	}

	originalPositionVertices = new Vector3[vertices.Length];
	centroid = GameObject.CreatePrimitive(PrimitiveType.Sphere);
	centroid.name = "centroid";
	centroid.GetComponent<Renderer>().enabled = false;
	centroid.GetComponent<SphereCollider>().enabled = false;

	Vector2 radius = new Vector2(15,15);
	Vector3 centroid_position = Vector3.zero;
	Vector3[] points = new Vector3[vertices.Length];



	for(int i=0;i<vertices.Length;i++) {
	int flag = isVertexinAdjacencyList(vertices[i].name);

	if(flag>=0)
	{
		foreach (string vertex in RHS(adjacencyList)[flag].Split(","[0])) {
			vertices[i].GetComponent<Vertex>().AddNeighbour(FindVertexByName(vertex));
		}
	}
	}


	for(int i=0;i<vertices.Length;i++) {
	originalPositionVertices[i] = vertices[i].transform.position;

	float _i = (i * 1.0f) / vertices.Length;
	float angle = _i * Mathf.PI * 2;

	points[i] = originalPositionVertices[i];
	vertices[i].transform.position = new Vector3(Mathf.Sin(angle) * radius.x,Mathf.Cos(angle) * radius.y,0.0f);

	if(vertices[i].GetComponent<Vertex>()!=null)
	{
	if(vertices[i].GetComponent<Vertex>().getNeighbours()!=null)
	{
		foreach(GameObject neighbour in vertices[i].GetComponent<Vertex>().getNeighbours())
		{
				if(neighbour!=null && neighbour.GetComponent<Vertex>().getNeighbours()!=null)
				{
					if(!neighbour.GetComponent<Vertex>().getNeighbours().Contains(vertices[i]))
					{
						neighbour.GetComponent<Vertex>().AddNeighbour(vertices[i]);
						if(!vertices[i].GetComponent<Vertex>().getNeighbours().Contains(neighbour))
						{
							vertices[i].GetComponent<Vertex>().AddNeighbour(neighbour);
						}
					}
				}
		}
	}
	}
		//Debug.Log(originalPositionVertices[i]);
		centroid_position += originalPositionVertices[i];
	}
	centroid_position /= vertices.Length;
	centroid.transform.position = centroid_position;
	List<Vector3> convexHull = ConvexHull.ComputeConvexHull(points);
	area = ConvexHull.PolygonArea(convexHull);
	}

	void Update () {
	if(centroid!=null && gameManager.particles!=null)
	{
	gameManager.particles.transform.position = centroid.transform.position;
	gameManager.particles.transform.rotation = centroid.transform.rotation;
	}
	if(isMainMenu==true)
	{
		Pulse();
	}
	else
	{
	for(int i=0;i<vertices.Length;i++)
	{
		if(!gameManager.introLevel.enabled)
		{
			vertices[i].transform.position = Vector3.Lerp(vertices[i].transform.position,originalPositionVertices[i],0.3f);
		}
	}
		/*if(isSolved())
		{
			foreach(Transform vertex in transform)
			{
			vertex.gameObject.GetComponent<Vertex>().enableEvents = false;
			}
		GameObject.Destroy(centroid);
		}*/
	}
	}

	void Pulse () {
		Vector2 radius = new Vector2(Mathf.Sin(Time.time)*3.0f,Mathf.Sin(Time.time)*3.0f);
		for(int i=0;i<vertices.Length;i++) {
		float _i = (i * 1.0f) / 10.0f;
		float angle = _i * Mathf.PI * 2+Time.time;
		if(i>=5)
		{
		vertices[i].transform.position = Vector3.Lerp(vertices[i].transform.position,new Vector3(8,3,0)+new Vector3(Mathf.Sin(angle) * radius.x,Mathf.Cos(angle) * radius.y,Mathf.Cos(Time.time)),Time.smoothDeltaTime*100);
		}
		else
		{
		vertices[i].transform.position = Vector3.Lerp(vertices[i].transform.position,originalPositionVertices[i],0.3f);
		}
		}
		centroid.transform.position = new Vector3(Mathf.Sin(Time.time)+3,0.0f,Mathf.Cos(Time.time)+3);
	}

	string[] RHS (string[] adjacencyList) {
		string[] s = new string[adjacencyList.Length];
		for(int i=0;i<adjacencyList.Length;i++)
		{
			s[i] = adjacencyList[i].Substring(adjacencyList[i].IndexOf(" -> ")+4);
		}
		return s;
	}


	int isVertexinAdjacencyList (string vertex_name) {
		int s = -1;
		for(int i=0;i<adjacencyList.Length;i++) {
			int aux = adjacencyList[i].IndexOf(" ->");
			if(aux>=0 && adjacencyList[i].Substring(0,aux)==vertex_name)
			{
				s = i;
				break;
			}
		}
		return s;
	}

	GameObject FindVertexByName (string vertex_name) {
		GameObject s = null;
		foreach(GameObject vertex in vertices) {
			if(vertex.name==vertex_name)
			{
				s = vertex;
				break;
			}
		}
		return s;
	}

	public bool isSolved () {
	bool s = true;
	if(vertices==null)
	{
		return false;
	}
	foreach (GameObject vertex in vertices) {
		 if(vertex==null) {
			return false;
		 }
	   if(vertex.GetComponent<Vertex>().getIsClicked()==true)
	   {
	   s = false;
	   break;
	   }
	}
	return s;
	}
}
