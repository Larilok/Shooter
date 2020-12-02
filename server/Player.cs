using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace server
{
  public class Player
  {
    public int id;
    public string username;
    public int health = 100;

    public int damage = 20;
    //public List<(Vector3, Vector2)> bulletList;

    public float damageMultiplier = 1;
    public float moveSpeedMultiplier = 1;
    public float bulletSpeedMultiplier = 1;

    public Vector3 position;
    public Quaternion rotation;
    public bool invertGunSprite;
    public float gunRotation;

    private float moveSpeed = 20f / Constants.TICKS_PER_SEC;
    private int verticalBorder = 24;
    private int horizontalBorder = 45;
    private bool[] inputs;

    public Player(int id, string username, Vector3 spawnPosition)
    {
      this.id = id;
      this.username = username;
      //this.bulletList = new List<(Vector3, Vector2)>();
      position = spawnPosition;
      rotation = Quaternion.Identity;

      inputs = new bool[4];
    }

    public void Update()
    {
      MoveRotate();
    }

    private void MoveRotate()
    {
      Vector2 inputDirection = Vector2.Zero;
      if (inputs[0])
      {
        inputDirection.Y += 1;
      }
      if (inputs[1])
      {
        inputDirection.Y -= 1;
      }
      if (inputs[2])
      {
        inputDirection.X -= 1;
      }
      if (inputs[3])
      {
        inputDirection.X += 1;
      }
      // Vector3 forward = Vector3.Transform(new Vector3(0, 1, 0), rotation);
      // Vector3 right = Vector3.Normalize(Vector3.Cross(forward, new Vector3(0, 1, 0)));
      Vector3 forward = new Vector3(0, 1, 0);
      Vector3 right = new Vector3(1, 0, 0);


      Vector3 moveDirection = right * inputDirection.X + forward * inputDirection.Y;
      position += moveDirection * moveSpeed;
      if (position.X < -horizontalBorder) position.X = -horizontalBorder;
      if (position.X > horizontalBorder) position.X = horizontalBorder;
      if (position.Y < -verticalBorder) position.Y = -verticalBorder;
      if (position.Y > verticalBorder) position.Y = verticalBorder;


      //Rotate

      ServerSend.PlayerPosition(this);
    }

    public void SetInput(bool[] inputs, Quaternion rotation)
    {
      this.inputs = inputs;
      this.rotation = rotation;
    }
    public void SetInput(bool[] inputs, bool invert, float aim)
    {
      this.inputs = inputs;
      this.invertGunSprite = invert;
      this.gunRotation = aim;

    }
    public void SetHealth(int hitByClientId)
    {
      Player hitByPlayer = Server.clients[hitByClientId].player;
      this.health -= hitByPlayer.CalculateDamage();
    }

    public int CalculateDamage()
    {
      return (int)(this.damage * this.damageMultiplier);
    }
  }
}
