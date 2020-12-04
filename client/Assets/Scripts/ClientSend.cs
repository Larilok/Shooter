using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{

    private static void SendTCPData(Packet packet)
    {
        //Debug.Log("TCP Send");
        packet.WriteLength();
        Client.instance.tcp.SendData(packet);
    }
    private static void SendUDPData(Packet packet)
    {
        // Debug.Log("UDP Send");
        packet.WriteLength();
        Client.instance.udp.SendData(packet);
    }


    public static void WelcomeReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            packet.Write(Client.instance.clientId);
            packet.Write(GameProperties.myName);

            SendTCPData(packet);
        }
    }
    internal static void RoundStart(int clientId)
    {
        using (Packet packet = new Packet((int)ClientPackets.roundStart))
        {
            packet.Write(Client.instance.clientId);
            SendTCPData(packet);
        }
    }
    public static void PlayerMovement(bool[] inputs, bool invert, float aimAngle)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerMovement))
        {
            packet.Write(inputs.Length);
            foreach (bool input in inputs)
            {
                packet.Write(input);
            }
            //packet.Write(GM.players[Client.instance.clientId].transform.rotation);
            packet.Write(invert);
            packet.Write(aimAngle);

            SendUDPData(packet);
        }
    }
    public static void BulletSpawn(Vector3 position, Vector2 velocity)
    {
        using (Packet packet = new Packet((int)ClientPackets.bulletSpawn))
        {
            //packet.Write(position.magnitude + velocity.magnitude);
            //Debug.Log($"CLI SEND Bullet Pos: {position.x} {position.y} {position.z}, vel: {velocity.x} {velocity.y}");
            packet.Write(position);
            packet.Write(velocity);
            SendUDPData(packet);
        }
    }

    public static void EnemyHit(int clientId)
    {
        using (Packet packet = new Packet((int)ClientPackets.enemyHit))
        {
            packet.Write(clientId);

            SendTCPData(packet);
        }
    }
    public static void BoostHandle(int boostId, Vector3 boostPos)
    {
        using (Packet packet = new Packet((int)ClientPackets.boostHandle))
        {
            //packet.Write(position.magnitude + velocity.magnitude);
            //Debug.Log($"CLI SEND Bullet Pos: {position.x} {position.y} {position.z}, vel: {velocity.x} {velocity.y}");
            packet.Write(false);//Removing Boost
            packet.Write(boostId);
            packet.Write(boostPos);
            SendTCPData(packet);
        }
    }
}
