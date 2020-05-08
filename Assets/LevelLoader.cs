using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {

	GameManager gameManager;

	void Awake () {
	gameManager = GetComponent<GameManager>();	
	}

	public string[] targets;
	public string[] levels;
	public int k = 0;

	public void Start () {
	levels = new string[targets.Length];
	Request();
	}
	
	public void Request () {
	GetComponent<FileManager>().Retrieve(targets[k]+".txt",FileManager.RetrievalMode.AUTO);
	}
	
	public void Update () {
	if(GetComponent<FileManager>().getResult()!=null && GetComponent<FileManager>().getResult().Contains("level:\n"+(k+2).ToString()+"\n") && k<targets.Length)
	{
	levels[k] = GetComponent<FileManager>().getResult();
	gameManager.level[k] = levels[k];
	GetComponent<FileManager>().WriteFile(targets[k]+".txt",levels[k]);
	k += k+1<targets.Length ? 1 : 0;
	Request();
	}
	else if(k+1==targets.Length)
	{
	}
	else if(k<targets.Length)
	{
	Request();
	}
	}
}
