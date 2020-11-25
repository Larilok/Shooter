using System;
using System.Net;
using System.Net.Sockets;

namespace server
{
  public class Client
  {

    public int id; 
    public UDP udp;

    public Client(int clientId)
    {
      this.id = clientId;
      udp = new UDP(id);
    } 

    public class UDP
    {
      public UdpClient socket;
      public IPEndPoint endPoint;
      
      private int id;

      public UDP(int id) {
        this.id = id;
      }
      
      public void Connect(IPEndPoint endPoint) {
        this.endPoint = endPoint;
      }
      
      // public void SendData(Packet packet) {
      //   Server.sendUDPData(endPoint, packet);
      // }
      
      // public void HandleData(Packet packetData) {
      //   int packetLength = packetData.ReadInt();
      //   byte[] packetBytes = packetData.ReadBytes(packetLength);
      // }
    }
  }
}