using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chain
{
    class Program
    {
        private static Socket _sender;
        private static Socket _listener;
        private static int _number = 0;
        static void Main(string[] args)
        {
            try
            {
                Arguments arguments = ParseArguments(args);
                StartProcessing(arguments.ListeningPort, arguments.NextHost, arguments.NextPort, arguments.IsInitiator);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexcepted exception : {0}", e.ToString());
            }
        }
        static Arguments ParseArguments(string[] args)
        {
            if (args.Length < 3)
            {
                throw new ArgumentException("Invalid arguments count.\n");
            }
            int listeningPort = Int32.Parse(args[0]);
            string nextHost = args[1];
            int nextPort = Int32.Parse(args[2]);
            bool isInitiator = false; 
            if (args.Length == 4 && args[3] == "true")
            {
                isInitiator = true;
            }
            return new Arguments(listeningPort, nextHost, nextPort, isInitiator);
        }
        private static void StartProcessing(int listeningPort, string nextHost, int nextPort, bool isInitiator)
        {
            _listener = SetupListener(listeningPort);
            _sender = SetupSender(nextHost, nextPort);


            if (isInitiator)
            {
                ProcessAsInitiator();
            }
            else
            {
                ProcessAsSimpleProcess();
            }
       
        }
        private static void ProcessAsInitiator()
        {
            string numberStr = Console.ReadLine();
            var bytesNumber = BitConverter.GetBytes(Int32.Parse(numberStr));

            _sender.Send(bytesNumber);

            var handler = _listener.Accept();

            byte[] buf = new byte[sizeof(int)];
            handler.Receive(buf);

            int y = BitConverter.ToInt32(buf);
            _number = y;
            Console.WriteLine(_number);

            var bytes = BitConverter.GetBytes(_number);
            int bytesSent = _sender.Send(bytes);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        private static void ProcessAsSimpleProcess()
        {
           var handler = _listener.Accept();

            string numberStr = Console.ReadLine();
            var bytesNumber = BitConverter.GetBytes(Int32.Parse(numberStr));
            var _number = BitConverter.ToInt32(bytesNumber);
            
            byte[] buf = new byte[sizeof(int)];
            handler.Receive(buf);
            int y = BitConverter.ToInt32(buf);
            var bytes = BitConverter.GetBytes(Math.Max(_number, y));
            int bytesSent = _sender.Send(bytes);

            buf = new byte[sizeof(int)];
            handler.Receive(buf);
            int receivedNumber = BitConverter.ToInt32(buf);
            Console.WriteLine(receivedNumber);

            _sender.Send(buf);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        private static Socket SetupListener(int port)
        {
            IPAddress ipAddress = IPAddress.Any; 
            
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            var listener = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            listener.Bind(remoteEP);

            listener.Listen(10);

            return listener; 
        }

        private static Socket SetupSender(string host, int port)
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

            var sender = new Socket(
                ipAddress.AddressFamily,
                SocketType.Stream, 
                ProtocolType.Tcp);
            
            var result = sender.BeginConnect(remoteEP, null, null);
            
            bool success = result.AsyncWaitHandle.WaitOne(7000, true);
            if (success)
            {
                sender.EndConnect(result);
            }
            else
            {
                sender.Close();
                throw new SocketException(10060); // Connection timed out.
            }

            return sender;
        }
    }

}
