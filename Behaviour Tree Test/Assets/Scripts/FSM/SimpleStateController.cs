using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStateController : StateController {

    public float Speed;
    public List<Transform> WayPointList;
    public LayerMask ChaseTargetLayers;
    public LayerMask AvoidLayers;
    public float ChaseRadius;
    public float AvoidRadius;

    [HideInInspector] public int NextWayPoint;
    [HideInInspector] public Transform ChaseTarget;
}
