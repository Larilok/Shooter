using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    public static Client instance;

    public string ip = "127.0.0.1";
    public int port = 8888;
    
    public int myId = 0;
    public UDP udp;
    
    private void Awake() {
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
    
    public void ConnectToServer() {
         
    }
    
    public class UDP
    {
      public UdpClient socket;
      public IPEndPoint endPoint;
      

      public UDP() {
        endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
      }
      
      public void Connect(int localPort) {
        //socket = new UdpClient(localPort);

        //socket.Connect(endPoint);
        //socket.BeginReceive(ReceiveCallback, null);

        //using (Packet packet = new Packet())
        //{
        //        SendData(packet);
        //}
      }
    }
      
      // public void SendData(Packet packet) {
      //   Server.sendUDPData(endPoint, packet);
      // }
      
      // public void HandleData(Packet packetData) {
      //   int packetLength = packetData.ReadInt();
      //   byte[] packetBytes = packetData.ReadBytes(packetLength);
      // }
    //}

}
