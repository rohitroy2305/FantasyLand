using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    //Start variables
    private Rigidbody2D rb;
    //[SerializeField] private Animator anim;     //if we want the variable to be accessible via unity but still make it private
    private Animator anim;
    private Collider2D coll;
    

    //Finite State Machine
    private enum State { idle, running, jumping, falling, hurt, climb}
    private State state = State.idle;

    //Ladder Variables
    [HideInInspector]public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    public ladder ladder1;
    private float naturalGravity = 2f;
    [SerializeField] float climbSpeed = 3f;

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 12f;
    //[SerializeField] private int cherries = 0;
    //[SerializeField] private int coins = 0;
    //[SerializeField] private TextMeshProUGUI cherryText;
    //[SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource coin;
    [SerializeField] private AudioSource gem;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource hurtaud;
    [SerializeField] private Vector3 respawnPoint;
    //[SerializeField] private int maxhealth;
    //[SerializeField] private int currenthealth;
    [SerializeField] private HealthBar HealthBar;
    //[SerializeField] private TextMeshProUGUI healthAmount;
    float dirx;

    private void Start()
    {     
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        //PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        naturalGravity = rb.gravityScale;
        PermanentUI.perm.currenthealth=PermanentUI.perm.maxhealth;
        HealthBar.SetMaxHealth(PermanentUI.perm.maxhealth);
        PermanentUI.perm.levelNo+=1;
        PermanentUI.perm.Level.text = PermanentUI.perm.levelNo.ToString();
        Cursor.lockState=CursorLockMode.None;
        Cursor.visible=true;
    }
    private void Update()
    {
        Cursor.lockState=CursorLockMode.None;
        Cursor.visible=true;
        if (state == State.climb)
        {
            Climb();
        }
        else if (state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state);   //sets Animation based on Enumerator states
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.tag == "Collectible")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            if (PermanentUI.perm.currenthealth < 10)
            {
                PermanentUI.perm.currenthealth += 1;
            }
                HealthBar.SetHealth(PermanentUI.perm.currenthealth);
            //PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        }
        if (collision.tag == "Currency")
        {
            coin.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.coins += 1;
            PermanentUI.perm.coinText.text = PermanentUI.perm.coins.ToString();
        }
        if (collision.tag == "PowerUp")
        {
            gem.Play();
            Destroy(collision.gameObject);
            jumpForce = 16.5f; speed = 10;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }
        if(collision.tag == "HurtObj" && PermanentUI.perm.currenthealth >0)
        {
            hurtaud.Play();
            state = State.hurt;
            HandleHealth();
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                //Enemy is to the right and therfore the player should be damaged and move left
                rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
            }
            else
            {
                //Enemy is to the left and therfore the player should be damaged and move right
                rb.velocity = new Vector2(hurtForce, rb.velocity.y);
            }
        }
        if (collision.tag == "Poison" && PermanentUI.perm.currenthealth >0)
        {
            hurtaud.Play();
            Destroy(collision.gameObject);
            GetComponent<SpriteRenderer>().color = Color.green;
            /*for (int i = 0; i < 2; i++)
            {
                StartCoroutine(TimeLapse());
            }*/
            HandleHealth();
            StartCoroutine(TimeLapse());
            StartCoroutine(ResetNormalcy());
        }
        if(PermanentUI.perm.currenthealth <= 0 && collision.tag=="Fall")
        {
            StartCoroutine(Respawn());
        }
        else if(collision.tag=="Fall")
        {
            StartCoroutine(Respawn());
        }
        if(collision.tag=="CheckPoint")
        {
            respawnPoint=collision.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && PermanentUI.perm.currenthealth >0)
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                hurtaud.Play();
                state = State.hurt;
                HandleHealth(); //Deals with health, updating UI and will reset level if health<=0
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right and therfore the player should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //Enemy is to the left and therfore the player should be damaged and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void HandleHealth()
    {
        PermanentUI.perm.currenthealth -= 2;
        HealthBar.SetHealth(PermanentUI.perm.currenthealth);
        if (PermanentUI.perm.currenthealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    private void Movement()
    {
        float hdirection = Input.GetAxis("Horizontal");
        float dirx=CrossPlatformInputManager.GetAxis("Horizontal");
        if(canClimb && Mathf.Abs(Input.GetAxis("Vertical"))>.1f) 
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder1.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
        }
        if (hdirection < 0 )    //moving left
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
            //anim.SetBool("running", true);                //currently running paramter deleted- alt way of switching between animations
        }
        else if (hdirection > 0)       //moving right
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
            //anim.SetBool("running", true);
        }
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))    //Jumping
        {
            Jump();
        }
        if(canClimb && Mathf.Abs(CrossPlatformInputManager.GetAxis("Vertical"))>.1f) 
        {
            state = State.climb;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder1.transform.position.x, rb.position.y);
            rb.gravityScale = 0f;
        }          //alt method for hardcoding controls, here we use unity default controls
        if (dirx <0)    //moving left
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
            //anim.SetBool("running", true);                //currently running paramter deleted- alt way of switching between animations
        }
        //else if (Input.GetKey(KeyCode.RightArrow))
        else if ( dirx>0)       //moving right
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
            //anim.SetBool("running", true);
        }
        if(CrossPlatformInputManager.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if( state == State.climb)
        {
            if (rb.velocity.y == 0)
            {
                anim.speed = 0f;
                rb.velocity = Vector2.zero;
            }
        }
        else if (state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            //moving
            state = State.running;
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
            else
            {
                state = State.falling;
            }
        }
    }
    
    private void Footstep()
    {
        footstep.Play();
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        transform.position=respawnPoint;
        state=State.idle;
        rb.velocity = new Vector2(0, 0);
        transform.localScale = new Vector2(1, 1);
        PermanentUI.perm.currenthealth=10;
        HealthBar.SetHealth(PermanentUI.perm.currenthealth);
        PermanentUI.perm.life -= 1;
        if(PermanentUI.perm.life<=0)
        {
            SceneManager.LoadScene("GameOver");
        }
        PermanentUI.perm.lifeCount.text = PermanentUI.perm.life.ToString();
        jumpForce = 14f;
        speed = 6f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private IEnumerator TimeLapse()
    {
        yield return new WaitForSeconds(2);
        HandleHealth();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        jumpForce = 14f;
        speed = 6f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private IEnumerator ResetNormalcy()
    {
        yield return new WaitForSeconds(5);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void Climb()
    {
        if (Input.GetButtonDown("Jump") || CrossPlatformInputManager.GetButtonDown("Jump"))    //Jumping
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            rb.gravityScale = naturalGravity;
            anim.speed = 1f;
            Jump();
            return;
        }
        float vDirection = Input.GetAxis("Vertical");
        float diry=CrossPlatformInputManager.GetAxis("Vertical");
        if((vDirection > .2f ) && !topLadder)    //Climbing up
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        if((vDirection > .2f )&& topLadder)    //Stop Climbing
        {
            rb.velocity = new Vector2(0f, 0);
            anim.speed = 1f;
        }
        else if ((vDirection < .2f )&& !bottomLadder) //Climbing down
        {
            rb.velocity = new Vector2(0f, vDirection * climbSpeed);
            anim.speed = 1f;
        }
        if((diry> .2f ) && !topLadder)    //Climbing up
        {
            rb.velocity = new Vector2(0f,diry * climbSpeed);
            anim.speed = 1f;
        }
        if((diry > .2f )&& topLadder)    //Stop Climbing
        {
            rb.velocity = new Vector2(0f, 0);
            anim.speed = 1f;
        }
        else if ((diry < .2f )&& !bottomLadder) //Climbing down
        {
            rb.velocity = new Vector2(0f, diry * climbSpeed);
            anim.speed = 1f;
        }
        //else    //Still
        else if(rb.velocity.y == 0)
        {
            anim.speed = 0f;
            rb.velocity = Vector2.zero;
        }
    }
        
}
