using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool player1;
    public float Velocity = 5f;
    public float JumpVelocity = 5f;
    public int JumpCountMax = 2;

    private int jumpCount;

    // Use this for initialization
    void Start ()
    {
        jumpCount = JumpCountMax;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(player1){
            Vector2 totalVel = Vector2.zero;
            //ir para a esquerda
            if(Input.GetKey(KeyCode.A))
            {
                totalVel += Vector2.left * Velocity;
            }
            //ir para a direita
            else if (Input.GetKey(KeyCode.D))
            {
                totalVel += Vector2.right * Velocity;
            }
            //parar
            else 
            {
                Vector2 vela = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                GetComponent<Rigidbody2D>().velocity = vela;
            }

            if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpVelocity, ForceMode2D.Impulse);
                jumpCount--;
            }

            Vector2 vel = new Vector2(totalVel.x, GetComponent<Rigidbody2D>().velocity.y);
            GetComponent<Rigidbody2D>().velocity = vel;
        }
        else{
            Vector2 totalVel = Vector2.zero;
            //ir para a esquerda
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                totalVel += Vector2.left * Velocity;
            }
            //ir para a direita
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                totalVel += Vector2.right * Velocity;
            }
            //parar
            else 
            {
                Vector2 vela = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                GetComponent<Rigidbody2D>().velocity = vela;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount > 0)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpVelocity, ForceMode2D.Impulse);
                jumpCount--;
            }

            Vector2 vel = new Vector2(totalVel.x, GetComponent<Rigidbody2D>().velocity.y);
            GetComponent<Rigidbody2D>().velocity = vel;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("platform"))
        {
            jumpCount = JumpCountMax;
        }
    }
    public void OnTriggerEnter2D(Collider2D collider2D){
        if(collider2D.gameObject.CompareTag("chao"))
        {

            Destroy(gameObject);
        }
    }
}
