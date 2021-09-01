using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sign;
    private SpriteRenderer checkpoint;
    public bool checkpointReached;
    void Start()
    {
        checkpoint=GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player")
        {
            checkpointReached=true;
        }
    }
}
