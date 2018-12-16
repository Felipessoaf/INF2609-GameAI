using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class ChaseBehaviour : MonoBehaviour {

	private NavMeshAgent agent;

	private ChaseBehaviour[] allAgents;
	private Vector3 lastKnownPlayerPosition;
	private bool newPlayerPosition;
	private Transform playerTransform;

	private bool destinationSet;

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<NavMeshAgent>();
		allAgents = GameObject.FindObjectsOfType<ChaseBehaviour>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		destinationSet = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Task]
	void AnnoucePlayerPosition()
	{
		//anucia a todos os outros agentes que a localização do player é sabida
		//isso inclui a mim mesmo
		foreach(ChaseBehaviour cb in allAgents)
		{
			cb.lastKnownPlayerPosition = playerTransform.position;
			cb.newPlayerPosition = true;
		}
		Task.current.Succeed();
	}

	[Task]
	void NewPlayerLocationNotified()
	{
		if(newPlayerPosition) Task.current.Succeed();
		else Task.current.Fail();
	}

	[Task]
	void CheckPlayerPosition()
	{
		if(!destinationSet)
		{
			agent.SetDestination(lastKnownPlayerPosition);
			destinationSet = true;
			Task.current.Succeed();
		}
		else if(Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1.0f)
		{
			newPlayerPosition = false;
			destinationSet = false;
			Task.current.Succeed();

		}
	}

}
