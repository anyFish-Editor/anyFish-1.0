using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable()]
public class KeyframeInfo: ISerializable {
	public float tx, ty, tz;
	public Quaternion rot;
	public Boolean isKeyed = false;
	public float dorsalAngle = 0.0f;
	public Vector2 lpelvicAngles = new Vector2(0.1f, 0.1f); //Not zero to prevent divide by zero
	public Vector2 rpelvicAngles = new Vector2(0.1f, 0.1f);
	public Vector2 analAngles = new Vector2(0.1f, 0.1f);
		
	public KeyframeInfo()
	{
		rot = new Quaternion(0,0,0,0);
	}
	
	public KeyframeInfo(SerializationInfo info, StreamingContext ctxt)
	{
    	this.tx = (float)info.GetValue("Tx", typeof(float));
    	this.ty = (float)info.GetValue("Ty", typeof(float));
		this.tz = (float)info.GetValue("Tz", typeof(float));
		this.rot = (Quaternion)info.GetValue("Rot", typeof(Quaternion));
		this.dorsalAngle = (float)info.GetValue("DAng", typeof(float));
		this.lpelvicAngles = (Vector2)info.GetValue("lpelAng", typeof(Vector2));
		this.rpelvicAngles = (Vector2)info.GetValue("rpelAng", typeof(Vector2));
		this.analAngles = (Vector2)info.GetValue("analAng", typeof(Vector2));
		
   	}
	
	public void position(Vector3 pos)
	{
		tx = pos.x;
		ty = pos.y;
		tz = pos.z;
	}
	
	public Vector3 getPosition()
	{
		return new Vector3(tx, ty, tz);	
	}
	public Quaternion getRotation()
	{
		return rot;
	}
	public void rotation(Quaternion newRotation)
	{
		rot = newRotation;
	}
	
	public void ObjectToSerialize(SerializationInfo info, StreamingContext ctxt)
   {
    	//this.tx = info.GetValue("Tx", typeof(float));
		//this.ty = info.GetValue("Ty", typeof(float));
		//this.tz = info.GetValue("Tz", typeof(float));
		//this.rx = info.GetValue("Rx", typeof(float));
		//this.ry = info.GetValue("Ry", typeof(float));
		//this.rz = info.GetValue("Rz", typeof(float));
   }

   public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
   {
    	info.AddValue("Tx", this.tx);
    	info.AddValue("Ty", this.ty);
		info.AddValue("Tz", this.tz);
		info.AddValue("Rot", this.rot);
		info.AddValue("DAng", this.dorsalAngle);
		info.AddValue("lpelAng", this.lpelvicAngles);
		info.AddValue("rpelAng", this.rpelvicAngles);
		info.AddValue("analAng", this.analAngles);
   }
}
