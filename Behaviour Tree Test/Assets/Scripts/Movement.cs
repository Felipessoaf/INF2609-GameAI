using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        Vector3 totalVel = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            totalVel += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            totalVel += Vector3.right;
        }
        if (Input.GetKey(KeyCode.W))
        {
            totalVel += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            totalVel += Vector3.back;
        }
        totalVel = totalVel.normalized * Speed;

        GetComponent<Rigidbody>().velocity = new Vector3(totalVel.x, GetComponent<Rigidbody>().velocity.y, totalVel.z);
    }
}
