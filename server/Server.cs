using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
namespace server
{
    public class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        private static bool roundStarted = false;

        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        public static Dictionary<int, BoostSpawner> spawners = new Dictionary<int, BoostSpawner>();
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

            if (roundStarted)
            {
                Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Round started!");
                return;
            }
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
                spawners.Add(i, new BoostSpawner(i));
            }
            packetHandlers = new Dictionary<int, PacketHandler>()
      {
        { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
        { (int)ClientPackets.playerMovement, ServerHandle.PlayerMovement },
        { (int)ClientPackets.enemyHit, ServerHandle.EnemyHit },
        { (int)ClientPackets.bulletSpawn, ServerHandle.BulletSpawn },
        { (int)ClientPackets.boostHandle, ServerHandle.BoostHandle },
        { (int)ClientPackets.roundStart, ServerHandle.RoundStart },
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
        public static int ClientsCount()
        {
            int count = 0;
            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (clients[i].tcp.socket != null)
                {
                    count++;
                }
            }
            return count;
        }
        public static void StartRound()
        {
            Console.WriteLine($"Round Started");

            roundStarted = true;
            foreach (BoostSpawner bs in spawners.Values)
            {
                bs.SpawnItem();
            }
        }
        public static void StopRound()
        {
            Console.WriteLine($"Round stop");

            roundStarted = false;
        }

        public static bool IsRoundStarted()
        {
            return roundStarted;
        }

    }
}