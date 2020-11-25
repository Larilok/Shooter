using System;
using System.Net.Sockets;
namespace server
{
  public class Server
  {
    public static int MaxPlayers { get; private set;}
    public static int Port { get; private set;}
    
    public static UdpClient client;
    
    public void Start(int maxPlayers, int port) {
      MaxPlayers = maxPlayers;
      Port = port;
      
      Console.WriteLine($"Starting server...");
      
    }
    
  }
  
}