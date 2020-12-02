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
        
        Debug.Log($"Message from server: {msg}");
        Client.instance.clientId = clientId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet packet)
    {
        int id = packet.ReadInt();
        string username = packet.ReadString();
        Vector3 position = packet.ReadVector3();
        Quaternion rotation = packet.ReadQuaternion();

        GM.instance.SpawnPlayer(id, username, position, rotation);
    }

    public static void PlayerPosition(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();
        Debug.Log($"Position: {position}");

        GM.players[id].transform.position = position;
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
        GM.players[id].aim.transform.localScale = localScale;
        GM.players[id].aim.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public static void PlayerRotation(Packet packet)
    {
        int id = packet.ReadInt();
        //Quaternion rotation = packet.ReadQuaternion();

        //GM.players[id].transform.rotation = rotation;
    }
}
