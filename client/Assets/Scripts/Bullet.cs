﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // public delegate void GameOver();
    // public static event GameOver gameOverEvent;
    public IEnumerator deactivate;
    void Start()
    {
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log($"Bullet hit");
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            if (player.id != 0) {
                ClientSend.EnemyHit(player.id);
                return;
            }
            player.SetHealth(player.health - 20);
            // if (player.health <= 0)
            // {
            //     collision.gameObject.SetActive(false);
            //     GM.instance.alivePlayers -= 1;
            //     if (GM.instance.alivePlayers <= 1) gameOverEvent?.Invoke();
            // }
        }
        StopCoroutine(deactivate);
        gameObject.SetActive(false);
    }
    public void Deactivate(int deactivateIn)
    {
        deactivate = DeactivateRoutine(deactivateIn);
        StartCoroutine(deactivate);
    }
    private IEnumerator DeactivateRoutine(int deactivateIn)
    {
        yield return new WaitForSeconds(deactivateIn);
        this.gameObject.SetActive(false);
    }
}
