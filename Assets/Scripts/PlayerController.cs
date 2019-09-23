using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public int health = 3;

    public float jumpStrength = 10f;
    public float WindStrength = 10f;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float runSpeed = 10f;
    public float floatingDivider = 4;
    float jumpWindow = 0;
    public AudioSource jumpSound;
    public AudioSource GameMusic;
    public AudioSource gameOver;
    public AudioSource hitSound;
    public GameObject restarter;
    public GameObject hazards;

    private Rigidbody2D player;
    private EdgeCollider2D center;
    private BoxCollider2D playerHitBox;
    private float groundRadius = 0.1f;
    private bool grounded = false;
    private bool hasDoubleJump;
    private Animator anim;
    private bool isCrouching;
    private bool isFloating;
    private bool isBlocking;
    private bool invincible = false;

    GameObject umbrella; 

    public bool GetCrouching()
    {
        return isCrouching;
    }
    public bool GetFloating()
    {
        return isFloating;
    }
    public bool GetBlocking()
    {
        return isBlocking;
    }
    public bool GetDoubleJump()
    {
        return hasDoubleJump;
    }
    public bool GetInvincible()
    {
        return invincible;
    }
    private void Start()
    {
        player = gameObject.GetComponent<Rigidbody2D>();
        center = GameObject.FindWithTag("Center").GetComponent<EdgeCollider2D>();
        playerHitBox = gameObject.GetComponent<BoxCollider2D>();
        umbrella = GameObject.FindGameObjectWithTag("Umbrella");
        hasDoubleJump = true;
        anim = gameObject.GetComponent<Animator>();
        isCrouching = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!invincible)
        {
            if (other.gameObject.tag == ("Debris"))
            {
                if (health > 1)
                {
                    //damage when hit by debris
                    health--;
                    print("OOF");
                    StartCoroutine(Invincibility());
                    hitSound.Play();
                }
                else if (health <= 1)
                {
                    //kill when hp = 0
                    health = 0;
                    
                    Die();
                }
            }
        }
        if (other.gameObject.tag == ("Wall"))
        {
            //instant kill when hit back wall
            health = 0;
            Die();
        }
    }
    IEnumerator Invincibility()
    {
        invincible = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, .5f);
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, .5f);
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        yield return new WaitForSeconds(.5f);
        invincible = false;
    }
    public void Die()
    {
        GameMusic.Stop();
        gameOver.Play();
        print("you lose");
        Jump(jumpStrength);
        player.freezeRotation = false;
        player.AddTorque(5);
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("grounded", grounded);
        if (grounded)
        {
            hasDoubleJump = true;
            anim.SetBool("has double jump", true);
        }

        if(gameObject.transform.position.x < center.transform.position.x && !isBlocking && !isFloating){
            player.velocity = new Vector2(runSpeed, player.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow)){
            //this section is handled in Update()
        }
        else
        {
            player.velocity = new Vector2(0, player.velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || !grounded) && !(Input.GetKey(KeyCode.DownArrow)))
            {
                if (grounded)
                {
                    jumpWindow = Time.time + .15f;
                    grounded = false;
                    Jump(jumpStrength);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow) && hasDoubleJump && !grounded)
                {
                    jumpWindow = Time.time + .15f;
                    hasDoubleJump = false;
                    player.velocity = new Vector2(0, 0);
                    Jump(jumpStrength);
                    anim.SetBool("has double jump", false);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && Time.time < jumpWindow)
            {
                player.gravityScale = 0;
            }
            if (Input.GetKeyUp(KeyCode.UpArrow) || Time.time > jumpWindow)
            {
                player.gravityScale = 1;
            }

            //floating downward
            if (Input.GetKey(KeyCode.UpArrow) && player.velocity.y < 0 && !hasDoubleJump)
            {
                //player.gravityScale /= floatingDivider;
                player.velocity = new Vector2(player.velocity.x, player.velocity.y / floatingDivider);
                isFloating = true;
            }
             if (Input.GetKeyUp(KeyCode.UpArrow) || (grounded && isFloating))
            {
                //player.gravityScale *= floatingDivider;
                isFloating = false;
            }

            //umbrella deploy
            if (Input.GetKey(KeyCode.RightArrow) && !isCrouching && !isFloating)
            {
               
                player.velocity = new Vector2(WindStrength * (-1), player.velocity.y);

                if (!umbrella.active)
                {
                    umbrella.SetActive(true);
                    anim.SetBool("blocking", true);
                    isBlocking = true;
                }
            }
            else
            {
                //umbrella stow
                isBlocking = false;
                umbrella.SetActive(false);
                anim.SetBool("blocking", false);
            }

            if (Input.GetKey(KeyCode.DownArrow) && grounded)
            {
                if (!(isCrouching))
                {
                    //crouch
                    playerHitBox.size = (playerHitBox.size) / (2);
                    isCrouching = true;
                    anim.SetBool("is crouching", true);
                }
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) && isCrouching)
            {
                //uncrouch
                playerHitBox.size = playerHitBox.size * 2;
                isCrouching = false;
                anim.SetBool("is crouching", false);
            }
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Application.LoadLevel(Application.loadedLevel);
            //Application.LoadLevel(0);
            SceneManager.LoadScene(0);

            //SceneManager.LoadScene("scene");
            //Destroy(gameObject, 2f);
        }
    }
    /*
    public void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Space))

            //Application.LoadLevel(Application.loadedLevel);
            //Application.LoadLevel(0);
            SceneManager.LoadScene("scene");

        //SceneManager.LoadScene("scene");
        //Destroy(gameObject, 2f);
    }*/

    void Jump(float jumpStrength)
    {
        player.AddForce(new Vector2(0, jumpStrength), ForceMode2D.Impulse);
        jumpSound.Play();
    }
}
