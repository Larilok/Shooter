using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public int maxPlayers = 4;
    public int alivePlayers = 4;
    public List<GameObject> players;
    
    void Start()
    {
        
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
        GameObject winner = players[0];
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].activeInHierarchy)
            {
                Debug.Log("Updating Winner");
                winner = players[i];
                break;
            }
        }
        GameProperties.winnerName = winner.name;
        SetWinnerString(winner.name);
        SceneManager.LoadScene("MainMenu");
    }
    public void SetWinnerString(string winnerName)
    {
        GameProperties.winnerString = "Round Winner:\n" + winnerName;
    }

}
