 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public int maxPlayers = 4;
    public int alivePlayers = 4;
    public static GM instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    //public List<GameObject> players;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        Bullet.gameOverEvent += GameOverEventhandler;
    }
    private void OnDisable()
    {
        Bullet.gameOverEvent -= GameOverEventhandler;
    }
    private void GameOverEventhandler()
    {
<<<<<<< HEAD
        //GameObject winner = players[0];
        //for (int i = 0; i < players.Count; i++)
        //{
        //    if (players[i].activeInHierarchy)
        //    {
        //        Debug.Log("Updating Winner");
        //        winner = players[i];
        //        break;
        //    }
        //}
        //GameProperties.winnerName = winner.name;
        //SetWinnerString(winner.name);
        //SceneManager.LoadScene("MainMenu");
=======
        GameObject winner = players[0];
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].activeInHierarchy)
            {//
                Debug.Log("Updating Winner");
                winner = players[i];
                break;
            }
        }
        GameProperties.winnerName = winner.name;
        SetWinnerString(winner.name);
        SceneManager.LoadScene("MainMenu");
>>>>>>> 70360820296540e3b6724e0f0e7fe1655b0bcd85
    }
    public void SetWinnerString(string winnerName)
    {
        GameProperties.winnerString = "Round Winner:\n" + winnerName;
    }

}
