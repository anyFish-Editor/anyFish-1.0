using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class PathData : ISerializable {
	public string pathName;
	public float tx, ty, tz;
	public Quaternion rot;
	
	public List<KeyframeInfo> keyframes;
	// Use this for initialization
	void Start () {
		pathName = null;
	}
	
	public void setCameraPosition(Vector3 pos)
	{
		tx = pos.x;
		ty = pos.y;
		tz = pos.z;
	}
	public Vector3 getCameraPosition()
	{
		return new Vector3(tx, ty, tz);	
	}
	public Quaternion getCameraRotation()
	{
		return rot;
	}
	public void setCameraRotation(Quaternion newRotation)
	{
		rot = newRotation;
	}
	
	public void SaveStateInfo(SerializationInfo info, StreamingContext ctxt)
	{
		this.pathName = 		(string)info.GetValue("pathName", typeof(string));
		this.keyframes = (List<KeyframeInfo>)info.GetValue("keyFrames", typeof(List<KeyframeInfo>));
		this.tx = (float)info.GetValue("Tx", typeof(float));
    	this.ty = (float)info.GetValue("Ty", typeof(float));
		this.tz = (float)info.GetValue("Tz", typeof(float));
		this.rot = (Quaternion)info.GetValue("Rot", typeof(Quaternion));
		/*
		this.projectFolderPath =(string)info.GetValue("projFolder", typeof(string));
		this.tankDimensions = 	(Vector3)info.GetValue("tankDim", typeof(Vector3));
		this.type = (FishType)info.GetValue("fishType", typeof(FishType));
		
		this.dialFrames = (int)info.GetValue("dialFrames", typeof(int));
		this.snapshotPer = (int)info.GetValue("snapshotPer", typeof(int));
		this.sceneVariations =	(List<AnimationScene>)info.GetValue("sceneVariations", typeof(List<AnimationScene>));
		*/
   	}
	
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
		info.AddValue("pathName", this.pathName);
		info.AddValue("keyFrames", keyframes);
		info.AddValue("Tx", this.tx);
    	info.AddValue("Ty", this.ty);
		info.AddValue("Tz", this.tz);
		info.AddValue("Rot", this.rot);
		/*
		info.AddValue("projFolder", this.projectFolderPath);
		info.AddValue("tankDim", this.tankDimensions);
		info.AddValue("fishType", this.type);
		info.AddValue("dialFrames", this.dialFrames);
		info.AddValue("snapshotPer", this.snapshotPer);
		info.AddValue("sceneVariations", this.sceneVariations);
		*/
    }
}
