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
	private bool drawCone;

	private Transform playerTransform;
	private LineRenderer lineRenderer;
	private Vector3[] points;

	public bool showVisionCone;

	public Color coneColor;

	void Start ()
    {
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		lineRenderer = GetComponent<LineRenderer>();
		points = new Vector3[5];
	}
	
	void Update()
	{

        if (drawCone)
		{
			lineRenderer.enabled = true;
			lineRenderer.startWidth = 0.1f;
			lineRenderer.endWidth = 0.1f;
			lineRenderer.startColor = coneColor;
			lineRenderer.endColor = coneColor;
			lineRenderer.positionCount = points.Length;
			lineRenderer.SetPositions(points);
			drawCone = false;
		}
		else
		{
			lineRenderer.enabled = false;
		}

    }

	[Task]
	void CheckPlayerInVisionCone()
	{
		RaycastHit hit;
		if(showVisionCone) CalculateVisionCone();

        //tenta traçar um raio de mim até o player.
        //Se a distância não superar o viewRadius (param maxDistance = viewRadius),
        //e não houver obstáculo na frente(Ray não chegaria se tivesse)
        //e o angulo entre a direção "frente" do inimigo e do player não superar viewAngle,
        //então inimigo viu o player
        //senão, não viu
#if UNITY_EDITOR
        Debug.DrawRay(transform.position, Quaternion.Euler(0, viewAngle / 2.0f , 0) * transform.forward * viewRadius, Color.red, 0.1f);
		Debug.DrawRay(transform.position, Quaternion.Euler(0, -viewAngle / 2.0f , 0) * transform.forward * viewRadius, Color.red, 0.1f);
		Debug.DrawRay(transform.position, transform.forward * viewRadius, Color.red, 0.1f);
#endif

        //checa ângulo primeiro:
        //ignora y, porque o que vale aqui é "top-down"
        Vector3 dir = playerTransform.position - transform.position;
		dir.y = 0.0f;
		Vector3 forw = transform.forward;
		forw.y = 0;
		if(Vector3.Angle(dir, forw) <= viewAngle / 2.0)
		{
			//Debug.Log("Angle pass");
			bool hasHit = Physics.Raycast(this.transform.position, dir, out hit, viewRadius, LayerMask.GetMask("Player", "Obstacle"));
			if(hasHit && hit.transform.tag == "Player")
			{
				//Debug.Log("Player Visto");

				//atualiza ultima visualização do player, ou seja, notifica outros agentes
				Task.current.Succeed();
				return;
			}
		}
		Task.current.Fail();
		return;

	}

	private void CalculateVisionCone()
	{
		RaycastHit hit;
		drawCone = true;
		//visualização legal: é mais complicada que a detecção em si
		points[0] = transform.position;
		if(Physics.Raycast(transform.position, Quaternion.Euler(0, viewAngle / 2.0f , 0) * transform.forward, out hit, viewRadius, LayerMask.GetMask("Obstacle")))
		{
			points[1] = hit.point;
		}
		else
		{
			points[1] = transform.position + Quaternion.Euler(0, viewAngle / 2.0f , 0) * transform.forward * viewRadius;
		}
			
		if(Physics.Raycast(transform.position, transform.forward, out hit, viewRadius, LayerMask.GetMask("Obstacle")))
		{
			points[2] = hit.point;
		}
		else
		{
			points[2] = transform.position + transform.forward * viewRadius;
		}

		if(Physics.Raycast(transform.position, Quaternion.Euler(0, -viewAngle / 2.0f , 0) * transform.forward, out hit, viewRadius, LayerMask.GetMask("Obstacle")))
		{
			points[3] = hit.point;
		}
		else
		{
			points[3] = transform.position + Quaternion.Euler(0, -viewAngle / 2.0f , 0) * transform.forward * viewRadius;
		}		
		points[4] = transform.position;
	}
}
