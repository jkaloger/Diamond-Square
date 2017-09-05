// Jack Kaloger 2017
// COMP30019
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

	public float speed = 5.0f;
	public float rotSpeed = 5.0f;
	public GameObject ground;
	public float lookSensitivity = 3.0f;

	private float yaw = 0f;
	private float pitch = 0f;
	private float roll = 0f;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
			Cursor.lockState = CursorLockMode.None;
		// WASD
		if(Input.GetKey(KeyCode.W))
			transform.position += speed * transform.forward * Time.deltaTime;
		if(Input.GetKey(KeyCode.A))
			transform.localPosition += speed * transform.right * -1 * Time.deltaTime;
			if(Input.GetKey(KeyCode.S))
			transform.localPosition += speed * transform.forward * -1 * Time.deltaTime;
		if(Input.GetKey(KeyCode.D))
			transform.localPosition += speed * transform.right * Time.deltaTime;
		

		// Mouse controls (thanks https://forum.unity3d.com/threads/how-to-lock-or-set-the-cameras-z-rotation-to-zero.68932/#post-441968)
		yaw += Input.GetAxis("Mouse X") * lookSensitivity;
		pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
		// QE
		if (Input.GetKey (KeyCode.Q))
			roll += Time.deltaTime * rotSpeed;
		if(Input.GetKey(KeyCode.E))
			roll -= Time.deltaTime * rotSpeed;
		transform.eulerAngles = new Vector3 (pitch, yaw, roll);


		int x = (int)this.transform.localPosition.x;
		int y = (int)this.transform.localPosition.y;
		int z = (int)this.transform.localPosition.z;
		float height = ground.GetComponent<Terrain> ().SampleHeight (this.transform.localPosition);
		float width = 256f;
		if(transform.localPosition.y <= height + 1f) {
			transform.localPosition = new Vector3(x,height+1f,z);
		}

		if (x <= -1) {
			transform.localPosition = new Vector3 (0, y, z);
		}
		if (x >= width) {
			transform.localPosition = new Vector3 (width  - 1, y, z);
		}

		if (z <= -1) {
			transform.localPosition = new Vector3 (x, y, 0);
		}
		if (z >= width) {
			transform.localPosition = new Vector3 (x, y, width - 1);
		}
	
	}
}
