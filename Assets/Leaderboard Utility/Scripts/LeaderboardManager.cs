using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;
    
    public string entrySceneName;
    public GameObject highscoreUI;
    public Transform leadboardParent;
    public GameObject scorePrefab;

    //a private list of the data we'll be populating our scoreboard with
    private List<Score> _scores = new List<Score>();
    private List<GameObject> _uiscores = new List<GameObject>();
    private int newScoreIndex;
    private int inputCharIndex;
    private AudioSource _source;


    private void Start()
    {
        if (instance != null)
            Debug.LogError("You have more than one LeaderboardManager in your scene, you can only have one");
        else
            instance = this;

        newScoreIndex = -1;
        inputCharIndex = 0;
        _source = this.GetComponent<AudioSource>();

        //load the scores using unity's playerprefs system
        for (int i = 1; i <= 10; i++)
        {
            //if there is a score saved, load it. If not, add a placeholder
            if (PlayerPrefs.HasKey(i + "initials"))
                _scores.Add(new Score(PlayerPrefs.GetString(i + "initials"), PlayerPrefs.GetInt(i + "value")));
            else
                _scores.Add(new Score("AAA", 0));
        }
    }

    private void Update()
    {
        //only get input if someone scored a new high score
        if (newScoreIndex != -1)
        {
            //grab the text
            char[] text = _scores[newScoreIndex].initials.ToCharArray();

            //get the keyboard input from this frame and loop through each character
            foreach (char c in Input.inputString)
            {
                if (c == '\b') //hit backspace, so move back an index but never past 0
                {
                    if (inputCharIndex == 2 && text[inputCharIndex] != '_')
                    {
                        text[inputCharIndex] = '_';
                    }
                    else
                    {
                        inputCharIndex = Mathf.Max(0, inputCharIndex - 1);
                        text[inputCharIndex] = '_';
                    }
                }
                else if ((c == '\n') || (c == '\r')) //hit enter
                {
                    //if you were on the last initial, enter the initials and save the score
                    if (inputCharIndex == 2)
                    {
                        for (int i = 1; i <= _scores.Count; i++)
                        {
                            PlayerPrefs.SetString(i + "initials", _scores[i - 1].initials);
                            PlayerPrefs.SetInt(i + "value", _scores[i - 1].value);
                        }
                        PlayerPrefs.Save();
                        Invoke("RestartGame", 5f);
                        break;
                    }
                }
                else { //hit a letter, record it and move to the next letter
                    text[inputCharIndex] = char.ToUpper(c);
                    inputCharIndex = Mathf.Min(inputCharIndex + 1, 2);
                }
            }



            //update the score with our new string
            _scores[newScoreIndex].initials = new string(text);
            _uiscores[newScoreIndex].transform.GetChild(0).GetComponent<Text>().text = new string(text);
            
        }
    }

    public void AddScoreAndDisplayLeaderboard(int score)
    {
        //the scores are always sorted highest to lowest
        for (int i = 0; i < _scores.Count; i++) {
            
            //if it's higher than the score, we insert it into the scores and drop the lowest one
            if (score > _scores[i].value) {
                _scores.Insert(i, new Score("___", score));
                _scores.RemoveAt(_scores.Count - 1);
                newScoreIndex = i;
                break;
            }
        }

        //if no new high score, just show the leaderboard and restart in 5 seconds
        if (newScoreIndex == -1)
            Invoke("RestartGame", 5f);

        ShowLeaderboard();
    }

    private void ShowLeaderboard()
    {
        highscoreUI.SetActive(true);

        //only play the sound effect if it's a new high score
        if (newScoreIndex != -1)
            _source.Play();

        foreach(Score s in _scores) {
            GameObject g = GameObject.Instantiate(scorePrefab, leadboardParent);
            g.transform.GetChild(0).GetComponent<Text>().text = s.initials;
            g.transform.GetChild(1).GetComponent<Text>().text = s.value.ToString("D6");
            _uiscores.Add(g);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(entrySceneName, LoadSceneMode.Single);
    }

    public void DeleteHighscores()
    {
        PlayerPrefs.DeleteAll();
    }

    public string HighScore()
    {
        return _scores[0].value.ToString();
    }

}
