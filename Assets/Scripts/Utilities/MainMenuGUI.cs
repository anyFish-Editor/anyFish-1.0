using UnityEngine;
using System.Collections;
//this class handles main menu
public class MainMenuGUI : MonoBehaviour 
{	
	public GUISkin currentSkin;
	public Texture2D projectLogo;
	public bool isMainMenuActive = false;
	
	void Start()
	{
		isMainMenuActive = true;
	}
	
	void OnGUI() 
	{
		//sets resolution indepence , nothing too fancy
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,new Vector3((float)1.0 * Screen.width/1280, (float)1.0 * Screen.height/800, 1.0f));
		GUI.skin = currentSkin;
		GUI.depth = 3;
		
		if (isMainMenuActive) 
		{	
			//draws the project logo above the menu
			GUI.DrawTexture(new Rect(340, 100, 600,262), projectLogo, ScaleMode.ScaleToFit);
			
			GUI.Label(new Rect(10f,750f,100f,50f),"V2.1");
			
			GUI.enabled = false;
			if(GUI.Button(new Rect(640 -150,(800 - 25) - 320,300,50),"Play"))
			{
				Application.LoadLevel("LevelSelection");
			}
			GUI.enabled = true;

            if (GUI.Button(new Rect(640 - 150, (800 - 25) - 250, 300, 50), "Level Editor"))
            {
                Application.LoadLevel("LevelEditor");
            }
		
			//this next 2, call another gui windows, firs it enables the gui, then sets the active flag for that dialog and then sets where its being called from
			if(GUI.Button(new Rect(640 - 150,(800 - 25) - 180,300,50),"Controls"))
			{
				(GetComponent("ControlsGUI") as ControlsGUI).enabled = true;
				(GetComponent("ControlsGUI") as ControlsGUI).isControlsGUIActive = true;
				(GetComponent("ControlsGUI") as ControlsGUI).wasFromMenu = true;
				isMainMenuActive = false;
			}
			

			if(GUI.Button(new Rect(640 - 150,(800 - 25) - 110,300,50),"Video Options"))
			{
				(GetComponent("VideoOptionsGUI") as VideoOptionsGUI).enabled = true;
				(GetComponent("VideoOptionsGUI") as VideoOptionsGUI).isVideoOptionsActive = true;
				(GetComponent("VideoOptionsGUI") as VideoOptionsGUI).wasFromMenu = true;
				isMainMenuActive = false;
			}
			
			if(GUI.Button(new Rect(640 - 150,(800 - 25) - 40,300,50),"Quit"))
			{
				if(isMainMenuActive)
				{
					Application.Quit();
				}
			}	
		}	
	}
}
