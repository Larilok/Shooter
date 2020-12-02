using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        handleShooting();
        //handleAiming();
    }
    private void FixedUpdate()
    {
        handleMovement();
        // handleMovement2();
    }

    private void handleShooting()
    {
        if (Input.GetMouseButtonDown(0))//shooting
        {
            //Debug.Log("Shooting");
            //Transform goodMuzzleTransform;
            //if(gun.GetComponent<SpriteRenderer>().flipY)
            //{
            //    goodMuzzleTransform = muzzle.transform;
            //} else
            //{
            //    goodMuzzleTransform = muzzle.transform;
            //}
            GameObject bullet = ObjectPool.SharedInstance.GetObject();
            Vector3 shotPos = (muzzle.transform.position - player.transform.position).normalized;
            //Debug.Log("ShotPos:" + shotPos);
            bullet.transform.position = muzzle.transform.position;
            //bullet.transform.rotation = muzzle.transform.rotation;
            bullet.SetActive(true);
            Vector2 velocity = new Vector2(shotPos.x * 20, shotPos.y * 20);
            bullet.GetComponent<Rigidbody2D>().velocity = velocity;
            StartCoroutine(DeactivateBullet(bullet, 10));
            ClientSend.BulletSpawn(muzzle.transform.position, velocity);
        }
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
        //Debug.Log("Angle: " + angle);
        //Vector3 localScale = Vector3.one * 2;
        bool invert;
        if (angle > 90 || angle < -90)
        {
            //localScale.y = -2f;
            invert = true;
        }
        else
        {
            //localScale.y = +2f;
            invert = false;
        }
        return (invert, angle);
    }

    // private void handleMovement2()
    // {
    //     Debug.Log("HANDLING MOVEMENT");
    //     Vector3 input = Vector3.zero;
    //     input.x = Input.GetAxisRaw("Horizontal");
    //     input.y = Input.GetAxisRaw("Vertical");
    //     //Debug.Log("X: " + input.x);
    //     Vector3 direction = input.normalized;
    // }

    private void handleAiming()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        Vector3 localScale = Vector3.one*2;
        if(angle > 90 || angle < -90)
        {
            localScale.y = -2f;
        } else
        {
            localScale.y = +2f;
        }
        aim.transform.localScale = localScale;
        aim.transform.eulerAngles = new Vector3(0, 0, angle);

    }

    public static IEnumerator DeactivateBullet(GameObject toDeactivate, int deactivateIn)
    {
        yield return new WaitForSeconds(deactivateIn);
        toDeactivate.SetActive(false);
    }
}
