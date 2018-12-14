using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour {
    public void CallWinScreen()
    {
        CanvasScript.GetInstance().Victory();
    }
}
