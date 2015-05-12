using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class KeyframeBar : MonoBehaviour {
	private ArrayList keys;
	public List<KeyframeInfo> keyInfo;
	private LabelBarTexture labelBar;
	private Vector2 scrollViewVector = Vector2.zero;
	
	public int currentFrame = 0;
	
	private bool isGuiVisible = true;
	
	private Rect containingRect;
	public GameObject keyedObject;
	public GameObject motionCaptureTarget;
	public bool debugMode = false;
	
	private int maxScrollLength = 56;
	private bool hasBeenResized = false;
	void Start () {
		//Messenger<List<KeyframeInfo>>.AddListener("KeyframeLoaded", onKeyframeLoaded);
		keys = new ArrayList();
		keyInfo = new List<KeyframeInfo>();
		
		updateFrameDisplay();
		labelBar = new LabelBarTexture(530);
		labelBar.y = 1;
		
		containingRect = new Rect((Screen.width / 2) - 250, Screen.height - 75, 500, 52);
		
		Messenger.AddListener("TurnOffGui", onTurnOffGui);
		Messenger.AddListener("TurnOnGui", onTurnOnGui);
		Messenger<List<KeyframeInfo>>.AddListener("KeyframeLoaded", onKeyframeLoaded);
		Messenger<int>.AddListener("KeyframeResize", respawnWithFrameCount);
	}
	
	// During the awake method we build a list of KeyframeTextures, and apply the isMultipleOfFive
	// boolean to have them shaded for easier recognition of frame breakdowns. All data under the commented
	// line is only meant to serve as "setup" variables for testing and can be removed
	void Awake() {
		Messenger<List<KeyframeInfo>>.AddListener("KeyframeLoaded", onKeyframeLoaded);
		Messenger<int>.AddListener("KeyframeResize", respawnWithFrameCount);
    }
	
	// UpdateFrameDisplay is used to toggle the current states of each frameTexture,
	// iterating through them and determining if it should be toggled. This fixes the removal
	// or addition of keyframes, and is called when a selection is changed.
	private void updateFrameDisplay()
	{
		for(int i = 0; i < keyInfo.Count; i++)
		{
			if(keyInfo[i].isKeyed)
			{
				KeyframeTexture key = (KeyframeTexture)keys[i];
				key.isKeyed = true;
			}else
			{
				KeyframeTexture key = (KeyframeTexture)keys[i];
				key.isKeyed = false;	
			}
		}
	}
	
	// A click on the timeline calls this, scrub by the current keyframe, if between frames find
	// and apply the lerp between the two
	private void gotoFrame(int i)
	{
		
		currentFrame = i;
		//MoCapAnimDataPlayer player = GameObject.Find("MotionCaptureTarget").GetComponent<MoCapAnimDataPlayer>();
		//player.getMocapFramePosition(i);
		//keyedObject.transform.position = motionCaptureTarget.transform.position;
		
		
		Debug.Log("GOTO " + i + " PureKeyframe transform "+ guiReadAnimation.selGridInt);
		/*
		if(guiReadAnimation.selGridInt == 0){
			keyedObject.transform.position = player.getMocapFramePosition(i);
			keyedObject.transform.eulerAngles = player.getMocapFrameRotation(i);
		}
		if(guiReadAnimation.selGridInt == 1){
		*/
		if(keyedObject){
				Debug.Log("goto keyframeRot transform");
			if(keyInfo[i].isKeyed)
			{
				keyedObject.transform.position = keyInfo[i].getPosition(); //new Vector3(keyInfo[i].tx, keyInfo[i].ty, keyInfo[i].tz); 
				keyedObject.transform.rotation = keyInfo[i].getRotation(); 
				AnimationEditorState.dorsalAmount = keyInfo[i].dorsalAngle;
				AnimationEditorState.lpelvicHAmount = keyInfo[i].lpelvicAngles.x;
				AnimationEditorState.lpelvicVAmount = keyInfo[i].lpelvicAngles.y;
				
				AnimationEditorState.rpelvicHAmount = keyInfo[i].rpelvicAngles.x;
				AnimationEditorState.rpelvicVAmount = keyInfo[i].rpelvicAngles.y;
				
				AnimationEditorState.analHAmount = keyInfo[i].analAngles.x;
				AnimationEditorState.analVAmount = keyInfo[i].analAngles.y;
			}else
			{
				Vector3 newPosition;
				Quaternion newRotation;
				float dorsalAmount;
				Vector2 newLPelvic;
				Vector2 newRPelvic;
				Vector2 newAnal;
				interpolateKeys(i, out newRotation, out newPosition, out dorsalAmount, out newLPelvic, out newRPelvic, out newAnal);
				keyedObject.transform.position = newPosition;
				keyedObject.transform.rotation = newRotation;
				AnimationEditorState.dorsalAmount = dorsalAmount;
				AnimationEditorState.lpelvicHAmount = newLPelvic.x;
				AnimationEditorState.lpelvicVAmount = newLPelvic.y;
				
				AnimationEditorState.rpelvicHAmount = newRPelvic.x;
				AnimationEditorState.rpelvicVAmount = newRPelvic.y;
				
				AnimationEditorState.analHAmount = newAnal.x;
				AnimationEditorState.analVAmount = newAnal.y;
			}
		}
		
		
	}
	
	private void interpolateKeys(int i, out Quaternion rotation, out Vector3 position, out float dorsalAmount, out Vector2 lPelvic, out Vector2 rPelvic, out Vector2 anal)
	{
		KeyframeInfo lastKey = new KeyframeInfo();
		KeyframeInfo nextKey = new KeyframeInfo();
		int j = 0;
		int k = 0;
		// Check past keys
		for(j = i; j >= 0; j--)
		{
			if(keyInfo[j].isKeyed){
				lastKey = (KeyframeInfo)keyInfo[j];
				break;
			}
		}
		
		for(k = i; k < keyInfo.Capacity; k++)
		{
			if(keyInfo[k].isKeyed){
				nextKey = (KeyframeInfo)keyInfo[k];
				break;
			}
		}
			
		//Debug.Log("Last : " + j + " , next : " + k + ", and current: " + i + ", float: " + test);
		position = Vector3.Lerp(lastKey.getPosition(), nextKey.getPosition(), (float)(i - j)/(k - j));
		rotation = Quaternion.Slerp(lastKey.getRotation(), nextKey.getRotation(), (float)(i - j)/(k - j));	
		dorsalAmount = Mathf.Lerp(lastKey.dorsalAngle, nextKey.dorsalAngle, (float)(i-j)/(k-j));
		
		lPelvic.x = Mathf.Lerp(lastKey.lpelvicAngles.x, nextKey.lpelvicAngles.x, (float)(i-j)/(k-j));
		lPelvic.y = Mathf.Lerp(lastKey.lpelvicAngles.y, nextKey.lpelvicAngles.y, (float)(i-j)/(k-j));
		
		rPelvic.x = Mathf.Lerp(lastKey.rpelvicAngles.x, nextKey.rpelvicAngles.x, (float)(i-j)/(k-j));
		rPelvic.y = Mathf.Lerp(lastKey.rpelvicAngles.y, nextKey.rpelvicAngles.y, (float)(i-j)/(k-j));
		
		anal.x = Mathf.Lerp(lastKey.analAngles.x, nextKey.analAngles.x, (float)(i-j)/(k-j));
		anal.y = Mathf.Lerp(lastKey.analAngles.y, nextKey.analAngles.y, (float)(i-j)/(k-j));
	}
	
	public KeyframeInfo getKeyinfoByFrame(int frame)
	{
		if(keyInfo != null)
		{
			if(keyInfo[frame].isKeyed)
				return keyInfo[frame];
		}
		
		return null;
	}
	private void setKey()
	{
		Debug.Log("KeyframeBar::setKey ");
		keyInfo[currentFrame] = new KeyframeInfo();
		keyInfo[currentFrame].isKeyed = true;
		//KeyframeInfo test = (KeyframeInfo)keyInfo[currentFrame];
		keyInfo[currentFrame].position(keyedObject.transform.position);
		keyInfo[currentFrame].rotation(keyedObject.transform.rotation);
		Debug.Log(keyedObject.transform.position);
		//test.position(new Vector3(pos.x, pos.y, pos.z)); 
		if(AnimationEditorState.dorsalAmount != 0.0f)
			keyInfo[currentFrame].dorsalAngle = AnimationEditorState.dorsalAmount;
		keyInfo[currentFrame].lpelvicAngles = new Vector2(AnimationEditorState.lpelvicHAmount, AnimationEditorState.lpelvicVAmount);
		keyInfo[currentFrame].rpelvicAngles = new Vector2(AnimationEditorState.rpelvicHAmount, AnimationEditorState.rpelvicVAmount);
		keyInfo[currentFrame].analAngles = new Vector2(AnimationEditorState.analHAmount, AnimationEditorState.analVAmount);
		updateFrameDisplay();
	}
	
	private void deleteKey()
	{
		if(keyInfo[currentFrame].isKeyed)
			keyInfo[currentFrame].isKeyed = false;
		
		updateFrameDisplay();
	}
	
	void saveKeyframeData()
	{
		
		XmlSerializer serializer = new XmlSerializer(typeof(List<KeyframeInfo>));
		//string exeFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
  		TextWriter textWriter = new StreamWriter(@".\test.xml");
  		serializer.Serialize(textWriter, keyInfo);
  		textWriter.Close();
	}
	
	void loadKeyframeData()
	{
		XmlSerializer deserializer = new XmlSerializer(typeof(List<KeyframeInfo>));
  		TextReader textReader = new StreamReader(@".\test.xml");
   		List<KeyframeInfo> newKeys;
   		newKeys = (List<KeyframeInfo>)deserializer.Deserialize(textReader);
   		textReader.Close();
		
		keyInfo = newKeys;
		updateFrameDisplay();
   	}
	
	// When new data is loaded in, rebuild the frames and the text labels over the timeline
	// to appropriate size
	public void respawnWithFrameCount(int count)
	{
		if(count == 0)
			count = 55;
		
		if(hasBeenResized == false)
		{
			hasBeenResized = true;
		
		}
		if(keys != null)
		{
		//saveKeyframeData();
		
		maxScrollLength = count;
		for(int i = 0; i < keys.Count; i++)
		{
			keys[i] = null;
		}
		}
		keyInfo = new List<KeyframeInfo>(count);
		
		keys = new ArrayList();
		for(int i = 0; i < count; i++)
		{
			KeyframeTexture keyTexture = new KeyframeTexture();
			keyTexture.x = i * 8;
			keyTexture.y = 20;
			if(i % 5 == 0)
				keyTexture.isMultipleFive = true;
			
			keys.Add(keyTexture);
			keyInfo.Add(new KeyframeInfo());
		}
		currentFrame = 1;
		labelBar = new LabelBarTexture(count);
		
		keyInfo[0] = new KeyframeInfo();
		keyInfo[0].position(new Vector3(150f, 50f, 0f));
		keyInfo[0].rotation(new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		keyInfo[0].isKeyed = true;
		keyInfo[count-1 ] = new KeyframeInfo();
		keyInfo[count-1].position(new Vector3(0f, 0f, 0f));
		keyInfo[count-1].rotation(new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
		keyInfo[count-1].isKeyed = true;
		
		gotoFrame(1);
		updateFrameDisplay();
		//Debug.Log("Rebuilding the Keyframebar took: " + (startTime - endTime) + " ms");
	}
	
	void Update() 
	{
	}
	
	// Update is called once per frame
	void OnGUI()
	{
		GUI.depth = 2;
		if(isGuiVisible){
			GUI.skin =  GuiManager.GetSkin();
		scrollViewVector = GUI.BeginScrollView(containingRect, scrollViewVector, new Rect (0, 0, maxScrollLength * 8, 36));
		
		foreach(KeyframeTexture key in keys)
		{
			key.draw();			
		}
		//GUI.Label(new Rect(0, 0, 200, 200), "150");
		GUI.DrawTexture(new Rect(0,0, 12 * maxScrollLength, 20), (Texture)Resources.Load("keyframeBarTop"));
		labelBar.draw();
		
		
		// End the ScrollView
		GUI.EndScrollView();
		
		if (Event.current.type == EventType.MouseDown)
            OnMouseClick();
		
		if (GUI.Button(new Rect((Screen.width / 2) + 250 + 30, Screen.height - 76, 70, 30), "Set Key"))
            setKey();
		if (GUI.Button(new Rect((Screen.width / 2) + 250 + 120, Screen.height - 76, 90, 30), "Delete Key"))
            deleteKey();
		
		//if (GUI.Button(new Rect((Screen.width / 2) + 250 + 30, Screen.height - 36, 70, 30), "Save Keys"))
        //    saveKeyframeData();
		//if (GUI.Button(new Rect((Screen.width / 2) + 250 + 120, Screen.height - 36, 90, 30), "Load Keys"))
        //    loadKeyframeData();
		}
	}
	
	private void onTurnOffGui()
	{
		isGuiVisible = false;	
	}
	private void onTurnOnGui()
	{
		isGuiVisible = true;	
	}
	
	private void onKeyframeLoaded(List<KeyframeInfo> newKeys)
	{
		Debug.Log("onKeyframeLoaded");
		if(newKeys.Count > 1)
		{
			Debug.Log("onKeyframeLoaded: Respawn with count: " + newKeys.Count);
			respawnWithFrameCount(newKeys.Count);
			keyInfo = newKeys;
		}else
		{
			//Populate it with empty stuff
			respawnWithFrameCount(45);
		}
		updateFrameDisplay();
	}
	public List<MoCapAnimData> buildPlaybackData()
	{
		List<MoCapAnimData> playbackList = new List<MoCapAnimData>();
		/*
		if(guiReadAnimation.selGridInt == 1){
			Debug.Log("goto keyframeRot transform");
			if(keyInfo[i].isKeyed)
			{
				keyedObject.transform.position = keyInfo[i].getPosition(); //new Vector3(keyInfo[i].tx, keyInfo[i].ty, keyInfo[i].tz); 
				keyedObject.transform.rotation = keyInfo[i].getRotation(); 
			}else
			{
				Vector3 newPosition;
				Quaternion newRotation;
				interpolateKeys(i, out newRotation, out newPosition);
				keyedObject.transform.position = newPosition;
				keyedObject.transform.rotation = newRotation;
			}
		}
		*/
		// Use this
		
		for(int i = 0; i < keyInfo.Count; i++)
		{
			if(keyInfo[i].isKeyed){
				//Debug.Log("Testing move");
				MoCapAnimData newKey;// = new MoCapAnimData();
				newKey.Position = keyInfo[i].getPosition();
				newKey.Rotation = keyInfo[i].getRotation().eulerAngles;
				newKey.QRotation = keyInfo[i].getRotation();
				newKey.LPecAmount = new Vector2(keyInfo[i].lpelvicAngles.x, keyInfo[i].lpelvicAngles.y);
				newKey.RPecAmount = new Vector2(keyInfo[i].rpelvicAngles.x, keyInfo[i].rpelvicAngles.y);
				newKey.AnalAmount = new Vector2(keyInfo[i].analAngles.x, keyInfo[i].analAngles.y);
				if(keyInfo[i].dorsalAngle != 0f)
					newKey.DorsalAmount = keyInfo[i].dorsalAngle;
				else
					newKey.DorsalAmount = 0.0f;
				playbackList.Add(newKey);
			}else
			{
				Vector3 newPosition;
				Quaternion newRotation;
				float dorsalAmount;
				Vector2 newLPelvic;
				Vector2 newRPelvic;
				Vector2 newAnal;
				interpolateKeys(i, out newRotation, out newPosition, out dorsalAmount, out newLPelvic, out newRPelvic, out newAnal);
				//keyedObject.transform.position = newPosition;
				//keyedObject.transform.rotation = newRotation;
				//Debug.Log("Blended Position: " + newPosition);
				MoCapAnimData newKey;// = new MoCapAnimData();
				newKey.Position = newPosition;
				newKey.Rotation = newRotation.eulerAngles;
				newKey.QRotation = newRotation;
				newKey.DorsalAmount = dorsalAmount;
				newKey.LPecAmount = newLPelvic;
				newKey.RPecAmount = newRPelvic;
				newKey.AnalAmount = newAnal;
				playbackList.Add(newKey);
			}
		}
		
		return playbackList;
	}
	
	
	/// <summary>
	///  On Mouseclick
	/// </summary>
	public void OnMouseClick()
	{
		if(containingRect.Contains(Event.current.mousePosition))
		{
			Vector2 mosPos = new Vector2(Event.current.mousePosition.x- containingRect.x + scrollViewVector.x, Event.current.mousePosition.y - containingRect.y);
			Debug.Log("InsideTest");
			for(int i = 0; i < keys.Count; i++)
			{
				if((keys[i] as KeyframeTexture).contains(mosPos))
				{
					Debug.Log("Clicked :" + i);
					gotoFrame(i);
					Messenger<int>.Broadcast("SetTextureByFrame", i);
				}
			}
		}
	}
}
