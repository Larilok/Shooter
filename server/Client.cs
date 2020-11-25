namespace server
{
  public class Client
  {

    public int id; 
    public UDP udp;

    public Client(int clientId)
    {
      id = clientId;
      udp = new UDP(id);
        
    } 
    public class UDP
    {
      public UdpClient socket;
      public IPendPoint endPoint;

      public UPD() {
        endPoint = new IPendPoint(IPAddress.Parse)
      }
      
    }
  }
}