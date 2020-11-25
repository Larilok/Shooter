using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
namespace server
{
  public class Server
  {
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }

    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

    public static UdpClient udpClient;

    public static void Start(int maxPlayers, int port)
    {
      MaxPlayers = maxPlayers;
      Port = port;

      Console.WriteLine($"Starting server...");
      InitServerData();
    }


    public static void InitServerData()
    {
      for (int i = 1; i <= MaxPlayers; i++)
      {
        clients.Add(i, new Client(i));
      }
    }
    public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
    {
      try
      {
        if (clientEndPoint != null)
        {
          udpClient.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error sending data to {clientEndPoint} via UDP: {ex}");
      }
    }
  }
}