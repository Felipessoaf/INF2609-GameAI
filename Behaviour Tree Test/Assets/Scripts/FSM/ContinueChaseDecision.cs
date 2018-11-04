using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
public class ContinueChaseDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        var sctrl = (SimpleStateController)controller;
        var colsChase = Physics.OverlapSphere(sctrl.transform.position, sctrl.ChaseRadius, sctrl.ChaseTargetLayers);
        var colsAvoid = Physics.OverlapSphere(sctrl.transform.position, sctrl.AvoidRadius, sctrl.AvoidLayers, QueryTriggerInteraction.Collide);

        if (colsChase.Length > 0 && colsAvoid.Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}