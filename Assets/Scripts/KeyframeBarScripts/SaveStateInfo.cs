using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveStateInfo : ISerializable {
	public string pathToFrontFrame;
	public string pathToTopFrame;
	public string pathToMocapData;
	
	// Use this for initialization
	public SaveStateInfo()
	{
		pathToTopFrame = null;
		pathToFrontFrame = null;
		pathToMocapData = null;
	}
	
	void Start () {
		
	}
	
	
	public SaveStateInfo(SerializationInfo info, StreamingContext ctxt)
	{
		this.pathToFrontFrame = (string)info.GetValue("pTFF", typeof(string));
		this.pathToTopFrame = (string)info.GetValue("pTTF", typeof(string));
		this.pathToFrontFrame = (string)info.GetValue("pTMD", typeof(string));
   	}
	
	public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
		info.AddValue("pTFF", this.pathToFrontFrame);
		info.AddValue("pTTF", this.pathToTopFrame);
		info.AddValue("pTMD", this.pathToMocapData);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
