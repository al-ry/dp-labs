using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

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
                    byte[] msgBytes = Encoding.UTF8.GetBytes(message + "<EOF>");

                    // SEND
                    int bytesSent = sender.Send(msgBytes);

                    // RECEIVE
                    string history = null;
                    List<string> serializedHistory = null;
                    byte[] buf = new byte[1024];

                    while (true)
                    {
                        // RECEIVE
                        int bytesRec = sender.Receive(buf);
                        history += Encoding.UTF8.GetString(buf, 0, bytesRec);
                        try
                        {
                            serializedHistory = JsonSerializer.Deserialize<List<string>>(history);
                            break;
                        }
                        catch (Exception)
                        {
                            continue;
                        }

                    }
                    foreach (var msg in serializedHistory)
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
