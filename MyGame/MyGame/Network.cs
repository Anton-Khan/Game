using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyGame
{
    static class Network
    {
        
        public static Pos enemyPos = new Pos();
        private static IPAddress _ipAddress = IPAddress.Loopback ;
        private static int _port = 1234;
        public static int id = 1;

        private static StreamReader sr;
        private static StreamWriter sw;
        private static TcpClient sender;
        



      

        public static void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        if (sender?.Connected == true)
                        {

                            var line = sr.ReadLine();
                            if (line != null)
                            {
                                string[] st = line.Split(' ');

                                if (float.Parse(st[0]) != id)
                                {
                                    enemyPos.x = float.Parse(st[1]);
                                    enemyPos.y = float.Parse(st[2]);
                                }
                            }
                            else
                            {
                                sender.Close();
                                MessageBox.Show("Connection error");
                            }
                        }
                        
                        Task.Delay(10).Wait();
                    }
                    catch { }
                    
                }
            });


        }

        public static void Connect()
        {

            if (sender?.Connected == false || sender == null)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        sender = new TcpClient();
                        sender.Connect(_ipAddress, _port);
                        sr = new StreamReader(sender.GetStream());
                        sw = new StreamWriter(sender.GetStream());
                        sw.AutoFlush = true;

                        sw.WriteLine(id+ " ");
                        
                    }
                    catch (Exception e) { MessageBox.Show(e.ToString()); }


                });
            }
        }

        public static void Send( Pos you)
        {
            if (sender?.Connected == true)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        sw.WriteLine($"{id} {you.x} {you.y}");
                    }
                    catch { }

                });
            }
        }
        

    }
}
