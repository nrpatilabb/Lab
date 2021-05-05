namespace Blogging.Models
{
    public class DBSettings : IDBSettings
    {
        public string BlogCollectionName { get; set; }

        public string UsersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDBSettings
    {
        string BlogCollectionName { get; set; }

        string UsersCollectionName { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
