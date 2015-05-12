using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class AnimationEditorState : BasicState {
	private bool isGuiVisible = true;
	public static int selGridInt = 1;
	public static int selShowMocapInt = 1;
    private string[] selStrings = new string[] {"Mocap", "Keyframe", "Blended"};
	private string[] selShowLineRendererStrings = new string[] {"Show MocapPath", "Hide MocapPath"};
	private string frameField = "90";
	private char pathChar = "/"[0];
	
	private bool isPlaying = false;
	private bool isFirstRun = true;
	private int currentFrame;
	private int subframeCount = 0;
	private int playbackRate = 15;
	private float playbackClock = 0.0f;
	public static float dorsalAmount = 0.0f;
	public static float lpelvicHAmount = 0.1f;
	public static float lpelvicVAmount = 0.1f;
	public static float rpelvicHAmount = 0.1f;
	public static float rpelvicVAmount = 0.1f;
	public static float analHAmount = 0.1f;
	public static float analVAmount = 0.1f;
	
	private GameObject target;
	private List<MoCapAnimData> playbackKeys;
	private FishType fishModel = FishType.Poeciliid;
	
	void Awake ()
	{
		
		Messenger.AddListener("TurnOffGui", onTurnOffGui);
		Messenger.AddListener("TurnOnGui", onTurnOnGui);
		int fishType = PlayerPrefs.GetInt("FishType");
		Debug.Log("AnimationEditorState fishtype: " + fishType);
		
		if(fishType == 0)
		{
			fishModel = FishType.Poeciliid;
			GameObject.Find("EditorPrefab/Root/SticklebackRig").SetActiveRecursively(false);
			KeyframeBar holder = GameObject.Find("KeyframeBar/KeyframeGameRegistry").GetComponent<KeyframeBar>();
			Debug.Log("Does holder equal null " + holder);
			holder.keyedObject = (GameObject)GameObject.Find("EditorPrefab/Root/SwordtailRigCore");
			holder.motionCaptureTarget = (GameObject)GameObject.Find("EditorPrefab/Root/SwordtailRigCore/MotionCaptureTarget");
			target = holder.motionCaptureTarget;
			if(PlayerPrefs.GetString("MorphPath") != "default")
			{
				Debug.Log("MorphPath: " + PlayerPrefs.GetString("MorphPath"));
				Messenger<string>.Broadcast("OnMorph", PlayerPrefs.GetString("MorphPath"));
			}
			if(PlayerPrefs.GetString("TexturePath") != "default")
				Messenger<string>.Broadcast("SwapTexture", PlayerPrefs.GetString("TexturePath"));
			
		}else
		{
			fishModel = FishType.Stickleback;
			GameObject.Find("SwordtailRigCore").SetActiveRecursively(false);
			KeyframeBar holder = GameObject.Find("KeyframeBar/KeyframeGameRegistry").GetComponent<KeyframeBar>();
			holder.keyedObject = (GameObject)GameObject.Find("EditorPrefab/Root/SticklebackRig");
			holder.motionCaptureTarget = (GameObject)GameObject.Find("EditorPrefab/Root/SticklebackRig/MotionCaptureTarget");
			target = holder.motionCaptureTarget;
		}
		
		
		//string path = PlayerPrefs.GetString("PathDir");
		
		//loadStateData(path);
	}
	
	void Start ()
	{
		checkFins();
	}
	private void onTurnOffGui()
	{
		isGuiVisible = false;	
	}
	private void onTurnOnGui()
	{
		isGuiVisible = true;	
	}
		
	void firstRun()
	{
		string path = PlayerPrefs.GetString("PathDir");
		Debug.Log("pathDir: " + path);
		loadStateData(path);
		isFirstRun = false;
	}
	
	void checkFins()
	{
		Messenger.Broadcast("UpdateFinTextures");
		
	}
	// Update is called once per frame
	IEnumerator playFrame()
	{
		KeyframeBar holder = GameObject.Find("KeyframeBar/KeyframeGameRegistry").GetComponent<KeyframeBar>();
			//Debug.Log("HOLDER TARGET---------------------" + holder.motionCaptureTarget.transform.parent);
			
		target.transform.position = playbackKeys[currentFrame].Position;
		target.transform.rotation = playbackKeys[currentFrame].QRotation;//subframeCount++;
		dorsalAmount = playbackKeys[currentFrame].DorsalAmount;
		lpelvicHAmount = playbackKeys[currentFrame].LPecAmount.x;
		lpelvicVAmount = playbackKeys[currentFrame].LPecAmount.y;
		rpelvicHAmount = playbackKeys[currentFrame].RPecAmount.x;
		rpelvicVAmount = playbackKeys[currentFrame].RPecAmount.y;
		analHAmount = playbackKeys[currentFrame].AnalAmount.x;
		analVAmount = playbackKeys[currentFrame].AnalAmount.y;
		
		playbackClock += Time.deltaTime;
		Debug.Log("playbackClock: " + playbackClock);
		Messenger.Broadcast("TurnOffGui");
		yield return new WaitForSeconds(2.0f);
		//Vector3 position = Vector3.Lerp(playbackKeys[currentFrame].Position, playbackKeys[currentFrame + 1].Position, (float)subframeCount / playbackRate);
		//Quaternion rotation = Quaternion.Slerp(playbackKeys[currentFrame].QRotation, playbackKeys[currentFrame + 1].QRotation, (float)subframeCount / playbackRate);	
		
		//target.transform.position = position;
		//target.transform.rotation = rotation;
		subframeCount++;
		
		if(playbackClock > (1.0f / (float)playbackRate))
		{
			//Debug.Log("playClock: " + playbackClock);
			playbackClock = 0.0f;	
			target.transform.position = playbackKeys[currentFrame].Position;
			target.transform.rotation = playbackKeys[currentFrame].QRotation;//subframeCount++;
			dorsalAmount = playbackKeys[currentFrame].DorsalAmount;
			//if(subframeCount == playbackRate)
			//{
				subframeCount = 1;
				currentFrame++;
			//ProduceScreenShots.TakePic();
			if(currentFrame == playbackKeys.Count -1)
			{
				isPlaying = false;
				yield return new WaitForSeconds(1.0f);
				target.transform.position = playbackKeys[0].Position;
				target.transform.rotation = playbackKeys[0].QRotation;
			Debug.Log("Test");
				Messenger.Broadcast("TurnOnGui");
				//Reset motioncaption's parent the actual fish. Fish is moved for keyframing
				// motioncapture for playback
				//Transform fish = target.transform.parent;
				//fish.position = Vector3.zero;
				//fish.rotation = Quaternion.identity;
			}
		}
	}
	
	void Update () {
		if(isFirstRun)
			firstRun();		
		if(fishModel == FishType.Poeciliid)
		{
			Messenger<float>.Broadcast("AdjustDorsalFin", dorsalAmount);
			Messenger<float, float>.Broadcast("AdjustLPelvicFin", lpelvicHAmount, lpelvicVAmount);
			Messenger<float, float>.Broadcast("AdjustRPelvicFin", rpelvicHAmount, rpelvicVAmount);
			Messenger<float, float>.Broadcast("AdjustAnalFin", analHAmount, analVAmount);
		}
		if(isPlaying)
		{
			StartCoroutine("playFrame");
		}
	}
	
	
	void OnGUI()
	{
		GUI.skin = GuiManager.GetSkin();
		GUI.depth = 3;
		
		/*
		if(isGuiVisible){
			// Make the first button.
			if (GUI.Button(new Rect (20,100,80,20), "Animation")) 
			{
				SendMessage("OpenFileWindow");
			}
			GUI.Label(new Rect (110,100,800,20), pathToFileLabel);
			
			selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 3);
				
			selShowMocapInt = GUILayout.SelectionGrid(selShowMocapInt, selShowLineRendererStrings, 2);
		}*/
		
		if(isGuiVisible)
		{
			GUI.Box(new Rect(15, 15, 250, 625), "Path Properties");
			GUI.BeginGroup(new Rect(30, 60, 240, 590));
				GUI.Label(new Rect(0, 0, 98, 30), "Frame Count");
				frameField = GUI.TextArea(new Rect(100, 0, 50, 25), frameField);
				if(GUI.Button(new Rect(160, 0, 50, 30), "Set"))
				{
					Messenger<int>.Broadcast("KeyframeResize", int.Parse(frameField) );
				}	
			
			GUI.Label(new Rect(50, 37, 150, 30), "Motion Blend Type");
			selGridInt = GUI.SelectionGrid(new Rect(0, 60, 220, 30), selGridInt, selStrings, 3);
			
			GUI.enabled = false; //TODO on mocap loaded
			selShowMocapInt = GUI.SelectionGrid(new Rect(0, 95, 220, 30), selShowMocapInt, selShowLineRendererStrings, 2);
			//Debug.Log("selGridInt set: " + selGridInt);
			GUI.enabled = true;
			GUI.Label(new Rect(35, 130, 180, 30), "Load Guide Sequences");
				if(GUI.Button(new Rect(0, 160, 110, 30), "Load Front"))
				{
					Messenger.Broadcast("LoadFront");
				}
				if(GUI.Button(new Rect(110, 160, 100, 30), "Load Top"))
				{
					Messenger.Broadcast("LoadTop");
				}	
			//
			GUI.Label(new Rect(70, 190, 180, 30), "Dorsal Fin");
			
			if(fishModel != FishType.Poeciliid)
				GUI.enabled = false;
			dorsalAmount = GUI.HorizontalSlider(new Rect(40, 210, 110, 15), dorsalAmount, -15.0f, 35.0f);
			// Pelvics are kept at just greater than zero to prevent division by zero in later calc
			GUI.Label(new Rect(5, 225, 180, 30), "LPecH");
			GUI.Label(new Rect(5, 245, 180, 30), "LPecV");
			lpelvicHAmount = GUI.HorizontalSlider(new Rect(60, 230, 110, 15), lpelvicHAmount, -35.0f, 35.0f);
			lpelvicVAmount = GUI.HorizontalSlider(new Rect(60, 250, 110, 15), lpelvicVAmount, -35.0f, 35.0f);
			
			GUI.Label(new Rect(5, 265, 180, 30), "RPecH");
			GUI.Label(new Rect(5, 285, 180, 30), "RPecV");
			rpelvicHAmount = GUI.HorizontalSlider(new Rect(60, 270, 110, 15), rpelvicHAmount, -35.0f, 35.0f);
			rpelvicVAmount = GUI.HorizontalSlider(new Rect(60, 290, 110, 15), rpelvicVAmount, -35.0f, 35.0f);
			
			GUI.Label(new Rect(5, 305, 180, 30), "AnalH");
			GUI.Label(new Rect(5, 325, 180, 30), "AnalV");
			analHAmount = GUI.HorizontalSlider(new Rect(60, 310, 110, 15), analHAmount, -35.0f, 35.0f);
			analVAmount = GUI.HorizontalSlider(new Rect(60, 330, 110, 15), analVAmount, -35.0f, 35.0f);
			
			GUI.enabled = true;
			
			
			GUI.Label(new Rect(45, 340, 180, 30), "Primary Camera");
				if(GUI.Button(new Rect(0, 365, 70, 30), "Front(1)"))
				{
					Messenger<int>.Broadcast("SwitchCamera",0);
				}
				if(GUI.Button(new Rect(70, 365, 70, 30), "Free(2)"))
				{
					Messenger<int>.Broadcast("SwitchCamera", 1);
				}
			GUI.enabled = false;
				if(GUI.Button(new Rect(140, 365, 70, 30), "Top(3)"))
				{
					//Messenger.Broadcast("LoadTop");
				}
			GUI.enabled = true;
			GUI.Label(new Rect(35, 405, 180, 30), "Playback Settings");
			GUI.Label(new Rect(0, 435, 80, 30), "Framerate");
			playbackRate = int.Parse(GUI.TextField(new Rect(80, 435, 35, 30), playbackRate.ToString()));
			if(GUI.Button(new Rect(140, 435, 70, 30), "Play"))
			{
				//ProduceScreenShots.newFolderName(PlayerPrefs.GetString("PathName"));
				playbackKeys = GameObject.Find("KeyframeGameRegistry").GetComponent<KeyframeBar>().buildPlaybackData();
				currentFrame = 0;
				isPlaying = true;
			}
			
			if(GUI.Button(new Rect(30, 490, 150, 30), "Save and Exit"))
			{
				PathData data = new PathData();
				CameraManager cameras = GameObject.Find("Cameras").GetComponent<CameraManager>();
				data.setCameraPosition(cameras.getActiveCameraPosition());
				data.setCameraRotation(cameras.getActiveCameraRotation());
				data.pathName = "test";
				data.keyframes = GameObject.Find("KeyframeGameRegistry").GetComponent<KeyframeBar>().keyInfo;
				saveNewPathData(data);
				GameRegistry activeRegistry = GameObject.Find("EditorApplication").GetComponent<GameRegistry>();
				activeRegistry.switchState(States.ProjectSelector);
			}
			
			if(GUI.Button(new Rect(30, 530, 150, 30), "Exit Without Saving"))
			{
				Application.Quit();
			}
			GUI.EndGroup();
		}
	}
	
	public void OpenFile(string pathToFile) 
	{
		//SaveState.setPathMoCap(pathToFile);
		
		//print(pathToFile);
		
		List<MoCapAnimData> animData = FileIO.ImportAnimation(pathToFile);
		print("Imported List");
		
		
		//player.moCapClip = animData;
		Debug.Log("DEFAULT POSITION: " + (animData[0].Rotation));
		print("AnimInit - Clip enabled");
		//Messenger.Broadcast("AnimInit");
		//Messenger<List<MoCapAnimData>>.Broadcast("MocapDataLoaded", animData);
	}
	
	private void saveNewPathData(PathData data)
	{
		//GameRegistry activeRegistry = GameObject.Find("EditorApplication").GetComponent<GameRegistry>();
		
		XmlSerializer serializer = new XmlSerializer(typeof(PathData));
		//string exeFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
  		TextWriter textWriter = new StreamWriter(PlayerPrefs.GetString("PathDir"));
  		serializer.Serialize(textWriter, data);
  		textWriter.Close();
	}
	
	private void loadStateData(string pathName)
	{
		//GameRegistry activeRegistry = GameObject.Find("EditorApplication").GetComponent<GameRegistry>();
		XmlSerializer deserializer = new XmlSerializer(typeof(PathData));
  		TextReader textReader = new StreamReader(pathName);
   		PathData data;
   		data = (PathData)deserializer.Deserialize(textReader);
   		textReader.Close();
		
		Debug.Log("LOADING PATH DATA : AES");
		Messenger<List<KeyframeInfo>>.Broadcast("KeyframeLoaded", data.keyframes);
		CameraManager cameras = GameObject.Find("Cameras").GetComponent<CameraManager>();
		cameras.setActiveCameraPosition(data.getCameraPosition());
		cameras.setActiveCameraRotation(data.getCameraRotation());
   	}
	
	public override void CloseState ()
	{
		Messenger.Broadcast("StateChange");	
		Application.LoadLevel(0);
		isFirstRun = true;
		Destroy(this);
	}
	
	public override void SaveState ()
	{
		//TODO Save State
	}
}
