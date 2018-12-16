using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//essa classe controla o cálculo da difusão, e é usada pelos agentes para determinar
//seus movimentos quando eles estão tracking

//referência: http://www.cs.colorado.edu/~ralex/papers/PDF/OOPSLA06antiobjects.pdf

public struct DiffTile
{
	public bool ignore;
	public float value;
}

public class DiffusionManager : MonoBehaviour {

	[Tooltip("O número de tiles discretos no eixo X")]
	public int NX;
	[Tooltip("O número de tiles discretos no eixo Z")]
	public int NZ;

	[SerializeField] private float dx;
	[SerializeField] private float dz;
	[SerializeField] private float x0;
	[SerializeField] private float z0;

	private int playerPosX;
	private int playerPosZ;

	private Dictionary<String, Vector2Int> agentPositions;

	private int lastTimeSeen;
	[Tooltip("Quanto maior, mais rápido o valor da difusão no ponto onde o player foi visto decai")]
	public float decayFactor;

	
	[SerializeField] public DiffTile[,] matrix;

	[SerializeField]
	private BoxCollider coll;

	[Tooltip("O valor máximo para a difusão, que ocorre onde o player foi visto por último")]
	public float ValueAtObjective;
	[Tooltip("O coeficiente de difusão")] public float DiffCoefficient;

	// Use this for initialization
	void Start () 
	{
		coll = GetComponent<BoxCollider>();
		SetMatrixPositions();

		var t = GameObject.FindGameObjectWithTag("Player").transform.position;
		UpdatePlayerPosition(GameObject.FindGameObjectWithTag("Player").transform.position);
		lastTimeSeen = -1000;

		agentPositions = new Dictionary<string, Vector2Int>();
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < NX; i++)
		{
			for(int j = 0; j < NZ; j++)
			{
				Color col = i == 0 && j == 0 ? Color.red : matrix[i,j].ignore ? Color.gray : Color.green;

				Vector3 pos = new Vector3(x0 + i*dx, coll.center.y, z0 + j * dz);
				Debug.DrawLine(pos, pos + (matrix[i,j].value + 1) * Vector3.up * 0.01f, col, 0.1f, false);
				//Debug.Log(matrix[i,j].value);
			}
		}

		//traça player position
		Vector3 pp = new Vector3(x0 + playerPosX * dx, transform.position.y,z0 + playerPosZ * dz);
		Debug.DrawLine(pp, pp + 5 * Vector3.up, Color.blue, 0.1f, false);

		//traça posição dos agentes
		foreach(KeyValuePair<string, Vector2Int> pair in agentPositions)
		{
			Vector3 ppg = new Vector3(x0 + pair.Value.x * dx, transform.position.y,z0 + pair.Value.y * dz);
			Debug.DrawLine(pp, pp + 5 * Vector3.up, Color.magenta, 0.1f, false);
		}

