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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"!!!!!!!!!!!!!!!!!!!!!!!Player collided. Name: {collision.name}. GameObj Name: {collision.gameObject.name}!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //Player player = collision.gameObject.GetComponent<Player>();
        if (collision.name == "HealthBoost")
        {
            Debug.Log("Health");
        } else if (collision.name == "PlayerSpeedBoost")
        {
            Debug.Log("Speed");
        }
        else if(collision.name == "BulletSpeedBoost")
        {
            Debug.Log("B Speed");
        }
        else if(collision.name == "BulletDamageBoost")
        {
            Debug.Log("Damage");
        }
    }
}
