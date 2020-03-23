    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static TcpListener listener = new TcpListener(IPAddress.Any, 1234);
        static List<ConnectedClient> clients = new List<ConnectedClient>();

        static void Main(string[] args)
        {
            listener.Start();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                

                Task.Factory.StartNew(() =>
                {
                    var sr = new StreamReader(client.GetStream());

                    while (client.Connected)
                    {
                        string st = sr.ReadLine();
                        string[] line = st.Split(' ');
                        int id = Convert.ToInt32(line[0]);
                        if (clients.FirstOrDefault(s => s.Id == id) == null)
                        {
                           
                            clients.Add(new ConnectedClient(client, Convert.ToInt32(line[0])));
                        }
                        //
                        SendToAllClients(st);
                    }

                    //while (client.Connected)
                    //{
                    //    try
                    //    {
                    //        sr = new StreamReader(client.GetStream());

                    //        var line = sr.ReadLine();

                    //        SendToAllClients(line);

                    //        Console.WriteLine(line);
                    //    }
                    //    catch { }
                    //}


                });


            }
            
        }

        private static async void SendToAllClients(string line)
        {
            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    try
                    {
                        if (clients[i].Client.Connected)
                        {
                            var sw = new StreamWriter(clients[i].Client.GetStream());
                            sw.AutoFlush = true;

                            sw.WriteLine(line);
                            
                        }
                        else
                        {
                            clients.RemoveAt(i);
                        }
                    }
                    catch { }
                }
                
            });
            Console.WriteLine(line);

        }
        
    }
}
