using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public delegate void GameOver();
    public static event GameOver gameOverEvent;

    public const int fullHealth = 100;
    public int id;
    public string username;
    public GameObject aim;
    public int health = 100;
    public string playerName;

    public GameObject HealthBar;
    Vector3 localScale;
    void Start()
    {
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        localScale.x = (float)(health * 1.2 / 100);
        HealthBar.transform.localScale = localScale;
    }

    public void SetHealth(int health)
    {
        this.health = health;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (id == Client.instance.clientId)
        {
            Client.instance.Disconnect();
            SceneManager.LoadScene("MainMenu");
            return;
        }
        Debug.Log($"Player with id {id} is dead");
        Destroy(GM.players[id].gameObject);
        GM.players.Remove(id);
        GM.instance.alivePlayers -= 1;
        if (GM.instance.alivePlayers <= 1) gameOverEvent?.Invoke();
    }
}
