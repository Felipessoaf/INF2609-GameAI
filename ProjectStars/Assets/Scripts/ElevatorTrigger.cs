using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour {
    public Animator ElevatorAnimator;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(Input.GetKeyDown(PlayerController.GetInstance().ActionButton) && PlayerController.GetInstance().CanEnterElevator) ElevatorAnimator.SetTrigger("Move");
        }
    }
}
