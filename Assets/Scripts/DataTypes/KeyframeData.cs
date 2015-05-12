using UnityEngine;
using System.Collections;

public class KeyframeData {
	private Vector3 pos;
	private Vector3 rot;
	
	public KeyframeData()
	{
		
	}
	
	public KeyframeData(Vector3 newPosition, Vector3 newRotation)
	{
		pos = newPosition;
		rot = newRotation;
	}
	
	public Quaternion rotationAsQuaternion()
	{
		Quaternion returnQuat = Quaternion.identity;
		returnQuat.eulerAngles = rot;
		return returnQuat;
	}
	
	public Vector3 position
	{
		get{ return pos; }
		set{ pos = value; }
	}
	public Vector3 rotation
    {
        get { return rot; }
        set { rot = value; }
    }
}
