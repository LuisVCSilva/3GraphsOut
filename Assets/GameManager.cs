using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoogleMobileAds.Api;


//APP_ID = ca-app-pub-6418661838970309~5884889876
//ID_bloco = ca-app-pub-6418661838970309/1726153012
public class GameManager : MonoBehaviour {


	private BannerView bannerView;
	[SerializeField] private string appID = "";
	[SerializeField] private string bannerID = "";
	[SerializeField] private string regularAD = "";

	string[] adjacencyList;
	Dictionary<string, Vector3> verticesList = new Dictionary<string, Vector3>();
	
	public bool isDebuggingNewLevel = false;
	public string[] levelSequence;

	float time = 0.0f;
	int currentLevel = -1;
	//int score = 0;
	public AudioClip beepOn;
	public AudioClip beepOff;

	public AudioClip getCurrentBeep () {
		return stepsMade%2==0 ? beepOn : beepOff;
	}

	public AudioClip bgMusic;
	public AudioClip victoryMusic;
	public AudioClip gameOverSound;

	public AudioClip okMusic;
	public AudioClip notOkMusic;

	public Color _onMouseEnter = Color.green;
	public Color _onMouseExit  = Color.white;
	public Color _onMouseDown = Color.yellow;

	public GameObject vertexPrefab;

	public int getCurrentLevel () {
		return currentLevel;
	}

	public GameObject mainMenuDeco;
	int countLevels;
	public string[] level;
	public int getCountLevels () {
			return countLevels;
	}

	GameObject grafo;
	public GameObject getGrafo () {
	return grafo;
	}


	public enum GameState {MAIN_MENU, WIN, LOSE, PLAYING, PAUSED, TIP, ENDGAME};
	public GameState gameState = GameState.MAIN_MENU;


	public Texture2D starIconOn;
	public Texture2D starIconOff;

	bool isAudioEnabled = true;
	public Texture2D iconAudioOn;
	public Texture2D iconAudioOff;

	public GameObject particles;

	int stepsMade = 0;
	public int getStepsMade () {
		return stepsMade;
	}

	public bool getIsAudioEnabled () {
		return isAudioEnabled;
	}


	ErrorMessage errorMessage;
	public ErrorMessage getErrorMessage () {
		return errorMessage;
	}

	PauseMenu pauseMenu;
	MainMenu mainMenu;
	public MainMenu getMainMenu () {
	return mainMenu;
	}

	Credits credits;
	public Credits getCredits () {
	return credits;
	}

	Options options;
	public Options getOptions () {
	return options;
	}

	Password password;
	public Password getPassword () {
	return password;
	}

	Highscores highscores;
	public Highscores getHighscores() {
		return highscores;
	}

	bool isPaused = false;
	public bool getIsPaused () {
	return isPaused;
	}

	public void setIsPaused (bool _v) {
	isPaused = _v;
	}


	Win win;
	Lose lose;
	public IntroLevel introLevel;

	public GUISkin skin;
	public void addStep () {
	stepsMade++;
	}


	void Awake () {
	GetComponent<FileManager>().Retrieve("__level_sequence__.txt",FileManager.RetrievalMode.ALWAYS_ONLINE);
	MobileAds.Initialize(appID);
	mainMenu = GetComponent<MainMenu>();
	options = GetComponent<Options>();
	pauseMenu = GetComponent<PauseMenu>();
	credits = GetComponent<Credits>();
	password = GetComponent<Password>();
	highscores = GetComponent<Highscores>();
	errorMessage = GetComponent<ErrorMessage>();
	win = GetComponent<Win>();
	lose = GetComponent<Lose>();
	introLevel = GetComponent<IntroLevel>();
	gameState = isDebuggingNewLevel==true ? GameState.PLAYING : GameState.MAIN_MENU;
	}

	public void OnClickShowBanner () {
		this.RequestBanner();	
	}

