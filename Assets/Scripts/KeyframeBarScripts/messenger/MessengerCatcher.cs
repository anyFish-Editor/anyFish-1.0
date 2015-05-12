using UnityEngine;
using System.Collections;

public class MessengerCatcher : MonoBehaviour {
	
	// Update is called once per frame
	void RunMotion (int n)
	{
		if(n == 1)
		{
			print("Morpher RunMotion Broadcast");
			Messenger.Broadcast("RunMotion");
		}
		if(n != 1)
		{
			Messenger.Broadcast("StopMotion");
		}
	}

}
