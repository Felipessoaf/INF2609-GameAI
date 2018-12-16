using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class DetectionCone : MonoBehaviour {

	[Range(0,50)]
	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	private Transform playerTransform;

	void Start () {
		
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	[Task]
	void CheckPlayerInVisionCone()
	{
		//tenta traçar um raio de mim até o player.
		//Se a distância não superar o viewRadius (param maxDistance = viewRadius),
		//e não houver obstáculo na frente(Ray não chegaria se tivesse)
		//e o angulo entre a direção "frente" do inimigo e do player não superar viewAngle,
		//então inimigo viu o player
		//senão, não viu

		Debug.DrawRay(transform.position, Quaternion.Euler(0, viewAngle / 2.0f , 0) * transform.forward * viewRadius, Color.red, 0.1f);
		Debug.DrawRay(transform.position, Quaternion.Euler(0, -viewAngle / 2.0f , 0) * transform.forward * viewRadius, Color.red, 0.1f);
		Debug.DrawRay(transform.position, transform.forward * viewRadius, Color.red, 0.1f);


		//checa ângulo primeiro:
		//ignora y, porque o que vale aqui é "top-down"
		Vector3 dir = playerTransform.position - transform.position;
		dir.y = 0.0f;
		Vector3 forw = transform.forward;
		forw.y = 0;
		if(Vector3.Angle(dir, forw) <= viewAngle / 2.0)
		{
			Debug.Log("Angle pass");
			RaycastHit hit;
			bool hasHit = Physics.Raycast(this.transform.position, dir, out hit, viewRadius, LayerMask.GetMask("Player", "Obstacle"));
			if(hasHit && hit.transform.tag == "Player")
			{
				Debug.Log("Player Visto");

				//atualiza ultima visualização do player, ou seja, notifica outros agentes
				Task.current.Succeed();
				return;
			}
		}
		Task.current.Fail();
		return;

	}

	[Task]
	void PrintTest()
	{
		Debug.Log("Print");
		Task.current.Succeed();
		
	}
}
