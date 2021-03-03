namespace Valuator
{
    public interface IMessageBroker
    {
        void Publish(string subject, byte[] data);
    }

}