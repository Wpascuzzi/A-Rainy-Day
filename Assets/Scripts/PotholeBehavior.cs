using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PotholeBehavior : MonoBehaviour {
    public AudioSource blockSound = GameObject.FindGameObjectWithTag("block").GetComponent<AudioSource>();
    public float hazardHeight = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Umbrella")
        {
            if (other.gameObject.tag == "Umbrella")
            {
                blockSound.Play();
            }
            Destroy(gameObject, 0.5f);
        }
    }
}
