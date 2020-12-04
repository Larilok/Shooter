using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    bool escPressed = false;

    public int maxPlayers = 4;
    public int alivePlayers = 4;
    public static GM instance;

    public TMPro.TextMeshProUGUI roundStartText;
    public bool handlingRoundStart = false;

    enum BoostEnum
    {
        HealthBoost,
        PlayerSpeedBoost,
        BulletSpeedBoost,
        BulletDamageBoost
    }

    public GameObject playerPrefab;
    public GameObject myPlayerPrefab;
    public static Dictionary<int, Player> players = new Dictionary<int, Player>();
    //public List<GameObject> players;

    //pools
    public ObjectPool bulletPool;
    public static ObjectPool bulletPoolInstance;
    public ObjectPool healthBoostPool;
    public static ObjectPool healthBoostPoolInstance;
    public ObjectPool playerSpeedBoostPool;
    public static ObjectPool playerSpeedBoostPoolInstance;
    public ObjectPool bulletSpeedBoostPool;
    public static ObjectPool bulletSpeedBoostPoolInstance;
    public ObjectPool bulletDamageBoostPool;
    public static ObjectPool bulletDamageBoostPoolInstance;

    void Start()
    {
        bulletPoolInstance = bulletPool;
        healthBoostPoolInstance = healthBoostPool;
        playerSpeedBoostPoolInstance = playerSpeedBoostPool;
        bulletSpeedBoostPoolInstance = bulletSpeedBoostPool;
        bulletDamageBoostPoolInstance = bulletDamageBoostPool;
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
    public void HandleRoundStart(bool startHandling)
    {
        if (startHandling)
        {
            roundStartText.gameObject.SetActive(true);
            handlingRoundStart = true;
        }
        else
        {
            roundStartText.gameObject.SetActive(false);
            handlingRoundStart = false;
        }
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
        Debug.Log("Adding a player");
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

    internal static void AddBoost(int boostId, Vector3 boostPos)
    {
        GameObject boost;
        if (boostId == (int)BoostEnum.HealthBoost)//HealthBoost
        {
            boost = healthBoostPoolInstance.GetObject();
            boost.name = "HealthBoost";
        }
        else if (boostId == (int)BoostEnum.PlayerSpeedBoost)//PlayerSpeedBoost
        {
            boost = playerSpeedBoostPoolInstance.GetObject();
            boost.name = "PlayerSpeedBoost";
        }
        else if (boostId == (int)BoostEnum.BulletSpeedBoost)//BulletSpeedBoost
        {
            boost = bulletSpeedBoostPoolInstance.GetObject();
            boost.name = "BulletSpeedBoost";
        }
        else //if (boostId == (int)BoostEnum.BulletDamageBoost)//BulletDamageBoost
        {
            boost = bulletDamageBoostPoolInstance.GetObject();
            boost.name = "BulletDamageBoost";
        }
        boost.transform.position = boostPos;
        boost.SetActive(true);
    }

    internal static void RemoveBoost(int boostId, Vector3 boostPos)
    {
        List<GameObject> boosts;
        if (boostId == (int)BoostEnum.HealthBoost)//HealthBoost
        {
            boosts = healthBoostPoolInstance.GetActiveObjectsInPositionList(boostPos);
        }
        else if (boostId == (int)BoostEnum.PlayerSpeedBoost)//PlayerSpeedBoost
        {
            boosts = playerSpeedBoostPoolInstance.GetActiveObjectsInPositionList(boostPos);
        }
        else if (boostId == (int)BoostEnum.BulletSpeedBoost)//BulletSpeedBoost
        {
            boosts = bulletSpeedBoostPoolInstance.GetActiveObjectsInPositionList(boostPos);
        }
        else //if (boostId == (int)BoostEnum.BulletDamageBoost)//BulletDamageBoost
        {
            boosts = bulletDamageBoostPoolInstance.GetActiveObjectsInPositionList(boostPos);
        }
        boosts.ForEach(b => b.SetActive(false));
    }
    internal void GoToMainMenu()
    {
        if (Client.instance.tcp != null)
        {
            Client.instance.Disconnect();
        }
        players.Clear();

        SceneManager.LoadScene("MainMenu");
    }
    private void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Escape.ToString())))//pressed Escape
        {
            if (escPressed)
            {
                GoToMainMenu();
            }
            else
            {
                escPressed = true;
            }
        }
    }
}
