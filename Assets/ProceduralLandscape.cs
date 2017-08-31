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
		DS ();
	}

	// diamond square
	void DS() {
		int max = this.resolution - 1;
		// initialise corner values
		this.heightMap[0,0] = 0f;
		this.heightMap[0,max] = 0f;
		this.heightMap[max,0] = 0f;
		this.heightMap[max,max] = 0f;

		int step = this.resolution / 2;

		// start the diamond step
		Diamond(0,0,step);
	}

	// diamond step
	void Diamond(int x, int y, int step) {
		int xx, yy;
		// apply the diamond step
		xx = Wrap (x + step);
		yy = Wrap (y + step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgDiamond (xx, yy);
		Square (xx, yy, step);

		xx = Wrap (x - step);
		yy = Wrap (y + step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgDiamond (xx, yy);
		Square (xx, yy, step);

		xx = Wrap (x + step);
		yy = Wrap (y - step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgDiamond (xx, yy);
		Square (xx, yy, step);

		xx = Wrap (x - step);
		yy = Wrap (y - step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgDiamond (xx, yy);
		Square (xx, yy, step);
	}

	// square step
	void Square(int x, int y, int step) {

	}

	float AvgDiamond(int x, int y) {


		return 0;
	}

	float AvgSquare(int x, int y) {

		return 0;
	}

	int Wrap(int c) {
		return (c + this.resolution) % this.resolution;
	}
}
