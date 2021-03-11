using System;
using NATS.Client;
using System.Text;
using Valuator;
namespace EventsLogger
{
    public class EventsLogger
    {
        
        private readonly IConnection _connection;
        private readonly IAsyncSubscription _subscr;
        public EventsLogger()
        {
            ConnectionFactory cf = new ConnectionFactory();
            _connection = cf.CreateConnection();

            _subscr = _connection.SubscribeAsync(Constants.SimilarityCalculatedEventName, _similarityCalculatedHandler);
            _subscr = _connection.SubscribeAsync(Constants.RankCalculatedEventName, _rankCalculatedHandler);
        }

        private EventHandler<MsgHandlerEventArgs> _similarityCalculatedHandler = (sender, args) =>
        {
            Console.WriteLine("from sim log ");
            Console.WriteLine(args.Message);
            args.Message.ArrivalSubcription.Unsubscribe();
        };
        private EventHandler<MsgHandlerEventArgs> _rankCalculatedHandler = (sender, args) =>
        {
            Console.WriteLine("from rank log");
            Console.WriteLine(args.Message);
            args.Message.ArrivalSubcription.Unsubscribe();
        };
        public void Start()
        {
            Console.WriteLine("Logger started");

            _subscr.Start();

            Console.WriteLine("Press Enter to exit RankCalculatorService");
            Console.ReadLine();

            _subscr.Unsubscribe();

            _connection.Drain();
            _connection.Close();
        }
    }
}