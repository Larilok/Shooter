using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int clientId = packet.ReadInt();
        if (clientId == 1) GM.instance.HandleRoundStart(true);
        Debug.Log($"Message from server: {msg}");
        Client.instance.clientId = clientId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet packet)
    {
        Debug.Log("Spawn player packet");
        int id = packet.ReadInt();
        string username = packet.ReadString();
        Vector3 position = packet.ReadVector3();
        Quaternion rotation = packet.ReadQuaternion();

        GM.instance.SpawnPlayer(id, username, position, rotation);
    }
    public static void PlayerPosition(Packet packet)
    {
        int playerId = packet.ReadInt();
        if(!GM.players.ContainsKey(playerId)) return;

        Vector3 position = packet.ReadVector3();
        GM.players[playerId].transform.position = position;
        bool invert = packet.ReadBool();
        float angle = packet.ReadFloat();
        Vector3 localScale = Vector3.one * 2;
        if (invert)
        {
            localScale.y = -2f;
        }
        else
        {
            localScale.y = +2f;
        }
        GM.players[playerId].aim.transform.localScale = localScale;
        GM.players[playerId].aim.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public static void PlayerHealth(Packet packet)
    {
        int hitPlayerId = packet.ReadInt();
        int hitPlayerHealth = packet.ReadInt();
        GM.players[hitPlayerId].SetHealth(hitPlayerHealth);
    }

    public static void BulletSpawn(Packet packet)
    {
        Vector3 bulletPosition = packet.ReadVector3();
        Vector3 bulletVelocity = packet.ReadVector2();
        GameObject bullet = GM.bulletPoolInstance.GetObject();
        bullet.transform.position = bulletPosition;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().velocity = bulletVelocity;
        bullet.GetComponent<Bullet>().Deactivate(10);//TODO IMPROVE
    }

    public static void PlayerRotation(Packet packet)
    {
        int id = packet.ReadInt();
    }
    
    public static void PlayerDisconnected(Packet packet)
    {
        int playerId = packet.ReadInt();
        if(!GM.players.ContainsKey(playerId)) return;

        Destroy(GM.players[playerId].gameObject);
        GM.players.Remove(playerId);
    }

    public static void BoostHandler(Packet packet)
    {
        bool intent = packet.ReadBool();
        int boostId = packet.ReadInt();//type of boost
        Vector3 boostPos = packet.ReadVector3();//boost Position
        if (intent) GM.AddBoost(boostId, boostPos);//Adding boost
        else GM.RemoveBoost(boostId, boostPos);//Removing Boost
    }

    internal static void killCountUpdate(Packet packet)
    {
        int killerId = packet.ReadInt();
        if (GM.players.ContainsKey(killerId)) GM.players[killerId].IncrementKillCount();
    }
}
