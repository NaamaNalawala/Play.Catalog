namespace Play.Common.Service.Settings{
    public class MongoDbSettings{
        public string Host { get; set; }
        public int Port { get; set; }
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}