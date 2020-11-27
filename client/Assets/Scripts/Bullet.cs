using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GM gm;
    public delegate void GameOver();
    public static event GameOver gameOverEvent;
    void Start()
    {
        gm = GM.FindObjectOfType<GM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collided");
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Player>().health -= 20;
            if (collision.gameObject.GetComponent<Player>().health <= 0)
            {
                collision.gameObject.SetActive(false);
                gm.alivePlayers -= 1;
                if (gm.alivePlayers <= 1) gameOverEvent?.Invoke();
            }
        }
        
        //Rigidbody2D target = collision.GetComponentInParent<Rigidbody2D>();
        //if(target != null)
        //{
        //    Debug.Log("Hit target");
        //}
        gameObject.SetActive(false);
    }
}
