using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        var sctrl = (SimpleStateController)controller;
        var targetPos = sctrl.ChaseTarget.position;
        var currentPos = sctrl.transform.position;
        targetPos.y = currentPos.y;
        var direction = targetPos - currentPos;

        sctrl.GetComponent<Rigidbody>().velocity = direction.normalized * sctrl.Speed;
    }
}