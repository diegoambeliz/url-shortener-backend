namespace UrlShortenerApi.Dto
{
    public class LinkViewDto
    {
        public Guid id { get; set; } = Guid.Empty;
        public string description { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
        public string shortLink { get; set; } = string.Empty;
        public long viewsCount { get; set; } = 0;
        public DateTime createdAt { get; set; } = DateTime.MinValue;
    }
}
