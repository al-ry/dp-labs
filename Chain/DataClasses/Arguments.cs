namespace Chain
{
    struct Arguments
    {
        public Arguments(int listeningPort, string nextHost, int nextPort, bool isInitiator)
        {
            ListeningPort = listeningPort;
            NextHost = nextHost;
            NextPort = nextPort;
            IsInitiator = isInitiator;
        }
        public int ListeningPort { get; }
        public string NextHost { get; }
        public int NextPort { get; }
        public bool IsInitiator { get; }
    }
}