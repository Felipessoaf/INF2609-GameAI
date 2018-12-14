using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SphereController : MonoBehaviour {
	[Tooltip("A estrela que este planeta gira ao redor")]public GameObject Star;
	public float DistanceToStar = 5.0f;
	public float angleSpeed = 50.0f;


	// Use this for initialization
	void Start () {
		//translação
		this.UpdateAsObservable ().Subscribe (_ => {
            if(Star)
            {
                transform.RotateAround(Star.transform.position, new Vector3(0, 1, 0), angleSpeed * Time.deltaTime);
            }
		});
	}
}
