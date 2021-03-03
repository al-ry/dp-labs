using NATS.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace Valuator
{
    public class NatsMessageBroker: IMessageBroker
    {
        public void Publish(string subject, byte[] data)
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Task.Factory.StartNew(() => Produce(cts.Token, data, subject), cts.Token);
        }

        static void Produce(CancellationToken ct, byte[] id, string eventName)
        {
            ConnectionFactory cf = new ConnectionFactory();
            using (IConnection c = cf.CreateConnection())
            {
                if (!ct.IsCancellationRequested)
                {
                    c.Publish(eventName, id);
                }
                c.Drain();

                c.Close();
            }
        }
    }
}