using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockYAxis : MonoBehaviour {

	public Camera cam;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//calls after everything is done
	void LateUpdate()
	{
		//GameObject.Find ("Camera").
		transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
		//Sets camera position back to .4 after everything is done
	}
}
