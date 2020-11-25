using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
  public class ServerSend
  {
    public static void SendUDPData(int clientId, Packet packet)
    {
      packet.WriteLength();
      Server.clients[clientId].udp.SendData(packet);
    }

    public static void Welcome(int clientId, string msg)
    {
      using (Packet packet = new Packet((int)ServerPackets.welcome))
      {
        packet.Write(msg);
        packet.Write(clientId);
        SendUDPData(clientId, packet);
      }

    }

  }

}