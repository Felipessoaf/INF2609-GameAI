using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;
public class TrackBehaviour : MonoBehaviour {

	//estou usando campos static para comparilhar informação entre os agentes
	private static Vector3 lastKnownPlayerPosition;
	private static int timeSeen;
	private DiffusionManager dm;
	private NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		dm = GameObject.FindObjectOfType<DiffusionManager>();
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateKnownPlayerPosition(Vector3 pos)
	{
		lastKnownPlayerPosition = pos;
		//timeSeen = 
	}

	[Task]
	void SetDestination_Track()
	{
		Vector3 pos = dm.GetBestNeighborPosition(this.transform.position, this.name);
		agent.SetDestination(pos);
		Task.current.Succeed();

		// agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
		// Task.current.Succeed();


	}
}
