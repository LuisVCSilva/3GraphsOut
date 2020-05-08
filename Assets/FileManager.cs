using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class FileManager : MonoBehaviour {

	string result = null;
	string filename = null;
	bool flagRun = false;


	public enum RetrievalMode {AUTO,ALWAYS_ONLINE, ALWAYS_OFFLINE};

	public string getResult () {
		return result;
	}

	public bool isFinished () {
		return !flagRun;
	}

        public void Reset () {
		result = null;
		filename = null;
		flagRun = false;
	}

	void Update () {
		result = result==null ? result : result.Trim();
		if(flagRun==true)
		{
		StartCoroutine(Download(filename));
		}
		flagRun = result==null;
	}

	public void Retrieve (string filename,RetrievalMode flag) {
		if(flag==RetrievalMode.ALWAYS_ONLINE)
		{
			this.filename = filename;
		}
		if(flag==RetrievalMode.ALWAYS_OFFLINE)
		{

			result = ReadFile(filename);
		}
		if(flag==RetrievalMode.AUTO)
		{
			Retrieve(filename,RetrievalMode.ALWAYS_OFFLINE);
			if(result==null)
			{
			Retrieve(filename,RetrievalMode.ALWAYS_ONLINE);		
			}
		}
	}

	string ReadFile (string filename)
	{
			string path = filename;
			string s = null;
			try{
			StreamReader reader = new StreamReader(path);
			s = reader.ReadToEnd();
			reader.Close();
			}
			catch(FileNotFoundException ex)
			{
			s = null;
			}
			return s;
	}

	public void WriteFile (string filename,string content) {
		//flagRun = false;
		if (File.Exists(filename))
		{
		return;
		}
		var sr = File.CreateText(filename);
		sr.WriteLine (content);
		sr.Close();
	}

	IEnumerator Download (string file) {
	string url = "http://luisvcsilva.000webhostapp.com/ko/"+file;
	WWW www = new WWW(url);
	yield return www;
	if (www.error==null)
	{
	result = www.data;
	}
	else
	{
	result = "error";
	}
	}

}
