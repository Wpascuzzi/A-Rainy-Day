using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatemoreBG : MonoBehaviour {
	public GameObject BackuRoundu;
	float x = 0;
	int CloneNumber = 0;
	public GameObject[] BackGroundArray;
	public List<GameObject> BackGroundList;
	// Use this for initialization
	void Start () {
		BackGroundList.Add(Instantiate (BackuRoundu, new Vector3 (x, 0, 0), Quaternion.identity));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Background") {
			x += 10.25f;
			CloneNumber++;
			BackGroundList.Add(Instantiate (BackuRoundu, new Vector3 (x, 0, 0), Quaternion.identity));
			if (CloneNumber > 1) {
				Destroy (BackGroundList[CloneNumber - 2]);
				Debug.Log ("Destroything");
			}
			print ("Collider Entered");
		} 

		else {
			print ("Begone THOT");
		}
	}
}
