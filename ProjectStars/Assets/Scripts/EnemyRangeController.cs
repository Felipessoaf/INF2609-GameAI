using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeController : MonoBehaviour {
    public EnemyController enemy;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enemy.MoveEnemy(PlayerController.GetInstance().transform.position);
        }
    }
}
