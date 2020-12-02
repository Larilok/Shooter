using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{

    private static void SendTCPData(Packet packet)
    {
        Debug.Log("TCP Send");
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
            packet.Write(Client.instance.myName);

            SendTCPData(packet);
        }
    }

    public static void PlayerMovement(bool[] inputs, bool invert, float aimAngle)
    {
        // Debug.Log("Pl move");
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
}
