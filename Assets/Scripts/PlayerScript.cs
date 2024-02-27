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
    private bool isWalking = false;

    //animation boolean
    private bool isAnimating = false;

    //direction variable
    private string direction;
    
    //collision function 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Cheez")
        {
            Destroy(collision.gameObject);
        }
    }

    //function for moving in certain directions
    private void MoveDirection(string direction)
    {
        //handle movement
        if(direction == "up")
        {
            transform.position += new Vector3(0,speed,0) * Time.deltaTime;
        }
        else if(direction == "left")
        {
            transform.position += new Vector3(-speed,0,0) * Time.deltaTime;
            transform.rotation = new Quaternion(0,180,0,0);
        }
        else if(direction == "down")
        {
            transform.position += new Vector3(0,-speed,0) * Time.deltaTime;
        }
        else if(direction == "right")
        {
            transform.position += new Vector3(speed,0,0) * Time.deltaTime;
            transform.rotation = new Quaternion(0,0,0,0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //handle movement
        if(Input.GetKeyDown(KeyCode.W))
        {
            direction = "up";
            isWalking = true;
            isAnimating = true;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            direction = "left";
            isWalking = true;
            isAnimating = true;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            direction = "down";
            isWalking = true;
            isAnimating = true;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            direction = "right";
            isWalking = true;
            isAnimating = true;
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
        
        //move player
        if(isWalking)
        {
            MoveDirection(direction);
        }

        //if walking, play walking animation, otherwise idle
        if(isAnimating)
        {
            anim.Play("mister_cat_walk");
        }
        else
        {
            anim.Play("mister_cat_idle");
        }
    }
}