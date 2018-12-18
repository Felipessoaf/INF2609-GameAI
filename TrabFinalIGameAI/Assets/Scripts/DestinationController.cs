using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using UnityEngine.AI;

public class DestinationController : MonoBehaviour {

	public List<Transform> WayPointList;
	[HideInInspector] public int NextWayPoint;

	private NavMeshAgent agent;
	public bool destinationSet;

	private DetectionCone dc;

	void Start ()
    {
		agent = GetComponent<NavMeshAgent>();
		dc = GetComponent<DetectionCone>();
	}
	
	void Update ()
    {
		destinationSet = false;
	}

	[Task]
    void Waypoint()
    {
        dc.coneColor = Color.blue;
		var nextPos = WayPointList[NextWayPoint].position;
        var currentPos = transform.position;
        nextPos.y = currentPos.y;
        var direction = nextPos - currentPos;

        if (Vector3.Distance(currentPos, nextPos) <= 1f)
        {
            NextWayPoint = (NextWayPoint + 1) % WayPointList.Count;
			
			nextPos = WayPointList[NextWayPoint].position;
			currentPos = transform.position;
			nextPos.y = currentPos.y;

			agent.SetDestination(nextPos);
			
        }

        //move on navmesh
		if(!destinationSet)
		{
			//Debug.Log("setting first dest");
			agent.SetDestination(nextPos);
			destinationSet = true;
		}

        Task.current.Succeed();
    }
}
