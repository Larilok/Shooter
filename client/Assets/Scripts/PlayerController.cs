using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    private Transform aimTransform;
    public GameObject player;
    public GameObject muzzle;

    float horizontal;
    float vertical;

    public float runSpeed = 20.0f;

    // Start is called before the first frame update
    void Awake()
    {
        aimTransform = transform.Find("Aim");
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        handleShooting();
    }

    private void handleShooting()
    {
        if (Input.GetMouseButtonDown(0))//shooting
        {
            GameObject bullet = ObjectPool.SharedInstance.GetObject();
            if (bullet != null)
            {
                //Vector3 shotDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - muzzle.transform.position).normalized;
                Vector3 muzzlePos = muzzle.transform.position;
                Vector3 shotPos = (muzzlePos - player.transform.position).normalized;
                Debug.Log("ShotPos:" + shotPos);
                bullet.transform.position = muzzle.transform.position;
                //bullet.transform.rotation = muzzle.transform.rotation;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shotPos.x * 20, shotPos.y * 20);
            }
            else
            {
                Debug.Log("NOT ENOUGH BULLETS!!!!!!");
            }
        }
    }

    private void handleMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }
    private void FixedUpdate()
    {
        handleMovement();
        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}
