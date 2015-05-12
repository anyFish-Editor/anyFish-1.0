using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

public class PathEditorSaveState : MonoBehaviour {
	public static SaveStateInfo currentState;
	private bool isGuiVisible = true;
	// Use this for initialization
	void Start () {
		Messenger.AddListener("TurnOffGui", onTurnOffGui);
		Messenger.AddListener("TurnOnGui", onTurnOnGui);
		currentState = new SaveStateInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void setPathToFront(string path)
	{
		currentState.pathToFrontFrame = path;
	}
	public static void setPathToTop(string path)
	{
		currentState.pathToTopFrame = path;
	}
	public static void setPathMoCap(string path)
	{
		currentState.pathToMocapData = path;
	}
	
	void OnGUI()
	{
		if(isGuiVisible){
			if (GUI.Button(new Rect (20,Screen.height - 30,80,20), "SaveState")) 
			{
				saveStateData();
			}
			if (GUI.Button(new Rect (20,Screen.height - 60,80,20), "LoadState")) 
			{
				loadStateData();
			}
		}
	}
	
	private void saveStateData()
	{
		
		XmlSerializer serializer = new XmlSerializer(typeof(SaveStateInfo));
		//string exeFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
  		TextWriter textWriter = new StreamWriter(@".\savedState.xml");
  		serializer.Serialize(textWriter, currentState);
  		textWriter.Close();
	}
	
	private void loadStateData()
	{
		XmlSerializer deserializer = new XmlSerializer(typeof(SaveStateInfo));
  		TextReader textReader = new StreamReader(@".\savedState.xml");
   		SaveStateInfo loadedState;
   		loadedState = (SaveStateInfo)deserializer.Deserialize(textReader);
   		textReader.Close();
		
		currentState = loadedState;
		
		Messenger<string>.Broadcast("ScrubberFrameAdvance0", currentState.pathToFrontFrame);
		Messenger<string>.Broadcast("ScrubberFrameAdvance1", currentState.pathToTopFrame);
		Messenger<string>.Broadcast("MocapLoadedState", currentState.pathToMocapData);
   	}
	
	private void onTurnOffGui()
	{
		isGuiVisible = false;	
	}
	private void onTurnOnGui()
	{
		isGuiVisible = true;	
	}
}
