using System;
using NATS.Client;
using System.Text;
using Valuator;
using System.Threading.Tasks;

namespace RankCalculatorService
{
    public class RankCalculatorService
    {
        private readonly IConnection _connection;
        private readonly IAsyncSubscription _subscr;
        private const string RankCalculatorQueue = "rank_calculator";

        private readonly IStorage _storage;

        public RankCalculatorService(IStorage storage)
        {
            _storage = storage;

            ConnectionFactory cf = new ConnectionFactory();
            _connection = cf.CreateConnection();

            _subscr = _connection.SubscribeAsync(Constants.RankCalculatorEventName, RankCalculatorQueue, (sender, args) =>
            {
                string id = Encoding.UTF8.GetString(args.Message.Data);
                string text = _storage.GetValue(Constants.TextKeyPrefix + id);
                var rank = GetRank(text);
                _storage.StoreValue(Constants.RankKeyPrefix + id, rank.ToString());
            });
        }

        public void Start()
        {
            Console.WriteLine("RankCalculator service started");

            _subscr.Start();

            Console.WriteLine("Press Enter to exit RankCalculatorService");
            Console.ReadLine();

            _subscr.Unsubscribe();

            _connection.Drain();
            _connection.Close();
        }

        private double GetRank(string text)
        {
            int nonalphaCounter = 0;
            foreach (var ch in text)
            {
                if (!Char.IsLetter(ch))
                {
                    nonalphaCounter++;
                }
            }
            return Convert.ToDouble(nonalphaCounter) / Convert.ToDouble(text.Length);
        }
    }
}