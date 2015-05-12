using UnityEngine;
using System.Collections;

public class WarningData{
	public string message = "Default Warning: Message not defined";
	public string headline = "Default Warning";
	public Code codeType = Code.Info;
	
	public WarningData(){}
	public WarningData(string aHeadline, string aMessage, Code aCode)
	{
		headline = aHeadline;
		message = aMessage; 
		codeType = aCode;
	}
}
