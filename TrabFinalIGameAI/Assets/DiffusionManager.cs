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
		

	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < NX; i++)
		{
			for(int j = 0; j < NZ; j++)
			{
				Color col = i == 0 && j == 0 ? Color.red : matrix[i,j].ignore ? Color.gray : Color.green;

				Vector3 pos = new Vector3(x0 + i*dx, coll.center.y, z0 + j * dz);
				Debug.DrawLine(pos, pos + (matrix[i,j].value + 1) * Vector3.up, col, 0.1f, false);
				//Debug.Log(matrix[i,j].value);
			}
		}

		UpdateDiffusionValues();
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
				else if(i == NX /2 && j == NZ / 2)//(i == playerPosX && j == playerPosZ)
				//o player está aqui
				{
					Debug.Log("fasdfasf");
					matrix[i,j].value = ValueAtObjective;
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

					matrix[i,j].value = matrix[i,j].value + DiffCoefficient * sum;
				}
			}
		}
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
}
