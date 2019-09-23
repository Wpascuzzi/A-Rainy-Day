using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HazardBehavior : MonoBehaviour {
    
    public AudioSource blockSound;
    public float hazardHeight = 1;
    private void Start()
    {
        blockSound = GameObject.FindGameObjectWithTag("block").GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Umbrella")
        {
            if(other.gameObject.tag == "Umbrella")
            {
                blockSound.Play();
            }
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-gameObject.GetComponent<Rigidbody2D>().velocity.x * 2, 1), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);
        }
    }
}
