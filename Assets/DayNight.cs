// Jack Kaloger 2017
// COMP30019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// change rotation of sun over time.... easy
		this.transform.localEulerAngles = new Vector3(Time.time * 10, 90f, 0f);
	}
}
