using System;
using NATS.Client;
using System.Text;
using Valuator;
namespace RankCalculatorService
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisStorage storage = new RedisStorage();
            RankCalculatorService rankCalculatorService = new RankCalculatorService(storage);
            rankCalculatorService.Start();
        }
    }
}
