using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector2(player.transform.position.x+0.24f, player.transform.position.y+0.07f );

        if(player.GetComponent<PlayerController>().health == 0)
        {
            Destroy(gameObject, 0);
        }
	}
}