	private void RequestBanner () {
		bannerView = new BannerView(bannerID, AdSize.Banner,AdPosition.Bottom);

	        // Called when an ad request has successfully loaded.
	        bannerView.OnAdLoaded += HandleOnAdLoaded;
	        // Called when an ad request failed to load.
	        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
	        // Called when an ad is clicked.
	        bannerView.OnAdOpening += HandleOnAdOpened;
	        // Called when the user returned from the app after an ad click.
	        bannerView.OnAdClosed += HandleOnAdClosed;
	        // Called when the ad click caused the user to leave the application.
	        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		AdRequest request = new AdRequest.Builder().Build();
		//AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice(SystemInfo.deviceUniqueIdentifier).Build();
		bannerView.LoadAd(request);	
	}

	/*GameObject mainMenuPrefab;
	public GameObject getMainMenuPrefab () {
		return mainMenuPrefab;
	}*/
	int kLevelLoaded = 0;
	
	void Start () {
	particles = GameObject.FindWithTag("Particles");
	GetComponent<AudioSource>().clip = bgMusic;
	GetComponent<AudioSource>().Play();
	grafo = GameObject.Instantiate(mainMenuDeco,new Vector3(1,0,0),Quaternion.identity) as GameObject;
	OnClickShowBanner();
	}

	void Update () {
		if(GetComponent<FileManager>().isFinished() && levelSequence.Length==0)
		{
			string data = GetComponent<FileManager>().getResult();
			levelSequence = data.Split("\n"[0]).ToArray();
			countLevels = levelSequence.Length;
			level = new string[countLevels];
			GetComponent<FileManager>().Reset();
			GetComponent<LevelLoader>().targets = levelSequence;
		}
		if(level.Length>0)
		{
		GetComponent<LevelLoader>().enabled = true;
		}

	Time.timeScale = gameState==GameState.PLAYING && !introLevel.enabled ? 1.0f : 1.0f;
	mainMenu.enabled = gameState==GameState.MAIN_MENU && !errorMessage.enabled;
	pauseMenu.enabled = !mainMenu.enabled && isPaused && !errorMessage.enabled;
	Camera.main.GetComponent<AudioListener>().enabled = isAudioEnabled;
	if (gameState!=GameState.MAIN_MENU)
	{
	onPlay();
	}
	else
	{
	Camera.main.GetComponent<SmoothFollow>().buttonValue = Mathf.Lerp(Camera.main.GetComponent<SmoothFollow>().buttonValue,0.0f,Time.smoothDeltaTime);
	Camera.main.transform.position = new Vector3(0,2.5f,-10);
	time = 0.0f;
	}
	}

	void OnGUI () {
	GUI.skin = skin;

	errorMessage.enabled = !(levelSequence.Length>0 && levelSequence[0]!="error");
	//mainMenu.enabled = gameState==GameState.MAIN_MENU && !errorMessage.enabled;
	pauseMenu.enabled = gameState==GameState.PAUSED && !errorMessage.enabled;

	if(errorMessage.enabled)
	{
	}
	else
	{
	if(GUI.Button(new Rect(Screen.width-Screen.width/8,0,Screen.width/8,Screen.height/8),isAudioEnabled ? iconAudioOn : iconAudioOff))//Audio button
	{
	isAudioEnabled = !isAudioEnabled;
	}
	if (gameState!=GameState.MAIN_MENU)
	{
	if(gameState==GameState.WIN)
	{
	Timer();
	win.enabled = true;
	}
	if(gameState==GameState.LOSE)
	{
	Timer();
	lose.enabled = true;
	}
	if(gameState==GameState.PLAYING)
	{
	canPlay++;
		if(canPlay==1)
		{
		GetComponent<AudioSource>().PlayOneShot(victoryMusic);
		}
	Timer();
	password.enabled = false;
	highscores.enabled = false;
	options.enabled = false;
	credits.enabled = false;
	win.enabled = false;
	lose.enabled = false;
	}
	if(gameState==GameState.TIP)
	{
	}
	if(gameState==GameState.ENDGAME)
	{
	}
	}
	else
	{
		if(grafo==null)
		{
		grafo = GameObject.Instantiate(mainMenuDeco,new Vector3(1,0,0),Quaternion.identity) as GameObject;
		}
	introLevel.enabled = false;
	win.enabled = !win.Reset();
	}
	}
	}


