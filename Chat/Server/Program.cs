﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;

namespace Server
{
    class Program
    {
        private static List<string> _history = new List<string>();
        public static void StartListening(int port)
        {

            // Разрешение сетевых имён
            
            // Привязываем сокет ко всем интерфейсам на текущей машинe
            IPAddress ipAddress = IPAddress.Any; 
            
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            // CREATE
            Socket listener = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            try
            {
                // BIND
                listener.Bind(localEndPoint);

                // LISTEN
                listener.Listen(10);

                while (true)
                {
                    // ACCEPT
                    Socket handler = listener.Accept();


                    byte[] buf = new byte[1024];
                    string data = null;
                    
                    while (true)
                    {
                        // RECEIVE
                        int bytesRec = handler.Receive(buf);

                        data += Encoding.UTF8.GetString(buf, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }

                    }
                    var EOFIndex = data.IndexOf("<EOF>");
                    var result = data.Remove(EOFIndex, 5);
                    Console.WriteLine("Message received: {0}", result);

                    _history.Add(result);


                    // SEND
                    var jsonString = JsonSerializer.Serialize(_history);
                    byte[] msg = Encoding.UTF8.GetBytes(jsonString);

                    handler.Send(msg);

                    // RELEASE
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static void Main(string[] args)
        {
            StartListening(Int32.Parse(args[0]));
        }
    }
}
