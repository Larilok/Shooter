using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
}
