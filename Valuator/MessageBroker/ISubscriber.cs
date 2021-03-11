using System;

namespace Valuator

{
    public interface ISubscriber<Args>
    {
        void SubscribeAsyncWithQueue(string subject, string queue, EventHandler<Args> handler);

        void Unsubscribe();

    }
}