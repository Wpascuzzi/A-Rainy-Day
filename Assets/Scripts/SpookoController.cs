using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookoController : MonoBehaviour {

	public float Jumpforce = 1;
	public float runSpeed = 1;
	public Rigidbody2D SkeleBoi;
	public Collider2D floor;
	public BoxCollider2D SkeleCollide;
	// Use this for initialization
	void Start () {
		
		SkeleBoi = gameObject.GetComponent<Rigidbody2D> ();//Assigns "Skeleboi" Rigidbody to Spooko's rigidbody
		SkeleCollide = gameObject.GetComponent<BoxCollider2D> ();//Assigns "SkeleCollide" Collider to Spooko's collider	

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//The below line is in fixed update so it can reassign the value of floor whenever new BG objects get created
		floor = GameObject.FindWithTag("Background").GetComponent<Collider2D> (); //Assigns "floor" collider to the Background Collider
		Vector2 JumpForce = new Vector2(0, Jumpforce);
		transform.position += transform.right * Time.deltaTime * runSpeed;//sets position to the right
		if ((Input.GetKeyDown("space")) & (SkeleCollide.IsTouching(floor))) {
			
			SkeleBoi.AddForce (JumpForce, ForceMode2D.Impulse);
			print ("VALID JUMP");
			
		} else if ((Input.GetKeyDown ("space")) & !(SkeleCollide.IsTouching (floor))) {
			print ("INVALID JUMP");
		}
}
}
