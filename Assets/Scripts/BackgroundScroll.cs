using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{

    public float runSpeed;
 
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position -= gameObject.transform.right * Time.deltaTime * runSpeed;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            Instantiate(gameObject, new Vector3(gameObject.transform.position.x + 6.02353f, gameObject.transform.position.y, 0), Quaternion.identity);
        }

        if (other.gameObject.tag == "Deleter")
        {
            Destroy(gameObject, 0);
        }
        else
        {
          //  print(other.gameObject.tag);
        }
    }
}
