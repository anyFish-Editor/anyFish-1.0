using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class DirectoryUtil : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Probably could have named this better, checks if the directory already exists, if it doesn't
	// creates one. The bool is optionally used to handle additional setup if files are expected.
	// For example if no project directory exists, recreate all needed xml files for saving states, etc.
	public static bool AssertDirectoryExistsOrRecreate(string directoryPath)
	{
		if(Directory.Exists(directoryPath))
			return true;
			
		Directory.CreateDirectory(directoryPath);
		return false;
	}
	
	public static DirectoryInfo[] getSubDirectoriesByParent(string directoryPath)
	{
		DirectoryInfo dir = new DirectoryInfo(directoryPath);
		DirectoryInfo[] subDirs = null;
		try
		{
			if(dir.Exists)
			{
				subDirs = dir.GetDirectories();				
				return(subDirs);	
			}
		}
		catch (Exception e)
		{
			Debug.Log(e);
			WarningSystem.addWarning("SubDirectory Error", "Error searching supplied path for subdirectories", Code.Error);
		}
		
		return subDirs;
	}
}
