using System;
using NATS.Client;
using System.Text;
using Valuator;
using System.Threading.Tasks;

namespace RankCalculatorService
{
    public class RankCalculatorService
    {
        private readonly ISubscriber<MsgHandlerEventArgs> _subscr;
        private const string RankCalculatorQueue = "rank_calculator";

        private readonly IStorage _storage;

        public RankCalculatorService(IStorage storage, ISubscriber<MsgHandlerEventArgs> sub)
        {
            _storage = storage;

            _subscr = sub;
            _subscr.SubscribeAsyncWithQueue(Constants.RankCalculatorEventName, RankCalculatorQueue, (sender, args) =>
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