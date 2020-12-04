using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerBody;
    public GameObject player;
    public GameObject aim;
    public GameObject gun;
    public GameObject muzzle;

    float horizontal;
    float vertical;

    public float runSpeed = 20.0f;
    public float bulletSpeedMultiplier = 1;
    public const int multiplierDuration = 30000;

    void Awake()
    {
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        handleShooting();
    }
    private void FixedUpdate()
    {
        handleMovement();
    }
    private void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent(KeyCode.Space.ToString())))//pressed Space
        {
            if (GM.instance.handlingRoundStart)
            {
                ClientSend.RoundStart(Client.instance.clientId);
                GM.instance.HandleRoundStart(false);
            }
        }
    }
    private void handleShooting()
    {
        if (Input.GetMouseButtonDown(0))//shooting
        {
            GameObject bullet = GM.bulletPoolInstance.GetObject();
            Vector3 shotPos = (muzzle.transform.position - player.transform.position).normalized;
            bullet.transform.position = muzzle.transform.position;
            bullet.SetActive(true);
            Vector2 velocity = new Vector2(shotPos.x * 20, shotPos.y * 20)*bulletSpeedMultiplier;
            bullet.GetComponent<Rigidbody2D>().velocity = velocity;
            bullet.GetComponent<Bullet>().Deactivate(10);//TODO IMPROVE
            ClientSend.BulletSpawn(muzzle.transform.position, velocity);
        }
    }

    internal async void ResetBulletSpeedMultiplierDelayed(int multiplierDuration)
    {
        await Task.Delay(multiplierDuration);
        bulletSpeedMultiplier = 1f;
    }

    private void handleMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        bool[] input = new bool[4];
        if (vertical == 1) input[0] = true;
        if (vertical == -1) input[1] = true;
        if (horizontal == -1) input[2] = true;
        if (horizontal == 1) input[3] = true;

        (bool, float) aim = getAim();
        ClientSend.PlayerMovement(input, aim.Item1, aim.Item2);
    }

    private (bool, float) getAim()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        bool invert;
        if (angle > 90 || angle < -90)
        {
            invert = true;
        }
        else
        {
            invert = false;
        }
        return (invert, angle);
    }

    private void handleAiming()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        Vector3 localScale = Vector3.one * 2;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -2f;
        }
        else
        {
            localScale.y = +2f;
        }
        aim.transform.localScale = localScale;
        aim.transform.eulerAngles = new Vector3(0, 0, angle);

    }

}
