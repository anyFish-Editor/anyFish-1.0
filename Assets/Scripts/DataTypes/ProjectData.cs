using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ProjectData: ISerializable{
	
	public string projectName;
	public string projectFolderPath;
	public int type;
	public Vector3 tankDimensions; // In milimeters
	
	public int dialFrames;
	public int snapshotPer;
	//protected List<AnimationScene> sceneVariations;
	public int backgroundR;
	public int backgroundG;
	public int backgroundB;
	
	public ProjectData()
	{
		// Probably unnecessary but just in case it's saved without change
		projectName = null;
		projectFolderPath = null;
		tankDimensions = Vector3.zero;
		type = (int)FishType.Poeciliid;
		
		dialFrames = 15;
		snapshotPer = 1;
		
		backgroundR = 255;
		backgroundG = 245;
		backgroundB = 175;
		//sceneVariations = null;
	}
	
	public void SaveStateInfo(SerializationInfo info, StreamingContext ctxt)
	{
		this.projectName = 		(string)info.GetValue("projName", typeof(string));
		this.projectFolderPath =(string)info.GetValue("projFolder", typeof(string));
		this.tankDimensions = 	(Vector3)info.GetValue("tankDim", typeof(Vector3));
		this.type = (int)info.GetValue("fishType", typeof(int));
		
		this.dialFrames = (int)info.GetValue("dialFrames", typeof(int));
		this.snapshotPer = (int)info.GetValue("snapshotPer", typeof(int));
		this.backgroundR = (int)info.GetValue("backgroundR", typeof(int));
		this.backgroundG = (int)info.GetValue("backgroundG", typeof(int));
		this.backgroundB = (int)info.GetValue("backgroundB", typeof(int));
		//this.sceneVariations =	(List<AnimationScene>)info.GetValue("sceneVariations", typeof(List<AnimationScene>));
   	}
	
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
		info.AddValue("projName", this.projectName);
		info.AddValue("projFolder", this.projectFolderPath);
		info.AddValue("tankDim", this.tankDimensions);
		info.AddValue("fishType", this.type);
		info.AddValue("dialFrames", this.dialFrames);
		info.AddValue("snapshotPer", this.snapshotPer);
		info.AddValue("backgroundR", this.backgroundR);
		info.AddValue("backgroundG", this.backgroundG);
		info.AddValue("backgroundB", this.backgroundB);
		//info.AddValue("sceneVariations", this.sceneVariations);
    }
}
