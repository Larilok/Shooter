using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public delegate void GameOver();
    public static event GameOver gameOverEvent;
    void Start()
    {
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            ClientSend.EnemyHit(player.id);
            // collision.gameObject.GetComponent<Player>().health -= 20;
            // if (collision.gameObject.GetComponent<Player>().health <= 0)
            // {
            //     collision.gameObject.SetActive(false);
            //     GM.instance.alivePlayers -= 1;
            //     if (GM.instance.alivePlayers <= 1) gameOverEvent?.Invoke();
            // }
        }
        
        gameObject.SetActive(false);
    }
}
