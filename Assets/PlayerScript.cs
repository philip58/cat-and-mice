using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public speed variable
    public int speed;

    //public animation variable
    public Animator anim;

    //if walking boolean
    bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //make idle when no keys are pressed
        isWalking = false;

        //handle movement
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0,speed,0) * Time.deltaTime;
            isWalking = true;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-speed,0,0) * Time.deltaTime;
            isWalking = true;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0,-speed,0) * Time.deltaTime;
            isWalking = true;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed,0,0) * Time.deltaTime;
            isWalking = true;
        }

        //wrap around screen if out of bounds
        if(transform.position.y > 7.3)
        {
            transform.position = new Vector3(transform.position.x,-5.6f,0);
        }
        if(transform.position.y < -5.6)
        {
            transform.position = new Vector3(transform.position.x,7.2f,0);
        }
        if(transform.position.x > 10.3)
        {
            transform.position = new Vector3(-10.5f,transform.position.y,0);
        }
        if(transform.position.x < -10.5)
        {
            transform.position = new Vector3(10.3f,transform.position.y,0);
        }

        //if walking, play walking animation, otherwise idle
        if(isWalking)
        {
            anim.Play("mister_cat_walk");
        }
        else
        {
            anim.Play("mister_cat_idle");
        }
    }
}
