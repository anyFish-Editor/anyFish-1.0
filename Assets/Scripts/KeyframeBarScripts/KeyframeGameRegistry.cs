using UnityEngine;
using System.Collections;

public class KeyframeGameRegistry : MonoBehaviour {
	public static string animationFilePath = "";
	public static string motionCapFilePath = "";
	public static string morphDataFilePath = "";
	public static string rigTextureFilePath = "";
	
	public static KeyframeBar keyBar;
	
	public bool isBatchRenderer = false;
	public int keyFrameBarX = 20;
	public int keyFrameBarY = 20;
	public GameObject keyableGameObject;
	public GameObject MotionCaptureTarget;
	
	public bool isDebugModeActive = false;
	
	// Use this for initialization
	void Awake () {
		keyBar = gameObject.AddComponent<KeyframeBar>();
		keyBar.keyedObject = keyableGameObject;
		keyBar.debugMode = isDebugModeActive;
		keyBar.motionCaptureTarget = MotionCaptureTarget;
		//TODO moveable keyframeBar
		//keyBar.transform.Translate((float)keyFrameBarX, (float)keyFrameBarY, 0.0f);
		
		// Switch the state
		GameRegistry activeRegistry = GameObject.Find("EditorApplication").GetComponent<GameRegistry>();
		if(isBatchRenderer)
			activeRegistry.switchState(States.BatchRenderer);
		else
			activeRegistry.switchState(States.AnimationEditor);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.P))
    	{
			GameObject.Find("FrontViews").renderer.enabled = false;
			GameObject.Find("TopViews").renderer.enabled = false;
			Messenger.Broadcast("TurnOffGui");
		}
		if (Input.GetKeyDown(KeyCode.L))
    	{
			GameObject.Find("FrontViews").renderer.enabled = true;
			GameObject.Find("TopViews").renderer.enabled = true;
			Messenger.Broadcast("TurnOnGui");
		}
	}
	
}
