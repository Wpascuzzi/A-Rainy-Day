using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreateHazard : MonoBehaviour {
    //smaller value of hazardRate = faster hazards
    public float hazardRate = 10;
    public float hazSpeedVariance = 1;
    public float hazardSpeed = 5;
    public int multipleHazRate = 0;
    public GameObject[] hazards = new GameObject[5];
    public GameObject inst;
    public GameObject credits;
    float hazardCreateTime = 0;
    public Transform[] heights = new Transform[4];
    public GameObject[] backgrounds;
    private bool gameStarted;
    // Use this for initialization

    void Start () {
        gameStarted = false;
        hazardRate = 10;
        hazardCreateTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameStarted = true;
            Destroy(inst);
            Destroy(credits);
        }
        if (hazardRate == 0)
        {
            //hazardCreateTime = Time.time + 1;
        }
		else if(Time.time >= hazardCreateTime)
        {
            hazardCreateTime += (10 / hazardRate); //* Random.Range(0.1f, 1f);
            if (gameStarted)
            {
                List<GameObject> hazardList = hazards.ToList();
                GameObject currentHazard = hazards[Random.Range(0, hazards.Length)];
                hazardList.Remove(currentHazard);
                SpawnHazard(currentHazard, 20);
                if (multipleHazRate != 0 && (int)Random.Range(0, 100) < multipleHazRate)
                {
                    GameObject currentHazard2 = hazardList[Random.Range(0, hazardList.Count)];
                    SpawnHazard(currentHazard2, 20);
                }
            }
        }
	}
   void SpawnHazard(GameObject spawnObject, float killDelay)
    {
        float hazardHeight = heights[(int)spawnObject.GetComponent<HazardBehavior>().hazardHeight].position.y;
        GameObject tempObject = Instantiate(spawnObject, new Vector3(gameObject.transform.position.x, hazardHeight, 0), Quaternion.identity);
        float projectileSpeed;
        float projectileTorque = 0;
        if ((int)spawnObject.GetComponent<HazardBehavior>().hazardHeight != 0)
        {
            projectileSpeed = -Random.Range(1f, 1f + Mathf.Abs(hazSpeedVariance)) * hazardSpeed;
            projectileTorque = Random.Range(0f, 1f);
            if (projectileTorque < .5)
            {
                projectileTorque *= -10;
            }
            else
            {
                projectileTorque *= 5;
            }
        }
        else
        {
            backgrounds = GameObject.FindGameObjectsWithTag("Background");

            //projectileSpeed = -backgrounds[1].GetComponent<BackgroundScroll>().runSpeed;
            projectileSpeed = -2f;
            print("speed set " + projectileSpeed.ToString());
        }
        tempObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(projectileSpeed, 0), ForceMode2D.Impulse);
        tempObject.GetComponent<Rigidbody2D>().AddTorque(projectileTorque, ForceMode2D.Impulse);

        Destroy(tempObject, killDelay);
    }
}
