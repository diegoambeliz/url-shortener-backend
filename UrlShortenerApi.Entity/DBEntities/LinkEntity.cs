using Dapper.Contrib.Extensions;

namespace UrlShortenerApi.Entity.DBEntities
{
    [Table("link")]
    public class LinkEntity
    {
        [ExplicitKey]
        public Guid id { get; set; } = Guid.Empty;
        public string description { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
        public string shortLink { get; set; } = string.Empty;
        public long viewsCount { get; set; } = 0;
        public string createdBy { get; set; } = string.Empty;
        public DateTime createdAt { get; set; } = DateTime.MinValue;
    }
}
