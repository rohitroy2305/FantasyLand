using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : Enemy
{
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float ceilCap;
    [SerializeField] private float floorCap;

    [SerializeField] private float flyLength = 10f;
    [SerializeField] private float flydepth = -4f;
    [SerializeField] private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        Move();
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
                //Test to see if eagle is not on ground, if so fly
                if (!coll.IsTouchingLayers(ground))
                {
                    //print("Leftcheck");
                    if((transform.position.y < ceilCap))
                    {
                        rb.velocity = new Vector2(-flyLength, flydepth);
                    }
                    /*else {
                        rb.velocity = new Vector2(-flyLength, flydepth);
                    }*/
                    else if(transform.position.y < floorCap)
                    {
                        rb.velocity = new Vector2(flyLength, -flydepth);
                    }
                    else
                    {
                        //rb.velocity = new Vector2(flyLength, -flydepth);
                    }
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
                //Test to see if agle is not on ground, if so fly
                if (!coll.IsTouchingLayers(ground))
                {
                    if (transform.position.y < ceilCap)
                    {
                        rb.velocity = new Vector2(flyLength, flydepth);
                    }
                    
                    if (transform.position.y < floorCap)
                    {
                        rb.velocity = new Vector2(flyLength, -flydepth);
                    }
                    else
                    {
                        //rb.velocity = new Vector2(flyLength, -flydepth);
                    }
                }

            }
            else
            {
                facingLeft = true;  //if it is not we are going to face left
            }

        }
    }

}
