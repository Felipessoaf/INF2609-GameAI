using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        var sctrl = (SimpleStateController)controller;
        var nextPos = sctrl.WayPointList[sctrl.NextWayPoint].position;
        var currentPos = sctrl.transform.position;
        nextPos.y = currentPos.y;
        var direction = nextPos - currentPos;

        sctrl.GetComponent<Rigidbody>().velocity = direction.normalized * sctrl.Speed;

        if (Vector3.Distance(currentPos, nextPos) <= 1f)
        {
            sctrl.NextWayPoint = (sctrl.NextWayPoint + 1) % sctrl.WayPointList.Count;
        }
    }
}