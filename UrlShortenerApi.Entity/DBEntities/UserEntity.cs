using Dapper.Contrib.Extensions;

namespace UrlShortenerApi.Entity.DBEntities
{
    [Table("[user]")]
    public class UserEntity
    {
        [ExplicitKey]
        public string username { get; set; } = string.Empty;
        public string first_name { get; set; } = string.Empty;
        public string last_name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string pwd { get; set; } = string.Empty;
        public bool activated { get; set; } = false;
        public byte[] salt { get; set; } = [];
    }
}
