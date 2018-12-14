using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handgrab : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            PlayerController.GetInstance().right.SetTargetTransform(transform.parent);
            PlayerController.GetInstance().right.enabled = true;
			PlayerController.GetInstance().Lright.SetTargetTransform(transform.parent);
            PlayerController.GetInstance().Lright.enabled = true;
			PlayerController.GetInstance().Lleft.SetTargetTransform(transform.parent);
            PlayerController.GetInstance().Lright.enabled = true;
			PlayerController.GetInstance().left.SetTargetTransform(transform.parent);
            PlayerController.GetInstance().left.enabled = true;
        }
    }
	private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            PlayerController.GetInstance().right.enabled = false;
            PlayerController.GetInstance().Lright.enabled = false;
            PlayerController.GetInstance().Lleft.enabled = false;
            PlayerController.GetInstance().left.enabled = false;
        }
    }
}
