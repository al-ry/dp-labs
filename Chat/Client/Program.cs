using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;

namespace Client
{
    class Arguments
    {
        public Arguments(string host, int port, string message)
        {
            Host = host;
            Port = port;
            Message = message;
        }
        public string Host { get; }
        public int Port { get; }
        public string Message { get; }
    }
    class Program
    {
        public static void StartClient(string host, int port, string message)
        {
            try
            {
                IPAddress ipAddress;
                if (host != "localhost")
                {
                    ipAddress = IPAddress.Parse(host);
                }
                else
                {
                    ipAddress = IPAddress.Loopback;
                }
                

                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // CREATE
                Socket sender = new Socket(
                    ipAddress.AddressFamily,
                    SocketType.Stream, 
                    ProtocolType.Tcp);

                try
                {
                    // CONNECT
                    sender.Connect(remoteEP);

                    // Подготовка данных к отправке
                    byte[] msgBytes = Encoding.UTF8.GetBytes(message);

                    // SEND
                    int bytesSent = sender.Send(msgBytes);

                    // RECEIVE
                    byte[] buf = new byte[1024];
                    int bytesRec = sender.Receive(buf);

                    var history = JsonSerializer.Deserialize<List<string>>(Encoding.UTF8.GetString(buf, 0, bytesRec));
                    
                    foreach (var msg in history)
                    {
                        Console.WriteLine(msg);
                    }

                    // RELEASE
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void Main(string[] args)
        {
            var arguments = ParseArguments(args);
            StartClient(arguments.Host, arguments.Port, arguments.Message);
        }

        static Arguments ParseArguments(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Invalid arguments count");
            }
            string host = args[0];
            var port = Int32.Parse(args[1]);
            string message = args[2];
            return new Arguments(host, port, message);
        }
    }
}
