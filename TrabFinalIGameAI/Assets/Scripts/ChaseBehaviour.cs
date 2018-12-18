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
	private DetectionCone dc;

	void Start () 
	{
		agent = GetComponent<NavMeshAgent>();
		allAgents = GameObject.FindObjectsOfType<ChaseBehaviour>();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		destinationSet = false;
		dc = GetComponent<DetectionCone>();
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
			cb.destinationSet = false;
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
		dc.coneColor = Color.red;
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
