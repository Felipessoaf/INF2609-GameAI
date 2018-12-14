using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;
using UniRx;

public class EnemyController : MonoBehaviour {
    [Header("Movimentation")]
    public Vector3[] Waypoints;
    private int curWaypoint;
    [Tooltip("Tolerância a distância entre o goal e a posição atual, quando o código checar se os dois são iguais")]public float DifTolerance = 0.1f;

    [Header("References")]
    //public ThirdPersonCharacter Character;

    private NavMeshAgent agent;
    private PlayerController player;

	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.GetInstance();
        curWaypoint = 0;
        MoveEnemy();

        Observable.EveryUpdate().Subscribe(_ =>
        {
            if (CheckIfReachedGoal()) MoveEnemy();
        });
    }

    public bool CheckIfReachedGoal()
    {
        if (this == null)
        {
            return false;
        }

        if(transform.position.x <= (agent.destination.x - DifTolerance))
        {
            return false;
        }

        if (transform.position.x >= (agent.destination.x + DifTolerance))
        {
            return false;
        }

        if (transform.position.y <= (agent.destination.y - DifTolerance + 1)) //O 1 é para correção do valor do goal
        {
            return false;
        }

        if (transform.position.y >= (agent.destination.y + DifTolerance + 1)) //O 1 é para correção do valor do goal
        {
            return false;
        }

        if (transform.position.z <= (agent.destination.z - DifTolerance))
        {
            return false;
        }

        if (transform.position.z >= (agent.destination.z + DifTolerance))
        {
            return false;
        }

        return true;
    }
	
	public void MoveEnemy(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    public void MoveEnemy()
    {
        MoveEnemy(Waypoints[curWaypoint]);
        curWaypoint++;
        if (curWaypoint >= Waypoints.Length) curWaypoint = 0;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.other.gameObject.tag == "Player")
        {
            CanvasScript.GetInstance().GameOver();
            PlayerController.GetInstance().gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 old = Waypoints[Waypoints.Length - 1];
        for(int i = 0; i < Waypoints.Length; i++)
        {
            Gizmos.DrawLine(old, Waypoints[i]);
            old = Waypoints[i];
        }
        Gizmos.DrawLine(Waypoints[Waypoints.Length - 1], Waypoints[0]);
    }
}
