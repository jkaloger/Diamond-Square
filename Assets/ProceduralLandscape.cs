// Jack Kaloger 2017
// COMP30019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLandscape : MonoBehaviour {

	public int resolutionScale = 3;
	public int scale = 20;
	public float bumpiness = 1;
	public Terrain terrain;

	private int resolution;
	private float[,] heightMap;

	// Use this for initialization
	void Start () {
		this.resolution = (int)Mathf.Pow (2, resolutionScale) + 1;
		heightMap = new float[this.resolution, this.resolution];
		DS ();

		this.terrain.terrainData.heightmapResolution = this.resolution;

		this.terrain.terrainData.SetHeights (0, 0, this.heightMap);
	}

	// diamond square
	void DS() {
		int max = this.resolution - 1;
		// initialise corner values
		this.heightMap[0,0] = 0;
		this.heightMap[0,max] = 0;
		this.heightMap[max,0] = 0;
		this.heightMap[max,max] = 0;

		int step = this.resolution / 2;

		// start the diamond step
		Diamond(0,0,step);
	}

	// diamond step
	void Diamond(int x, int y, int step) {
		int xx, yy;
		// apply the diamond step, see readme.txt for detailed explanation
		// up and to the right
		xx = Wrap (x + step);
		yy = Wrap (y + step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgDiamond (xx, yy, step);
		Square (xx, yy, step);

		// left and up
		xx = Wrap (x - step);
		yy = Wrap (y + step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgDiamond (xx, yy, step);
		Square (xx, yy, step);

		// right and down
		xx = Wrap (x + step);
		yy = Wrap (y - step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgDiamond (xx, yy, step);
		Square (xx, yy, step);

		// left and down
		xx = Wrap (x - step);
		yy = Wrap (y - step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgDiamond (xx, yy, step);
		Square (xx, yy, step);
	}

	// square step
	void Square(int x, int y, int step) {
		if (step < 1)
			return;
		int xx, yy;
		// apply the square step, see readme.txt
		// grab right
		xx = Wrap (x + step);
		yy = Wrap (y       );
		heightMap [Wrap (xx), Wrap (yy)] = AvgSquare (xx, yy, step);
		Diamond (xx, yy, step / 2);

		// grab above
		xx = Wrap (x       );
		yy = Wrap (y + step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgSquare (xx, yy, step);
		Diamond (xx, yy, step / 2);

		// grab below
		xx = Wrap (x      );
		yy = Wrap (y - step);
		heightMap [Wrap (xx), Wrap (yy)] = AvgSquare (xx, yy, step);
		Diamond (xx, yy, step / 2);

		// grab left
		xx = Wrap (x - step);
		yy = Wrap (y       );
		heightMap [Wrap (xx), Wrap (yy)] = AvgSquare (xx, yy, step);
		Diamond (xx, yy, step / 2);
	}

	float AvgDiamond(int x, int y, int step) {
		float total =
			heightMap [Wrap (x + step), Wrap (y + step)] +
			heightMap [Wrap (x - step), Wrap (y + step)] +
			heightMap [Wrap (x + step), Wrap (y - step)] +
			heightMap [Wrap (x - step), Wrap (y - step)] ;
		float avg = total > 0 ? total / 4 : 0;
		return getRandom(avg, step);
	}

	float AvgSquare(int x, int y, int step) {
		float total =
			heightMap [Wrap (x + step), Wrap (y       )] +
			heightMap [Wrap (x - step), Wrap (y       )] +
			heightMap [Wrap (x       ), Wrap (y + step)] +
			heightMap [Wrap (x       ), Wrap (y - step)] ;
		float avg = total > 0 ? total / 4 : 0;
		return getRandom(avg, step);
	}

	int Wrap(int c) {
		if (c < 0)
			c += (this.resolution - 1);
		if (c >= this.resolution)
			c -= (this.resolution - 1);
		return c;
	}

	float getRandom(float avg, int seed) {
		return avg + ((Random.Range (-1, 2) * Random.value * (bumpiness * seed)));
	}
}