	int canPlay = 0;

	void onPlay () {
	if(grafo!=null)
	{
		if(grafo.GetComponent<Grafo>().isSolved())
		{
		gameState = GameState.WIN;
		}
		/*else if(Input.GetKeyDown(KeyCode.Escape))
		{
		isPaused = !isPaused;
		gameState = isPaused ? GameState.PAUSED : gameState;
		}*/
		else
		{
		gameState = isPaused ? GameState.PAUSED : GameState.PLAYING;
		}
	}
	else
	{
	if(currentLevel==-1)
	{
	}
	else
	{
	if(level.Length>currentLevel)
	{
	//grafo = GameObject.Instantiate(level[currentLevel],Vector3.zero,Quaternion.identity) as GameObject;
	ParseLevel(levelSequence[currentLevel],level[currentLevel]);
	}
	}
	}
	}

	/*public void LoadGraph () {
		grafo = GameObject.Instantiate(level[currentLevel],Vector3.down*-5,Quaternion.identity) as GameObject;
	}*/
	string buffer_level = null;

	public void LoadLevel (int nLevel) {
		if(grafo!=null)
		{
		GameObject.Destroy(grafo);
		GameObject.Destroy(grafo.GetComponent<Grafo>().getCentroid());
		}
			GetComponent<FileManager>().Retrieve(levelSequence[currentLevel+1]+".txt",FileManager.RetrievalMode.AUTO);
			buffer_level = GetComponent<FileManager>().getResult();
			GetComponent<FileManager>().Reset();
			if(buffer_level!=null)
			{
			currentLevel = ParseLevel(levelSequence[currentLevel+1],buffer_level);
			//buffer_level = "";
			}

		if(buffer_level!=null)
		{
		password.enabled = !password.Reset();
		//options.enabled = !options.Reset();
		credits.enabled = !credits.Reset();
		highscores.enabled = !highscores.Reset();
		win.enabled = !win.Reset();
		//lose.enabled = false;

		time = 0.0f;
		stepsMade = 0;
		introLevel.enabled = true;
		canPlay = 0;
		currentLevel = nLevel;
		gameState = GameState.PLAYING;
		}
	}

	public int ParseLevel (string levelName,string raw_data) {
		raw_data = raw_data.Trim();
		GetComponent<FileManager>().WriteFile(levelName+".txt",raw_data);
		GetComponent<FileManager>().Reset();
		if(grafo!=null)
		{
		GameObject.Destroy(grafo);
		GameObject.Destroy(grafo.GetComponent<Grafo>().getCentroid());
		}
		password.enabled = !password.Reset();
		//options.enabled = !options.Reset();
		credits.enabled = !credits.Reset();
		highscores.enabled = !highscores.Reset();
		win.enabled = !win.Reset();
		//lose.enabled = false;

		time = 0.0f;
		stepsMade = 0;
		introLevel.enabled = true;
		canPlay = 0;
		gameState = GameState.PLAYING;


		string[] data = raw_data.Split("\n"[0]);
		int nLevel = int.Parse(data[1]);
		int index = -1;
		for(int i=0;i<data.Length;i++)
		{
			if(data[i].Contains("adjacency"))
			{
				index = i;
				break;
			}
		}
		currentLevel = int.Parse(data[1])-2;
		adjacencyList = new string[data.Length-index-1];
		verticesList = new Dictionary<string, Vector3>();
		//verticesList = new Vector3[index-3];
		for(int i=3;i<data.Length;i++)
		{
			if(i>index)
			{
			adjacencyList[i-1-index] = data[i];
			}
			else if(i<index)
			{
			verticesList.Add(data[i].Substring(0,data[i].IndexOf(" -> ")),String2Vector3(data[i].ToString().Substring(data[i].ToString().IndexOf(" -> ")+4)));
			}
		}

		grafo = new GameObject();
		grafo.AddComponent<Grafo>();
		grafo.name = levelName;
		grafo.tag = "Grafo";
		grafo.GetComponent<Grafo>().adjacencyList = adjacencyList;
		grafo.GetComponent<Grafo>().verticesList = verticesList;
		gameState = GameState.PLAYING;
		return nLevel;
	}

