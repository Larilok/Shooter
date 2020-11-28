using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{

  private static void SendTCPData(Packet packet)
  {
    packet.WriteLength();
    Client.instance.tcp.SendData(packet);
  }
  private static void SendUDPData(Packet packet)
  {
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
  
  public static void PlayerMovement(bool[] inputs)
  {
    using (Packet packet = new Packet((int)ClientPackets.playerMovement))
    {
      packet.Write(inputs.Length);
      foreach (bool input in inputs)
      {
        packet.Write(input);
      }
      packet.Write(GM.players[Client.instance.clientId].transform.rotation);

      SendUDPData(packet);
    }
  }
}
