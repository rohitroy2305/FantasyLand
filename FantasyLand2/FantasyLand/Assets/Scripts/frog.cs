using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : Enemy
{ 
    private Collider2D coll;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    private bool facingLeft = true;


    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        //transition from Jump to Fall
        if (anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < 0.1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        //transition from Fall to idle
        if ((coll.IsTouchingLayers(ground))&& anim.GetBool("Falling"))
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            //Test to see if we are beyond leftCap
            if (transform.position.x > leftCap)
            {
                //makes sure sprite is facing right direction and if it is not then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                //Test to see if frog in on ground, if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }

            }
            else
            {
                facingLeft = false; //if it is not we are going to face right
            }

        }
        else
        {
            //Test to see if we are beyond rightCap
            if (transform.position.x < rightCap)
            {
                //makes sure sprite is facing left direction and if it is not then face the left direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                //Test to see if frog in on ground, if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }

            }
            else
            {
                facingLeft = true;  //if it is not we are going to face left
            }

        }
    }
   
}

