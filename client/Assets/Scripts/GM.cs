 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public int maxPlayers = 4;
    public int alivePlayers = 4;
    public static GM instance;
    
    public GameObject playerPrefab;
    public GameObject myPlayerPrefab;
    public static Dictionary<int, Player> players = new Dictionary<int, Player>();
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

        Client.instance.ConnectToServer();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        Player.gameOverEvent += GameOverEventhandler;
    }
    private void OnDisable()
    {
        Player.gameOverEvent -= GameOverEventhandler;
    }
    private void GameOverEventhandler()
    {
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
    }
    public void SetWinnerString(string winnerName)
    {
        GameProperties.winnerString = "Round Winner:\n" + winnerName;
    }
    
    public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation)
    {
        GameObject player;
        if (id == Client.instance.clientId)
        {
            player = Instantiate(myPlayerPrefab, position, rotation);
        }
        else
        {
            player = Instantiate(playerPrefab, position, rotation);
        }

        player.GetComponent<Player>().id = id;
        player.GetComponent<Player>().username = username;
        players.Add(id, player.GetComponent<Player>());
    }

}
