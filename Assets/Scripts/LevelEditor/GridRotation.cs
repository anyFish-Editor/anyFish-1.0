using UnityEngine;
using System.Collections;

public class GridRotation: MonoBehaviour
{
	// Update is called once per frame
	void Update ()
	{	
		
		// if its 2d mode, rotate the grid 90 degrees backwards which is an angle of 270, otherwise return it to its default rotation
		if(GameManager.is2DMode) 
		{
			Quaternion tempRot = transform.localRotation;
			tempRot.eulerAngles = new Vector3(270f,0f,0f);
			transform.localRotation = tempRot;
		}
		else
		{
			transform.localRotation = Quaternion.identity;	
		}
	}
}

