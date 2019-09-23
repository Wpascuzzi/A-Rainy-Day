using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text score;
    public Sprite[] heartHUD;
    public Image hearts;

    public Sprite[] lightningHUD;
    public Image lightnings;
    public GameObject lightning;
    float nextLightning;
    public AudioSource zap;
    public AudioSource thunder;


    float scoreMult = 1;
    public float firstStageTime = 20;
    public float stageTime = 20;
    public GameObject gameOver;
    float gameScore;
    GameObject player;
    GameObject background;
    GameObject[] backgrounds = new GameObject[2];
    GameObject hazardMaker;
    bool stillPlaying = true;
    // Use this for initialization
    void Start()
    {
        nextLightning = Random.Range(10f, 30f);
        player = GameObject.FindGameObjectWithTag("Player");

        Time.timeScale = 0f;
    }
    IEnumerator BeginLightning()
    {
        float lightningStart = Time.time;
        float lightningEnd = Time.time + 5;
        lightnings.gameObject.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            zap.Play();
            lightnings.sprite = lightningHUD[i];
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(LightningStrike());
        lightnings.gameObject.SetActive(false);
    }

    IEnumerator LightningStrike()
    {
        bool lightningLethal = false;
        PlayerController playerScript = player.GetComponent<PlayerController>();
        if (!playerScript.GetDoubleJump() || playerScript.GetFloating() || playerScript.GetBlocking())
        {
            lightning.transform.position = new Vector2(player.transform.position.x, lightning.transform.position.y);
            lightningLethal = true;
        }
        else
        {
            lightning.transform.position = new Vector2(player.transform.position.x + 1, lightning.transform.position.y);
        }
        thunder.Play();
        lightning.GetComponent<SpriteRenderer>().enabled = true;
        if (lightningLethal && !playerScript.GetInvincible())
        {
            player.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            playerScript.health = 0;
            playerScript.Die();
        }
        yield return new WaitForSeconds(0.2f);
        lightning.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        hearts.sprite = heartHUD[player.GetComponent<PlayerController>().health];
        if (player.GetComponent<PlayerController>().health > 0)
        {
            gameScore += (Time.deltaTime * scoreMult);
            score.text = "Metres travelled: " + Mathf.Round(gameScore);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
        }

            if (stillPlaying)
        {
            if (Time.time > firstStageTime)
            {
                firstStageTime += stageTime;
                backgrounds = GameObject.FindGameObjectsWithTag("Background");
                hazardMaker = GameObject.FindGameObjectWithTag("Spawner");
                /*for (int i = 0; i < backgrounds.Length; i++)
                {
                    backgrounds[i].GetComponent<BackgroundScroll>().runSpeed += .5f;
                }*/

                /*if(hazardMaker.GetComponent<CreateHazard>().multipleHazRate < 100)
                hazardMaker.GetComponent<CreateHazard>().multipleHazRate += 5;
            }*/
                if (Time.time > nextLightning)
                {
                    nextLightning += Random.Range(10f, 30f); ;
                    StartCoroutine(BeginLightning());
                }


                //hearts.sprite = heartHUD[player.GetComponent<PlayerController>().health];
                background = GameObject.FindGameObjectWithTag("Background");
                //gameScore += (Time.deltaTime * scoreMult);
                //score.text = "Metres travelled: " + Mathf.Round(gameScore);

                if (player.GetComponent<PlayerController>().health == 0)
                {
                    stillPlaying = false;
                    print("game fuckingg over");
                    backgrounds = GameObject.FindGameObjectsWithTag("Background");
                    hazardMaker = GameObject.FindGameObjectWithTag("Spawner");
                    for (int i = 0; i < backgrounds.Length; i++)
                    {
                        backgrounds[i].GetComponent<BackgroundScroll>().runSpeed = 0;
                    }

                    hazardMaker.GetComponent<CreateHazard>().hazardRate = 0;
                    hazardMaker.GetComponent<CreateHazard>().hazardSpeed = 0;
                    gameOver.SetActive(true);
                }
            }

        }
    }
}
