using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilMouseScript : MonoBehaviour
{
    //player object
    public PlayerScript player;

    //animator componenet
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {   
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.GetCheezBool().ToString());
        if(player.GetCheezBool() == true)
        {
            anim.Play("scared_mouse_walking");
        }
        else
        {
            anim.Play("evil_mouse_walking");
        }
    }
}
