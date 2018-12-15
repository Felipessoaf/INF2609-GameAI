using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class Enemy : MonoBehaviour {

    public float Speed;
    public List<Transform> WayPointList;
    public LayerMask ChaseTargetLayers;
    public LayerMask AvoidLayers;
    public float ChaseRadius;
    public float AvoidRadius;

    [HideInInspector] public int NextWayPoint;
    [HideInInspector] public Transform ChaseTarget;

    [Task]
    void CheckInRange()
    {
        var cols = Physics.OverlapSphere(transform.position, ChaseRadius, ChaseTargetLayers);

        if (cols.Length > 0)
        {
            ChaseTarget = cols[0].transform;
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }    

    [Task]
    void FollowPlayer()
    {
        var targetPos = ChaseTarget.position;
        var currentPos = transform.position;
        targetPos.y = currentPos.y;
        var direction = targetPos - currentPos;

        GetComponent<Rigidbody>().velocity = direction.normalized * Speed;

        var colsChase = Physics.OverlapSphere(transform.position, ChaseRadius, ChaseTargetLayers);
        var colsAvoid = Physics.OverlapSphere(transform.position, AvoidRadius, AvoidLayers, QueryTriggerInteraction.Collide);

        if (colsChase.Length > 0 && colsAvoid.Length == 0)
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    void Waypoint()
    {
        var nextPos = WayPointList[NextWayPoint].position;
        var currentPos = transform.position;
        nextPos.y = currentPos.y;
        var direction = nextPos - currentPos;

        GetComponent<Rigidbody>().velocity = direction.normalized * Speed;

        if (Vector3.Distance(currentPos, nextPos) <= 1f)
        {
            NextWayPoint = (NextWayPoint + 1) % WayPointList.Count;
        }

        //move on navmesh?

        Task.current.Succeed();
    }
}
