using UnityEngine;
using System.Collections;

public class KeyframeTexture {
	private Texture keyOn;
	private Texture keyOff;
	private Texture keySelected;
	private Texture keyOffFive;
	private Rect textureSize;
	
	public int x { get; set; }
	public int y { get; set; }
	public bool isKeyed { get; set; }
	public bool isMultipleFive {get; set;}
	public bool isSelected {get; set;}
	
	// isKeyed means that there is associated keyframe information translation/rotation
	// isMultiple of Five means that there is an alternate image used for visual readability
	// isSelected uses a blue image to display the current 'head' of the timeline scrubber.
	
	
	public KeyframeTexture()
	{
		x = 0;
		y = 0;
		isKeyed = false;
		isMultipleFive = false;
		isSelected = false;
		keyOffFive = (Texture)Resources.Load("keyframeMFive");
		keyOn = (Texture)Resources.Load("keyframeKeyed");
		keyOff = (Texture)Resources.Load("keyframeEmpty");
		keySelected = (Texture)Resources.Load("keyframeSelected");
	}
	
	public void OnMouseDown()
	{
		Debug.Log("TEST MOUSE DOWN");	
	}
	
	public void draw()
	{
		textureSize = new Rect(x, y, 8, 16);
		
		if(isSelected)
			GUI.DrawTexture(textureSize, keySelected);
		else if(isKeyed)
			GUI.DrawTexture(textureSize, keyOn);
		else if(isMultipleFive)
			GUI.DrawTexture(textureSize, keyOffFive);
		else
			GUI.DrawTexture(textureSize, keyOff);
		
	}
	
	// When a keyframeTexture contains the Vector2 position we've been clicked
	// this method is necessary as opposed to the GUI onMouse because of using the 
	// drawTexture methods to display the graphics. DrawTextures don't report clicks
	public bool contains(Vector2 position)
	{
		if(textureSize.Contains(position))
		{
			isSelected = true;
			return true;
		}
		else 
		{
			isSelected = false;
			return false;
		}
		
	}
}
