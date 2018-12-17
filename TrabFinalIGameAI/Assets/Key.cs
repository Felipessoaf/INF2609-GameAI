using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public List<Transform> SpawnPoints;

    private void Awake()
    {
        Vector3 pos = SpawnPoints[Random.Range(0, SpawnPoints.Count)].position;
        pos.y = 1.8f;
        transform.position = pos;
    }
}
