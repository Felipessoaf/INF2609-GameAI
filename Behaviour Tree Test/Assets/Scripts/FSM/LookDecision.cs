using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        var sctrl = (SimpleStateController)controller;
        var cols = Physics.OverlapSphere(sctrl.transform.position, sctrl.ChaseRadius, sctrl.ChaseTargetLayers);

        if (cols.Length > 0)
        {
            sctrl.ChaseTarget = cols[0].transform;
            return true;
        }
        else
        {
            return false;
        }
    }
}