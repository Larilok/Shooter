using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;

public class Client : MonoBehaviour
{
  public static Client instance;

  public string ip = "127.0.0.1";
  public int port = 8888;
    
  public int clientId = 0;
  public UDP udp;

  private delegate void PacketHandler(Packet packet);
  private static Dictionary<int, PacketHandler> packetHandlers;

  private void Awake() {
        Debug.Log("Client Awake");
    if (instance == null)
    {
      instance = this;
    }
    else if (instance != this)
    {
      Debug.Log("Client already exists. Destroying...");
      Destroy(this);
    }
  }

  private void Start() {
    udp = new UDP();
  }
  
  public void ConnectToServer()
  {
    InitializeClientData();
        udp.Connect(8889);
  }
    
  public class UDP
  {
    public UdpClient socket;
    public IPEndPoint endPoint;

    public UDP()
    {
      endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
    }

    public void Connect(int localPort)
    {
      socket = new UdpClient(localPort);

      socket.Connect(endPoint);
      socket.BeginReceive(ReceiveCallback, null);

      using (Packet packet = new Packet())
      {
                packet.Write(1234567890123456789);
        Debug.Log("Sending: " + packet);
        SendData(packet);
      }
    }

    public void SendData(Packet packet)
    {
      try
      {
        packet.InsertInt(instance.clientId);
        if (socket != null)
        {
                    Debug.Log("Begining Ttransmition");
          socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
        }
      }
      catch (Exception ex)
      {
        Debug.Log($"Error sending data to server via UDP: {ex}");
      }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
      try
      {
        byte[] data = socket.EndReceive(result, ref endPoint);
        socket.BeginReceive(ReceiveCallback, null);

        if (data.Length < 4)
        {
          // TODO: disconnect
          return;
        }

        HandleData(data);
      }
      catch
      {
        // TODO: disconnect
      }
    }

    private void HandleData(byte[] data)
    {
      using (Packet packet = new Packet(data))
      {
        int packetLength = packet.ReadInt();
        data = packet.ReadBytes(packetLength);
      }

      ThreadManager.ExecuteOnMainThread(() =>
      {
        using (Packet packet = new Packet(data))
        {
          int packetId = packet.ReadInt();
          packetHandlers[packetId](packet);
        }
      });
    }
  }
  private void InitializeClientData()
  {
    packetHandlers = new Dictionary<int, PacketHandler>()
    {
      { (int)ServerPackets.welcome, ClientHandle.Welcome },
      // { (int)ServerPackets.udpTest, ClientHandle.UDPTest }
    };
    Debug.Log("Initialized packets.");
  }
}
