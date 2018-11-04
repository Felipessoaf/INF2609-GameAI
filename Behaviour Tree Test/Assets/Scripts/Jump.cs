using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Modifiers")]
    public float JumpVelocity;
    public float FallGravity = 2.5f;
    public float JumpGravity = 2f;

    private Rigidbody rb;

    private int jumpCount;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpCount = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (jumpCount > 0))
        {
            rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
            jumpCount--;
        }

        //if (rb.velocity.y < -0.1f)
        //{
        //    rb.gravityScale = FallGravity;
        //}
        //else if (rb.velocity.y > 0.1f)
        //{
        //    rb.gravityScale = JumpGravity;
        //}
        //else
        //{
        //    rb.gravityScale = 1f;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            jumpCount = 1;
        }
    }
}
