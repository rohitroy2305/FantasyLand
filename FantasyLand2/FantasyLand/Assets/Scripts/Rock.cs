using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private Vector3 respawnPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity=new Vector2(0,rb.velocity.y);
    }
    private void Update()
    {
        rb.velocity=new Vector2(0,rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Fall")
        {
            //StartCoroutine(Respawn());
            transform.position=respawnPoint;
            rb.velocity=new Vector2(0,0);
        }
    }

}
