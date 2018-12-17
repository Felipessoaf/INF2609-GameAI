using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Manager Man;

    private bool hasKey;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("enemy"))
        {
            Man.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("end"))
        {
            Man.Victory();
        }
        else if (other.gameObject.CompareTag("door") && hasKey)
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("open");
        }
        else if (other.gameObject.CompareTag("doorcell"))
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("open");
        }
        else if (other.gameObject.CompareTag("key"))
        {
            hasKey = true;
            Destroy(other.gameObject);
        }
    }
}

