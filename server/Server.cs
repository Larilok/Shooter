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
    public delegate void PacketHandler(int fromClient, Packet packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpClient;

    public static void Start(int maxPlayers, int port)
    {
      MaxPlayers = maxPlayers;
      Port = port;

      Console.WriteLine($"Starting server...");
      InitServerData();
      tcpListener = new TcpListener(IPAddress.Any, Port);
      tcpListener.Start();
      tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

      udpClient = new UdpClient(Port);
      udpClient.BeginReceive(UDPReceiveCallback, null);

      Console.WriteLine($"Server started on port {Port}.");
    }

    private static void TCPConnectCallback(IAsyncResult result)
    {
      TcpClient client = tcpListener.EndAcceptTcpClient(result);
      tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
      Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");

      for (int i = 1; i <= MaxPlayers; i++)
      {
        if (clients[i].tcp.socket == null)
        {
          clients[i].tcp.Connect(client);
          return;
        }
      }

      Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    private static void UDPReceiveCallback(IAsyncResult result)
    {
      try
      {
        IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = udpClient.EndReceive(result, ref clientEndPoint);
        udpClient.BeginReceive(UDPReceiveCallback, null);

        if (data.Length < 4)
        {
          return;
        }

        using (Packet packet = new Packet(data))
        {
          int clientId = packet.ReadInt();

          if (clientId == 0)
          {
            return;
          }

          if (clients[clientId].udp.endPoint == null)
          {
            clients[clientId].udp.Connect(clientEndPoint);
            return;
          }

          if (clients[clientId].udp.endPoint.ToString() == clientEndPoint.ToString())
          {
            clients[clientId].udp.HandleData(packet);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error receiving UDP data: {ex}");
      }
    }

    public static void InitServerData()
    {
      for (int i = 1; i <= MaxPlayers; i++)
      {
        clients.Add(i, new Client(i));
      }
      packetHandlers = new Dictionary<int, PacketHandler>()
      {
        { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
        { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
      };
      Console.WriteLine("Initialized packets.");
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