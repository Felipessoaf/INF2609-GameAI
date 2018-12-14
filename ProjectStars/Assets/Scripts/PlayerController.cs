using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerController : MonoBehaviour {
    private static PlayerController instance;

    public GameObject Cam;
    public GameObject Body;
    public Animator anim;
    public Transform headaxis;
    public BioIK.LookAt eyes;
    public BioIK.Position right,left;
    public BioIK.LookAt Lright,Lleft;
    public float ycrouch,ystand,xrun,xslow;

    [Header("Movimentação")]
    public KeyCode Up, Down, Left, Right,Run,Crouch,Slow,TLeft,TRight,ActionButton;
    public float Speed = 1.0f;
    public float runmult = 2;
    public float slowmult = 2;

    public float blendtime = 2;

    [Header("Modifiers")] 
    public float RotationSpeed = 100f;
    public float JumpForce = 2f;
    public float GroundedSkin = 0.05f;
    public LayerMask Mask;
    public BoxCollider colsize;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    [HideInInspector] public bool CanEnterElevator = false;

    private Rigidbody rb;
    private bool grounded;

    bool crouching = false;
    int mov = 0;//-1 slow, 0 normal, 1 run

    private Vector3 playerSize;
    private Vector3 boxSize;
    private Vector3 boxCenter;


    public static PlayerController GetInstance()
    {
        return instance;
    }

    // Use this for initialization
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        
        rb = GetComponent<Rigidbody>();
        playerSize = colsize.size;
        boxSize = new Vector3(playerSize.x - (GroundedSkin * 2), GroundedSkin, playerSize.z - (GroundedSkin * 2));

        //Subscribe de movimentação
        this.UpdateAsObservable().Subscribe(_ => {
            //float rotSpeed = 0f;
            //if (Input.GetKey(TRight))
            //{
            //    rotSpeed = RotationSpeed;
            //}
            //if (Input.GetKey(TLeft))
            //{
            //    rotSpeed = -RotationSpeed;
            //}
            //transform.Rotate(new Vector3(0, 1, 0), rotSpeed * Time.deltaTime); // Around(Player.transform.position, new Vector3(0, 1, 0), speed * Time.deltaTime);

            Vector3 movement = Vector3.zero;
            if (Input.GetKey(Up))
            {
                movement += Body.transform.forward;
            }
            if (Input.GetKey(Down))
            {
                movement -= Body.transform.forward;
            }
            if (Input.GetKey(Left))
            {
                movement -= Body.transform.right;
            }
            if (Input.GetKey(Right))
            {
                movement += Body.transform.right;
            }
            mov = 0;
            if (Input.GetKey(Run) && !Input.GetKey(Slow))
            {
                mov = 1;
            }
            if (Input.GetKey(Slow) && !Input.GetKey(Run))
            {
                mov = -1;
            }
            if (Input.GetKeyDown(Crouch))
            {
                crouching = !crouching;
            }
            Vector3 ha = new Vector3();
            movement.Normalize();
            if(crouching) ha.y = ycrouch;
            if(!crouching) ha.y = ystand;
            if(mov == -1) {ha.z = xslow*movement.z*-1; movement /= slowmult;}
            if(mov == 1) {ha.z = xrun*movement.z*-1; movement *= runmult;}
            if(mov == 0) ha.z = 0f;
            headaxis.localPosition = ha;
                
            GetComponent<Rigidbody>().velocity = (movement * Speed) + (Vector3.up * GetComponent<Rigidbody>().velocity.y);
            movement.Normalize();
            float blend = Mathf.Lerp(anim.GetFloat("Blend"),movement.magnitude,Time.deltaTime*blendtime);
            anim.SetFloat("Blend",blend);
            boxCenter = (Vector3)transform.position + Vector3.down * (playerSize.y + boxSize.y) * 0.5f;
            Collider[] colliders = Physics.OverlapBox(boxCenter, boxSize, Body.transform.rotation, Mask);

            if (Input.GetKeyDown(KeyCode.Space) && (colliders != null && colliders.Length > 0))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }

            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            Cam.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            Body.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}
