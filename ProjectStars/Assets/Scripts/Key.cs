using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Key : MonoBehaviour {

    public GameObject[] Visual;
    public Collider Col;
    public float RotationSpeed = 1f;
    public Vector3[] PossibleSpawnPoints;

    private void Awake()
    {
        int SpawnPos = Random.Range(0, PossibleSpawnPoints.Length);
        transform.position = PossibleSpawnPoints[SpawnPos];
    }

    void Start()
    {
        this.UpdateAsObservable().Subscribe(_ =>
        {
            transform.Rotate(new Vector3(0,1,0) * RotationSpeed);
        });
     }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.GetInstance().CanEnterElevator = true;
            PlayerController.GetInstance().eyes.SetTargetTransform(GameObject.Find("ElevatorBox").transform);
            PlayerController.GetInstance().left.enabled = false;
            PlayerController.GetInstance().right.enabled = false;
            PlayerController.GetInstance().Lleft.enabled = false;
            PlayerController.GetInstance().Lright.enabled = false;
            PlayerController.GetInstance().GetComponentInChildren<BioIK.BioIK>().ResetPosture(PlayerController.GetInstance().GetComponentInChildren<BioIK.BioIK>().Root);

            foreach (var visual in Visual)
            {
                visual.SetActive(false);
            }
            Col.enabled = false;
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Vector3 v in PossibleSpawnPoints)
        {
            Gizmos.DrawWireSphere(v, 1f);
        } 
    }
}
