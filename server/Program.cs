using System;

namespace server
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      Server.Start(4, 8888);
    }
  }
}