		UpdateDiffusionValues();
	}

	void FixedUpdate()
	{
		//UpdateDiffusionValues();
	}

	public void UpdateDiffusionValues()
	{
		//cria cópia
		DiffTile[,] old_matrix = new DiffTile[NX, NZ];
		Array.Copy(matrix, old_matrix, matrix.Length);

		for(int i = 0; i < NX; i++)
		{
			for(int j = 0; j < NZ; j++)
			{
				if(matrix[i,j].ignore)
				//é uma parede ou coisa assim, ignora
				{

				}
				else if(i == playerPosX && j == playerPosZ)
				//o player está aqui
				{
					//decaimento pela exponecial
					matrix[i,j].value = ValueAtObjective;// * Mathf.Exp(decayFactor * (lastTimeSeen - Time.frameCount));
				}
				else if(agentPositions.ContainsValue(new Vector2Int(i, j)))
				// senão, tentamos ver no dicionario se ha um agente nessa posição
				{
					matrix[i,j].value = -ValueAtObjective;
				}
				else
				{
					//fórmula da difusão
					float sum = 0.0f;
					//tenta pegar os valores de:
					//cima
					if(j - 1 >= 0) sum += matrix[i,j-1].value - matrix[i,j].value;
					//baixo
					if(j + 1 < NZ) sum += matrix[i,j+1].value - matrix[i,j].value;
					//direita
					if(i + 1 < NX) sum += matrix[i+1,j].value - matrix[i,j].value;
					//esquerda
					if(i - 1 >= 0) sum += matrix[i-1,j].value - matrix[i,j].value;

					matrix[i,j].value = matrix[i,j].value + DiffCoefficient * sum / 4.0f;
				}
			}
		}
	}

	public void UpdatePlayerPosition(Vector3 playerPosition)
	{
		//descobre o tile em que o player está baseado na posição dele
		playerPosX = (int) ((playerPosition.x - x0) / dx);
		playerPosZ = (int) ((playerPosition.z - z0) / dz);
		matrix[playerPosX,playerPosZ].value = ValueAtObjective;
		lastTimeSeen = Time.frameCount;

	}

	public void SetMatrixPositions()
	{
		coll = GetComponent<BoxCollider>();
		dx = coll.bounds.size.x / NX;
		dz = coll.bounds.size.z / NZ;

		matrix = new DiffTile[NX, NZ];

		var min = coll.transform.TransformPoint(coll.center - coll.size * 0.5f) - transform.position;
     	var max = coll.transform.TransformPoint(coll.center + coll.size * 0.5f) - transform.position;

		x0 = (float) (transform.position.x + min.x + dx/2.0);
		z0 = (float) (transform.position.z + min.z + dz/2.0);


		//inicializa matriz com os valores da difusão
		for(int i = 0; i < NX; i++)
		{
			for(int j = 0; j < NZ; j++)
			{
				Vector3 position = new Vector3(x0 + i*dx, coll.center.y, z0 + j * dz);
				DiffTile tile = new DiffTile();
				
				//se tem uma parede em cima desse tile, ele deve ser excluído do cálculo da difusão
				if(Physics.Raycast(position, Vector3.up, 100f, LayerMask.GetMask("Obstacle")))
				{
					tile.ignore = true;	
				}
				else tile.ignore = false;

				tile.value = 0.0f;
				matrix[i,j] = tile;
				Debug.Log(matrix[i,j]);
			}
		}

		Debug.Log("Matrix set succesfully");

	}

	public Vector3 GetBestNeighborPosition(Vector3 currentPosition, string name)
	{
		
		int PosX = (int) ((currentPosition.x - x0) / dx);
		int PosZ = (int) ((currentPosition.z - z0) / dz);

		//quando alguém pergunta por uma boa solução, pegamos o lugar onde essa pessoa está e setamos para 0
		//matrix[PosX, PosZ].value = - ValueAtObjective;

		//registramos ou atualizamos a posição desse agente
		agentPositions[name] = new Vector2Int(PosX, PosZ);

		int i = PosX, j = PosZ;

		float cima = -1, baixo = -1, direita = -1, esquerda = -1;
		
		//cima
		if(j - 1 >= 0 && !matrix[i,j-1].ignore) cima = matrix[i,j-1].value;
		//baixo
		if(j + 1 < NZ && !matrix[i,j+1].ignore) baixo = matrix[i,j+1].value;
		//direita
		if(i + 1 < NX && !matrix[i+1,j].ignore) direita = matrix[i+1,j].value;
		//esquerda
		if(i - 1 >= 0 && !matrix[i-1,j].ignore) esquerda = matrix[i-1,j].value;

		if(cima == Mathf.Max(cima, baixo, direita, esquerda))
		{
			return new Vector3(x0 + PosX * dx, currentPosition.y, z0 + (PosZ - 1) * dz);
		}
		else if(baixo == Mathf.Max(cima, baixo, direita, esquerda))
		{
			return new Vector3(x0 + PosX * dx, currentPosition.y, z0 + (PosZ + 1) * dz);
		}
		else if(direita == Mathf.Max(cima, baixo, direita, esquerda))
		{
			return new Vector3(x0 + (PosX + 1) * dx, currentPosition.y, z0 + PosZ * dz);
		}
		else if(esquerda == Mathf.Max(cima, baixo, direita, esquerda))
		{
			return new Vector3(x0 + (PosX - 1) * dx, currentPosition.y, z0 + PosZ * dz);
		}
		
		//não é pra cair aqui, mas evita erro de compilação
		return currentPosition;
	}
}
