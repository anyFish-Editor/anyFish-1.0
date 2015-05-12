using UnityEngine;
using System.Collections;

public class ProduceScreenShots : MonoBehaviour {
	
	private static string folder = "benthic01";
	
	static int frameRate = 25;
	// Use this for initialization
	private static bool isRecordingActive = false;
	
	private static int startFrameCount = 1;
	
	void Awake()
	{
		//newFolderName(folder);
	}
	public static void newFolderName(string folderName)
	{
		folder = folderName;
		System.IO.Directory.CreateDirectory(PlayerPrefs.GetString("ProjectPath") + "\\" + folder);
		reset();
	}
	
	public static void reset(){
		startFrameCount = 1;	
	}
	public static void TakePic () {
		//gets the current position of the main camera
		Debug.Log("Take Pick");
    	// Set the playback framerate! (real time doesn't influence time anymore)
    	var name = string.Format("{0}/{1:D05}shot.bmp", PlayerPrefs.GetString("ProjectPath") + "\\" + folder, startFrameCount );
		startFrameCount++;
    	// Capture the screenshot
	    Application.CaptureScreenshot (name);
	}
}
