using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PlayerScript : MonoBehaviour
{
    //public speed variable
    public float speed;

    //public animation variable
    public Animator anim;

    //if walking boolean
    private bool isWalking = false;

    //animation boolean
    private bool isAnimating = false;

    //direction variable
    private string direction;

    //score variable
    private int score;
    
    //text variable
    public TMP_Text txt;

    public TMP_Text highScore;

    //leaderboard variable
    public LeaderboardManager leaderboard;

    //cheez game object array
    private GameObject[] cheezArray;

    //enemy game object array
    private GameObject[] enemyArray;

    //enemy spawn point
    public GameObject spawnPoint;

    //game start boolean
    private bool gameStarted;

    //boolean if player ate super cheez
    public bool playerAteSuperCheez = false;

    public bool GetCheezBool()
    {
        return playerAteSuperCheez;
    }

    //respawn enemy after death at its spawn point
    private IEnumerator EnemyDied(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = spawnPoint.transform.position;
        yield return new WaitForSeconds(4);
        obj.SetActive(true);
    }

    //turn off super mode for player
    private IEnumerator SuperMode()
    {
        playerAteSuperCheez = true;
        yield return new WaitForSeconds(8);
        playerAteSuperCheez = false;
    }

    //collision function 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Cheez")
        {
            score+=10;
            //test update score for later leaderboard injection
            
            collision.gameObject.SetActive(false);
        }

        if(collision.gameObject.tag =="EvilMouse" && !playerAteSuperCheez)
        {
            gameObject.SetActive(false);
            leaderboard.AddScoreAndDisplayLeaderboard(score);
        }
        else if(collision.gameObject.tag =="EvilMouse" && playerAteSuperCheez)
        {
            StartCoroutine(EnemyDied(collision.gameObject.transform.parent.gameObject));
        }

        if(collision.gameObject.tag =="SuperCheez")
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(SuperMode());
        }
    }

    //enemy spawning coroutine
    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemyArray.Length; i++)
        {
            yield return new WaitForSeconds(5);

            enemyArray[i].SetActive(true);
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
        playerAteSuperCheez = false;
        cheezArray = GameObject.FindGameObjectsWithTag("Cheez");
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < cheezArray.Length; i++)
        {
            cheezArray[i].SetActive(true);
        }
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i].SetActive(false);
            enemyArray[i].transform.position = spawnPoint.transform.position;
        }
        gameObject.SetActive(true);
        score = 0;
        anim = GetComponent<Animator>();

        //spaning enemy ai
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        //restart game
        if(Input.GetKeyDown(KeyCode.R))
        {
            leaderboard.RestartGame();
        }

        //display the score
        txt.text = "Score: \n" + score;

        highScore.text = "High Score: " + leaderboard.HighScore();

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
