using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStateController : StateController {

    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;


}
