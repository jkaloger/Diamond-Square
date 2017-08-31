using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLandscape : MonoBehaviour {

	public int scale = 3;
	public float maxHeight = 1;

	private int resolution;
	private float[,] heightMap;

	// Use this for initialization
	void Start () {
		this.resolution = Mathf.Pow ((int)scale, 2) + 1;
		int step = this.resolution / 2;

		DS ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DS() {

	}

	void Diamond(int x, int y, int step) {

	}

	void Square(int x, int y, int step) {

	}
}
