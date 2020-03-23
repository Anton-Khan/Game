using System.Net.Sockets;

namespace Server
{
    class ConnectedClient
    {
        public TcpClient Client { get; set; }
        public int Id { get; set; }

        public ConnectedClient(TcpClient client, int id)
        {
            Client = client;
            Id = id;
        }
        
    }
}