	public void LoadMainMenu () {
		GameObject grafo = getGrafo();
		if(grafo!=null)
		{
		GameObject.Destroy(grafo);
		GameObject.Destroy(grafo.GetComponent<Grafo>().getCentroid());
		}

		gameState = GameManager.GameState.MAIN_MENU;
		//options.enabled = false;
		setIsPaused(false);
		GameObject.Destroy(getGrafo());

		password.enabled = !password.Reset();
		//options.enabled = false;
		credits.enabled = !credits.Reset();
		highscores.enabled = !highscores.Reset();
		verticesList = new Dictionary<string, Vector3>();

		currentLevel = -1;
		time = 0.0f;
		stepsMade = 0;
		Camera.main.transform.rotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
	}

	void Timer () {
	time = gameState==GameState.PLAYING && introLevel.enabled==false ? time+Time.deltaTime : time;
	GUI.Box(new Rect(0,0,Screen.width-Screen.width/8,Screen.height/8),"");
	GUI.enabled = false;
	GUI.SelectionGrid(new Rect(5,5,Screen.width-Screen.width/8-10,Screen.height/16-5),-1, new string[] {"Timer","Level","Steps"}, 3);
	GUI.SelectionGrid(new Rect(5,Screen.height/16,Screen.width-Screen.width/8-10,Screen.height/16-5),-1, new string[] {getTimer(),(currentLevel+1).ToString(),stepsMade.ToString()}, 3);
	GUI.enabled = true;
	GUI.skin = null;
	if(GUI.RepeatButton(new Rect(0,Screen.height/8,Screen.width/10,Screen.height-Screen.height/8-Screen.height/8),""))
	{
	Camera.main.GetComponent<SmoothFollow>().buttonValue += Time.smoothDeltaTime;
	}
	else if(GUI.RepeatButton(new Rect(Screen.width-Screen.width/10,Screen.height/8,Screen.width/10,Screen.height-Screen.height/8-Screen.height/8),""))
	{
	Camera.main.GetComponent<SmoothFollow>().buttonValue -= Time.smoothDeltaTime;
	}
	else
	{
		Camera.main.GetComponent<SmoothFollow>().buttonValue = Mathf.Lerp(Camera.main.GetComponent<SmoothFollow>().buttonValue,0.0f,Time.smoothDeltaTime);
	}
	GUI.skin = skin;
	if(GUI.Button(new Rect(Screen.width-Screen.width/5,Screen.height-Screen.height/8,Screen.width/5,Screen.height/8),"Main Menu"))
	{
	LoadMainMenu();
	}
	}

	public Vector3 String2Vector3(string rString){
	    string[] temp = rString.Substring(2,rString.Length-2).Split(new string[] {", "},StringSplitOptions.None);
			temp[0] = rString.Substring(rString.IndexOf("(")+1,rString.IndexOf(", ")-1);
			temp[temp.Length-1] = temp[temp.Length-1].Substring(0,temp[temp.Length-1].Length-1);
			float x = float.Parse(temp[0]);
	    float y = float.Parse(temp[1]);
	    float z = float.Parse(temp[2]);
	    Vector3 rValue = new Vector3(x,y,z);
	    return rValue;
	}

	public string getTimer () {
		var minutes = time / 60;
		var seconds = time % 60;
		var fraction = (time * 100) % 100;
		return string.Format ("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
	}


    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

}
