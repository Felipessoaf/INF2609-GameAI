using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;
public class TrackBehaviour : MonoBehaviour {

	//estou usando campos static para comparilhar informação entre os agentes
	private static Vector3 lastKnownPlayerPosition;
	private static int timeSeen;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateKnownPlayerPosition(Vector3 pos)
	{
		lastKnownPlayerPosition = pos;
		//timeSeen = 
	}
}
